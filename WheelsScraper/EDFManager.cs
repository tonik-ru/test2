using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WheelsScraper
{
	public class EDFManager
	{
		protected static Dictionary<ISimpleScraper, EDFManager> EDFManagers;

		public static EDFManager GetEDFManager(ISimpleScraper scrap)
		{
			lock (EDFManagers)
			{
				if(EDFManagers.ContainsKey(scrap))
					return EDFManagers[scrap];
				var mgr = new EDFManager(scrap);
				EDFManagers[scrap] = mgr;
				return mgr;
			}
		}

		public UndoManager UndoMgr { get; set; }
		public ISimpleScraper Scraper { get; protected set; }

		protected EDFManager(ISimpleScraper scrap)
		{
			UndoMgr = new UndoManager(scrap);
		}

		static EDFManager()
		{
			EDFManagers = new Dictionary<ISimpleScraper, EDFManager>();
		}
	}
}