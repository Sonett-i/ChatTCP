using System.Net;
using System.Net.Sockets;
using Libs.Terminal;
using ChatTCP.Data.Formatting;
using ChatTCP.Connection;
using ChatTCP.Config;
using ChatTCP.Logging;
using ChatTCP.Data.Client;
using ChatTCP.Data.Database;
using ChatTCP.Data.Packets;

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

		public static Database database = null;

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
		private bool ConnectDatabase()
		{
			bool connectionValid = false;
			try
			{

				database = Database.Create(DatabaseConfig.Host, DatabaseConfig.User, DatabaseConfig.Pwd, DatabaseConfig.Database);
				Log.Event(Log.LogType.LOG_EVENT, $"Connecting to database: {database.host}; uid:{database.uid}...");
			}
			catch (Exception ex)
			{
				Log.Event(Log.LogType.LOG_ERROR, $"{ex.Message.ToString()}");
			}

			// attempt query

			try
			{
				connectionValid = database.Test("SHOW INDEX FROM users;");
			}
			catch (Exception ex)
			{
				Log.Event(Log.LogType.LOG_ERROR, $"{ex.Message}");
			}

			return connectionValid;
		}
		public async Task Setup(CancellationToken cancellationToken)
		{
			Terminal.Print(ServerConfig.PrintAbout());
			// Database 
			if (!ConnectDatabase())
			{
				Log.Event(Log.LogType.LOG_ERROR, $"Could not connect to database.");
				return;
			}

			Log.Event(Log.LogType.LOG_EVENT, $"Database connected");

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

				Packet handshake = new AckPacket(newClient.clientSocket, Packet.PacketSubType.ACK_HANDSHAKE, "CONNECTING");
				handshake.Send();

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