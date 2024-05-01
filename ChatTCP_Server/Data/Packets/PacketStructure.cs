using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTCP.Data.Packets
{
	public static class PacketStructure
	{
		public enum PacketType
		{
			PACKET_AUTH,
			PACKET_ACK,
			PACKET_NAK,
			PACKET_ACK_ACK,
			PACKET_MESSAGE
		}

		public static Dictionary<PacketType, string> packetStruct = new Dictionary<PacketType, string>()
		{
			[PacketType.PACKET_AUTH] = $"%i{Packet.field}%s{Packet.field}%s{Packet.record}",
		};
	}
}
