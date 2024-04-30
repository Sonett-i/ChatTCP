using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClient.Data.Sockets;

namespace TCPClient.Messaging
{
	internal class Message
	{
		public static char field = (char)30;
		public static char record = (char)31;
		public enum AuthMessage
		{
			MESSAGE_LOGIN,
			MESSAGE_REGISTER
		}

		public static Dictionary<AuthMessage, string> AuthMessages = new Dictionary<AuthMessage, string>() 
		{
			[AuthMessage.MESSAGE_LOGIN] = $"%i{field}%s{field}%s{record}",
			[AuthMessage.MESSAGE_REGISTER] = $"%i{field}%s{field}%s{record}",

		};


		public static void Receive(byte[] buffer, ClientSocket clientSocket)
		{

		}
	}
}
