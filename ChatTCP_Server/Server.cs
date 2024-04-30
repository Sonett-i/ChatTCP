using System.Net;
using System.Net.Sockets;
using Libs.Terminal;
using ChatTCP.Data.Formatting;
using ChatTCP.Connection;
using ChatTCP.Config;
using ChatTCP.Logging;
using ChatTCP.Data.Client;

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
		
		public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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
			serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
			Log.Event(Log.LogType.LOG_EVENT, $"Binding IP {IPAddress.Any}:{port}");

			serverSocket.Listen();
			Log.Event(Log.LogType.LOG_EVENT, $"Listening...");

			await Listen(cancellationToken);
		}

		private async Task Listen(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				Socket joiningSocket = await Aurora.AcceptConnection(this, cancellationToken);

				Client newClient = new Client() { clientSocket = new ClientSocket() { socket = joiningSocket } };

				Log.Event(Log.LogType.LOG_EVENT, $"{newClient.clientSocket.socket.RemoteEndPoint.ToString()} connecting");
				newClient = await Aurora.AuthorizeConnection(newClient, cancellationToken);
			}
		}

		// Static Methods
		public static Server Instance(int port)
		{
			return (ValidPort(port)) ? new Server(port) : new Server(Config.ServerConfig.defaultPort);
		}

		static bool ValidPort(int port)
		{
			return (port > 0 && port < 65535);
		}
		#endregion
	}
}