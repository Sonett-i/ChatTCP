using System;
using System.Text;
using System.Net.Sockets;
using TCPClientSocket;
using Libs.Formatting;

namespace TCPPacket
{
	public partial class Packet
	{

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

		public static event EventHandler<ClientSocket> PacketReceived;

		public Packet(ClientSocket clientSocket, int sender)
		{
			this.clientSocket = clientSocket;
			this.socket = clientSocket.socket;
			this.sender = sender;
			//data = encoding.GetBytes(message);
		}

		public void Send()
		{
			this.data = encoding.GetBytes(content);
			try
			{
				this.socket.Send(data, 0, data.Length, SocketFlags.None);
			}
			catch
			{

			}
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

		public static Packet Receive(ClientSocket sender, byte[] data)
		{
			Packet packet = PacketHandler.FromBytes(sender, data);

			PacketReceived?.Invoke(packet, sender);

			return packet;
		}
	}
}