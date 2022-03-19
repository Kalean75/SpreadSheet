using System;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
	class GuiApplicationContext : ApplicationContext
	{
		// Number of open forms
		private int formCount = 0;

		// Singleton ApplicationContext
		private static GuiApplicationContext appContext;

		/// <summary>
		/// Private constructor for singleton pattern
		/// </summary>
		private GuiApplicationContext()
		{
		}

		/// <summary>
		/// Returns the one DemoApplicationContext.
		/// </summary>
		public static GuiApplicationContext getAppContext()
		{
			if (appContext == null)
			{
				appContext = new GuiApplicationContext();
			}
			return appContext;
		}

		/// <summary>
		/// Runs the form
		/// </summary>
		public void RunForm(Form form)
		{
			// One more form is running
			formCount++;

			// When this form closes, we want to find out
			form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

			// Run the form
			form.Show();
		}

	}
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			GuiApplicationContext appContext = GuiApplicationContext.getAppContext();
			appContext.RunForm(new Form1());
			Application.Run(appContext);
		}
	}
}
