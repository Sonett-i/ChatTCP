using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ChatTCP.Data.Client;
using ChatTCP.Data.Packets;
using ChatTCP.Logging;


namespace ChatTCP.Connection
{
	public class Aurora
	{
		public enum ConnectionState
		{
			STATE_CONNECTING,
			STATE_AUTHORIZING,
			STATE_CONNECTED,
			STATE_DISCONNECTED,
		}
		// Client Connection Protocol

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

		public static Task<Client> AuthorizeConnection(Client newClient, CancellationToken cancellationToken)
		{
			TaskCompletionSource<Client> tcs = new TaskCompletionSource<Client>();

			newClient.clientSocket.connectionState = Aurora.ConnectionState.STATE_AUTHORIZING;

			cancellationToken.Register(() =>
			{
				tcs.TrySetCanceled();
			});

			// to-do: Handshake between server and client.

			Packet handshake = new AckPacket(newClient.clientSocket, Packet.PacketSubType.ACK_HANDSHAKE, "AUTHORIZING");

			handshake.Send();

			newClient.clientSocket.socket.BeginReceive(newClient.clientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, AuthorizationCallback, newClient);

			return tcs.Task;
		}

		private static void AuthorizationCallback(IAsyncResult AR)
		{
			Client? currentClient = (Client)AR.AsyncState;
			int received = ClientSocket.BUFFER_SIZE;

			try
			{
				received = currentClient.clientSocket.socket.EndReceive(AR);
			}
			catch (SocketException)
			{
				Log.Event(Log.LogType.LOG_EVENT, $"{currentClient.clientSocket.socket.RemoteEndPoint.ToString()} disconnected");
				currentClient.clientSocket.socket.Close();
				currentClient.clientSocket.connectionState = ConnectionState.STATE_DISCONNECTED;
				currentClient = null;
				return;
			}

			byte[] buffer = new byte[received];
			Array.Copy(currentClient.clientSocket.buffer, buffer, received);

			Packet authPacket = Packet.Receive(currentClient, buffer);

			Log.Event(Log.LogType.LOG_EVENT, $"AURORA: CALLBACK");

			//Message message = Message.Receive(buffer, currentClientSocket);
			if (!currentClient.clientSocket.authorized && currentClient.clientSocket.connectionState == ConnectionState.STATE_AUTHORIZING)
			{
				currentClient.clientSocket.socket.BeginReceive(currentClient.clientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, AuthorizationCallback, currentClient);
			}
			
		}
	}
}
