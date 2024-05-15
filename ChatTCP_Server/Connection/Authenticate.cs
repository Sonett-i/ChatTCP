using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Database;
using ChatTCP.Data.Client;
using ChatTCP.Logging;
using TCPPacket;
using TCPClientSocket;
using Libs.Formatting;

namespace ChatTCP.Connect
{
	public static class Authenticate
	{
		public static bool UserExists(string username)
		{
			Query selectQuery = new Query(Format.String(PreparedStatements.SELECT_USER_BY_USERNAME, username));
			object[] result = Server.database.Query(selectQuery);

			return result.Length > 0;
		}

		public static Int64 GetUnusedID()
		{
			Query selectQuery = new Query(PreparedStatements.SELECT_COUNT_USERS);

			object[][] result = Server.database.Query(selectQuery);

			if (result.Length > 0)
			{
				return (Int64)result[0][0] + 1;
			}

			return 0;
		}
		public static bool AuthenticateUser(Client client, AuthPacket packet, out string result)
		{
			result = "";
			if (!UserExists(packet.username))
			{
				result = "User does not exist";
				CommandPacket.Send(client.clientSocket, "Invalid Username");
				return false;
			}

			// is user already logged in?
			ClientSocket existing = Server.GetClientSocket(packet.username);

			if (existing != null)
			{
				result = "user already logged in";
				CommandPacket.Send(client.clientSocket, "User already logged in");

				return false;
			}

			Query selectQuery = new Query(Format.String(PreparedStatements.SELECT_USER_BY_USERNAME, packet.username));

			object[][] userData = Server.database.Query(selectQuery);

			if (userData.Length > 0)
			{
				string password = (string)userData[0][2];

				if (password == packet.password)
				{
					client.ID = (Int32) userData[0][0];
					client.username = (string)userData[0][1];
					client.displayName = (string)userData[0][3];
					client.secLevel = (Int16) userData[0][4];
					result = $"{client.username}";
					client.clientSocket.username = client.username;
					client.clientSocket.displayName = client.displayName;
					client.clientSocket.userID = client.ID;
					client.clientSocket.secLevel = client.secLevel;
					return true;
				}
				else
				{
					CommandPacket.Send(client.clientSocket, "Incorrect password");
				}
			}

			return false;
		}
		public static bool RegisterNewUser(Client client, AuthPacket packet, out string result)
		{
			// now i get to do the database stuff
			if (UserExists(packet.username))
			{
				result = "Registration failed: user exists.";
				CommandPacket.Send(client.clientSocket, $"User {packet.username} already exists");
				return false;
			}

			Int64 newID = GetUnusedID();

			if (newID < 1)
			{
				result = "Registration failed: invalid ID";
				CommandPacket.Send(client.clientSocket, "Invalid ID");
				return false;
			}

			int secLevel = 1;

			if (ChatTCP.Config.ServerConfig.isAdmin(packet.username))
			{
				secLevel = 2;
			}
			
			Query insertQuery = new Query(Format.String(PreparedStatements.INSERT_NEW_USER, newID, packet.username, packet.password, packet.username, (Int64)secLevel));
			Query playerScoresQuery = new Query(Format.String(PreparedStatements.INSERT_NEW_USER_SCORES, newID, 0, 0, 0));

			Log.Event(Log.LogType.LOG_EVENT, $"{client.clientSocket.socket.RemoteEndPoint.ToString()} registered as new user: {packet.username}");
			CommandPacket.Send(client.clientSocket, "Registered new account with username: " + packet.username);
			
			Server.database.Query(insertQuery);
			Server.database.Query(playerScoresQuery);
			result = "Registered new user: " + packet.username;

			return false;
		}

		public static Client Client(Client client, AuthPacket packet, out string result)
		{
			result = "";
			if (packet.packetSubType == Packet.PacketSubType.AUTH_REGISTER)
			{
				bool newUser = RegisterNewUser(client, packet, out string registerResult);
				result = registerResult;

				if (!newUser)
				{
					AckPacket.Send(client.clientSocket, Packet.PacketSubType.ACK_NAK, result);
				}
				else
				{
					AckPacket.Send(client.clientSocket, Packet.PacketSubType.ACK_ACK, result);
				}
			}
			else if (packet.packetSubType == Packet.PacketSubType.AUTH_AUTHORIZE)
			{
				bool validUser = AuthenticateUser(client, packet, out string authResult);

				if (!validUser)
				{
					AckPacket.Send(client.clientSocket, Packet.PacketSubType.ACK_NAK, authResult);
				}
				else
				{
					//ConnectionPacket.Send(client.clientSocket, client.username, ClientSocket.ConnectionState.STATE_AUTHORIZED);
					AuthPacket.Send(client.clientSocket, Packet.PacketSubType.AUTH_AUTHORIZE, client.username, "");
					client.clientSocket.SetConnectionState(ClientSocket.ConnectionState.STATE_AUTHORIZED);
				}
			}

			return client;
		}
	}
}
