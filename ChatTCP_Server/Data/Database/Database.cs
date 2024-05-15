using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ChatTCP.Data;
using Libs.Terminal;
using Libs.Formatting;

namespace ChatTCP.Data.Database
{
	public class Database
	{
		public string connectionString = "";

		// connection factory for MySQL server.
		MySqlConnection connection = new MySqlConnection();

		public string host;
		public string uid;
		public string password;
		public string database;
		public Database(string server, string uid, string password, string database)
		{
			this.host = server;
			this.uid = uid;
			this.password = password;
			this.database = database;

			connection = new MySqlConnection();
			connection.ConnectionString = Format.String("server=%s;uid=%s;pwd=%s;database=%s", this.host, this.uid, this.password, this.database);
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
			using (MySqlCommand command = new MySqlCommand(query, this.connection))
			{
				using (MySqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{

					}
				}
			}
		}

		// Queries the database usinf input query, and returns an 2-Dimensional object of the retrieved data.
		public object[][] Query(Query query)
		{
			List<object[]> results = new List<object[]>();

			using (MySqlCommand command = new MySqlCommand(query.command, this.connection))
			{
				using (MySqlDataReader reader = command.ExecuteReader())
				{
					// for each row read:
					while (reader.Read())
					{
						object[] record = new object[reader.FieldCount];

						// Iterate through each records' heading and add to object record.
						for (int i = 0; i < reader.FieldCount; i++)
						{
							record[i] = reader[i];
						}

						results.Add(record);
					}
				}
			}

			return results.ToArray();
		}

		// Tests if query returns information or not.
		public bool Test(string query)
		{
			using (MySqlCommand command = new MySqlCommand(query, this.connection))
			{
				try
				{
					using (MySqlDataReader reader = command.ExecuteReader())
					{
						if (reader.HasRows)
							return true;

					}
				}
				catch (Exception ex)
				{

				}

			}

			return false;
		}
	}
}
