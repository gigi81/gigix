using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gigix.Server
{
	public class HttpRequestMessage : HttpInputMessage
	{
		public HttpRequestStartLine StartLine { get; private set; }

		protected override void CreateStartLine(byte[] data)
		{
			this.StartLine = new HttpRequestStartLine(data);
		}
	}
}
