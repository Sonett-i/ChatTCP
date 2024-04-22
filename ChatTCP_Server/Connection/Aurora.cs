using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatTCP.Connection
{
	internal class Aurora
	{
		public enum ConnectionState
		{
			STATE_CONNECTING,
			STATE_AUTHORIZING,
			STATE_CONNECTED,
		}
		// Client Connection Protocol

		public static Task<Socket> AcceptConnection(Server server, CancellationToken cancellationToken)
		{
			TaskCompletionSource<Socket> tcs = new TaskCompletionSource<Socket>();

			cancellationToken.Register(() =>
			{
				tcs.TrySetCanceled();
			});

			server.serverSocket.BeginAccept(asyncResult => 
			{ 
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}

				try
				{
					Socket socket = server.serverSocket.EndAccept(asyncResult);
					tcs.TrySetResult(socket);
				}
				catch (Exception ex)
				{
					tcs.TrySetException(ex);
				}
			}, null);

			return tcs.Task;
		}
	}
}
