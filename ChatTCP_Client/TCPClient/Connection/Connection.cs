using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient.Connection
{
	public class Connection : Client
	{
		public enum ConnectionState
		{
			STATE_DISCONNECTED,
			STATE_CONNECTING,
			STATE_AUTHORIZING,
			STATE_CONNECTED,
		}

		public static bool ValidPort(int port, out int validPort)
		{
			if (port > 0 && port < 65535)
			{
				validPort = port;
				return true;
			}

			validPort = -1;
			return false;
		}

		public static void Connect(Client client)
		{
			client.ConnectToServer();
		}


	}
}
