using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using TCPClient.Data.Sockets;
using TCPClient.Messaging;

namespace TCPClient.Data.Packets
{
	public partial class Packet : Client
	{

		public Client parent;
		public static char field = (char)30;
		public static char record = (char)31;

		public static Encoding encoding = Encoding.UTF8;
		int sender;
		byte[] data;
		Socket socket;
		public ClientSocket clientSocket;
		public string content;

		public PacketType packetType;
		public PacketSubType packetSubType;

		public static event EventHandler<string> PacketReceived;


		public Packet(ClientSocket clientSocket, int sender)
		{
			this.clientSocket = clientSocket;
			this.socket = clientSocket.socket;
			this.sender = sender;
			//data = encoding.GetBytes(message);
		}

		public void Send()
		{
			//this.Serialize();
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

		public static Packet Receive(ClientSocket sender, byte[] data)
		{
			Packet packet = PacketHandler.FromBytes(sender, data);

			PacketHandler.HandlePacket(packet);

			packet.DispatchInfo();

			return packet;
		}

		public void DispatchInfo()
		{
			PacketReceived?.Invoke(this, this.content);
		}
	}

	public partial class AuthPacket : Packet
	{
		int userID;
		public string username { get; }
		public string password { get; }

		public AuthPacket(ClientSocket clientSocket, int subType, int sender, string username, string password) : base(clientSocket, sender)
		{
			base.packetType = PacketType.PACKET_AUTH;
			base.packetSubType = (PacketSubType)subType;

			this.userID = sender;
			this.username = username;
			this.password = password;

			this.Serialize();
		}

		public new void Serialize()
		{
			string serialized = Format.String(Packet.PacketFormat[packetType][packetSubType],
				(int)packetType,
				(int)packetSubType,
				this.userID,
				$"{this.username}{Packet.field}{this.password}");

			base.content = serialized;
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
		public AckPacket(ClientSocket clientSocket, int subType, string content) : base(clientSocket, 0)
		{
			base.packetType = PacketType.PACKET_ACK;
			base.packetSubType = (PacketSubType) subType;
			base.content = content;
		}
	}
}
