﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ChatTCP.Data.Client;
using ChatTCP.Data.Packets;
using ChatTCP.Logging;


namespace ChatTCP.Connect
{
	public class Aurora
	{

		// Client Connection Protocol
		public static void HandleNewConnection(Socket joiningSocket)
		{
			Client newClient = new Client() { clientSocket = new ClientSocket() { socket = joiningSocket } };
			Log.Event(Log.LogType.LOG_EVENT, $"{newClient.clientSocket.socket.RemoteEndPoint} connecting");

			newClient.clientSocket.ChangeConnectionState(Connection.ConnectionState.STATE_CONNECTING);
			Server.ConnectedClients.Add(newClient);

			AuthorizeNewConnection(newClient);
		}

		private static void AuthorizeNewConnection(Client client)
		{
			client.clientSocket.ChangeConnectionState(Connection.ConnectionState.STATE_AUTHORIZING);

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
				Log.Event(Log.LogType.LOG_EVENT, $"{currentClient.clientSocket.socket.RemoteEndPoint} disconnected");
				currentClient.clientSocket.socket.Close();
				Server.ConnectedClients.Remove(currentClient);
				return;
			}

			byte[] buffer = new byte[received];
			Array.Copy(currentClient.clientSocket.buffer, buffer, received);

			Packet packet = Packet.Receive(currentClient, buffer);

			if (currentClient.clientSocket.connectionState == Connection.ConnectionState.STATE_CONNECTED)
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
