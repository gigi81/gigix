using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gigix
{
	public class HttpOutputMessage : HttpMessage
	{
		public HttpOutputMessage()
		{
			this.HeadersSent = false;
		}

		public bool HeadersSent { get; protected set; }

		public IHttpContent Content { get; set; }
	}
}
