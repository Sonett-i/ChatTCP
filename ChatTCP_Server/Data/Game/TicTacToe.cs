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

		int wins;
		int losses;
		int draws;

		public GameStats(int wins, int losses, int draws)
		{
			this.wins = wins;
			this.losses = losses;
			this.draws = draws;
		}

		public static GameStats GetStats(Player player)
		{
			Query statQuery = PreparedStatements.GetQuery(PreparedStatements.SELECT_USER_SCORES, (Int64)player.clientSocket.userID);

			object[][] stats = Server.database.Query(statQuery);

			GameStats gameStats = new GameStats((int)stats[0][1], (int)stats[0][2], (int)stats[0][3]);
			return gameStats;
		}

		public enum GameState
		{
			STATE_WIN,
			STATE_LOSS,
			STATE_DRAW,
		}

		

		public static void Win(Player player)
		{
			player.stats.wins += 1;
			GamePacket.Send(player.clientSocket, Packet.PacketSubType.GAME_RESULT, player.currentGame, "YOU WIN!");
			//Query updateWin = PreparedStatements.GetQuery(PreparedStatements.UPDATE_USER_WIN, )
		}

		public static void Lose(Player player)
		{
			player.stats.losses += 1;
			GamePacket.Send(player.clientSocket, Packet.PacketSubType.GAME_RESULT, player.currentGame, "YOU LOSE!");
		}

		public static void Draw(Player player)
		{
			player.stats.draws += 1;
			GamePacket.Send(player.clientSocket, Packet.PacketSubType.GAME_RESULT, player.currentGame, "DRAW!");
		}

		public static void UpdateStats(Player player)
		{
			Query updateStatQuery = PreparedStatements.GetQuery(PreparedStatements.UPDATE_USER_STATS, 
				player.stats.wins,
				player.stats.losses,
				player.stats.draws,
				(Int64)player.clientSocket.userID);

			Server.database.Query(updateStatQuery);
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
		public int currentGame;
		public int ID;
		public GameStats stats;

		public Player(ClientSocket socket, Check sprite)
		{
			this.clientSocket = socket;
			this.sprite = sprite;
			this.stats = GameStats.GetStats(this);
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

			int gameResult = Validate(player.ID);

			GamePacket.Send(playerA.clientSocket, Packet.PacketSubType.GAME_MOVE, gameID, boardinfo);
			GamePacket.Send(playerB.clientSocket, Packet.PacketSubType.GAME_MOVE, gameID, boardinfo);

			if (gameResult != 0)
			{
				EndGame(gameResult);
			}

			Log.Event(Log.LogType.LOG_GAME, boardinfo);
		}

		public void Start()
		{

			SendOpponentInfo(playerA);
			SendOpponentInfo(playerB);

		}

		void EndGame(int result)
		{
			if (result == -1)
			{
				GameStats.Draw(playerA);
				GameStats.Draw(playerB);
			}

			if (playerA.ID == result)
			{
				GameStats.Win(playerA);
				GameStats.Lose(playerB);
			}
			else if (playerB.ID == result)
			{
				GameStats.Win(playerB);
				GameStats.Lose(playerA);
			}

			GameStats.UpdateStats(playerA);
			GameStats.UpdateStats(playerB);

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


		// Pathfinding algo?
		public int Validate(int player)
		{
			// Check rows
			for (int x = 0; x < 3; x++)
			{
				if (board[x, 0] == player && board[x, 1] == player && board[x, 2] == player)
				{
					Console.WriteLine("Player " + player + " wins by row " + x);
					return player;
				}
			}

			// Check columns
			for (int y = 0; y < 3; y++)
			{
				if (board[0, y] == player && board[1, y] == player && board[2, y] == player)
				{
					return player;
				}
			}

			// Check diagonals
			if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
			{
				return player;
			}

			if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
			{
				return player;
			}

			// Finally check to see if game is a draw. If there are 0 free places left on the board, the game is a draw.
			int freeSpots = board.GetLength(0) * board.GetLength(1);

			for (int x = 0; x < 3; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					if (board[x, y] != 0)
						freeSpots--;
				}
			}

			if (freeSpots == 0)
			{
				return -1;
			}

			return 0;
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
