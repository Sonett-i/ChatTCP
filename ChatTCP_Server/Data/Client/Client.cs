using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TCPClientSocket;

namespace ChatTCP.Data.Client
{
	public class Client
	{
		public Int32 ID;
		public string username;
		public Int16 secLevel;

		public ClientSocket clientSocket;
	}
}
