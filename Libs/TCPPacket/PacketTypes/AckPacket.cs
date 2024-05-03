using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;
using Libs.Formatting;

namespace TCPPacket
{
	public partial class AckPacket : Packet
	{
		public int flag = 0;
		public AckPacket(ClientSocket clientSocket, PacketSubType subType, string content) : base(clientSocket, 0)
		{
			base.packetType = PacketType.PACKET_ACK;
			base.packetSubType = subType;
			base.content = content;
			this.Serialize();
		}

		public void Serialize()
		{
			string serialized = Format.String(Packet.PacketFormat[packetType][packetSubType],
				(int)packetType,
				(int)packetSubType,
				flag,
				content);

			base.content = serialized;
		}

		public static void Send(ClientSocket clientSocket, Packet.PacketSubType subType, string content)
		{
			AckPacket ackPacket = new AckPacket(clientSocket, subType, content);
			ackPacket.Send();
		}

		public void Handle()
		{

		}
	}
}
