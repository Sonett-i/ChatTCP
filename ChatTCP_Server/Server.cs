using System.Net;
using System.Net.Sockets;
using Libs.Terminal;
using ChatTCP.Data.Formatting;

namespace ChatTCP
{
	public class Server
	{
		#region config
		public Server(int port)
		{
			this.port = (port == -1) ? Config.ServerConfig.defaultPort : port;
		}

		// Server Config 
		public int port;
		
		private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		// Program Flow
		public bool initialized = false;
		public bool running = false;
		#endregion

		#region Classes

		// Classes

		// Connected Clients

		// Connected Database
		#endregion


		#region Methods

		public async Task Setup(CancellationToken cancellationToken)
		{
			
		}

		// Static Methods
		public static Server? Instance(int port)
		{
			return (ValidPort(port)) ? new Server(port) : null;
		}

		static bool ValidPort(int port)
		{
			return (port > 0 && port < 65535);
		}
		#endregion
	}
}