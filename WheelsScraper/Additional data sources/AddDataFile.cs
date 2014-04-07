using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class AddDataFile
	{
		public string FileName { get; set; }
		public string BrandColumn { get; set; }
		public string PartNoColumn { get; set; }
		public Dictionary<string, Dictionary<string, string>> XlsDataByKey { get; set; }
	}
}
