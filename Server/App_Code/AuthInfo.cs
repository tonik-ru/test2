using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AuthInfo
/// </summary>
public class AuthInfo
{
	public AuthInfoResult AuthResult { get; set; }
	public TimeSpan StartTime { get; set; }
	public TimeSpan EndTime { get; set; }
	public int MaxThread { get; set; }
	public int MaxSeconds { get; set; }
	public int Random { get; set; }
	public TimeSpan TimeLeft { get; set; }
	public TimeSpan StartIn { get; set; }
	public int Delay { get; set; }
}