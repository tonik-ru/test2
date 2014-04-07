using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper;

namespace Databox.Libs.UniversalMapper
{
	public class ExtWareInfo : WareInfo, IExtWareInfoProvider
	{
		public ExtWareInfo()
		{
			Values = new Dictionary<string, string>();
		}

		public Dictionary<string, string> Values { get; set; }

		public string this[string paramName]
		{
			get
			{
				return GetFieldValue(paramName);
			}
			set
			{
				Values[paramName] = value;
			}
		}

		public string GetFieldValue(string fieldName)
		{
			string val = null;
			Values.TryGetValue(fieldName, out val);
			return val;
		}

		public void SetFieldValue(string fieldName, object value)
		{
			string strVal = null;
			if (value != null)
				strVal = value.ToString();
			Values[fieldName] = strVal;
		}

	}
}