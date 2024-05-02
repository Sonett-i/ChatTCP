﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTCP.Data.Database
{
	public static class DatabaseConfig
	{
		public static string Host = "localhost";
		public static string User = "server";
		public static string Pwd = "";
		public static string Database = "chatTCP";


		public enum DatabaseTable
		{
			TABLE_USERS,
			TABLE_GAMESCORES
		}

		public static Dictionary<DatabaseTable, string> DatabaseTables = new Dictionary<DatabaseTable, string>()
		{ 
			[DatabaseTable.TABLE_USERS] = "users",
			[DatabaseTable.TABLE_GAMESCORES] = "gamescores"
		};
	}
}