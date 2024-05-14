using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;

namespace TCPPacket
{
	public partial class Packet
	{

		/* Packet Structure
		 * index	datatype	desc
		 * 0		int			packetType: auth, ACK, NAK, message etc.
		 * 1		int			packetSubType: auth_register, auth_login etc.
		 * 2		int			senderID: aligns with userID who sent, -1 for guest.
		 * 3		string		message string being sent.
		 */

		public enum PacketType
		{
			PACKET_AUTH,
			PACKET_ACK,
			PACKET_CONNECTION,
			PACKET_COMMAND,
			PACKET_MESSAGE,
			PACKET_GAME,
		}
		public enum PacketSubType
		{
			AUTH_AUTHORIZE,
			AUTH_REGISTER,
			ACK_HANDSHAKE,
			ACK_ACK,
			ACK_NAK,
			MESSAGE_MESAGE,
			MESSAGE_BROADCAST,
			MESSAGE_WHISPER,
			MESSAGE_COMMAND,
			GAME_START,
			GAME_MOVE,
			GAME_RESULT
		}

		public static Dictionary<Packet.PacketType, Dictionary<Enum, string>> PacketFormat = new Dictionary<Packet.PacketType, Dictionary<Enum, string>>()
		{
			// Auth Packet Structure
			{ Packet.PacketType.PACKET_AUTH, new Dictionary<Enum, string>
				{
					{ Packet.PacketSubType.AUTH_AUTHORIZE, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" },
					{ Packet.PacketSubType.AUTH_REGISTER, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" }
				}
			},
			// Acknowledge packet Structure
			{ Packet.PacketType.PACKET_ACK, new Dictionary<Enum, string>
				{
					{ Packet.PacketSubType.ACK_ACK, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" },
					{ Packet.PacketSubType.ACK_HANDSHAKE, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" },
					{ Packet.PacketSubType.ACK_NAK, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.record}" }
				}
			},
			// Connection State packet structure
			{ Packet.PacketType.PACKET_CONNECTION, new Dictionary<Enum, string>
				{
					{ ClientSocket.ConnectionState.STATE_DISCONNECTED, $"%i{Packet.field}%i{Packet.record}" },
					{ ClientSocket.ConnectionState.STATE_AUTHORIZING, $"%i{Packet.field}%i{Packet.record}" },
					//													type		subtype		username
					{ ClientSocket.ConnectionState.STATE_AUTHORIZED, $"%i{Packet.field}%i{Packet.record}" },
					{ ClientSocket.ConnectionState.STATE_CONNECTED, $"%i{Packet.field}%i{Packet.record}" },
					{ ClientSocket.ConnectionState.STATE_CONNECTING, $"%i{Packet.field}%i{Packet.record}" },
				}
			},
			// Message packet structure
			{ Packet.PacketType.PACKET_MESSAGE, new Dictionary<Enum, string>
				{									// type				subtype			sender
					{ Packet.PacketSubType.MESSAGE_MESAGE, $"%i{Packet.field}%i{Packet.field}%s{Packet.field}%s{Packet.record}" },
					{ Packet.PacketSubType.MESSAGE_BROADCAST, $"%i{Packet.field}%i{Packet.field}%s{Packet.field}%s{Packet.record}" },
																										// commandID
					{ Packet.PacketSubType.MESSAGE_COMMAND, $"%i{Packet.field}%i{Packet.field}%s{Packet.field}%i{Packet.field}%s{Packet.record}" },
																										// recipientID
					{ Packet.PacketSubType.MESSAGE_WHISPER, $"%i{Packet.field}%i{Packet.field}%s{Packet.field}%s{Packet.field}%s{Packet.record}" },
				}
			},
			// Game packet structure

			/*
			 * 
			 * 
			 */

			{ Packet.PacketType.PACKET_GAME, new Dictionary<Enum, string>
				{
													// type,	subtype,	gameID,	player1,	player2
					{ Packet.PacketSubType.GAME_START, $"%i{Packet.field}%i{Packet.field}%i{Packet.field}%s{Packet.field}%s{Packet.record}" },
					{ Packet.PacketSubType.GAME_MOVE, $"%i{Packet.field}%i{Packet.field}%s{Packet.field}%s{Packet.record}" },
					{ Packet.PacketSubType.GAME_RESULT, $"%i{Packet.field}%i{Packet.field}%s{Packet.field}%s{Packet.record}" },
				}
			},
		};
	}
}
