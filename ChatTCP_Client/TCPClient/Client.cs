using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TCPClient.Connection;
using TCPClientSocket;
using TCPPacket;

namespace TCPClient
{
	public partial class Client
	{
		public Socket socket;// = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		public ClientSocket clientSocket = new ClientSocket();

		public Authentication Authenticator;
		public EventManager EventManager;
		public int serverPort;
		public string serverIP;
		public bool active = true;

		public static Client CreateInstance(int port, int serverPort, string serverIP)
		{
			Client tcp = null;
			

			if (Connection.Connection.ValidPort(port, out serverPort))
			{
				
				tcp = new Client();
				tcp.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				tcp.serverPort = serverPort;
				tcp.serverIP = serverIP;
				tcp.clientSocket.socket = tcp.socket;
				tcp.Authenticator = new Authentication();
				tcp.EventManager = new EventManager(tcp);
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
				this.clientSocket.connectionState = ClientSocket.ConnectionState.STATE_DISCONNECTED;
			}
			else
			{
				try
				{
					clientSocket.socket.BeginReceive(clientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, clientSocket);
				}
				catch (Exception ex)
				{
					this.clientSocket.connectionState = ClientSocket.ConnectionState.STATE_DISCONNECTED;
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

			Packet packet = Packet.Receive(currentClientSocket, recBuffer);

			// To-do do stuff with packet
			Client.Receive(currentClientSocket, packet);

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