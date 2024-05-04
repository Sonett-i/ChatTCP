using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;

namespace TicTacToe
{
	internal class Player
	{
		ClientSocket clientSocket;
		string name;
		int wins;
		int losses;
		int draws;

		public Player(ClientSocket clientSocket, string name)
		{
			this.clientSocket = clientSocket;
			this.name = name;
		}
	}
}
