using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public static class MessagesPanelManager
	{
		private static Form1 _mainForm;

		//private delegate void run();

		public static void Init(Form1 mainForm)
		{
			_mainForm = mainForm;
		}

		public static void PrintMessage(string appName, string description, ImportanceLevel impLevel)
		{
			if (impLevel == ImportanceLevel.High || impLevel == ImportanceLevel.Critical)
				Program.Log.ErrorFormat("{0}. {1} ", appName, description);
			else
				Program.Log.InfoFormat("{0}. {1} ", appName, description);
			var msg = new InnerMessage() { Importance = (int)impLevel, Description = description, Time = DateTime.Now, AppName = appName };
			if (_mainForm != null)
			{
				if (_mainForm.InvokeRequired)
				{
					_mainForm.Invoke(new Action(() => _mainForm.Messages.Add(msg)));
				}
				else
				{
					_mainForm.Messages.Add(msg);
				}
			}
		}

		public static void PrintMessage(string description, ImportanceLevel impLevel)
		{
			PrintMessage("", description, impLevel);
		}

		public static void PrintMessage(string description)
		{
			PrintMessage(description, ImportanceLevel.Low);
		}

		public static void Clear()
		{
			_mainForm.Messages.Clear();
		}

		public static void TraceException(Exception err)
		{
			//PrintMessage(ExceptionToString(err), ImportanceLevel.Критическая);
			PrintMessage(err.Message, ImportanceLevel.Critical);
		}
	}



	public class InnerMessage
	{
		public string AppName { get; set; }
		public int Importance { get; set; }
		public string Description { get; set; }
		public DateTime Time { get; set; }
	}

	public class MessagePrinter : IMessagePrinter
	{
		private static MessagePrinter _current;

		public string AppName { get; set; }

		/*
		public static MessagePrinter Current
		{
			get
			{
				if (_current == null)
					_current = new MessagePrinter();
				return _current;
			}
		}
		*/

		public void PrintMessage(string description, ImportanceLevel impLevel)
		{
			MessagesPanelManager.PrintMessage(AppName, description, impLevel);
		}

		public void PrintMessage(string description)
		{
			MessagesPanelManager.PrintMessage(AppName, description, ImportanceLevel.Low);
		}
	}
}
