using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ChatTCP.Data.Client;

namespace ChatTCP.Data.Packets
{
	public static class PacketStructure
	{
		public enum PacketType
		{
			PACKET_AUTH,
			PACKET_ACK,
			PACKET_NAK,
			PACKET_ACK_ACK,
			PACKET_MESSAGE
		}

		public enum PacketSubType
		{
			AUTH_AUTHORIZE,
			AUTH_REGISTER,
			ACK_ACK,
			ACK_NAK,
			MESSAGE_MESAGE,
			MESSAGE_BROADCAST,
			MESSAGE_WHISPER,
			MESSAGE_COMMAND
		}

		public static Dictionary<PacketType, Dictionary<PacketSubType, string>> PacketFormat = new Dictionary<PacketType, Dictionary<PacketSubType, string>>()
		{
			{ PacketType.PACKET_AUTH, new Dictionary<PacketSubType, string>
				{
					{ PacketSubType.AUTH_AUTHORIZE, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" },
					{ PacketSubType.AUTH_REGISTER, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" }
				}
			},
			{ PacketType.PACKET_ACK, new Dictionary<PacketSubType, string>
				{
					{ PacketSubType.ACK_ACK, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" },
					{ PacketSubType.ACK_NAK, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" }
				}
			},
			{ PacketType.PACKET_MESSAGE, new Dictionary<PacketSubType, string>
				{									// type				subtype			sender
					{ PacketSubType.MESSAGE_MESAGE, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" },
					{ PacketSubType.MESSAGE_BROADCAST, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" },
																										// commandID
					{ PacketSubType.MESSAGE_COMMAND, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" },
																										// recipientID
					{ PacketSubType.MESSAGE_WHISPER, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" },


				}
			},
		};

		public static Packet GetPacket(ClientSocket clientSocket, byte[] buffer)
		{
			Packet packet = null;
			string[] blob = Packet.encoding.GetString(buffer).Replace(Packet.record.ToString(), "").Split(Packet.field);

			PacketType packetType = (PacketType) int.Parse(blob[0]);
			if (packetType == PacketType.PACKET_AUTH)
			{
				packet = GetAuthPacket(clientSocket, blob);
			}

			if (packetType == PacketType.PACKET_MESSAGE)
			{
				packet = GetMessagePacket(clientSocket.socket, blob);
			}

			return packet;
		}

		public static AuthPacket GetAuthPacket(ClientSocket clientSocket, string[] blob)
		{
			AuthPacket authPacket = new AuthPacket(clientSocket, int.Parse(blob[1]), int.Parse(blob[2]), (string)blob[3], (string)blob[4]);

			return authPacket;
		}

		public static MessagePacket GetMessagePacket(Socket socket, object[] blob)
		{
			return null;
		}

		/* Packet Structure
		 * index	datatype	desc
		 * 0		int			packetType: auth, ACK, NAK, message etc.
		 * 1		int			packetSubType: auth_register, auth_login etc.
		 * 2		int			senderID: aligns with userID who sent, -1 for guest.
		 * 3		string		message string being sent.
		 */

		
	}
}
