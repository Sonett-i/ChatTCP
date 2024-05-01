using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TCPClient.Data.Sockets;
using TCPClient.Connection;
using TCPClient.Messaging;
using TCPClient.Data.Packets;


namespace TCPClient
{
	public partial class Client
	{
		public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		public ClientSocket clientSocket = new ClientSocket();

		public Authentication Authenticator;

		public int serverPort;
		public string serverIP;
		public bool active = true;

		public static Client CreateInstance(int port, int serverPort, string serverIP)
		{
			Client tcp = null;
			

			if (Connection.Connection.ValidPort(port, out serverPort))
			{
				tcp = new Client();

				tcp.serverPort = serverPort;
				tcp.serverIP = serverIP;
				tcp.clientSocket.socket = tcp.socket;
				tcp.Authenticator = new Authentication();
				tcp.Authenticator.parent = tcp;
			}

			return tcp;
		}

		public void ConnectToServer()
		{
			int attempt = 0;


			while (!socket.Connected && attempt < 3)
			{
				try
				{
					attempt++;
					
					clientSocket.socket.Connect(serverIP, serverPort);
				}
				catch (SocketException)
				{

				}
			}

			if (!socket.Connected)
			{
				this.clientSocket.connectionState = Connection.Connection.ConnectionState.STATE_DISCONNECTED;
			}
			else
			{
				this.clientSocket.connectionState = Connection.Connection.ConnectionState.STATE_AUTHORIZING;

				try
				{
					clientSocket.socket.BeginReceive(clientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, clientSocket);
				}
				catch (Exception ex)
				{
					this.clientSocket.connectionState = Connection.Connection.ConnectionState.STATE_DISCONNECTED;
				}
			}
		}

		public void ReceiveCallback(IAsyncResult AR)
		{
			ClientSocket currentClientSocket = (ClientSocket)AR.AsyncState;

			int received;

			try
			{
				received = socket.EndReceive(AR);
			}
			catch (SocketException)
			{
				//AddToChat("Client disconnected");
				active = false;
				// Don't shutdown because the socket may be disposed and its disconnected anyway.
				socket.Close();

				//Program.NDSChat.ResetConnection();
				return;
			}

			byte[] recBuffer = new byte[received];
			Array.Copy(currentClientSocket.buffer, recBuffer, received);

			Packet.Receive(currentClientSocket.socket, recBuffer);


			//text is from server but could have been broadcast from the other clients
			//AddToChat(message.Format());

			//we just received a message from this socket, better keep an ear out with another thread for the next one
			try
			{
				currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
			}
			catch (Exception e)
			{

			}

		}
	}
}