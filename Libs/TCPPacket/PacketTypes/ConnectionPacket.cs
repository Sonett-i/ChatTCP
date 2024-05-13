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
		public string? username ="";
		public ConnectionPacket(ClientSocket clientSocket, ClientSocket.ConnectionState connectionState, string username = "") : base(clientSocket, 0)
		{
			base.packetType = PacketType.PACKET_CONNECTION;
			this.connectionState = connectionState;
			this.username = username;
			this.Serialize();
		}

		new public void Serialize()
		{
			string serialize = Format.String(Packet.PacketFormat[packetType][this.connectionState], (int)this.packetType, (int)this.connectionState);
			this.content = serialize;
		}

		public static void Send(ClientSocket clientSocket, ClientSocket.ConnectionState connectionState)
		{
			ConnectionPacket conPacket = new ConnectionPacket(clientSocket, connectionState);
			conPacket.Send();
		}
	}
}
