using ChatTCP;
using ChatTCP.Data.Debugging;
using Libs.Terminal;


namespace Program
{
	class Program
	{
		public static Server server = new Server(-1);
		public static int port = 6666;
		// CancelToken required for stopping async Tasks.
		static CancellationTokenSource cts = new CancellationTokenSource();

		static void OnCancelKeypress(object? sender, ConsoleCancelEventArgs e)
		{
			Terminal.Print("Exiting...");

			e.Cancel = true;

			return;
		}

		public static async Task Initialize(CancellationToken cancellationToken)
		{
			Console.CancelKeyPress += OnCancelKeypress;
			server = Server.Instance(port);

			await server.Setup(cancellationToken);
		}

		static async Task Main(string[] args)
		{
			cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			//await Initialize(ct);

			ChatTCP.Data.Debugging.Debugging.Debug();

			return;
		}
	}
}