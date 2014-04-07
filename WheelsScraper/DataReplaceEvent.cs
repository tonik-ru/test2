using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class DataReplaceEvent : EventArgs
	{
		public string Column { get; set; }
		public string SearchText { get; set; }
		public string ReplaceText { get; set; }
		public bool MatchCase { get; set; }
		public bool WholeCell { get; set; }
		public bool IsRegex { get; set; }
	}
}
