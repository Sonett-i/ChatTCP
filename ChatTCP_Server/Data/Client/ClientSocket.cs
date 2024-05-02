using System.Net;
using System.Net.Sockets;
using ChatTCP.Connection;

namespace ChatTCP.Data.Client
{
	public class ClientSocket
	{
		// Encoding - do not alter


		public Socket? socket;
		public const int BUFFER_SIZE = 2048;
		public byte[] buffer = new byte[BUFFER_SIZE];

		public int userID;

		public IPAddress? IP;
		public bool authorized = false;
		public Aurora.ConnectionState connectionState = Aurora.ConnectionState.STATE_CONNECTING;

		public EndPoint GetIP()
		{
			return socket.RemoteEndPoint;
		}
	}
}
