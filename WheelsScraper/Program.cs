using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace WheelsScraper
{
	static class Program
	{

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger("");

		public static log4net.ILog Log
		{
			get { return Program.log; }
		}

		public static bool ChangeLogFileName(string appenderName, string newFilename)
		{
			var rootRepository = log4net.LogManager.GetRepository();
			foreach (var appender in rootRepository.GetAppenders())
			{
				if (appender.Name.Equals(appenderName) && appender is log4net.Appender.FileAppender)
				{
					var fileAppender = appender as log4net.Appender.FileAppender;
					fileAppender.File += "_" + newFilename;
					fileAppender.ActivateOptions();
					return true;  // Appender found and name changed to NewFilename
				}
			}
			return false; // appender not found
		}
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			ChangeLogFileName("RollingLogFileAppender", Guid.NewGuid().ToString());

			//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
			//ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			DevExpress.Skins.SkinLoader.RegisterSkins(DevExpress.Skins.SkinLoader.DevExpressSkins.Bonus);
			//DevExpress.Skins.SkinLoader.RegisterSkins(DevExpress.Skins.SkinLoader.DevExpressSkins.Office);
			DevExpress.Skins.SkinManager.EnableFormSkins();
			//var curConfig = System.Reflection.Assembly.GetExecutingAssembly().Location + ".config";
			//var curConfig = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
			//MessageBox.Show(curConfig);
			//AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
			if (!Properties.Settings.Default.DisableAutoupdate)
			{
				Updater.CheckForUpdates(true);
				//	AutoupdateForm frm = new AutoupdateForm();
				//	frm.ShowDialog();
				//	if (frm.NeedRestart)
				//	{
				//		Application.Restart();
				//		return;
				//	}
				//}
			}
			Application.Run(new Form1());
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			CreateDump(e.ExceptionObject);
			Program.Log.Error(e.ExceptionObject);
			MessagesPanelManager.PrintMessage(e.ExceptionObject.ToString(), ImportanceLevel.High);
		}

		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			CreateDump(e.Exception);
			Program.Log.Error(e.Exception);
			MessagesPanelManager.PrintMessage(e.Exception.Message, ImportanceLevel.High);
		}


		static void CreateDump(object err)
		{
			var settingsDir = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "EDF");
			var dumpFile = Path.Combine(settingsDir, "DUMP_"+Path.GetRandomFileName());
			Directory.CreateDirectory(settingsDir);
			using (var sw = new StreamWriter(dumpFile))
			{
				sw.WriteLine(err.ToString());
			}
		}
	}
}