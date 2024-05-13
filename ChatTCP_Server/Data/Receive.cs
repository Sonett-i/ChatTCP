using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Client;
using TCPPacket;
using TCPClientSocket;
using ChatTCP.Connect;

namespace ChatTCP
{
	public partial class Server
	{
		public static void Receive(Client client, Packet packet)
		{
			// unathorized clients can only be auth and ack handshakes.

			if (packet.packetType == Packet.PacketType.PACKET_ACK)
			{
				Receive(client, (AckPacket)packet);
			}
			else if (packet.packetType == Packet.PacketType.PACKET_AUTH)
			{
				Receive(client, (AuthPacket)packet);
			}
			else if (packet.packetType == Packet.PacketType.PACKET_COMMAND)
			{
				Receive(client, (CommandPacket)packet);
			}
			else if (packet.packetType == Packet.PacketType.PACKET_CONNECTION)
			{
				Receive(client, (ConnectionPacket)packet);
			}
			else if (packet.packetType == Packet.PacketType.PACKET_MESSAGE)
			{
				Receive(client, (MessagePacket)packet);
			}
		}

		public static void Receive(Client client, AckPacket ackPacket)
		{

		}

		public static void Receive(Client client, AuthPacket authPacket)
		{
			client = Authenticate.Client(client, authPacket, out string result);
		}

		public static void Receive(Client client, CommandPacket commandPacket)
		{

		}

		public static void Receive(Client client, MessagePacket messagePacket)
		{
			Server.SendToAll(messagePacket);
		}
	}
}
