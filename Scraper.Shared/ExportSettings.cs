using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Scraper.Shared
{
	public class ExportSettings
	{
		public string FileName { get; set; }
		[XmlIgnore]
		public ExportProfile Profile { get; set; }

		public string ProfileName { get; set; }

		public bool Zip { get; set; }

		public int MaxRecords { get; set; }
		/*
		protected string _profileName;
		public string ProfileName
		{
			get
			{
				if (Profile != null)
					return Profile.Name;
				return _profileName;
			}
			set { _profileName = value; }
		}
		*/
	}
}