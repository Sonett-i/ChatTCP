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
		public MessagePacket(ClientSocket clientSocket, int sender, string message) : base(clientSocket, sender)
		{
			base.packetType = PacketType.PACKET_MESSAGE;
			base.packetSubType = PacketSubType.MESSAGE_MESAGE;
			this.username = clientSocket.username;
			base.content = message;

			Serialize();
		}

		public string MessageFormat(string message)
		{
			return $"{username}{Packet.field}{message}";
		}

		void Serialize()
		{
			string serialized = Format.String(Packet.PacketFormat[packetType][packetSubType],
				(int)packetType,
				(int)packetSubType,
				username,
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
	}
}
