using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class ProfileManager
	{
		public static ExportProfile CreateExportProfile(ISimpleScraper scraper, ScraperSettings sett, ExportProfile prof = null)
		{
			var Scraper = scraper;
			if (!(Scraper is IFieldInfoProvider))
				return null;
			if (prof == null)
			{
				prof = new ExportProfile();
				prof.Name = "New profile";
			}

			var scrap = (IFieldInfoProvider)Scraper;
			Scraper.Url = sett.Url;
			var fields = scrap.GetFieldNames();
			var defHeaders = WheelsScraper.Properties.Resources.DefaulHeaders.Split(',');
			int i = 0;
			foreach (var h1 in defHeaders)
			{
				var h2 = fields.Where(x => x.ToLower() == h1.ToLower()).FirstOrDefault();
				{
					prof.Fields.Add(new FieldInfo { FieldName = h2, Header = h1, Export = true, VisibleIndex = i++ });
				}
			}
			return prof;
		}
	}
}
