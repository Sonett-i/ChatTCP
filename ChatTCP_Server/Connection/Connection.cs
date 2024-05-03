using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Client;
using ChatTCP.Data.Packets;

namespace ChatTCP.Connect
{
	public static class Connection
	{
		/*
		 * 			STATE_CONNECTING,
			STATE_AUTHORIZING,
			STATE_CONNECTED,
			STATE_DISCONNECTED,
		*/
		public enum ConnectionState
		{
			STATE_CONNECTING,
			STATE_AUTHORIZING,
			STATE_AUTHORIZED,
			STATE_CONNECTED,
			STATE_DISCONNECTED,
		}
	}
}
