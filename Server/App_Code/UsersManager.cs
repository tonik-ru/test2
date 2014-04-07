using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

/// <summary>
/// Summary description for UsersManager
/// </summary>
public class UsersManager
{
	public UsersManager()
	{
		
	}

	public static SCEUsers FindUserByAPIKey(string sceAPIKey)
	{
		using (var dc = DataManager.GetDC())
		{
			var user = dc.SCEUsers.Where(x => x.SCEAPIKey == sceAPIKey).FirstOrDefault();
			if (user == null)
				user = dc.SCEUsers.Where(x => x.SCEAPIKey == "_DEFAULT_").FirstOrDefault();
			return user;
		}
	}

	public static SCEUsers GetUser(int userId)
	{
		using (var dc = DataManager.GetDC())
		{
			var r = dc.SCEUsers.Where(x => x.ID == userId).FirstOrDefault();
			return r;
		}
	}

	public static IEnumerable<EDFModules> GetModules()
	{
		using (var dc = DataManager.GetDC())
		{
			var r = dc.EDFModules.ToList();
			return r;
		}
	}

	public static EDFModules GetModule(string name)
	{
		using (var dc = DataManager.GetDC())
		{
			var r = dc.EDFModules.Where(x => x.Name == name).FirstOrDefault();
			return r;
		}
	}

	public static int RegisterModule(string moduleName)
	{
		if (string.IsNullOrEmpty(moduleName))
			return 0;
		using (var dc = DataManager.GetDC())
		{
			var r = dc.EDFModules.Where(x => x.Name == moduleName).FirstOrDefault();
			if (r == null)
			{
				r = new EDFModules { Name = moduleName };
				dc.EDFModules.Add(r);
				dc.SaveChanges();
			}
			return r.Id;
		}
	}

	public static List<int> GetUserModules(int userId)
	{
		using (var dc = DataManager.GetDC())
		{
			var r = dc.UserModules.Where(x => x.UserId == userId).Select(x => x.ModuleId).ToList();
			return r;
		}
	}

	public static void SetUserApps(int userId, IEnumerable<string> appIds)
	{
		using (var dc = DataManager.GetDC())
		{
			var u = dc.SCEUsers.Where(x => x.ID == userId).First();
			u.UserModules.Clear();
			foreach (var appId in appIds)
				u.UserModules.Add(new UserModules { ModuleId = int.Parse(appId) });
			dc.SaveChanges();
		}
	}


	public static void RegisterModuleStart(int userId, int moduleId, string ip, int resultCode)
	{
		using (var dc = DataManager.GetDC())
		{
			var r = new AppUsage { UserId = userId, ModuleId = moduleId, IP = ip, Date = DateTime.UtcNow, ResultCode = resultCode };
			dc.AppUsage.Add(r);
			dc.SaveChanges();
		}
	}
}