using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Client;
using System.Net.Sockets;
using ChatTCP.Connect;
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
		public Client.Client client;

		public ClientSocket clientSocket;
		public string content;

		public PacketType packetType;
		public PacketSubType packetSubType;

		public Packet(ClientSocket clientSocket, int sender, Client.Client client = null)
		{
			this.clientSocket = clientSocket;
			this.socket = clientSocket.socket;
			this.sender = sender;
			//data = encoding.GetBytes(message);
		}

		public void Send()
		{
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

		public void Handle()
		{
			if (this is AuthPacket)
			{
				AuthPacket authPacket = (AuthPacket)this;
				authPacket.Handle();
			}
			else if (this is AckPacket)
			{
				AckPacket ackPacket = (AckPacket)this;
				ackPacket.Handle();
			}
			else if (this is MessagePacket)
			{
				MessagePacket messagePacket = (MessagePacket)this;
				messagePacket.Handle();
			}
		}

		public static Packet Receive(Client.Client sender, byte[] data)
		{
			Packet packet = PacketHandler.FromBytes(sender.clientSocket, data);
			packet.client = sender;

			packet.Handle();

			return packet;
		}
	}

}
