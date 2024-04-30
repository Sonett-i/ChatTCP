using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TCPClient.Data.Sockets;

namespace TCPClient.Data
{
	internal class Packet
	{
		public static Encoding encoding = Encoding.UTF8;
		string sender;
		byte[] data;
		Socket socket;


		public Packet(Socket socket, string sender, string message)
		{
			this.socket = socket;
			this.sender = sender;
			data = encoding.GetBytes(message);
		}

		public void Send()
		{
			this.socket.Send(data, 0, data.Length, SocketFlags.None);
		}
	}
}
