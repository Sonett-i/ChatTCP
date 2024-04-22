using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libs.Terminal;

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
			LOG_COMMAND
		}
	}
}
