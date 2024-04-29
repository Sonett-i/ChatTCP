using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ChatTCP.Data;
using Libs.Terminal;

namespace ChatTCP.Data.Database
{
	internal class Database
	{
		public string connectionString = "";

		MySqlConnection connection = new MySqlConnection();

		string server;
		string uid;
		string password;
		string database;
		public Database(string server, string uid, string password, string database)
		{
			this.server = server;
			this.uid = uid;
			this.password = password;
			this.database = database;

			connection = new MySqlConnection();
			connection.ConnectionString = Data.Formatting.Format.String("server=%s;uid=%s;pwd=%s;database=%s", this.server, this.uid, this.password, this.database);
			this.Connect();
		}

		public void Connect()
		{
			try
			{
				connection.Open();
			}
			catch (Exception ex)
			{
				Terminal.Print(ex);
			}
		}
		public static Database Create(string server, string uid, string password, string database)
		{
			Database db = new Database(server, uid, password, database);

			return db;
		}

		public void Query(string query)
		{
			string sqlQuery = "SELECT * FROM users;";

			MySqlCommand command = new MySqlCommand(sqlQuery, this.connection);
			MySqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				Terminal.Print(reader[0] + " " + reader[1]);
			}
		}
	}
}
