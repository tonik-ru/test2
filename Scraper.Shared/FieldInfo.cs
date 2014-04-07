using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using WheelsScraper.Export;

namespace WheelsScraper
{
	public class FieldInfo
	{
		public FieldInfo()
		{
			ExportRules = new List<ExportFieldRule>();
		}

		public string FieldName { get; set; }
		public string Header { get; set; }
		public int VisibleIndex { get; set; }
		public bool Export { get; set; }
		//public bool IsCustom { get; set; }
		public string CustomValue { get; set; }
		
		public ProfileRule ProfileRule { get; set; }
		public string ValidValues { get; set; }
		public bool IsImageColumn { get; set; }
		public bool UseDataFromAdditionalSources { get; set; }
		public Scraper.Shared.Export.Case Case { get; set; }

		public List<ExportFieldRule> ExportRules { get; set; }
		public bool StripHtml { get; set; }
		public int MaxLength { get; set; }
		public bool SearchForImage { get; set; }

		public string UnitConvertion { get; set; }
	}
}
