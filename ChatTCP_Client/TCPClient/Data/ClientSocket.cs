using System.Net;
using System.Net.Sockets;
using TCPClient.Connection;

namespace TCPClient.Data.Sockets
{
	public class ClientSocket
	{
		// Encoding - do not alter

		public Socket? socket;
		public const int BUFFER_SIZE = 2048;
		public byte[] buffer = new byte[BUFFER_SIZE];

		public Connection.Connection.ConnectionState connectionState = Connection.Connection.ConnectionState.STATE_DISCONNECTED;

		public IPAddress? IP;

		public EndPoint GetIP()
		{
			return socket.RemoteEndPoint;
		}
	}
}
