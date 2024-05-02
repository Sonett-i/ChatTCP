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



namespace ChatTCP_Client.LoginForm
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Window
	{
		public Login()
		{
			InitializeComponent();
			App.currentScreen = App.screen.SCREEN_LOGIN;
			App.output.loginOutput = loginResult;

			Connect();

			TCPClient.Data.Packets.Packet.PacketReceived += Output.RegisterMessage;

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
