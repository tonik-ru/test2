using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WheelsScraper.srAuth;

namespace WheelsScraper
{
	public class RuntimeManager
	{
		protected Dictionary<ISimpleScraper, RuntimeCheckInfo> RuntimeCheckInfos;

		public Action<ISimpleScraper> ResumeScraper { get; set; }

		public RuntimeManager()
		{
			RuntimeCheckInfos = new Dictionary<ISimpleScraper, RuntimeCheckInfo>();
		}

		public void StartScraperResumeTimer(ISimpleScraper scrap, AuthInfo authInfo)
		{
			if (authInfo.StartIn.TotalSeconds == 0)
				return;
			var r1 = authInfo.StartTimeLocal;
			lock (this)
			{
				RuntimeCheckInfo info;
				RuntimeCheckInfos.TryGetValue(scrap, out info);
				if (info != null)
					info.Tmr.Change(Timeout.Infinite, Timeout.Infinite);
				else
				{
					info = new RuntimeCheckInfo { AuthInfo = authInfo, LastCheck = DateTime.MinValue, StartDate = DateTime.Now };
					RuntimeCheckInfos[scrap] = info;
				}
				var tmr = new Timer(TimerCallback, scrap, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
				info.Tmr = tmr;
			}
		}

		protected bool ValidateRunLimit(ISimpleScraper scrap, bool showMsg)
		{
			var info = RuntimeCheckInfos[scrap];
			
			var startT = info.AuthInfo.StartTimeLocal;
			bool shouldFinish = false;

			if (info.LastCheck == DateTime.MinValue)
				info.LastCheck = DateTime.Now;

			if (info.LastCheck > DateTime.Now)
				shouldFinish = true;
			else
				info.LastCheck = DateTime.Now;

			if (info.StartDate + info.AuthInfo.TimeLeft < DateTime.Now)
				shouldFinish = true;

			if (shouldFinish && showMsg)
			{
				MessagesPanelManager.PrintMessage("Work limit reached.", ImportanceLevel.High);
			}
			return shouldFinish;
		}
		
		public void TimerCallback(object state)
		{
			var scrap = state as ISimpleScraper;
			if (!scrap.IsRunning)
				return;

			var showMsg = !scrap.IsPaused;
			var shouldFinish = ValidateRunLimit(scrap, showMsg);

			if (!scrap.IsPaused && !shouldFinish)
				return;

			var info = RuntimeCheckInfos[scrap];
			var tmr = info.Tmr;

			if (shouldFinish && !scrap.IsPaused)
			{
				if (ResumeScraper != null)
					ResumeScraper(scrap);
				return;
			}

			if (shouldFinish && (info.StartDate + info.AuthInfo.StartIn) < DateTime.Now)
			{
				//if (shouldFinish)
				//	return;

				if (!scrap.IsPaused)
					return;

				if (ResumeScraper != null)
					ResumeScraper(scrap);
			}
		}
	}

	public class RuntimeCheckInfo
	{
		public DateTime LastCheck { get; set; }
		public Timer Tmr { get; set; }
		public AuthInfo AuthInfo { get; set; }
		public DateTime StartDate { get; set; }
	}
}
