using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatTCP_Client.Events;
using TCPPacket;
using TCPClientSocket;
using TCPClient.Data;
using TCPClient;
using ChatTCP_Client.TicTacToe;

namespace ChatTCP_Client
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		TCPEvent tcpEventHandler;
		Game game;
		public MainWindow()
		{
			InitializeComponent();
			tcpEventHandler = new TCPEvent() { activeText = chatEditBox};

			Packet.PacketReceived += RegisterPacket;
			Client.MessageReceived += MessageEvents;
			Client.GameStateReceived += GameEvents;
			Client.CommandReceived += CommandEvents;

			UsernameLabel.Content = App.tcpClient.clientSocket.username;

			messageInput.Focus();


			game = new Game();

			// Add game buttons to game class
			game.gameButtons.Add(t00);
			game.gameButtons.Add(t01);
			game.gameButtons.Add(t02);
			game.gameButtons.Add(t10);
			game.gameButtons.Add(t11);
			game.gameButtons.Add(t12);
			game.gameButtons.Add(t20);
			game.gameButtons.Add(t21);
			game.gameButtons.Add(t22);

			game.opponentLabel = opponentName;
			game.gameLog = gameLog;
			game.turnLabel = playerTurn;

		}

		public void RegisterPacket(object sender, ClientSocket client)
		{
			Packet packet = (Packet)sender;
			if (packet.packetType == Packet.PacketType.PACKET_MESSAGE)
			{
				//AddToChat(packet.content);
			}
		}

		public void CommandEvents(object sender, CommandPacket packet)
		{
			AddToChat(packet.FormatString());
		}

		public void RegisterMessage(object sender, Message message)
		{
			Packet packet = (Packet)sender;
			if (packet.packetType == Packet.PacketType.PACKET_MESSAGE)
			{
				AddToChat(message.Format());
			}
		}

		private void sendMessage(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				string _message = messageInput.Text;

				HandleInput(messageInput.Text);

				MessagePacket message = new MessagePacket(App.tcpClient.clientSocket, App.tcpClient.clientSocket.userID, _message);

				message.Send();

				messageInput.Text = "";
				messageInput.Focus();
			}
		}


		public void AddToChat(string text)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				chatEditBox.AppendText(text + "\n");
			});
		}

		void HandleInput(string text)
		{
			
		}

		void MessageEvents(object sender, MessagePacket packet)
		{
			AddToChat(packet.FormatMessage());
		}

		char field = (char)31;
		void GameEvents(object sender, GamePacket packet)
		{
			string[] gameBlob = packet.gameInfo.Split(field.ToString());
			game.ReceiveInfo(packet.packetSubType, packet.gameID, gameBlob);
		}

		private void t00_Click(object sender, RoutedEventArgs e)
		{
			GameMove(0, 0);
		}

		private void t01_Click(object sender, RoutedEventArgs e)
		{
			GameMove(0, 1);
		}

		private void t02_Click(object sender, RoutedEventArgs e)
		{
			GameMove(0, 2);
		}

		private void t10_Click(object sender, RoutedEventArgs e)
		{
			GameMove(1, 0);
		}

		private void t11_Click(object sender, RoutedEventArgs e)
		{
			GameMove(1, 1);
		}

		private void t12_Click(object sender, RoutedEventArgs e)
		{
			GameMove(1, 2);
		}

		private void t20_Click(object sender, RoutedEventArgs e)
		{
			GameMove(2, 0);
		}

		private void t21_Click(object sender, RoutedEventArgs e)
		{
			GameMove(2, 1);
		}

		private void t22_Click(object sender, RoutedEventArgs e)
		{
			GameMove(2, 2);
		}

		void GameMove(int x, int y)
		{
			game.Move(x, y);
		}
	}
}
