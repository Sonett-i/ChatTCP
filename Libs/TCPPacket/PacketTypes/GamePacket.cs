using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libs.Formatting;
using TCPClientSocket;


namespace TCPPacket
{
	public class GamePacket : Packet
	{
		public int gameID;
		public string opponent;
		public string gameInfo;

		public GamePacket(ClientSocket clientSocket, PacketSubType subType, int gameID, string gameInfo) : base(clientSocket, -1)
		{
			base.packetType = PacketType.PACKET_GAME;
			base.packetSubType = subType;
			this.gameID = gameID;
			this.gameInfo = gameInfo;
			this.Serialize();
		}

		public static void Send(ClientSocket clientSocket, PacketSubType subType, int gameID, string message)
		{
			GamePacket gamePacket = new GamePacket(clientSocket, subType, gameID, message);

			gamePacket.Send();
		}

		// To do 


		public void Serialize()
		{
			string serialized = Format.String(Packet.PacketFormat[packetType][packetSubType],
				(int)packetType,
				(int)packetSubType,
				gameID,
				gameInfo);

			base.content = serialized;
		}
	}
}
