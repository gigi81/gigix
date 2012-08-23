using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gigix
{
	public interface IHttpContent
	{
		long Length { get; }

		string Mime { get; }

		void WriteTo(Stream stream);
	}
}
