using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Gigix
{
	public class HttpHeaders : NameValueCollection
	{
		public long ContentLength
		{
			get
			{
				var ret = this[System.Net.HttpKnownHeaderNames.ContentLength];
				if (!String.IsNullOrEmpty(ret))
					return Int64.Parse(ret);

				return 0;
			}
			set
			{
				this[System.Net.HttpKnownHeaderNames.ContentLength] = value.ToString();
			}
		}

		public string ContentType
		{
			get
			{
				return this[System.Net.HttpKnownHeaderNames.ContentType] ?? String.Empty;
			}
			set
			{
				this[System.Net.HttpKnownHeaderNames.ContentType] = value;
			}
		}
	}
}
