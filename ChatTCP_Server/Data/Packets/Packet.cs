using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatTCP.Data.Client;

namespace ChatTCP.Data.Packets
{
	public class Packet
	{
		Encoding encoding = Encoding.UTF8;
		string content;

		public Packet(Encoding encoding, string content)
		{
			this.encoding = encoding;
			this.content = content;
		}

		public void Send(ClientSocket client)
		{

		}
	}
}
