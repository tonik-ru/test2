using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scraper.Lib.Main
{
	public class FileUploadInfo
	{
		public string LocalPath { get; set; }
		public string Url { get; set; }
		public bool Overwrite { get; set; }
	}
}
