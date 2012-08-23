using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gigix
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>Request line format: Method SP Request-URI SP HTTP-Version</remarks>
	public class HttpRequestStartLine : HttpStartLine
	{
		private const int MethodIndex = 0;
		private const int UriIndex = 1;
		private const int VersionIndex = 2;

		private const int TotalTokens = 3;

		public HttpRequestStartLine(byte[] data)
			: base(TotalTokens, VersionIndex, data)
		{
		}

		public string Method
		{
			get { return this.GetToken(MethodIndex); }
			set { this.SetToken(MethodIndex, value); }
		}

		public string Uri
		{
			get { return this.GetToken(UriIndex); }
			set { this.SetToken(UriIndex, value); }
		}

		protected override string ParseToken(int index, string value)
		{
			switch (index)
			{
				case MethodIndex:
					return value.Trim().ToUpper();

				case UriIndex:
					return value.Trim();

				case VersionIndex:
				default:
					return base.ParseToken(index, value);
			}
		}
	}
}
