using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Client;
using ChatTCP.Data.Formatting;
using ChatTCP.Connect;

namespace ChatTCP.Data.Packets
{
	public partial class ConPacket : Packet
	{
		public Connection.ConnectionState connectionState;

		public ConPacket(ClientSocket clientSocket, Connection.ConnectionState connectionState) : base(clientSocket, 0)
		{
			base.packetType = PacketType.PACKET_CON;
			this.connectionState = connectionState;
			this.Serialize();
		}

		public void Serialize()
		{
			string serialize = Format.String(Packet.PacketFormat[packetType][this.connectionState],(int)this.packetType, (int)this.connectionState);
			this.content = serialize;
		}

		new public static void Send(ClientSocket clientSocket, Connection.ConnectionState connectionState)
		{
			ConPacket conPacket =new ConPacket(clientSocket, connectionState);
			conPacket.Send();
		}
	}
}
