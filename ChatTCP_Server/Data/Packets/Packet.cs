using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Client;
using System.Net.Sockets;
using ChatTCP.Connection;
using ChatTCP.Data.Formatting;

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
		ClientSocket clientSocket;

		public PacketStructure.PacketType packetType;
		public PacketStructure.PacketSubType packetSubType;

		string content;

		public Packet(ClientSocket clientSocket, int sender)
		{
			this.clientSocket = clientSocket;
			this.socket = clientSocket.socket;
			this.sender = sender;
			//data = encoding.GetBytes(message);
		}

		public void Send()
		{
			this.Serialize();
			this.data = encoding.GetBytes(content);
			this.socket.Send(data, 0, data.Length, SocketFlags.None);
		}

		public void Serialize()
		{
			string serialized = Format.String(PacketStructure.PacketFormat[packetType][packetSubType],
				(int)packetType,
				(int)packetSubType,
				sender, 
				content);

			content = serialized;
		}

		public static Packet Receive(Client.Client sender, byte[] data)
		{
			Packet packet = null;
			packet = PacketStructure.GetPacket(sender.clientSocket, data);



			Packet response = null;  new Packet(sender.clientSocket, 0) { packetType = PacketStructure.PacketType.PACKET_ACK, packetSubType = PacketStructure.PacketSubType.ACK_ACK, content = "null" };
			

			if (sender.clientSocket.connectionState == Connection.Aurora.ConnectionState.STATE_AUTHORIZING)
			{
				sender = Authenticate.Client(sender, (AuthPacket) packet, out string result);

				response = new Packet(sender.clientSocket, 0) { packetType = PacketStructure.PacketType.PACKET_ACK, packetSubType = PacketStructure.PacketSubType.ACK_ACK, content = result };
			}

			response.Send();
			return packet;
		}
	}

	public partial class AuthPacket : Packet
	{
		int userID;
		public string username { get; }
		public string password { get; }

		public AuthPacket(ClientSocket clientSocket, int subType, int sender, string username, string password) : base (clientSocket, sender)
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
		public MessagePacket(ClientSocket clientSocket, int sender, string message) : base(clientSocket, sender)
		{

		}
	}
}
