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

namespace ChatTCP_Client
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		TCPEvent tcpEventHandler;
		public MainWindow()
		{
			InitializeComponent();
			tcpEventHandler = new TCPEvent() { activeText = chatEditBox};

			Packet.PacketReceived += RegisterPacket;
			Client.MessageReceived += MessageEvents;
			Client.GameStateReceived += GameEvents;

			UsernameLabel.Content = App.tcpClient.clientSocket.username;

			messageInput.Focus();
		}

		public void RegisterPacket(object sender, ClientSocket client)
		{
			Packet packet = (Packet)sender;
			if (packet.packetType == Packet.PacketType.PACKET_MESSAGE)
			{
				//AddToChat(packet.content);
			}
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

		void GameEvents(object sender, GamePacket packet)
		{

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

		}
	}
}
