using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gigix
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>Response line format: HTTP-Version SP Status-Code SP Reason-Phrase</remarks>
	public class HttpResponseStartLine : HttpStartLine
	{
		private const int DefaultStatus = 200;
		private static readonly byte[] DefaultStartLine = Encoding.UTF8.GetBytes(GetDefaultStatusLine());

		private const int VersionIndex = 0;
		private const int StatusIndex = 1;
		private const int ReasonIndex = 2;

		private const int TotalTokens = 3;

		public HttpResponseStartLine()
			: base(TotalTokens, VersionIndex, DefaultStartLine)
		{
		}

		public HttpResponseStartLine(byte[] data)
			: base(TotalTokens, VersionIndex, data)
		{
		}

		public int Status
		{
			get { return Int32.Parse(this.GetToken(StatusIndex)); }
			set
			{
				this.SetToken(StatusIndex, value.ToString());
				this.SetToken(ReasonIndex, HttpStatusDescription.Get(value));
			}
		}

		public string Reason
		{
			get { return this.GetToken(ReasonIndex); }
			set { this.SetToken(ReasonIndex, value); }
		}

		protected override string ParseToken(int index, string value)
		{
			switch (index)
			{
				case StatusIndex:
					return Int32.Parse(value).ToString();

				case ReasonIndex:
					return value.Trim();

				case VersionIndex:
				default:
					return base.ParseToken(index, value);
			}
		}

		private static string GetDefaultStatusLine()
		{
			return String.Format("{0} {1} {2}",
								 HttpConsts.HttpVersion11,
								 DefaultStatus,
								 HttpStatusDescription.Get(DefaultStatus));
		}
	}
}
