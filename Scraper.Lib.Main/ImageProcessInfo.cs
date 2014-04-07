using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Scraper.Lib.Main
{
	public class ImageProcessInfo
	{
		public WareInfo WareInfo { get; set; }
		public string Url { get; set; }
		public string FieldName { get; set; }
		public bool MergePdf { get; set; }
		public bool SkipDownload { get; set; }
	}
}
