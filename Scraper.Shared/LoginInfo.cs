using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class LoginInfo
	{
		public string Login { get; set; }
		public string Password { get; set; }
		public bool IsActive { get; set; }
		public DateTime LastTried { get; set; }
		public DateTime LastSuccessed { get; set; }
	}
}
