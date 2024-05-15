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
		// Event handlers

		public static event EventHandler<MessagePacket> MessageReceived;
		public static event EventHandler<GamePacket> GameStateReceived;
		public static event EventHandler<CommandPacket> CommandReceived;
		public static event EventHandler<AuthPacket> AuthReceived;
		public static event EventHandler<AckPacket> AckReceived;


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
			else if (packet.packetType == Packet.PacketType.PACKET_GAME)
			{
				Receive(client, (GamePacket)packet);
			}
		}

		public static void Receive(ClientSocket client, AckPacket ackPacket)
		{
			//AckReceived.Invoke(client, ackPacket);
		}

		public static void Receive(ClientSocket client, AuthPacket authPacket)
		{
			if (authPacket.packetSubType == Packet.PacketSubType.AUTH_AUTHORIZE)
			{
				client.username = authPacket.username;
				client.displayName = authPacket.displayname;
			}
			if (authPacket.packetSubType == Packet.PacketSubType.AUTH_UPDATE)
			{
				client.displayName = authPacket.displayname;
			}

			//AuthReceived.Invoke(client, authPacket);
		}

		public static void Receive(ClientSocket client, CommandPacket commandPacket)
		{
			client.displayName = commandPacket.clientSocket.displayName;
			CommandReceived.Invoke(client, commandPacket);
		}

		public static void Receive(ClientSocket client, ConnectionPacket connectionPacket)
		{
			client.SetConnectionState(connectionPacket.connectionState);
		}

		public static void Receive(ClientSocket client, MessagePacket messagePacket)
		{
			MessageReceived.Invoke(client, messagePacket);
		}

		public static void Receive(ClientSocket client, GamePacket gamePacket)
		{
			GameStateReceived.Invoke(client, gamePacket);
		}
	}
}
