using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gigix.Server
{
	public class HttpResponseMessage : HttpOutputMessage
	{
		public HttpResponseMessage()
		{
			this.StartLine = new HttpResponseStartLine();
			this.Headers.ContentLength = 0;
		}

		public HttpResponseStartLine StartLine { get; private set; }

		public void WriteTo(Stream stream)
		{
			if (this.Content != null)
			{
				this.Headers.ContentLength = this.Content.Length;
				this.Headers.ContentType = this.Content.Mime;
			}

			using (var writer = new StreamWriter(stream, Encoding.UTF8))
			{
				writer.NewLine = HttpConsts.CRLF;

				writer.WriteLine(this.StartLine.Line);
				for (int i = 0; i < this.Headers.Count; i++)
				{
					writer.Write(this.Headers.Keys[i]);
					writer.Write(" : ");
					writer.WriteLine(this.Headers[i]);
				}

				writer.WriteLine();
				writer.Flush();

				if (this.Content != null)
					this.Content.WriteTo(stream);

				writer.Close();
			}
		}

		public byte[] ToArray()
		{
			using (var stream = new MemoryStream())
			{
				this.WriteTo(stream);
				return stream.ToArray();
			}
		}
	}
}
