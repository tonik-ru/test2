using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

/// <summary>
/// Summary description for ModulesManager
/// </summary>
public class ModulesManager
{
	public ModulesManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}

	public static IQueryable GetModuleUsage(int moduleId, int userId)
	{
		var dc = DataManager.GetDC();
		var u = dc.EDFUsage.Include(x => x.SCEUsers).Include(x => x.EDFModules);
		if (userId != 0)
			u = u.Where(x => x.UserId == userId);
		if (moduleId != 0)
			u = u.Where(x => x.ModuleId == moduleId);
		return u;
	}
}