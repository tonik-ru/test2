using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Scraper.Shared
{
	public class ExportProfile
	{
		public string Name { get; set; }
		public List<FieldInfo> Fields { get; set; }
		public bool IsDefault { get; set; }

		/*
		public bool DownloadImages { get; set; }
		public bool ConvertImages { get; set; }
		public bool UploadImagesToFtp { get; set; }
		public string PublishPrefix { get; set; }
		*/

		//public List<ExportProfileAddHeaderInfo> AddHeaderInfos { get; set; }
		public List<FieldInfo> AdditionalStaticFields { get; set; }
		public bool UseStaticFields { get; set; }
		public string AdditionalStaticFieldsFile { get; set; }


		public ExportProfile()
		{
			Fields = new List<FieldInfo>();
			AdditionalStaticFields = new List<FieldInfo>();
		}
	}
}
