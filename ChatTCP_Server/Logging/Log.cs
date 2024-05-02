using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libs.Terminal;
using ChatTCP.Data.Formatting;

namespace ChatTCP.Logging
{
	class Log
	{
		public enum LogType
		{
			LOG_WARNING,
			LOG_ERROR,
			LOG_EVENT,
			LOG_MESSAGE,
			LOG_PACKET,
			LOG_COMMAND
		}

		public static Dictionary<LogType, string> logStrings = new Dictionary<LogType, string>() 
		{ 
			[LogType.LOG_WARNING] = "WARNING",
			[LogType.LOG_ERROR] = "ERROR",
			[LogType.LOG_EVENT] = "EVENT",
			[LogType.LOG_MESSAGE] = "MESSAGE",
			[LogType.LOG_COMMAND] = "COMMAND",
			[LogType.LOG_PACKET] = "PACKET",
		};

		DateTime timestamp;
		string message = string.Empty;
		LogType logType = LogType.LOG_MESSAGE;

		public Log(LogType logType, string message)
		{
			this.logType = logType;
			this.message = message;
			this.timestamp = DateTime.UtcNow;
		}

		public static void Event(LogType logType, string message)
		{
			Log eventLog = new Log(logType, message);

			Terminal.Print(eventLog.ToString());
		}

		public override string ToString()
		{
			return Format.String("%s: [%s]: %s", this.timestamp.ToString(), logStrings[logType], message);
		}
	}
}
