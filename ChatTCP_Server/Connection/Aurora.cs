using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ChatTCP.Data.Client;
using ChatTCP.Logging;
using TCPClientSocket;
using TCPPacket;


namespace ChatTCP.Connect
{
	public class Aurora
	{


		// Client Connection Protocol
		public static void HandleNewConnection(Socket joiningSocket)
		{
			Client newClient = new Client() { clientSocket = new ClientSocket() { socket = joiningSocket } };
			

			newClient.clientSocket.ConnectionStateChanged += newClient.StateChanged;

			newClient.clientSocket.SetConnectionState(ClientSocket.ConnectionState.STATE_CONNECTING);
			Server.ConnectedClients.Add(newClient);

			AuthorizeNewConnection(newClient);
		}

		private static void AuthorizeNewConnection(Client client)
		{
			client.clientSocket.SetConnectionState(ClientSocket.ConnectionState.STATE_AUTHORIZING);

			client.clientSocket.socket.BeginReceive(client.clientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, AuthReceiveCallback, client);
		}

		private static void AuthReceiveCallback(IAsyncResult AR)
		{
			Client currentClient = (Client)AR.AsyncState;

			int received = 0;

			try
			{
				received = currentClient.clientSocket.socket.EndReceive(AR);
			}
			catch (SocketException)
			{
				currentClient.clientSocket.SetConnectionState(ClientSocket.ConnectionState.STATE_DISCONNECTED);
				currentClient.clientSocket.socket.Close();
				Server.ConnectedClients.Remove(currentClient);
				return;
			}

			byte[] buffer = new byte[received];
			Array.Copy(currentClient.clientSocket.buffer, buffer, received);

			Packet packet = Packet.Receive(currentClient.clientSocket, buffer);

			if (currentClient.clientSocket.connectionState == ClientSocket.ConnectionState.STATE_CONNECTED)
			{
				return;
			}

			currentClient.clientSocket.socket.BeginReceive(currentClient.clientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, AuthReceiveCallback, currentClient);
		}

		public static Task<Socket> AcceptConnection(Server server, CancellationToken cancellationToken)
		{
			TaskCompletionSource<Socket> tcs = new TaskCompletionSource<Socket>();

			cancellationToken.Register(() =>
			{
				tcs.TrySetCanceled();
			});

			server.serverSocket.BeginAccept(asyncResult => 
			{ 
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				try
				{
					Socket socket = server.serverSocket.EndAccept(asyncResult);
					tcs.TrySetResult(socket);
				}
				catch (Exception ex)
				{
					tcs.TrySetException(ex);
				}
			}, null);

			return tcs.Task;
		}
	}
}
