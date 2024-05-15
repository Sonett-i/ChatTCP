using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libs.Formatting;

namespace ChatTCP.Data.Database
{
	// Dictionaries of SQL Queries.
	public static class PreparedStatements
	{
		// SELECT
		public static string SELECT_USER_BY_USERNAME = $"SELECT * FROM {DatabaseConfig.DatabaseTables[DatabaseConfig.DatabaseTable.TABLE_USERS]} WHERE username=\"%s\";";
		public static string SELECT_USER_BY_DISPLAYNAME = $"SELECT * FROM {DatabaseConfig.DatabaseTables[DatabaseConfig.DatabaseTable.TABLE_USERS]} WHERE displayname=\"%s\";";


		public static string SELECT_USER_ALL = $"SELECT * FROM {DatabaseConfig.DatabaseTables[DatabaseConfig.DatabaseTable.TABLE_USERS]};";
		public static string SELECT_COUNT_USERS = $"SELECT COUNT(*) FROM {DatabaseConfig.DatabaseTables[DatabaseConfig.DatabaseTable.TABLE_USERS]};";
		public static string SELECT_USER_SCORES = $"SELECT * FROM {DatabaseConfig.DatabaseTables[DatabaseConfig.DatabaseTable.TABLE_GAMESCORES]} WHERE user = %b;";
		// INSERT
		public static string INSERT_NEW_USER = $"INSERT INTO {DatabaseConfig.DatabaseTables[DatabaseConfig.DatabaseTable.TABLE_USERS]}(`userID`, `username`, `password`, `displayname`, `role`) VALUES (%b, \"%s\", \"%s\", \"%s\", %b);";
		public static string INSERT_NEW_USER_SCORES = $"INSERT INTO {DatabaseConfig.DatabaseTables[DatabaseConfig.DatabaseTable.TABLE_GAMESCORES]}(`user`, `wins`, `losses`, `draws`) VALUES (%b, \"%i\", \"%i\", %i);";

		// UPDATE
		
		public static string UPDATE_USER_STATS = $"UPDATE {DatabaseConfig.DatabaseTables[DatabaseConfig.DatabaseTable.TABLE_GAMESCORES]} SET `wins`=%i,`losses`=%i,`draws`=%i WHERE `user`=%b;";
		public static string UPDATE_USER_DISPLAYNAME = $"UPDATE {DatabaseConfig.DatabaseTables[DatabaseConfig.DatabaseTable.TABLE_USERS]} SET `displayname`=%s WHERE `userID`=%b;";
		

		public static Query GetQuery(string query, params object[] args)
		{
			Query sqlQuery = new Query(Format.String(query, args));

			return sqlQuery;
		}

		// Creation Queries

		// Insert Queries

		// Select Queries
		//

		// Update Queries
	}

	public class Query
	{
		public string command;

		public Query(string command)
		{
			this.command = command;
		}

		public void Post()
		{

		}
	}
}
