using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class SCEFile
	{
		public string ModuleName { get; set; }
		public string FileName { get; set; }
		public int RequestID { get; set; }
		public string ProcessResult { get; set; }

		public bool Selected { get; set; }
	}
}
