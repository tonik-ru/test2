using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public enum AuthInfoResult
{
	Success,
	APIKeyNotFound,
	UserIsBlocked,
	NotWorkingTime,
	NotAllowedModule
}