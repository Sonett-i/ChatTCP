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
			Message.messageReceived += RegisterMessage;
		}

		public void RegisterPacket(object sender, ClientSocket client)
		{
			Packet packet = (Packet)sender;
			if (packet.packetType == Packet.PacketType.PACKET_MESSAGE)
			{
				AddToChat(packet.content);
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

				MessagePacket message = new MessagePacket(App.tcpClient.clientSocket, App.tcpClient.clientSocket.userID, _message);

				message.Send();
			}
		}


		public void AddToChat(string text)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				chatEditBox.AppendText("\n" + text);
			});
		}
	}
}
