using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AuthDataManager
/// </summary>
public class AuthDataManager
{
	public AuthDataManager()
	{
	}

	public static AuthInfo LoginUser(string sceAPIKey, string moduleName, string ip)
	{
		var res = new AuthInfo { AuthResult = AuthInfoResult.APIKeyNotFound };
		using (var dc = DataManager.GetDC())
		{
			var moduleID = UsersManager.RegisterModule(moduleName);

			var r = dc.SCEUsers.Where(x => x.SCEAPIKey == sceAPIKey).FirstOrDefault();
			if (r == null)
				r = dc.SCEUsers.Where(x => x.SCEAPIKey == "_DEFAULT_").FirstOrDefault();
			if (r == null)
				return res;
			if (!r.IsActive)
			{
				res.AuthResult = AuthInfoResult.UserIsBlocked;
				UsersManager.RegisterModuleStart(r.ID, moduleID, ip, (int)res.AuthResult);
				return res;
			}

			if (!r.UserModules.Any(x => x.EDFModules.Name == moduleName) && r.UserModules.Count() > 0)
			{
				res.AuthResult = AuthInfoResult.NotAllowedModule;
				UsersManager.RegisterModuleStart(r.ID, moduleID, ip, (int)res.AuthResult);
				return res;
			}
			
			var estNow = DateTime.UtcNow.AddHours(-5);
			var estStart = estNow.Date.Add(r.Timetables.StartTime.TimeOfDay);
			var estEnd = estNow.Date.Add(r.Timetables.EndTime.TimeOfDay);
			if (estEnd < estStart)
				estEnd = estEnd.AddDays(1);

			if (r.Timetables.StartTime == r.Timetables.EndTime)
			{
				res.AuthResult = AuthInfoResult.Success;
				res.TimeLeft = TimeSpan.FromDays(10);
			}
			else if (estNow >= estStart && estNow < estEnd)
			{
				res.TimeLeft = estEnd - estNow;
				res.StartIn = estStart.AddDays(1) - estNow;
				res.AuthResult = AuthInfoResult.Success;
			}
			else if (estNow < estStart)
			{
				res.TimeLeft = TimeSpan.FromSeconds(0);
				res.StartIn = estStart - estNow;
				res.AuthResult = AuthInfoResult.NotWorkingTime;
			}
			else//estNow>estStart
			{
				res.TimeLeft = estStart.AddDays(1) - estNow;
				res.StartIn = estStart - estNow;
				res.AuthResult = AuthInfoResult.NotWorkingTime;
			}

			if (res.AuthResult == AuthInfoResult.Success)
			{
				var intStartUTC = estStart.AddHours(5);
				var intEndUTC = estEnd.AddHours(5);
				var totalWorked = dc.EDFUsage.Where(x => x.UserId == r.ID && x.ModuleId == moduleID && x.StartDate >= intStartUTC && x.StartDate < intEndUTC).Sum(x => (int?)x.WorkTime) ?? 0;
				if (r.Timetables.MaxSeconds > 0)
				{
					res.MaxSeconds = r.Timetables.MaxSeconds - totalWorked;
					res.TimeLeft = TimeSpan.FromSeconds(Math.Min(res.MaxSeconds, res.TimeLeft.TotalSeconds));
				}
			}

			res.StartTime = r.Timetables.StartTime.TimeOfDay;
			res.EndTime = r.Timetables.EndTime.TimeOfDay;		
			res.MaxThread = r.Timetables.MaxThreads;
			res.MaxSeconds = r.Timetables.MaxSeconds;

			UsersManager.RegisterModuleStart(r.ID, moduleID, ip, (int)res.AuthResult);
		}
		return res;
	}

	public static void ReportUsage(string sceAPIKey, string moduleName, string ip, DateTime startDate, TimeSpan workTime)
	{
		using (var dc = DataManager.GetDC())
		{
			var module = UsersManager.GetModule(moduleName);
			if (module == null)
				return;
			var user = UsersManager.FindUserByAPIKey(sceAPIKey);
			if (user == null)
				return;

			//var rep = dc.EDFUsage.Where(x => x.ModuleId == module.Id && x.UserId == user.ID && x.StartDate == startDate).FirstOrDefault();
			//if (rep == null)
			//{
				var rep = new EDFUsage { ModuleId = module.Id, UserId = user.ID, StartDate = startDate, WorkTime = (int)workTime.TotalSeconds };
				dc.EDFUsage.Add(rep);
			//}
			//rep.WorkTime = (int)workTime.TotalSeconds;
			dc.SaveChanges();
		}
	}
}