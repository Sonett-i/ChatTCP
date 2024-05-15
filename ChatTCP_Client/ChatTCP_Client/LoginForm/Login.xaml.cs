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
using System.Windows.Shapes;
using TCPPacket;
using ChatTCP_Client.Events;
using TCPClient;
using TCPClientSocket;



namespace ChatTCP_Client.LoginForm
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Window
	{
		TCPEvent eventHandler;
		public Login()
		{
			InitializeComponent();

			App.loginForm = this;
			App.currentScreen = App.Screen.SCREEN_LOGIN;
			App.output.loginOutput = loginResult;

			App.NewClient();

			eventHandler = new TCPEvent() { activeLabel = loginResult };

			Connect();
			//Packet.PacketReceived += RegisterMessage;
			App.tcpClient.clientSocket.ClientAuthorized += ConState;

			Client.CommandReceived += AuthUpdate;
			//App.tcpClient.clientSocket.ConnectionStateChanged += ChatTCP_Client.App.
		}

		public void ConState(object sender, ClientSocket.ConnectionState state)
		{
			if (state == ClientSocket.ConnectionState.STATE_AUTHORIZED)
			{
				App.ChangeWindow(App.Screen.SCREEN_MAIN);
			}
		}

		void AuthUpdate(object sender, CommandPacket packet)
		{
			App.output.SetLabel(loginResult, packet.result);
		}

		public void RegisterMessage(object sender, ClientSocket client)
		{
			Packet packet = (Packet)sender;

			App.output.SetLabel(loginResult, packet.content);

		}

		void Connect()
		{
			//App.tcpClient = TCPClient.Client.CreateInstance(TCPClient.ClientConfig.defaultPort, TCPClient.ClientConfig.defaultPort, TCPClient.ClientConfig.defaultServer);

			loginResult.Content = "Connecting...";
			try
			{
				TCPClient.Connection.Connection.Connect(App.tcpClient);
			}
			catch (Exception ex)
			{
				// :(
				loginResult.Content = "Connection to server failed";
			}

			if (App.tcpClient.socket.Connected)
			{
				loginResult.Content = "Please log in";
			}
			else
			{
				loginResult.Content = "Connection to server failed";
			}
		}

		private void loginButton_Click(object sender, RoutedEventArgs e)
		{
			if (!App.tcpClient.socket.Connected)
				Connect();

			string username = usernameInput.Text;
			string password = passwordInput.Text;



			string authResult;
			App.tcpClient.Authenticator.Authorize(username, password, out authResult);
			//Authorize(username, password);

			loginResult.Content = authResult;
		}

		private void registerButton_Click(object sender, RoutedEventArgs e)
		{
			if (!App.tcpClient.socket.Connected)
				Connect();

			string username = usernameInput.Text;
			string password = passwordInput.Text;

			string authResult;
			App.tcpClient.Authenticator.AuthorizeNew(username, password, out authResult);

			loginResult.Content = authResult;
		}
	}
}
