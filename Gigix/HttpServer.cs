using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpUV;

namespace Gigix
{
	public class HttpServer : TcpServer
	{
		protected override TcpServerSocket CreateClientSocket()
		{
			return new HttpServerSocket(this);
		}

		class HttpServerSocket : TcpServerSocket
		{
			private Server.HttpRequestMessage _message;

			public HttpServerSocket(HttpServer server)
				: base(server)
			{
				this.ReadStart();
			}

			protected Server.HttpRequestMessage Message
			{
				get
				{
					if(_message != null)
						return _message;

					_message = new Server.HttpRequestMessage();
					_message.Completed += new EventHandler(Message_Completed);

					return _message;
				}
			}

			private void Message_Completed(object sender, EventArgs e)
			{
				var message = new Server.HttpResponseMessage();
				message.StartLine.Status = 404;
				message.Content = new Content.TextContent("sorry! page not found :-(");
				this.Write(message.ToArray());
			}

			protected override void OnRead(byte[] data)
			{
				this.Message.Parse(data);
			}

			protected override void OnWrite()
			{
				base.OnWrite();
				this.Close();
			}

			protected override void OnClose()
			{
				base.OnClose();
			}
		}
	}
}
