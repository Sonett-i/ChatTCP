using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP;
using TCPClientSocket;
using TCPPacket;
using ChatTCP.Data.Game;

namespace ChatTCP_Server.Data
{
	public static class Commands
	{

		public static char CommandChar = '/';
		public enum ChatCommand
		{
			COMMAND_USERINFO,
			COMMAND_WHO,
			COMMAND_ABOUT,
			COMMAND_WHISPER,
			COMMAND_PROMOTE,
			COMMAND_DEMOTE,
			COMMAND_MODS,
			COMMAND_USERNAME,
			COMMAND_COMMANDS,
			COMMAND_HELP,
			COMMAND_KICK,
			//tictactoe
			COMMAND_INVITE,
			COMMAND_STOPGAME,
			COMMAND_GAMESTATS,
			COMMAND_INVALID
		}

		public static Dictionary<string, ChatCommand> commandStrings = new Dictionary<string, ChatCommand>()
		{
			["userinfo"] = ChatCommand.COMMAND_USERINFO,
			["who"] = ChatCommand.COMMAND_WHO,
			["about"] = ChatCommand.COMMAND_ABOUT,
			["whisper"] = ChatCommand.COMMAND_WHISPER,
			["promote"] = ChatCommand.COMMAND_PROMOTE,
			["demote"] = ChatCommand.COMMAND_DEMOTE,
			["username"] = ChatCommand.COMMAND_USERNAME,
			["kick"] = ChatCommand.COMMAND_KICK,
			["commands"] = ChatCommand.COMMAND_COMMANDS,
			["help"] = ChatCommand.COMMAND_HELP,
			["mods"] = ChatCommand.COMMAND_MODS,
			["invite"] = ChatCommand.COMMAND_INVITE,
			["stop"] = ChatCommand.COMMAND_STOPGAME,
			["stats"] = ChatCommand.COMMAND_GAMESTATS,
		};

		public static Dictionary<ChatCommand, string> commandDetails = new Dictionary<ChatCommand, string>()
		{
			[ChatCommand.COMMAND_USERINFO] = "Prints out users information, IP, SecLvl, username.",
			[ChatCommand.COMMAND_WHO] = "Returns a list of connected users.",
			[ChatCommand.COMMAND_ABOUT] = "Prints information about this chat application.",
			[ChatCommand.COMMAND_WHISPER] = "Message another client directly.",
			[ChatCommand.COMMAND_PROMOTE] = "Promote a user's security rank.",
			[ChatCommand.COMMAND_DEMOTE] = "Demote a user's security rank.",
			[ChatCommand.COMMAND_USERNAME] = "Change your username.",
			[ChatCommand.COMMAND_KICK] = "Kick another user.",
			[ChatCommand.COMMAND_COMMANDS] = "Lists all available commands",
			[ChatCommand.COMMAND_HELP] = "Lists all available commands",
			[ChatCommand.COMMAND_MODS] = "List available moderators",
			[ChatCommand.COMMAND_INVITE] = "Invite player to play tic tac toe",
			[ChatCommand.COMMAND_STOPGAME] = "Stop playing tic tac toe",
			[ChatCommand.COMMAND_GAMESTATS] = "Display your tictactoe game stats",
		};

		public delegate CommandPacket CommandDelegate(CommandPacket message, string[] args);

		// https://stackoverflow.com/questions/21924359/pass-a-dictionary-from-one-function-to-another-function-and-print-it
		public static Dictionary<ChatCommand, CommandDelegate> commands = new Dictionary<ChatCommand, CommandDelegate>()
		{
			[ChatCommand.COMMAND_WHO] = (message, args) => Placeholder(message),
			[ChatCommand.COMMAND_USERNAME] = (message, args) => Placeholder(message, args),
			[ChatCommand.COMMAND_USERINFO] = (message, args) => Placeholder(message, args),
			[ChatCommand.COMMAND_ABOUT] = (message, args) => Placeholder(message),
			[ChatCommand.COMMAND_WHISPER] = (message, args) => Placeholder(message, args),
			[ChatCommand.COMMAND_PROMOTE] = (message, args) => Placeholder(message, args),
			[ChatCommand.COMMAND_DEMOTE] = (message, args) => Placeholder(message, args),
			[ChatCommand.COMMAND_COMMANDS] = (message, args) => Placeholder(message),
			[ChatCommand.COMMAND_HELP] = (message, args) => Placeholder(message),
			[ChatCommand.COMMAND_KICK] = (message, args) => Placeholder(message, args),
			[ChatCommand.COMMAND_MODS] = (message, args) => Placeholder(message),
			[ChatCommand.COMMAND_INVITE] = (message, args) => Invite(message, args),
			[ChatCommand.COMMAND_STOPGAME] = (message, args) => StopGame(message),
			[ChatCommand.COMMAND_GAMESTATS] = (message, args) => GameStats(message),
		};


		// iterates through commandStrings dictionary and returns enum if match.
		public static ChatCommand CommandFromString(string input)
		{
			try
			{
				return commandStrings[input];
			}
			catch
			{
				return ChatCommand.COMMAND_INVALID;
			}

			return ChatCommand.COMMAND_INVALID;
		}

		// splits string using space delimeters and returns string array.
		public static string[] GetCommandArgs(string input)
		{
			input = input.Replace(CommandChar.ToString(), "");
			return input.Split(' ');
		}

		public static CommandPacket HandleCommand(MessagePacket message)
		{
			string[] args = GetCommandArgs(message.message);
			ChatCommand command = CommandFromString(args[0]);


			if (command != ChatCommand.COMMAND_INVALID)
			{
				CommandPacket _command = new CommandPacket(message.clientSocket, message.userID, "");

				commands[command].Invoke(_command, args);

				//Log.Event($"[{message.sender}] used the {commandStrings[command]} command.", Log.LogType.LOG_COMMAND);
			}
			else
			{
				message.content = "Error: Invalid Command";
			}

			return null;
		}

		// Commands
		public static CommandPacket Placeholder(CommandPacket message, string[] args)
		{
			return message;
		}

		public static CommandPacket Placeholder(CommandPacket message)
		{
			return message;
		}

		
		public static CommandPacket Invite(CommandPacket message, string[] args)
		{
			ClientSocket opponent = Server.GetClientSocket(args[1]);

			// if opponent is null, or opponent is the same player as inviter, do not proceed.
			if (opponent == null || opponent.username == message.clientSocket.username)
				return null;

			TicTacToe game = TicTacToe.NewGame(message.clientSocket, opponent);

			if (game != null)
			{
				Server.AddNewGame(game);
				game.Start();
			}

			return message;
		}

		public static CommandPacket StopGame(CommandPacket message)
		{
			return message;
		}

		public static CommandPacket GameStats(CommandPacket message)
		{
			Game.GetStats(message.clientSocket);

			
			return message;
		}
		//void methods for commands

		// Start on tictactoe
	}
}
