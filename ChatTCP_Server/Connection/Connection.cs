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
			STATE_CONNECTED,
			STATE_DISCONNECTED,
		}
		//type			  //subtype       //connectionstate
		public readonly static string STATE_CONNECTING = $"%i{Packet.field}%i{Packet.field}%i";
		public readonly static string STATE_AUTHORIZING = $"";
		public readonly static string STATE_AUTHORIZED = $"";

		public readonly static string STATE_CONNECTED = $"";
		public readonly static string STATE_DISCONNECTED = $"";

		public static readonly Dictionary<ConnectionState, string> STATE_DICTIONARY = new Dictionary<ConnectionState, string>()
		{
			[ConnectionState.STATE_DISCONNECTED] = STATE_DISCONNECTED,
			[ConnectionState.STATE_CONNECTED] = STATE_CONNECTED,
			[ConnectionState.STATE_CONNECTING] = STATE_CONNECTING,
			[ConnectionState.STATE_AUTHORIZING] = STATE_AUTHORIZING,
		};

		public static void ConnectionChanged(Client client, ConnectionState connectionState)
		{
			//ConPacket.
		}
	}
}
