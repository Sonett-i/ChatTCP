using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP;
using TCPClientSocket;
using TCPPacket;
using ChatTCP.Data.Game;
using ChatTCP.Data.Client;
using ChatTCP.Data.Database;

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
			//	tictactoe
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
			["changename"] = ChatCommand.COMMAND_USERNAME,
			["kick"] = ChatCommand.COMMAND_KICK,
			["commands"] = ChatCommand.COMMAND_COMMANDS,
			["help"] = ChatCommand.COMMAND_HELP,
			["mods"] = ChatCommand.COMMAND_MODS,
			["play"] = ChatCommand.COMMAND_INVITE,
			["stop"] = ChatCommand.COMMAND_STOPGAME,
			["stats"] = ChatCommand.COMMAND_GAMESTATS,
		};

		static string GetKey(ChatCommand command)
		{
			return commandStrings.FirstOrDefault(x => x.Value == command).Key;
		}

		public static Dictionary<ChatCommand, string> commandDetails = new Dictionary<ChatCommand, string>()
		{
			[ChatCommand.COMMAND_USERINFO] = "Prints out users information, IP, SecLvl, username.",
			[ChatCommand.COMMAND_WHO] = "Returns a list of connected users.",
			[ChatCommand.COMMAND_ABOUT] = "Prints information about this chat application.",
			[ChatCommand.COMMAND_WHISPER] = "Message another client directly.",
			[ChatCommand.COMMAND_PROMOTE] = "Promote a user's security rank.",
			[ChatCommand.COMMAND_DEMOTE] = "Demote a user's security rank.",
			[ChatCommand.COMMAND_USERNAME] = "Change your display name.",
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
			[ChatCommand.COMMAND_WHO] = (message, args) => Who(message),
			[ChatCommand.COMMAND_USERNAME] = (message, args) => ChangeName(message, args),
			[ChatCommand.COMMAND_USERINFO] = (message, args) => UserInfo(message, args),
			[ChatCommand.COMMAND_ABOUT] = (message, args) => About(message, args),
			[ChatCommand.COMMAND_WHISPER] = (message, args) => Whisper(message, args),
			[ChatCommand.COMMAND_PROMOTE] = (message, args) => Promote(message, args),
			[ChatCommand.COMMAND_DEMOTE] = (message, args) => Demote(message, args),
			[ChatCommand.COMMAND_COMMANDS] = (message, args) => Help(message),
			[ChatCommand.COMMAND_HELP] = (message, args) => Help(message),
			[ChatCommand.COMMAND_KICK] = (message, args) => Kick(message, args),
			[ChatCommand.COMMAND_MODS] = (message, args) => Mods(message),
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

		public static CommandPacket About(CommandPacket message, string[] args)
		{
			if (args[1] == "1")
			{
				Server.Joined(message.clientSocket);
			}
			string result = $"Welcome to ChatTCP, {message.clientSocket.username}. For help, type {Commands.CommandChar}help";
			CommandPacket.Send(message.clientSocket, result);
			return message;
		}

		public static CommandPacket Help(CommandPacket message)
		{
			string result = $"Help\n";

			foreach (ChatCommand command in Enum.GetValues(typeof(ChatCommand)))
			{
				if (command != ChatCommand.COMMAND_INVALID)
				{
					result += $"\t{CommandChar}{GetKey(command)} - {commandDetails[command]}\n";
				}
			};

			CommandPacket.Send(message.clientSocket, result);
			return message;
		}

		public static CommandPacket Who(CommandPacket message)
		{
			string result = "Who";
			foreach(Client client in Server.ConnectedClients)
			{
				result += $"\n\t{client.clientSocket.displayName}";
			}

			CommandPacket.Send(message.clientSocket, result);
			return message;
		}


		public static CommandPacket UserInfo(CommandPacket message, string[] args)
		{
			string result = "Invalid User";
			string arg = message.clientSocket.displayName;

			if (args.Length > 1)
			{
				arg = args[1];
			}

			Query userQuery = PreparedStatements.GetQuery(PreparedStatements.SELECT_USER_BY_DISPLAYNAME, arg);
			object[][] dbQuery = Server.database.Query(userQuery);

			if (dbQuery.Length > 0)
			{
				result = $"User info for {arg}:" +
					$"\n\tUserID: {dbQuery[0][0]}" +
					$"\n\tUsername: {dbQuery[0][1]}" +
					$"\n\tDisplay Name: {dbQuery[0][3]}" +
					$"\n\tSecLevel: {dbQuery[0][4]}";
			}

			CommandPacket.Send(message.clientSocket, result);

			return message;
		}

		public static CommandPacket Whisper(CommandPacket message, string[] args)
		{
			string recipient = args[1];

			
			ClientSocket client = Server.GetClientSocket(recipient);

			if (client == null)
			{
				CommandPacket.Send(message.clientSocket, "User " + recipient + "is not available");
			}
			else
			{
				string whisperMessage = "";

				for (int i = 2; i < args.Length; i++)
				{
					whisperMessage += args[i] + " ";
				}

				CommandPacket.Send(client, $"Whisper from [{message.clientSocket.displayName}]: {whisperMessage}");
				CommandPacket.Send(message.clientSocket, $"Whisper to [{client.displayName}]: {whisperMessage}");
			}

			return message;
		}

		public static CommandPacket Promote(CommandPacket message, string[] args)
		{
			if (message.clientSocket.secLevel < 2)
			{
				CommandPacket.Send(message.clientSocket, $"You do not have permission to do this.");
				return message;
			}

			Query selectUser = PreparedStatements.GetQuery(PreparedStatements.SELECT_USER_BY_USERNAME, args[1]); // get update query
			object[][] selectResult = Server.database.Query(selectUser);

			if (selectResult.Length > 0)
			{
				short secLevel = (short)selectResult[0][4];
				secLevel++;

				int userID = (int)selectResult[0][0];

				if (secLevel <= 2)
				{
					Query updateUser = PreparedStatements.GetQuery(PreparedStatements.UPDATE_SET_SECLVL, (Int16)secLevel, (Int64)userID);
					Server.database.Query(updateUser);
					CommandPacket.Send(message.clientSocket, $"Promoted {selectResult[0][1]} to secLvl {secLevel}");
				}
				else
				{
					CommandPacket.Send(message.clientSocket, selectResult[0][1] + " is already at the maximum security level.");
				}
			}
			else
			{
				CommandPacket.Send(message.clientSocket, "Invalid user");
			}

			return message;
		}

		public static CommandPacket Demote(CommandPacket message, string[] args)
		{
			if (message.clientSocket.secLevel < 2)
			{
				CommandPacket.Send(message.clientSocket, $"You do not have permission to do this.");
				return message;
			}

			Query selectUser = PreparedStatements.GetQuery(PreparedStatements.SELECT_USER_BY_USERNAME, args[1]); // get update query
			object[][] selectResult = Server.database.Query(selectUser);

			if (selectResult.Length > 0)
			{
				short secLevel = (short)selectResult[0][4];
				secLevel--;

				int userID = (int)selectResult[0][0];

				if (secLevel >= 1)
				{
					Query updateUser = PreparedStatements.GetQuery(PreparedStatements.UPDATE_SET_SECLVL, (Int16)secLevel, (Int64)userID);
					Server.database.Query(updateUser);
					CommandPacket.Send(message.clientSocket, $"Demoted {selectResult[0][1]} to secLvl {secLevel}");
				}
				else
				{
					CommandPacket.Send(message.clientSocket, selectResult[0][1] + " is already at the minimum security level.");
				}
			}
			else
			{
				CommandPacket.Send(message.clientSocket, "Invalid user");
			}

			return message;
		}

		public static CommandPacket Kick(CommandPacket message, string[] args)
		{
			if (message.clientSocket.secLevel < 2)
			{
				CommandPacket.Send(message.clientSocket, $"You do not have permission to do this.");
				return message;
			}

			Client client = Server.GetClient(Server.GetClientSocket(args[1]));

			if (client != null)
			{
				CommandPacket.Send(client.clientSocket, $"You have been kicked from the server.");
				CommandPacket.Send(message.clientSocket, $"{client.clientSocket.displayName} has been kicked from the server.");
				client.clientSocket.isActive = false;
				client.clientSocket.socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
			}
			else
			{
				CommandPacket.Send(message.clientSocket, $"Invalid user.");
			}

			return message;
		}

		public static CommandPacket Mods(CommandPacket message)
		{
			List<string> mods = new List<string>();

			foreach (Client client in Server.ConnectedClients)
			{
				if (client.clientSocket.secLevel > 1)
				{
					mods.Add(client.clientSocket.displayName);
				}
			}

			string result = $"There are {mods.Count} moderators online{((mods.Count > 0) ? ':' : '.')}";

			foreach (string mod in mods)
			{
				result += "\n\t" + mod;
			}

			CommandPacket.Send(message.clientSocket, result);

			return message;
		}

		public static CommandPacket ChangeName(CommandPacket message, string[] args)
		{
			string result = "";

			string oldName = message.clientSocket.displayName;
			string newName = args[1];

			Query statQuery = PreparedStatements.GetQuery(PreparedStatements.UPDATE_USER_DISPLAYNAME, newName, (Int64)message.clientSocket.userID);
			Server.database.Query(statQuery);

			message.clientSocket.displayName = newName;
			AuthPacket.Send(message.clientSocket, Packet.PacketSubType.AUTH_UPDATE, message.clientSocket.username, "");

			Server.Broadcast(message, message.clientSocket, $"{oldName} has changed their name to {newName}");
			CommandPacket.Send(message.clientSocket, "Changed display name to: " + newName);
			return message;
		}
		public static CommandPacket Invite(CommandPacket message, string[] args)
		{
			ClientSocket opponent = Server.GetClientSocket(args[1]);

			// if opponent is null, or opponent is the same player as inviter, do not proceed.
			if (opponent == null)
			{
				CommandPacket.Send(message.clientSocket, "Invalid Opponent");
				return null;
			}

			if (opponent.username == message.clientSocket.username)
			{
				CommandPacket.Send(message.clientSocket, "You can't invite yourself!");
				return null;
			}
				

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
			string[] stats = Game.GetStats(message.clientSocket);

			string result = $"Game Stats\nWins: {stats[0]}\nLosses: {stats[1]}\nDraws: {stats[2]}{Packet.record}";

			CommandPacket.Send(message.clientSocket, result);
			return message;
		}
	}
}
