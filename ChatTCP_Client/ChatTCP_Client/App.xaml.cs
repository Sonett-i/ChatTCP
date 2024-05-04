using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TCPClient;
using TCPClient.Data;
using TCPClientSocket;

namespace ChatTCP_Client
{

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static User currentUser;
		public static ClientSocket currentClient;
		public static Client tcpClient = Client.CreateInstance(ClientConfig.defaultPort, ClientConfig.defaultPort, ClientConfig.defaultServer);

		

		public static Output output = new Output();

		public enum screen
		{
			SCREEN_LOGIN,
			SCREEN_MAIN
		}

		public static screen currentScreen = screen.SCREEN_LOGIN;


		public static void StateChanged(object sender, int arg)
		{
			object currentWindow = App.Current.Windows;
		}

		public void ChangeWindow()
		{
			MainWindow.Show();
		}
	}
}
