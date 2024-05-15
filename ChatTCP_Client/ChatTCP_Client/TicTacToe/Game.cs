using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TCPPacket;
using ChatTCP_Client;

namespace ChatTCP_Client.TicTacToe
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
	public class Game
	{
		public static char field = (char)32;
		public static char record = (char)33;

		public List<Button> gameButtons = new List<Button>();
		public Label opponentLabel;
		public Label gameLog;
		public Label turnLabel;
		public bool gamePlaying = false;

		public int[,] board;
		public int gameID = -1;

		string opponent;
		bool turn;

		public static int LinearIndex(int x, int y)
		{
			int index = x*3+y;

			return index;
		}

		public static Vector2Int CoordinatesFromIndex(int index)
		{
			int x = (int)index / 3;
			int y = index % 3;

			return new Vector2Int(x, y);
		}

		public void ReceiveInfo(Packet.PacketSubType subType, int ID, string[] gameInfo)
		{
			if (subType == Packet.PacketSubType.GAME_START)
			{
				gameID = ID;
				opponent = gameInfo[0];
				updateLabel(opponentLabel, gameInfo[0]);
				updateLabel(gameLog, $"game started with {opponent}");
				gamePlaying = true;
			}

			if (subType == Packet.PacketSubType.GAME_MOVE)
			{
				board = HandleMove(gameInfo[0]);

				if (board != null)
				{
					UpdateBoard();
				}
			}
		}

		int[,] HandleMove(string input)
		{
			string[] coords = input.Replace(record.ToString(), "").Split(field);

			if (coords.Length == 10)
			{
				int[,] boardInfo = new int[3, 3];
				for (int i = 0; i < coords.Length-1; i++)
				{
					int x = (int)i / 3;
					int y = i % 3;
					boardInfo[x, y] = int.Parse(coords[i]);
				}
				return boardInfo;
			}

			return null;
		}
		public void UpdateOpponent()
		{

		}

		public void UpdateBoard()
		{
			for (int i = 0; i < gameButtons.Count; i++)
			{
				int x = (int)i / 3;
				int y = i % 3;

				int player = board[x, y];

				if (player != 0)
				{
					updateButton(gameButtons[i], board[x, y].ToString());
				}
			}
		}

		public void UpdateLog()
		{

		}

		public void ResetBoard()
		{

		}

		public void Move(int x, int y)
		{
			if (!gamePlaying)
			{
				updateLabel(gameLog, "Not currently playing a game");
				//return;
			}

			/*
			if (!turn)
			{
				updateLabel(gameLog, "It isn't your turn!");
				return;
			}
			*/
			// linearIndex
			int linearIndex = LinearIndex(x, y);
			string gameinfo = $"{linearIndex}{Game.record}";
			GamePacket move = new GamePacket(App.tcpClient.clientSocket, Packet.PacketSubType.GAME_MOVE, this.gameID, gameinfo);
			move.Send();
		}
		
		public void updateLabel(Label label, string text)
		{
			
			Application.Current.Dispatcher.Invoke(() =>
			{
				label.Content = text;
			});
		}

		public void updateButton(Button button, string content)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				button.Content = content;
			});
		}
	}
}
