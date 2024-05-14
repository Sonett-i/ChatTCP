using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;
using TCPPacket;
using ChatTCP;

namespace ChatTCP_Server.Data.Game
{
	public class Player
	{
		public enum Check
		{
			Cross,
			Nought,
		}

		public ClientSocket clientSocket;
		public Check sprite;

		public Player(ClientSocket socket, Check sprite)
		{
			this.clientSocket = socket;
			this.sprite = sprite;
		}
	}
	public class TicTacToe
	{

		public int gameID;
		public Player playerA;
		public Player playerB;

		int[,] board = new int[3, 3];

		public static TicTacToe NewGame(ClientSocket playerA, ClientSocket playerB)
		{
			if (Server.CurrentlyPlaying(playerA) || Server.CurrentlyPlaying(playerB))
			{
				return null;
			}
			int ID = Server.GetNewGameID();
			Player a = new Player(playerA, Player.Check.Cross);
			Player b = new Player(playerB, Player.Check.Nought);

			//first check to see if either player is already playing a game;
			TicTacToe newGame = new TicTacToe(a, b);

			return newGame;
		}

		public TicTacToe(Player playerA, Player playerB)
		{
			this.playerA = playerA;
			this.playerB = playerB;
			this.board = new int[3,3];
		}

		public void Move(int x, int y)
		{

		}

		public void Start()
		{
			string gameInfo = SerializedGameInformation();

			SendGameInfo(gameInfo);
			//GamePacket.Send(playerA.clientSocket, Packet.PacketSubType.GAME_START, this.gameID, playerA.clientSocket.username, playerB.clientSocket.username);

		}

		public string SerializedGameInformation()
		{
			string output = "";

			output += $"{this.gameID}{Packet.field}" 
					+ $"{playerA.clientSocket.username}{Packet.field}"
					+ $"{playerB.clientSocket.username}{Packet.field}";

			for (int x = 0; x < board.GetLength(0); x++)
			{
				for (int y = 0; y < board.GetLength(1); y++)
				{
					//	linear standard form: wx + y = index
					// where w = width

					int linearIndex = board.GetLength(1) * x + y;
					output += $"{linearIndex}{Packet.field}";
				}
			}


			return output;
		}

		void SendGameInfo(string gameinfo)
		{
			// to do need to serialize this
			GamePacket gamePacket1 = new GamePacket(playerA.clientSocket, Packet.PacketSubType.GAME_START, gameID) { opponent = playerB.clientSocket.username, gameInfo = gameinfo };
			GamePacket gamePacket2 = new GamePacket(playerB.clientSocket, Packet.PacketSubType.GAME_START, gameID) { opponent = playerA.clientSocket.username, gameInfo = gameinfo };



			// to do: need to send x, y coords
		}

		// Pathfinding algo?
		void Validate()
		{
			for (int x = 0; x < board.GetLength(0); x++)
			{
				for (int y = 0; y < board.GetLength(1); y++)
				{

				}
			}
		}

		void EndGame()
		{
			// Update Win/Loss Record SQL
		}
	}
}
