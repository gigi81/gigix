//------------------------------------------------------------------------------
// <copyright file="Internal.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace System.Net
{
	using System.Globalization;
	using System.Text.RegularExpressions;
	using System.Threading;

	internal static class NclUtilities
	{
		internal static bool IsFatal(Exception exception)
		{
			return exception != null && (exception is OutOfMemoryException || exception is StackOverflowException || exception is ThreadAbortException);
		}
	}

	//
	// support class for Validation related stuff.
	//
	internal static class ValidationHelper
	{
		public static readonly string[] EmptyArray = new string[0];

		internal static readonly char[] InvalidMethodChars = new char[]
		{
			' ',
			'\r',
			'\n',
			'\t'
		};

		// invalid characters that cannot be found in a valid method-verb or http header
		internal static readonly char[] InvalidParamChars = new char[]
		{
			'(',
			')',
			'<',
			'>',
			'@',
			',',
			';',
			':',
			'\\',
			'"',
			'\'',
			'/',
			'[',
			']',
			'?',
			'=',
			'{',
			'}',
			' ',
			'\t',
			'\r',
			'\n'
		};

		public static string[] MakeEmptyArrayNull(string[] stringArray)
		{
			if (stringArray == null || stringArray.Length == 0)
				return null;

			return stringArray;
		}

		public static string MakeStringNull(string stringValue)
		{
			if (stringValue == null || stringValue.Length == 0)
				return null;

			return stringValue;
		}

		public static string ExceptionMessage(Exception exception)
		{
			if (exception == null)
			{
				return string.Empty;
			}
			if (exception.InnerException == null)
			{
				return exception.Message;
			}
			return exception.Message + " (" + ExceptionMessage(exception.InnerException) + ")";
		}

		public static string ToString(object objectValue)
		{
			if (objectValue == null)
			{
				return "(null)";
			}
			else if (objectValue is string && ((string)objectValue).Length == 0)
			{
				return "(string.empty)";
			}
			else if (objectValue is Exception)
			{
				return ExceptionMessage(objectValue as Exception);
			}
			else if (objectValue is IntPtr)
			{
				return "0x" + ((IntPtr)objectValue).ToString("x");
			}
			else
			{
				return objectValue.ToString();
			}
		}
		public static string HashString(object objectValue)
		{
			if (objectValue == null)
			{
				return "(null)";
			}
			else if (objectValue is string && ((string)objectValue).Length == 0)
			{
				return "(string.empty)";
			}
			else
			{
				return objectValue.GetHashCode().ToString(NumberFormatInfo.InvariantInfo);
			}
		}

		public static bool IsInvalidHttpString(string stringValue)
		{
			return stringValue.IndexOfAny(InvalidParamChars) != -1;
		}

		public static bool IsBlankString(string stringValue)
		{
			return stringValue == null || stringValue.Length == 0;
		}

		public static bool ValidateTcpPort(int port)
		{
			// on false, API should throw new ArgumentOutOfRangeException("port");
			return port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort;
		}

		public static bool ValidateRange(int actual, int fromAllowed, int toAllowed)
		{
			// on false, API should throw new ArgumentOutOfRangeException("argument");
			return actual >= fromAllowed && actual <= toAllowed;
		}

		// There are threading tricks a malicious app can use to create an ArraySegment with mismatched 
		// array/offset/count.  Copy locally and make sure they're valid before using them.
		internal static void ValidateSegment(ArraySegment<byte> segment)
		{
			// Length zero is explicitly allowed
			if (segment.Offset < 0 || segment.Count < 0
				|| segment.Count > (segment.Array.Length - segment.Offset))
			{
				throw new ArgumentOutOfRangeException("segment");
			}
		}
	}

	internal static class ExceptionHelper
	{
		internal static readonly WebPermission WebPermissionUnrestricted = new WebPermission(NetworkAccess.Connect, new Regex(".*"));
	}

	//
	// this class contains known header names
	//

	internal static class HttpKnownHeaderNames
	{

		public const string CacheControl = "Cache-Control";
		public const string Connection = "Connection";
		public const string Date = "Date";
		public const string KeepAlive = "Keep-Alive";
		public const string Pragma = "Pragma";
		public const string ProxyConnection = "Proxy-Connection";
		public const string Trailer = "Trailer";
		public const string TransferEncoding = "Transfer-Encoding";
		public const string Upgrade = "Upgrade";
		public const string Via = "Via";
		public const string Warning = "Warning";
		public const string ContentLength = "Content-Length";
		public const string ContentType = "Content-Type";
		public const string ContentDisposition = "Content-Disposition";
		public const string ContentEncoding = "Content-Encoding";
		public const string ContentLanguage = "Content-Language";
		public const string ContentLocation = "Content-Location";
		public const string ContentRange = "Content-Range";
		public const string Expires = "Expires";
		public const string LastModified = "Last-Modified";
		public const string Age = "Age";
		public const string Location = "Location";
		public const string ProxyAuthenticate = "Proxy-Authenticate";
		public const string RetryAfter = "Retry-After";
		public const string Server = "Server";
		public const string SetCookie = "Set-Cookie";
		public const string SetCookie2 = "Set-Cookie2";
		public const string Vary = "Vary";
		public const string WWWAuthenticate = "WWW-Authenticate";
		public const string Accept = "Accept";
		public const string AcceptCharset = "Accept-Charset";
		public const string AcceptEncoding = "Accept-Encoding";
		public const string AcceptLanguage = "Accept-Language";
		public const string Authorization = "Authorization";
		public const string Cookie = "Cookie";
		public const string Cookie2 = "Cookie2";
		public const string Expect = "Expect";
		public const string From = "From";
		public const string Host = "Host";
		public const string IfMatch = "If-Match";
		public const string IfModifiedSince = "If-Modified-Since";
		public const string IfNoneMatch = "If-None-Match";
		public const string IfRange = "If-Range";
		public const string IfUnmodifiedSince = "If-Unmodified-Since";
		public const string MaxForwards = "Max-Forwards";
		public const string ProxyAuthorization = "Proxy-Authorization";
		public const string Referer = "Referer";
		public const string Range = "Range";
		public const string UserAgent = "User-Agent";
		public const string ContentMD5 = "Content-MD5";
		public const string ETag = "ETag";
		public const string TE = "TE";
		public const string Allow = "Allow";
		public const string AcceptRanges = "Accept-Ranges";
		public const string P3P = "P3P";
		public const string XPoweredBy = "X-Powered-By";
		public const string XAspNetVersion = "X-AspNet-Version";
		public const string SecWebSocketKey = "Sec-WebSocket-Key";
		public const string SecWebSocketExtensions = "Sec-WebSocket-Extensions";
		public const string SecWebSocketAccept = "Sec-WebSocket-Accept";
		public const string SecWebSocketOrigin = "Sec-WebSocket-Origin";
		public const string SecWebSocketProtocol = "Sec-WebSocket-Protocol";
		public const string SecWebSocketVersion = "Sec-WebSocket-Version";
	}
}
