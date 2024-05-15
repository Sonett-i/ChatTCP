using System.Net;
using System.Net.Sockets;
using Libs.Terminal;
using ChatTCP.Connect;
using ChatTCP.Config;
using ChatTCP.Logging;
using ChatTCP.Data.Client;
using ChatTCP.Data.Database;
using TCPPacket;
using TCPClientSocket;
using ChatTCP.Data.Game;

namespace ChatTCP
{
	public partial class Server
	{
		public static void SendToAll(MessagePacket packet)
		{
			foreach (Client client in Server.ConnectedClients)
			{
				if (client.clientSocket.connectionState == ClientSocket.ConnectionState.STATE_AUTHORIZED)
				{
					Packet.Send(client.clientSocket, packet);
				}
			}
		}

		public static void Broadcast(CommandPacket packet, ClientSocket sender, string message = "")
		{
			string broadcast = "";

			foreach (Client client in Server.ConnectedClients)
			{
				if (client.clientSocket != sender)
				{
					if (client.clientSocket.connectionState == ClientSocket.ConnectionState.STATE_AUTHORIZED)
					{
						CommandPacket.Send(client.clientSocket, message);
					}
				}
			}
		}

		public static void Broadcast(ClientSocket sender, string message)
		{
			foreach (Client client in Server.ConnectedClients)
			{
				if (client.clientSocket != sender)
				{
					if (client.clientSocket.connectionState == ClientSocket.ConnectionState.STATE_AUTHORIZED)
					{
						CommandPacket.Send(client.clientSocket, message);
					}
				}
			}
		}

		public static void Broadcast(string message)
		{
			string broadcast = "";

			foreach (Client client in Server.ConnectedClients)
			{
				if (client.clientSocket.connectionState == ClientSocket.ConnectionState.STATE_AUTHORIZED)
				{
					CommandPacket.Send(client.clientSocket, message);
				}
			}
		}



		public static void Joined(ClientSocket socket)
		{
			Server.Broadcast(socket, $"{socket.displayName} has joined the server.");
		}

		public void ClientDisconnect(object packet, ClientSocket clientSocket)
		{
			foreach (Client client in Server.ConnectedClients)
			{
				if (client.clientSocket == clientSocket)
				{
					Server.RemoveClient(client);
					return;
				}
			}
		}

		public static Client GetClient(ClientSocket clientSocket)
		{
			foreach (Client client in Server.ConnectedClients)
			{
				if (client.clientSocket == clientSocket)
				{
					return client;
				}
			}
			return null;
		}

		public static void ShutdownSocket(Socket socket)
		{
			try
			{
				socket.Shutdown(SocketShutdown.Both);
			}
			catch (Exception e)
			{
				Terminal.Print("Socket Shutdown failed");
			}

			try
			{
				socket.Close();
			}
			catch (Exception e)
			{
				Terminal.Print("Socket close failed");
			}
		}
		public static void RemoveClient(Client client)
		{
			ShutdownSocket(client.clientSocket.socket);

			if (ConnectedClients.Contains(client))
			{
				ConnectedClients.Remove(client);
			}
		}

		public static void AddNewClient(Client client)
		{
			if (!ConnectedClients.Contains(client))
			{
				ConnectedClients.Add(client);
			}
		}

		public static void AddNewGame(TicTacToe game)
		{
			if (!CurrentGames.Contains(game))
			{
				Log.Event(Log.LogType.LOG_EVENT, $"Game started between {game.playerA.clientSocket.username} and {game.playerB.clientSocket.username}");
				CurrentGames.Add(game);
			}
		}

		public static void RemoveGame(TicTacToe game)
		{
			if (CurrentGames.Contains(game))
			{
				CurrentGames.Remove(game);
			}
		}

		public static bool CurrentlyPlaying(ClientSocket player)
		{
			foreach (TicTacToe game in CurrentGames)
			{
				if (player == game.playerA.clientSocket || player == game.playerB.clientSocket)
				{
					return true;
				}
			}
			return false;
		}

		public static int GetNewGameID()
		{
			int ID = 0;
			foreach (TicTacToe game in CurrentGames)
			{
				ID = game.gameID;
			}

			return ID + 1;
		}

		public static ClientSocket GetClientSocket(string username)
		{
			foreach (Client client in Server.ConnectedClients)
			{
				if (client.clientSocket.username == username || client.clientSocket.displayName == username)
				{
					return client.clientSocket;
				}
			}

			return null;
		}

		#region TicTacToe
		public static TicTacToe GetGame(int ID)
		{
			foreach (TicTacToe game in CurrentGames)
			{
				if (game.gameID == ID)
				{
					return game;
				}
			}
			return null;
		}

		public static TicTacToe GetGame(ClientSocket client)
		{

			return null;
		}
		#endregion

		public static void Kick(string user)
		{
			Client client = Server.GetClient(Server.GetClientSocket(user));

			if (client != null)
			{
				RemoveClient(client);
			}
		}
	}
}
