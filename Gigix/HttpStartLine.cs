using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gigix
{
	public class HttpStartLine
	{
		private string[] _tokens;
		private int _current = 0;

		protected HttpStartLine(int expectedTokens, int versionIndex, byte[] data)
		{
			this.ExpectedTokens = expectedTokens;
			this.VersionIndex = versionIndex;
			this.ParseData(data);
		}

		public string Line
		{
			get
			{
				var builder = new StringBuilder();

				for (int i = 0; i < this.ExpectedTokens - 1; i++)
				{
					builder.Append(this.GetToken(i));
					builder.Append(" ");
				}

				builder.Append(this.GetToken(this.ExpectedTokens - 1));

				return builder.ToString();
			}
		}

		public Version Version
		{
			get
			{
				switch (this.GetToken(this.VersionIndex))
				{
					case HttpConsts.HttpVersion11:
						return System.Net.HttpVersion.Version11;

					case HttpConsts.HttpVersion10:
						return System.Net.HttpVersion.Version10;

					default:
						return System.Net.HttpVersion.Version10;
				}
			}
			set
			{
				if (System.Net.HttpVersion.Version11.Equals(value))
					this.SetToken(this.VersionIndex, HttpConsts.HttpVersion11);
				else if (System.Net.HttpVersion.Version10.Equals(value))
					this.SetToken(this.VersionIndex, HttpConsts.HttpVersion10);
				else
					throw new ArgumentException("value");
			}
		}

		private void ParseData(byte[] data)
		{
			int last = 0, l = data.Length;

			for (int i = 0; i < l; i++)
			{
				if (data[i] != HttpConsts.SPACE)
					continue;

				this.SetToken(this.Current++, Encoding.UTF8.GetString(data, last, i - last));
				last = i + 1;

				if (this.IsLast)
					break;
			}

			this.SetToken(this.Current, Encoding.UTF8.GetString(data, last, l - last));
		}

		protected int ExpectedTokens
		{
			get { return _tokens.Length; }
			set { _tokens = new string[value]; }
		}

		private int VersionIndex { get; set; }

		private bool IsLast
		{
			get { return this.Current == (this.ExpectedTokens - 1); }
		}

		private int Current
		{
			get { return _current; }
			set
			{
				if (value >= this.ExpectedTokens)
					throw new ArgumentException("value");

				_current = value;
			}
		}

		protected string GetToken(int index)
		{
			return _tokens[index];
		}

		protected void SetToken(int index, string value)
		{
			_tokens[index] = this.ParseToken(index, value);
		}

		protected virtual string ParseToken(int index, string value)
		{
			if (index == this.VersionIndex)
				return value.Trim().ToUpper();

			return value;
		}
	}
}
