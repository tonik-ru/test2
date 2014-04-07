using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class ScheduleItem
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public int Interval { get; set; }
		public bool Enabled { get; set; }
		public TimeSpan NextRunIn { get; set; }
		public ScheduleItem()
		{
			Start = new DateTime(1, 1, 1, 10, 0, 0);
			End = new DateTime(1, 1, 1, 23, 0, 0);
			Interval = 60;
		}
	}
}
