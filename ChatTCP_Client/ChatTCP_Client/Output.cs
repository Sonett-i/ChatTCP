using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TCPPacket;

namespace ChatTCP_Client
{
	public class Output
	{
		public Label loginOutput;

		public Label tictacToeOutput;


		public void SetLabel(Label label, string text)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				label.Content = text;
			});
		}

		public void AddToChat(TextBox textBox, string text)
		{
			Application.Current.Dispatcher.Invoke(() => 
			{
				textBox.Text += text;
			});
		}

		public static void RegisterMessage(object sender, TCPClientSocket.ClientSocket client)
		{
			Packet packet = (Packet)sender;
			if (App.currentScreen == App.Screen.SCREEN_LOGIN)
			{
				App.output.SetLabel(App.output.loginOutput, packet.content);
			}
			else
			{

			}
		}
	}
}
