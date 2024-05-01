using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Client;
using System.Net.Sockets;
using ChatTCP.Connection;

namespace ChatTCP.Data.Packets
{
	public partial class Packet
	{
		public static char field = (char)30;
		public static char record = (char)31;

		public static Encoding encoding = Encoding.UTF8;
		int sender;
		byte[] data;
		Socket socket;

		public PacketStructure.PacketType packetType;
		public PacketStructure.PacketSubType packetSubType;

		string content;

		public Packet(Socket socket, int sender)
		{
			this.socket = socket;
			this.sender = sender;
			//data = encoding.GetBytes(message);
		}

		public void Send()
		{
			this.socket.Send(data, 0, data.Length, SocketFlags.None);
		}


		public static Packet Receive(Client.Client sender, byte[] data)
		{
			Packet packet = null;
			packet = PacketStructure.GetPacket(sender.clientSocket.socket, data);

			if (sender.clientSocket.connectionState == Connection.Aurora.ConnectionState.STATE_AUTHORIZING)
			{
				sender = Authenticate.Client(sender, (AuthPacket) packet, out string result);

			}

			return packet;
		}
	}

	public partial class AuthPacket : Packet
	{
		int userID;
		public string username { get; }
		public string password { get; }

		public AuthPacket(Socket socket, int subType, int sender, string username, string password) : base (socket, sender)
		{
			base.packetType = PacketStructure.PacketType.PACKET_AUTH;
			base.packetSubType = (PacketStructure.PacketSubType) subType;

			this.userID = sender;
			this.username = username;
			this.password = password;
		}
	}

	public partial class MessagePacket : Packet
	{
		public MessagePacket(Socket socket, int sender, string message) : base(socket, sender)
		{

		}
	}
}
