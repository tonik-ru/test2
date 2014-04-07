using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LogManager
/// </summary>
public class LogManager
{
	public LogManager()
	{
		//
		// TODO: Add constructor logic here
		//
	}
	private static readonly log4net.ILog log = log4net.LogManager.GetLogger("");

	public static log4net.ILog Log
	{
		get { return log; }
	}


}