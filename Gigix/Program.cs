using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using SharpUV;

namespace Gigix
{
	class Program
	{
		static void Main(string[] args)
		{
			var server = new HttpServer();
			server.StartListening(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 10000));
			server.Loop.Run(); //blocking call

			server = null;
			System.GC.Collect();

			Debug.WriteLine("Memory report: allocated {0}, deallocated {1}", Loop.Default.AllocatedBytes, Loop.Default.DeAllocatedBytes);
			Console.Read();
		}
	}
}
