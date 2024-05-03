using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;
using Libs.Formatting;

namespace TCPPacket
{
	public partial class AuthPacket : Packet
	{
		int userID;
		public string username { get; }
		public string password { get; }
		public PacketSubType packetSubType { get; }

		public AuthPacket(ClientSocket clientSocket, PacketSubType subType, int sender, string username, string password) : base(clientSocket, sender)
		{
			base.packetType = PacketType.PACKET_AUTH;
			this.packetSubType = (AuthPacket.PacketSubType)subType;

			this.userID = sender;
			this.username = username;
			this.password = password;

			this.Serialize();
		}

		public void Serialize()
		{
			base.content = Format.String(PacketFormat[PacketType.PACKET_AUTH][packetSubType], (int)packetType, (int)packetSubType, userID, $"{username}{Packet.field}{password}");
		}
	}
}
