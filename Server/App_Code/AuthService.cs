using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AuthService" in code, svc and config file together.
public class AuthService : IAuthService
{
	public string GetRemoteIP()
	{
		var oc = System.ServiceModel.OperationContext.Current;
		var remoteIP = ((System.ServiceModel.Channels.RemoteEndpointMessageProperty)(oc.IncomingMessageProperties[System.ServiceModel.Channels.RemoteEndpointMessageProperty.Name])).Address;
		return remoteIP;
	}

	public AuthInfo Login(string sceAPIKey, string moduleName)
	{
		var ip = GetRemoteIP();
		LogManager.Log.InfoFormat("Login. IP = {2}, SCEAPIKey: {0}, Module: {1}", sceAPIKey, moduleName, ip);
		var r = AuthDataManager.LoginUser(sceAPIKey, moduleName, ip);
		LogManager.Log.InfoFormat("Result: {0}, TimeLeft: {1}", r.AuthResult, r.TimeLeft);

		return r;
	}

	public void ReportWorkTime(string sceAPIKey, string moduleName, DateTime startDate, TimeSpan workTime)
	{
		var ip = GetRemoteIP();
		LogManager.Log.InfoFormat("ReportWorkTime. IP = {2}, SCEAPIKey: {0}, Module: {1}", sceAPIKey, moduleName, ip);
		var realStartDate = DateTime.UtcNow - workTime;
		AuthDataManager.ReportUsage(sceAPIKey, moduleName, ip, realStartDate, workTime);
	}
}
