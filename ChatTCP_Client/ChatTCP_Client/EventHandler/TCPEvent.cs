using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPPacket;
using TCPClientSocket;
using System.Windows.Controls;

namespace ChatTCP_Client.Events
{
	public class TCPEvent
	{
		public TextBox activeText;
		public Label activeLabel;

		public TCPEvent()
		{
			
		}

		public void RegisterMessage(object sender, ClientSocket client)
		{
			Packet packet = (Packet)sender;

			if (App.currentScreen == App.Screen.SCREEN_LOGIN)
			{
				App.output.SetLabel(activeLabel, packet.content);
			}
			else
			{
				App.output.AddToChat(activeText, packet.content);
			}
		}

		public void LoginStateChanged(object sender, ClientSocket.ConnectionState state)
		{
			if (state == ClientSocket.ConnectionState.STATE_AUTHORIZED)
			{
				App.ChangeWindow(App.Screen.SCREEN_MAIN);
			}
		}

		public void Subscribe()
		{

		}
	}
}
