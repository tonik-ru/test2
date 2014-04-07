using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WheelsScraper.srAuth;

namespace WheelsScraper.SCE
{
	public class LicenseValidator
	{
		private static AuthServiceClient GetAuthServiceClient()
		{
			return new AuthServiceClient();
		}

		public bool Authorize(string sceAPIKey, string moduleName, out AuthInfo authInfo)
		{
			var client = GetAuthServiceClient();
			authInfo = client.Login(sceAPIKey, moduleName);
			if (authInfo.AuthResult == AuthInfoResult.Success)
				return true;
			else if (authInfo.AuthResult == AuthInfoResult.APIKeyNotFound)
				MessagesPanelManager.PrintMessage("Incorrect SCE API key", ImportanceLevel.Critical);
			else if (authInfo.AuthResult == AuthInfoResult.UserIsBlocked)
				MessagesPanelManager.PrintMessage("This SCE API key is blocked", ImportanceLevel.Critical);
			else if (authInfo.AuthResult == AuthInfoResult.NotWorkingTime)
				MessagesPanelManager.PrintMessage(string.Format("Its not a work time yet. You can run EDF in range {0:HH:mm:ss} {1:HH:mm:ss} EST", authInfo.StartTime, authInfo.EndTime), ImportanceLevel.High);
			else if (authInfo.AuthResult == AuthInfoResult.NotAllowedModule)
				MessagesPanelManager.PrintMessage(string.Format("You are not allowed to use this module"), ImportanceLevel.High);
			return false;
		}

		public static void ReportUsage(string sceAPIKey, string moduleName, DateTime startDate, TimeSpan workTime)
		{
			var client = GetAuthServiceClient();
			Utils.Executor.ExecuteWithRetry(() => client.ReportWorkTime(sceAPIKey, moduleName, startDate, workTime), 10, 1000);
		}
	}
}