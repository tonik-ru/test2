using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public interface IMessagePrinter
	{
		void PrintMessage(string description, ImportanceLevel impLevel);
		void PrintMessage(string description);
	}

	public enum ImportanceLevel
	{
		//Information,
		//Warning,
		//Error
		Low,
		Mid,
		High,
		Critical

	}
}
