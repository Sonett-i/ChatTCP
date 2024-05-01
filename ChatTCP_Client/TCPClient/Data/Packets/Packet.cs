using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TCPClient.Data.Sockets;

namespace TCPClient.Data.Packets
{
	public partial class Packet
	{
		public static char field = (char)30;
		public static char record = (char)31;

		public static Encoding encoding = Encoding.UTF8;
		public int sender;
		public byte[] data;
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


		public static Packet Receive(Socket sender, byte[] data)
		{
			Packet packet = null;
			packet = PacketStructure.GetPacket(sender, data);

			return packet;
		}
	}

	public partial class AuthPacket : Packet
	{
		int userID;
		string username;
		string password;
		public AuthPacket(Socket socket, int subType, int sender, string username, string password) : base(socket, sender)
		{
			base.packetType = PacketStructure.PacketType.PACKET_AUTH;
			base.packetSubType = (PacketStructure.PacketSubType)subType;

			userID = sender;
			this.username = username;
			this.password = password;
		}

		private string Serialize()
		{
			string output = Messaging.Format.String(PacketStructure.PacketFormat[base.packetType][this.packetSubType], 
				(int)base.packetType, 
				(int)base.packetSubType,
				this.userID, 
				$"{this.username}{Packet.field}{this.password}");

			return output;
		}

		public new void Send()
		{
			string serialized = Serialize();

			base.data = encoding.GetBytes(serialized);

			base.Send();
		}
	}

	public partial class MessagePacket : Packet
	{
		public MessagePacket(Socket socket, int sender, string message) : base(socket, sender)
		{

		}
	}
}
