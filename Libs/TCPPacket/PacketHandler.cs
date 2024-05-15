using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;

namespace TCPPacket
{
	public static class PacketHandler
	{
		public delegate Packet CommandDelegate(ClientSocket clientSocket, string[] args);

		static Dictionary<Packet.PacketType, CommandDelegate> GetPacket = new Dictionary<Packet.PacketType, CommandDelegate>()
		{
			[Packet.PacketType.PACKET_AUTH] = (clientSocket, args) => GetAuthPacket(clientSocket, args),
			[Packet.PacketType.PACKET_ACK] = (clientSocket, args) => GetAckPacket(clientSocket, args),
			[Packet.PacketType.PACKET_MESSAGE] = (clientSocket, args) => GetMessagePacket(clientSocket, args),
			[Packet.PacketType.PACKET_CONNECTION] = (clientSocket, args) => GetConnectionPacket(clientSocket, args),
			[Packet.PacketType.PACKET_GAME] = (clientSocket, args) => GetGamePacket(clientSocket, args),
			[Packet.PacketType.PACKET_COMMAND] = (clientSocket, args) => GetCommandPacket(clientSocket, args),
		};

		public static Packet FromBytes(ClientSocket clientSocket, byte[] buffer)
		{
			string[] blob = Packet.encoding.GetString(buffer).Replace(Packet.record.ToString(), "").Split(Packet.field);

			if (blob.Length > 1)
			{
				Packet.PacketType packetType = (Packet.PacketType)int.Parse(blob[0]);

				Packet packet = GetPacket[packetType].Invoke(clientSocket, blob);
				return packet;
			}

			return null;
		}

		public static ConnectionPacket GetConnectionPacket(ClientSocket clientSocket, string[] blob)
		{
			ConnectionPacket conPacket;
			if (blob.Length > 2)
			{
				conPacket = new ConnectionPacket(clientSocket, (ClientSocket.ConnectionState)int.Parse(blob[1]), blob[2]);
			}
			else
			{
				conPacket = new ConnectionPacket(clientSocket, (ClientSocket.ConnectionState)int.Parse(blob[1]));
			}
			


			return conPacket;
		}
		public static AuthPacket GetAuthPacket(ClientSocket clientSocket, string[] blob)
		{
			AuthPacket authPacket = new AuthPacket(clientSocket, (Packet.PacketSubType)int.Parse(blob[1]), int.Parse(blob[2]), (string)blob[3], (string)blob[4]);


			return authPacket;
		}

		public static AckPacket GetAckPacket(ClientSocket clientSocket, string[] blob)
		{
			AckPacket ackPacket = new AckPacket(clientSocket, (Packet.PacketSubType)int.Parse(blob[1]), blob[2]);

			return ackPacket;
		}

		public static MessagePacket GetMessagePacket(ClientSocket clientSocket, string[] blob)
		{
			MessagePacket messagePacket = new MessagePacket(clientSocket, clientSocket.userID, blob[2], blob[3]);

			return messagePacket;
		}

		public static GamePacket GetGamePacket(ClientSocket clientSocket, string[] blob)
		{
			if (blob.Length > 0)
			{
				GamePacket gamePacket = new GamePacket(clientSocket, (Packet.PacketSubType)int.Parse(blob[1]), int.Parse(blob[2]), blob[3]);

				return gamePacket;
			}
			
			return null;
		}

		public static CommandPacket GetCommandPacket(ClientSocket clientSocket, string[] blob)
		{
			if (blob.Length > 0)
			{
				CommandPacket commandPacket = new CommandPacket(clientSocket, -1, blob[2]);
				return commandPacket;
			}

			return null;
		}
	}
}
