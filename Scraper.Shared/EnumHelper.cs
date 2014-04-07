using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Scraper.Shared
{
	public static class EnumHelper
	{
		public static string GetDescription(this Enum value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			string description = value.ToString();
			var fieldInfo = value.GetType().GetField(description);

			var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length >= 1)
				description = attributes[0].Description;

			return description;
		}
	}
}