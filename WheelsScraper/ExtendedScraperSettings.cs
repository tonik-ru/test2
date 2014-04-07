using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class ExtendedScraperSettings
	{
		public string ModuleName { get; set; }
		public ExtendedScraperSettings()
		{
			AddDataSourceFiles = new List<AdditionalDataSource>();
		}

		public List<AdditionalDataSource> AddDataSourceFiles { get; set; }
		public bool ValidateAgainstAddData { get; set; }
	}
}
