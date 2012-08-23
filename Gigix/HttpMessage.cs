using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Gigix
{
	public class HttpMessage
	{
		private HttpHeaders _headers;

		protected HttpMessage()
		{
		}

		public HttpHeaders Headers
		{
			get { return _headers ?? (_headers = new HttpHeaders()); }
		}
	}
}
