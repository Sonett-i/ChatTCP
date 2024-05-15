using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;
using Libs.Formatting;

namespace TCPPacket
{
	public partial class MessagePacket : Packet
	{
		public int userID;
		public string username;
		public string displayname;
		public string message;

		public MessagePacket(ClientSocket clientSocket, int sender, string message) : base(clientSocket, sender)
		{
			base.packetType = PacketType.PACKET_MESSAGE;
			base.packetSubType = PacketSubType.MESSAGE_MESAGE;
			this.username = clientSocket.username;
			this.displayname = clientSocket.displayName;
			base.content = message;
			this.message = GetMessage(message);
			Serialize();
		}

		public MessagePacket(ClientSocket clientSocket, int sender, string username, string message) : base(clientSocket, sender)
		{
			base.packetType = PacketType.PACKET_MESSAGE;
			base.packetSubType = PacketSubType.MESSAGE_MESAGE;
			this.username = clientSocket.displayName;
			this.displayname = username;
			base.content = message;
			this.message = GetMessage(message);
			Serialize();
		}

		string GetMessage(string content)
		{
			string[] blob = content.Split(Packet.field);

			return content;
		}
		public string MessageFormat(string message)
		{
			return $"{displayname}{Packet.field}{message}";
		}

		void Serialize()
		{
			string serialized = Format.String(Packet.PacketFormat[packetType][packetSubType],
				(int)packetType,
				(int)packetSubType,
				this.displayname,
				content);

			content = serialized;
		}

		public void Handle()
		{

		}

		public override string ToString()
		{
			return $"{this.userID}:{this.username}:{base.content}";
		}

		public string FormatMessage()
		{
			return $"[{this.displayname}]: {this.message}";
		}
	}
}
