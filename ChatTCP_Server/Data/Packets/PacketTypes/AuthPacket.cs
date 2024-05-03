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
		}

		public void Handle()
		{
			if (this.clientSocket.connectionState == Connection.ConnectionState.STATE_AUTHORIZING)
			{
				this.client = Authenticate.Client(this.client, (AuthPacket)this, out string result);

				if (this.client == null)
				{
					AckPacket nak = new AckPacket(this.clientSocket, Packet.PacketSubType.ACK_NAK, result);
					nak.Send();
				}
			}
		}

	}
}
