using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scraper.Shared
{
	public class ExportFieldCategoryInfo
	{
		public const string SpcSeparator = "|";
		public ExportFieldCategoryInfo()
		{
			Categories = new List<ExportFieldSpcCategoryInfo>();
		}

		public string OutputHeader { get; set; }
		public List<ExportFieldSpcCategoryInfo> Categories { get; set; }
	}

	public class ExportFieldSpcCategoryInfo
	{
		public ExportFieldSpcCategoryInfo()
		{
			Values = new List<ExportFieldSpcValueInfo>();
		}

		public const string NameDescSeparator = "##";

		public string Name { get; set; }
		public List<ExportFieldSpcValueInfo> Values { get; set; }
	}

	public class ExportFieldSpcValueInfo
	{
		public const string DescValSeparator = "~";
		public const string DescSepataror = "^";

		public bool Visible { get; set; }
		public string Description { get; set; }
		public string Value { get; set; }
	}
}
