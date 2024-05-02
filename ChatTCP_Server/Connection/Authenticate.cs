using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Database;
using ChatTCP.Data.Client;
using ChatTCP.Data.Packets;
using ChatTCP.Data.Formatting;
using ChatTCP.Logging;

namespace ChatTCP.Connection
{
	public static class Authenticate
	{
		public static bool UserExists(string username)
		{
			Query selectQuery = new Query(Format.String(PreparedStatements.SELECT_USER, username));
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

		public static bool RegisterNewUser(Client client, AuthPacket packet, out string result)
		{
			// now i get to do the database stuff
			if (UserExists(packet.username))
			{
				result = "user exists, not registered.";
				return false;
			}

			Int64 newID = GetUnusedID();

			if (newID < 1)
			{
				result = "invalid ID";
				return false;
			}

			Query insertQuery = new Query(Format.String(PreparedStatements.INSERT_NEW_USER, newID, packet.username, packet.password, (Int64)1));

			Log.Event(Log.LogType.LOG_EVENT, $"{client.clientSocket.socket.RemoteEndPoint.ToString()} registered as new user: {packet.username}");
			object[] insertResult = Server.database.Query(insertQuery);

			result = "user not exist";

			return false;
		}
		public static Client Client(Client client, AuthPacket packet, out string result)
		{
			result = "";
			if (packet.packetSubType == Packet.PacketSubType.AUTH_REGISTER)
			{
				RegisterNewUser(client, packet, out string registerResult);
				result = registerResult;
			}

			return client;
		}
	}
}
