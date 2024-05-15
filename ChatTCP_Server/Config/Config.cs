using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTCP.Config
{
	public static class ServerConfig
	{
		public static int defaultPort = 6666;

		public static string defaultMOTD = "Welcome to ChatTCP";


		// About
		public static string[] asciiart = 
		{
			"   _____ _           _     _______ _____ _____  ",
			"  / ____| |         | |   |__   __/ ____|  __ \\ ",
			" | |    | |__   __ _| |_     | | | |    | |__) |",
			" | |    | '_ \\ / _` | __|    | | | |    |  ___/ ",
			" | |____| | | | (_| | |_     | | | |____| |     ",
			"  \\_____|_| |_|\\__,_|\\__|    |_|  \\_____|_|     ",
		};

		public static string author = "Sam C";
		public static string date = "May 15, 2024";

		public static string version = "2.0.0";
		public static string printLogo()
		{
			string output = "";

			for (int i = 0; i < asciiart.Length; i++)
			{
				output += asciiart[i] + "\n";
			}
			return output;
		}

		public static string PrintAbout()
		{
			string output = "";

			output += printLogo();

			output += $"\nAuthor: {author}\nDate: {date}\nVersion: {version}\n";

			return output;
		}
	}
}
