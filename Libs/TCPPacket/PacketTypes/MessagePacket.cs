﻿using System;
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
			base.content = message;

			this.Serialize();
		}

		public void Handle()
		{

		}
	}
}
