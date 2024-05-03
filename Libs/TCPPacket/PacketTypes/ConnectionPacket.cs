using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;
using Libs.Formatting;

namespace TCPPacket
{
	public partial class ConnectionPacket : Packet
	{
		public ClientSocket.ConnectionState connectionState;

		public ConnectionPacket(ClientSocket clientSocket, ClientSocket.ConnectionState connectionState) : base(clientSocket, 0)
		{
			base.packetType = PacketType.PACKET_CONNECTION;
			this.connectionState = connectionState;
			this.Serialize();
		}

		public void Serialize()
		{
			string serialize = Format.String(Packet.PacketFormat[packetType][this.connectionState], (int)this.packetType, (int)this.connectionState);
			this.content = serialize;
		}

		new public static void Send(ClientSocket clientSocket, ClientSocket.ConnectionState connectionState)
		{
			ConnectionPacket conPacket = new ConnectionPacket(clientSocket, connectionState);
			conPacket.Send();
		}
	}
}
