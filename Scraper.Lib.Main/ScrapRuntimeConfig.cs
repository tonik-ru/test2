using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Scraper.Lib.Main
{

	public class ScrapRuntimeConfig
	{
		public List<WareInfo> Wares { get; set; }
		public List<ProcessQueueItem> LstProcessQueue { get; set; }
		public List<string> BrandsToLoad { get; set; }
	}
}
