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

		public GamePacket(ClientSocket clientSocket, PacketSubType subType, int gameID) : base(clientSocket, -1)
		{
			base.packetType = PacketType.PACKET_GAME;
			base.packetSubType = subType;
		}

		public static void Send(ClientSocket clientSocket, PacketSubType subType, params object[] args)
		{
			string format = "";
		}

		// To do 


		public void Serialize(string format, params object[] args)
		{
			string serialized = Format.String(format, args);
			base.content = serialized;
		}

		public void Serialize()
		{
			string serialized = Format.String(Packet.PacketFormat[packetType][packetSubType],
				(int)packetType,
				(int)packetSubType,
				gameID,
				gameInfo);

			base.content = serialized;
		}
		public static GamePacket Receive(string[] blob)
		{
			return null;
		}
	}
}
