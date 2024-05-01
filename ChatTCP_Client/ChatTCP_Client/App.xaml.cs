using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TCPClient;
using TCPClient.Data;
using TCPClient.Data.Sockets;

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

	}
}
