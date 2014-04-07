using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class AdditionalDataSource
	{
		public string File { get; set; }
		public string SheetName { get; set; }
		public string BrandColumnName { get; set; }
		public string PartNoColumnName { get; set; }
	}

	public class ColumnMatch
	{
		public string PrimaryKey { get; set; }
		public string ForeignKey { get; set; }
	}
}
