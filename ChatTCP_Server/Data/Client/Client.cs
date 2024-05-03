using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TCPClientSocket;
using TCPPacket;
using ChatTCP.Logging;

namespace ChatTCP.Data.Client
{
	public class Client
	{
		public Int32 ID;
		public string username;
		public Int16 secLevel;

		public ClientSocket clientSocket;

		public void StateChanged(object sender, int i)
		{
			Log.Event(Log.LogType.LOG_EVENT, $"{clientSocket.socket.RemoteEndPoint} {clientSocket.connectionState}");
			try
			{
				ConnectionPacket.Send(clientSocket, clientSocket.connectionState);
			}
			catch (Exception ex)
			{

			}
		}
	}
}
