using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DataManager
/// </summary>
public class DataManager
{
	public static EDFServerEntities GetDC()
	{
		return new EDFServerEntities();
	}
}