using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChatTCP_Client
{
	public class Output
	{
		public Label loginOutput;

		public Label tictacToeOutput;


		public void SetLabel(Label label, string text)
		{
			label.Content = text;
		}

		public void AddToChat(TextBox textBox, string text)
		{
			textBox.Text += text;
		}

		public static void RegisterMessage(object sender, string text)
		{
			if (App.currentScreen == App.screen.SCREEN_LOGIN)
			{
				App.output.SetLabel(App.output.loginOutput, text);
			}
		}
	}
}
