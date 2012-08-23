using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gigix
{
	internal class HttpConsts
	{
		internal const byte CR = 0x0D;
		internal const byte LF = 0x0A;
		internal const byte COLON = 58;
		internal const byte SPACE = 0x20;

		internal static string CRLF = new String(new[] { (char)HttpConsts.CR, (char)HttpConsts.LF });

		internal const string HttpVersion10 = "HTTP/1.0";
		internal const string HttpVersion11 = "HTTP/1.1";

		internal const int DefaultLineBufferSize = 512;
	}
}
