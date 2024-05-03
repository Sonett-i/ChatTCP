using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Client;
using ChatTCP.Data.Formatting;

namespace ChatTCP.Data.Packets
{
	public partial class CommandPacket : Packet
	{
		public CommandPacket(ClientSocket clientSocket, int sender, string message) : base(clientSocket, sender)
		{

		}
	}
}
