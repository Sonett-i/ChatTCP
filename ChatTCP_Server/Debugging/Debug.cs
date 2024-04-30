﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP;
using Libs.Terminal;
using ChatTCP.Data;
using ChatTCP.Data.Database;

namespace ChatTCP.Data.Debugging
{
	public static class Debugging
	{
		public static void Debug()
		{
			//Terminal.Print(Formatting.Format.String("Hi, my name is %s and I am %i years old.", "Sam", 28));

			Database.Database db = Database.Database.Create("localhost", "chatTCP", "", "chatTCP");

			db.Query("SELECT * FROM users");
		}
	}
}
