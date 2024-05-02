using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPClient.Data.Packets;
using TCPClient.Messaging;

namespace TCPClient
{
	public partial class Client
	{
		public class Authentication
		{
			public enum AuthState
			{
				AUTH_LOGIN,
				AUTH_REGISTER,
			}

			public Client parent { get; set; }

			public bool Authorize(string username, string password, out string result)
			{
				result = "authorize";
				return false;
			}

			public bool AuthorizeNew(string username, string password, out string result)
			{
				if (username == "")
				{
					result = "Username cannot be blank";
					return false;
				}

				if (password == "")
				{
					result = "Password cannot be blank";
				}

				if (username != "" && password != "")
				{
					string hashedPassword = Hash(password);

					AuthPacket authPacket = new AuthPacket(parent.clientSocket, (int)Packet.PacketSubType.AUTH_REGISTER, -1, username, hashedPassword);
					authPacket.Send();
				}
				result = "new acc";


				//base.clientSocket.socket.Send();

				return false;
			}

			private string Hash(string password)
			{
				string hashedPassword = Atbash(password);

				hashedPassword = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(hashedPassword));

				return hashedPassword;
			}

			private string Atbash(string input)
			{
				string bashed = "";
				for (int i = 0; i < input.Length; i++)
				{
					char c = input[i];
					if (c >= 'a' && c <= 'z')
					{
						c = (char)reverse(c);
					}

					if (c >= 'A' && c <= 'Z')
					{
						c = (char)reverse(c);
					}
					bashed += c;
				}

				return bashed;
			}

			int reverse(char c)
			{
				char min = (c <= 96) ? (char)64 : (char)96;
				return (char)min + (min + 26 - c);
			}
		}
	}
}
