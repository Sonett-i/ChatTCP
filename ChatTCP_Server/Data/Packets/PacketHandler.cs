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
	public static class PacketHandler
	{
		public delegate Packet CommandDelegate(ClientSocket clientSocket, string[] args);

		static Dictionary<Packet.PacketType, CommandDelegate> GetPacket = new Dictionary<Packet.PacketType, CommandDelegate>()
		{
			[Packet.PacketType.PACKET_AUTH] = (clientSocket, args) => GetAuthPacket(clientSocket, args),
			[Packet.PacketType.PACKET_ACK] = (clientSocket, args) => GetAckPacket(clientSocket, args),
			[Packet.PacketType.PACKET_MESSAGE] = (clientSocket, args) => GetMessagePacket(clientSocket, args),
		};

		public static Packet FromBytes(ClientSocket clientSocket, byte[] buffer)
		{
			string[] blob = Packet.encoding.GetString(buffer).Replace(Packet.record.ToString(), "").Split(Packet.field);

			Packet.PacketType packetType = (Packet.PacketType)int.Parse(blob[0]);

			Packet packet = GetPacket[packetType].Invoke(clientSocket, blob);

			return packet;
		}


		public static AuthPacket GetAuthPacket(ClientSocket clientSocket, string[] blob)
		{
			AuthPacket authPacket = new AuthPacket(clientSocket, (Packet.PacketSubType)int.Parse(blob[1]), int.Parse(blob[2]), (string)blob[3], (string)blob[4]);

			return authPacket;
		}

		public static AckPacket GetAckPacket(ClientSocket clientSocket, string[] blob)
		{
			AckPacket ackPacket = new AckPacket(clientSocket, (Packet.PacketSubType) int.Parse(blob[1]), blob[2]);

			return ackPacket;
		}

		public static MessagePacket GetMessagePacket(ClientSocket clientSocket, string[] blob)
		{
			MessagePacket messagePacket = null;

			return messagePacket;
		}
	}
}
