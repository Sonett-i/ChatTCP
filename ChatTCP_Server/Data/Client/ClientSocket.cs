using System.Net;
using System.Net.Sockets;

namespace ChatTCP.Data.Client
{
	public class ClientSocket
	{
		// Encoding - do not alter


		public Socket? socket;
		public const int BUFFER_SIZE = 2048;
		public byte[] buffer = new byte[BUFFER_SIZE];

		public IPAddress? IP;

		public EndPoint GetIP()
		{
			return socket.RemoteEndPoint;
		}
	}
}
