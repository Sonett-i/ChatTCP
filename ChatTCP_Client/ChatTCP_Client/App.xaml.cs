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
using ChatTCP_Client.Events;

namespace ChatTCP_Client
{

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static User currentUser;
		public static ClientSocket currentClient;
		public static Client tcpClient;// = Client.CreateInstance(ClientConfig.defaultPort, ClientConfig.defaultPort, ClientConfig.defaultServer);

		public static TCPEvent eventHandler;

		public static Output output = new Output();

		public static LoginForm.Login loginForm;

		public enum Screen
		{
			SCREEN_LOGIN,
			SCREEN_MAIN
		}

		public static void NewClient()
		{
			tcpClient = Client.CreateInstance(ClientConfig.defaultPort, ClientConfig.defaultPort, ClientConfig.defaultServer);
		}

		public static Screen currentScreen = Screen.SCREEN_LOGIN;


		public static void StateChanged(object sender, int arg)
		{
			object currentWindow = App.Current.Windows;
		}

		public static void ChangeWindow(Screen screen)
		{

			if (screen == Screen.SCREEN_LOGIN)
			{
				App.currentScreen = Screen.SCREEN_LOGIN;
			}
			else if (screen == Screen.SCREEN_MAIN)
			{
				App.currentScreen = Screen.SCREEN_MAIN;
				//close login form


				Application.Current.Dispatcher.Invoke(() => 
				{
					MainWindow mainWindow = new MainWindow();
					mainWindow.Show();
				});

				Application.Current.Dispatcher.Invoke(() =>
				{
					loginForm.Close();
				});
			}
		}
	}
}
