using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClientSocket;

namespace TCPClient
{
	public class EventManager
	{
		public enum EVENT
		{
			CLIENT_LOGGED_IN,
			CLIENT_DISCONNECTED,
			MESSAGE_RECEIVED,
			MESSAGE_SENT,
			COMMAND_RECEIVED,
			COMMAND_SENT,
			GAME_RECEIVED,
			GAME_SENT,
		}

		public Client parent;

		public event EventHandler<EVENT> managedEvent;

		public EventManager(Client parent)
		{
			this.parent = parent;

			this.SubscribeToEvents();
		}

		void SubscribeToEvents()
		{
			this.parent.clientSocket.ClientAuthorized += ClientAuthorizedEvent;
		}

		void ClientAuthorizedEvent(object sender, ClientSocket.ConnectionState args)
		{

		}
	}
}
