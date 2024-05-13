using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;

namespace ChatTCP_Server.Data.Game
{
	internal class TicTacToe
	{
		public enum Check
		{
			Cross,
			Nought,
		}

		ClientSocket playerA;
		ClientSocket playerB;

		int[,] board = new int[3, 3];

		public TicTacToe(ClientSocket playerA, ClientSocket playerB, int[,] board)
		{
			this.playerA = playerA;
			this.playerB = playerB;
			this.board = board;
		}

		public void Move(int x, int y)
		{

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
	}
}
