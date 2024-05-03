using System.Net.Sockets;

namespace TCPClientSocket
{
	public class ClientSocket
	{
		public enum ConnectionState
		{
			STATE_CONNECTING,
			STATE_AUTHORIZING,
			STATE_AUTHORIZED,
			STATE_CONNECTED,
			STATE_DISCONNECTED,
		}

		public Socket socket;
		public const int BUFFER_SIZE = 2048;
		public byte[] buffer = new byte[BUFFER_SIZE];

		public int userID;

		public ConnectionState connectionState;
		public bool authorized = false;

		public event EventHandler<int> ConnectionStateChanged;

		public void SetConnectionState(ConnectionState state)
		{
			connectionState = state;
			ConnectionStateChanged.Invoke(this, (int)this.connectionState);
		}


	}
}