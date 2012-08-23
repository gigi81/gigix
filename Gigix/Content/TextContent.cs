using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gigix.Content
{
	public class TextContent : IHttpContent
	{
		public TextContent(string content)
			: this(content, Encoding.UTF8)
		{
		}

		public TextContent(string content, Encoding encoding)
		{
			this.Data = encoding.GetBytes(content);
		}

		private byte[] Data { get; set; }

		public long Length
		{
			get { return this.Data.Length; }
		}

		public string Mime
		{
			get { return "text/plain"; }
		}

		public void WriteTo(System.IO.Stream stream)
		{
			stream.Write(this.Data, 0, this.Data.Length);
		}
	}
}
