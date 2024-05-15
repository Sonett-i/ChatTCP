using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;
using TCPPacket;
using ChatTCP;
using ChatTCP.Logging;
using ChatTCP.Data.Database;

namespace ChatTCP.Data.Game
{
	public struct Vector2Int
	{
		public int x;
		public int y;

		public Vector2Int(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}
	

	public class GameStats
	{
		public enum GameState
		{
			STATE_WIN,
			STATE_LOSS,
			STATE_DRAW,
		}

		public static void UpdateStats(Player player, GameState state)
		{

		}
	}
	public class Player
	{
		public enum Check
		{
			Cross,
			Nought,
		}

		public ClientSocket clientSocket;
		public Check sprite;
		public string name;
		public int ID;

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


		public Player currentTurn;
		int[,] board = new int[3, 3];


		public int LinearIndex(int x, int y)
		{
			int index = x*3+y;

			return index;
		}

		public Vector2Int CoordinatesFromIndex(int index)
		{
			int x = (int)index / 3;
			int y = index % 3;

			return new Vector2Int(x, y);
		}

		public static TicTacToe NewGame(ClientSocket playerA, ClientSocket playerB)
		{
			if (Server.CurrentlyPlaying(playerA) || Server.CurrentlyPlaying(playerB))
			{
				return null;
			}
			int ID = Server.GetNewGameID();
			Player a = new Player(playerA, Player.Check.Cross) { name = playerA.username, ID = 1};
			Player b = new Player(playerB, Player.Check.Nought) { name = playerB.username, ID = 2};

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

		public void Move(int x, int y, Player player)
		{
			if (board[x,y] == 0)
			{
				board[x,y] = player.ID;
			}

			string boardinfo = BoardInformation();

			Validate(player.ID);
			if (!Validate(player.ID))
			{
				GamePacket.Send(playerA.clientSocket, Packet.PacketSubType.GAME_MOVE, gameID, boardinfo);
				GamePacket.Send(playerB.clientSocket, Packet.PacketSubType.GAME_MOVE, gameID, boardinfo);
			}
			else
			{
				EndGame();
				// handle win and lose situation.
			}
			


			Log.Event(Log.LogType.LOG_GAME, boardinfo);
		}

		public void Start()
		{

			SendOpponentInfo(playerA);
			SendOpponentInfo(playerB);

		}

		void EndGame()
		{
			// Update Win/Loss Record SQL

			Server.RemoveGame(this);
		}

		public string BoardInformation()
		{
			string output = "";

			for (int x = 0; x < board.GetLength(0); x++)
			{
				for (int y = 0; y < board.GetLength(1); y++)
				{
					output += $"{board[x,y]}{Game.field}";
				}
			}

			output += Game.record;

			return output;
		}

		void SendOpponentInfo(Player player)
		{
			string output = $"";

			if (player == playerA)
			{
				// Send Player B
				output += $"{playerB.clientSocket.username}{Game.field}";
			}
			else
			{
				output += $"{playerA.clientSocket.username}{Game.field}";
			}

			GamePacket.Send(player.clientSocket, Packet.PacketSubType.GAME_START, gameID, output);
		}

		void SendTurnInfo()
		{

		}

		void SendGameInfo(string gameinfo)
		{
			// to do need to serialize this
			GamePacket gamePacket1 = new GamePacket(playerA.clientSocket, Packet.PacketSubType.GAME_START, gameID, gameinfo) { opponent = playerB.clientSocket.username };
			GamePacket gamePacket2 = new GamePacket(playerB.clientSocket, Packet.PacketSubType.GAME_START, gameID, gameinfo) { opponent = playerA.clientSocket.username };


			// to do: need to send x, y coords
		}

		// Pathfinding algo?
		public bool Validate(int player)
		{
			// Check rows
			for (int x = 0; x < 3; x++)
			{
				if (board[x, 0] == player && board[x, 1] == player && board[x, 2] == player)
				{
					Console.WriteLine("Player " + player + " wins by row " + x);
					return true;
				}
			}

			// Check columns
			for (int y = 0; y < 3; y++)
			{
				if (board[0, y] == player && board[1, y] == player && board[2, y] == player)
				{
					Console.WriteLine("Player " + player + " wins by column " + y);
					return true;
				}
			}

			// Check diagonals
			if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
			{
				Console.WriteLine("Player " + player + " wins by diagonal");
				return true;
			}

			if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
			{
				Console.WriteLine("Player " + player + " wins by opposite diagonal");
				return true;
			}

			return false;
		}

		public Player GetPlayer(string name)
		{
			if (playerA.name == name)
				return playerA;

			if (playerB.name == name)
				return playerB;

			return null;
		}

		public Player GetPlayer(int index)
		{
			if (playerA.ID == index)
				return playerA;

			if (playerB.ID == index)
				return playerB;

			return null;
		}
	}
}
