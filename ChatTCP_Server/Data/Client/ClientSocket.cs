﻿using System.Net;
using System.Net.Sockets;
using ChatTCP.Connect;
using ChatTCP.Data.Packets;

namespace ChatTCP.Data.Client
{
	public class ClientSocket
	{
		// Encoding - do not alter


		public Socket? socket;
		public const int BUFFER_SIZE = 2048;
		public byte[] buffer = new byte[BUFFER_SIZE];

		public int userID;

		public IPAddress? IP;
		public bool authorized = false;

		public Connection.ConnectionState connectionState = Connection.ConnectionState.STATE_CONNECTING;

		public EndPoint GetIP()
		{
			return socket.RemoteEndPoint;
		}

		public void ChangeConnectionState(Connection.ConnectionState connectionState, bool authorized = false)
		{
			this.connectionState = connectionState;
			this.authorized = (authorized == true) ? authorized : this.authorized;

			ConPacket.Send(this, this.connectionState);
		}
	}
}
