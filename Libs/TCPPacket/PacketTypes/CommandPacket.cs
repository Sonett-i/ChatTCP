using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;
using Libs.Formatting;

namespace TCPPacket
{
	public partial class CommandPacket : Packet
	{
		public string result;
		public enum CommandType
		{
			COMMAND_RESULT,
			COMMAND_GAME,
		}

		public CommandPacket(ClientSocket clientSocket, int sender, string message) : base(clientSocket, sender)
		{
			base.packetType = PacketType.PACKET_COMMAND;
			base.packetSubType = PacketSubType.COMMAND_RESULT;
			this.result = message;
			this.Serialize();

		}

		void Serialize()
		{
			string serialized = Format.String(Packet.PacketFormat[packetType][packetSubType],
				(int)packetType,
				(int)packetSubType,
				this.result);

			base.content = serialized;
		}

		public static void Send(ClientSocket clientSocket, string result)
		{
			CommandPacket commandPacket = new CommandPacket(clientSocket, 0, result);
			commandPacket.Send();
		}

		public string FormatString()
		{
			return $"{result}";
		}
	}
}
