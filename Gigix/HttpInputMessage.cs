using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Gigix
{
	public abstract class HttpInputMessage : HttpMessage
	{
		public event EventHandler StartLineReceived;
		public event EventHandler HeadersReceived;
		public event EventHandler Completed;

		private MemoryStream _lineBuffer = null;
		private int _colonIndex = 0;
		private bool _crFound = false;
		private Stream _stream = null;

		public HttpInputMessage()
		{
			this.StartLineRead = false;
			this.HeadersRead = false;
		}

		public bool StartLineRead { get; protected set; }

		public bool HeadersRead { get; protected set; }

		public bool IsComplete
		{
			get
			{
				if (!this.StartLineRead || !this.HeadersRead)
					return false;

				if(this.Headers.ContentLength <= 0)
					return true;

				return this.Content != null && this.Content.Length == this.Headers.ContentLength;
			}
		}

		public Stream Content
		{
			get
			{
				return _stream ?? (_stream = this.CreateContentStream());
			}
		}

		protected virtual Stream CreateContentStream()
		{
			return new MemoryStream();
		}

		public void Parse(byte[] data)
		{
			int i = 0;

			if (_lineBuffer == null)
				_lineBuffer = new MemoryStream(HttpConsts.DefaultLineBufferSize);

			for ( ; i < data.Length && !this.HeadersRead; i++)
			{
				this.AddByte(data[i]);
			}

			if (i < data.Length)
				this.Content.Write(data, i, data.Length - i);

			if (this.IsComplete)
			{
				_lineBuffer = null;
				this.OnCompleted();
			}
		}

		protected virtual void OnStartLineReceived()
		{
			if (this.StartLineReceived != null)
				this.StartLineReceived(this, EventArgs.Empty);
		}

		protected virtual void OnHeadersReceived()
		{
			if (this.HeadersReceived != null)
				this.HeadersReceived(this, EventArgs.Empty);
		}

		protected virtual void OnCompleted()
		{
			if (this.Completed != null)
				this.Completed(this, EventArgs.Empty);
		}

		protected abstract void CreateStartLine(byte[] data);

		private void AddByte(byte data)
		{
			if (_crFound)
			{
				if (data != HttpConsts.LF)
					throw new InvalidCharactersSequenceException();

				this.ParseLine();
				this.Reset();
			}
			else
			{
				switch (data)
				{
					case HttpConsts.COLON:
						if (_colonIndex <= 0)
							_colonIndex = (int)_lineBuffer.Length;
						break;

					case HttpConsts.CR:
						if (_crFound)
							throw new InvalidCharactersSequenceException();

						_crFound = true;
						break;
				}
			}

			if (data != HttpConsts.CR && data != HttpConsts.LF)
				_lineBuffer.WriteByte(data);
		}

		private void Reset()
		{
			_colonIndex = 0;
			_crFound = false;
			_lineBuffer.Position = 0;
			_lineBuffer.SetLength(0);
		}

		private void ParseLine()
		{
			byte[] data = _lineBuffer.ToArray();

			if (!this.StartLineRead)
			{
				this.CreateStartLine(data);
				this.StartLineRead = true;
				this.OnStartLineReceived();
			}
			else
			{
				if (_lineBuffer.Length > 0)
				{
					if (_colonIndex <= 0)
						throw new InvalidDataException("Found header line without comma");

					string name = Encoding.UTF8.GetString(data, 0, _colonIndex);
					string value = Encoding.UTF8.GetString(data, _colonIndex + 2, (int)_lineBuffer.Length - _colonIndex - 2);

					this.Headers.Add(name, value);
				}
				else //empty line
				{
					this.HeadersRead = true;
					this.OnHeadersReceived();
				}
			}
		}
	}
}
