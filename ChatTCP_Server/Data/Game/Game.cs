using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP;
using ChatTCP.Data.Database;
using Libs.Formatting;
using TCPClientSocket;
using TCPPacket;

namespace ChatTCP.Data.Game
{
	public class Game
	{
		public static char field = (char)32;
		public static char record = (char)33;

		public static void ReceiveInfo(GamePacket gamePacket)
		{
			TicTacToe game = Server.GetGame(gamePacket.gameID);

			if (game == null)
				return;

			string[] parsed = gamePacket.gameInfo.Replace(record.ToString(), "").Split(Game.field);

			if (gamePacket.packetSubType == Packet.PacketSubType.GAME_MOVE)
			{
				Player player = game.GetPlayer(gamePacket.clientSocket.username);
				int index = int.Parse(parsed[0]);

				Vector2Int coord = game.CoordinatesFromIndex(index);

				game.Move(coord.x, coord.y, player);
			}
		}

		public static string[] GetStats(ClientSocket clientSocket)
		{
			Query statQuery = PreparedStatements.GetQuery(PreparedStatements.SELECT_USER_SCORES, (Int64)clientSocket.userID);

			object[][] stats = Server.database.Query(statQuery);

			if (stats == null || stats.Length == 0)
			{

			}
			int wins = (int)stats[0][1];
			int losses = (int)stats[0][2];
			int draws = (int)stats[0][3];


			return new string[] { wins.ToString(), losses.ToString(), draws.ToString() };
		}
	}
}
