using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scraper.Shared
{
	public interface IMainApp
	{
		void LoadEDF(string edfFileName, string libFileName);
	}
}
