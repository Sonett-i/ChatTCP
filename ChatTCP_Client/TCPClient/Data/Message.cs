using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPPacket;

namespace TCPClient.Data
{
	public class Message
	{
		int senderID;
		string sender = "guest";
		string content;

		public static event EventHandler<Message> messageReceived;
		public Message(int senderID, string content)
		{
			this.senderID = senderID;
			this.content = content;
		}

		public static Message Receive(MessagePacket packet)
		{
			Message message = new Message(packet.userID, packet.content);

			messageReceived.Invoke(packet, message);
			return message;
		}

		public string Format()
		{
			return $"{sender}: {content}\r\n";
		}
	}
}
