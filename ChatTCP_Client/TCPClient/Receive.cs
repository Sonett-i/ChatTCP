using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPPacket;
using TCPClientSocket;

namespace TCPClient
{
	public partial class Client
	{
		public static void Receive(ClientSocket client, Packet packet)
		{
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


		public static void Receive(ClientSocket client, AckPacket ackPacket)
		{

		}

		public static void Receive(ClientSocket client, AuthPacket authPacket)
		{
			
		}

		public static void Receive(ClientSocket client, CommandPacket commandPacket)
		{

		}

		public static void Receive(ClientSocket client, ConnectionPacket connectionPacket)
		{
			client.SetConnectionState(connectionPacket.connectionState);
		}

		public static void Receive(ClientSocket client, MessagePacket messagePacket)
		{

		}
	}
}
