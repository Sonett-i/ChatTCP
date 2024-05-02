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
		public string content;

		public PacketType packetType;
		public PacketSubType packetSubType;

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
			string serialized = Format.String(Packet.PacketFormat[packetType][packetSubType],
				(int)packetType,
				(int)packetSubType,
				sender, 
				content);

			content = serialized;
		}

		public static Packet Receive(Client.Client sender, byte[] data)
		{
			Packet packet = PacketHandler.FromBytes(sender.clientSocket, data);

			packet.Handle();
			Packet response = null; 
			
			/*
			if (sender.clientSocket.connectionState == Connection.Aurora.ConnectionState.STATE_AUTHORIZING)
			{
				sender = Authenticate.Client(sender, (AuthPacket) packet, out string result);

				response = new Packet(sender.clientSocket, 0) 
				{ 
					packetType = PacketType.PACKET_ACK, 
					//packetSubType = PacketSubType.ACK_ACK, 
					content = result
				};
			}
			*/
			response.Send();
			return packet;
		}
	}

	public partial class AuthPacket : Packet
	{
		int userID;
		public string username { get; }
		public string password { get; }
		public PacketSubType packetSubType { get; }

		public AuthPacket(ClientSocket clientSocket, PacketSubType subType, int sender, string username, string password) : base (clientSocket, sender)
		{
			base.packetType = PacketType.PACKET_AUTH;
			this.packetSubType = (AuthPacket.PacketSubType) subType;

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

	public partial class AckPacket : Packet
	{
		public AckPacket(ClientSocket clientSocket, PacketSubType subType, string content) : base(clientSocket, 0)
		{
			base.packetType = PacketType.PACKET_ACK;
			base.packetSubType = PacketSubType.ACK_ACK;
			base.content = content;
		}

		public static AckPacket Acknowledge(ClientSocket clientSocket, string message)
		{
			return new AckPacket(clientSocket, 0, message);
		}
	}
}
