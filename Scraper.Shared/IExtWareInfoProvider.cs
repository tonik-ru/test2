using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scraper.Shared
{
	public interface IExtWareInfoProvider
	{
		string GetFieldValue(string fieldName);
		void SetFieldValue(string fieldName, object value);
	}
}
