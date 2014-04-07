using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper.srAuth
{
	public partial class AuthInfo
	{
		public TimeSpan StartTimeLocal
		{
			get
			{
				var r1 = new DateTime(DateTime.Today.Ticks, DateTimeKind.Utc).Add(StartTime).AddHours(5);
				var s1 = r1.ToLocalTime();
				return s1.TimeOfDay;
			}
		}

		public TimeSpan EndTimeLocal
		{
			get
			{
				var r1 = new DateTime(DateTime.Today.Ticks, DateTimeKind.Utc).Add(EndTime).AddHours(5);
				var s1 = r1.ToLocalTime();
				return s1.TimeOfDay;
			}
		}
	}
}
