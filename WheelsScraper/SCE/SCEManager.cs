using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WheelsScraper.srSCE;

namespace WheelsScraper.SCE
{
	public class SCEManager
	{
		public static sceApiSoapClient GetClient(string url)
		{
			var binding = new BasicHttpBinding();
			var client = new srSCE.sceApiSoapClient(binding, new EndpointAddress(url));
			return client;
		}

		public static sceApiSoapClient GetClient()
		{
			return GetClient("http://api.shoppingcartelite.com/master/sceapi.asmx");
		}

		public static bool ValidateLicense(ScraperSettings sett, bool keyRequired)
		{
			if (string.IsNullOrEmpty(sett.SCEAPIKey) && keyRequired)
			{
				MessagesPanelManager.PrintMessage("This module requires SCE API key");
				return false;
			}
			if (string.IsNullOrEmpty(sett.SCEAPIKey))
				return true;
			try
			{
				var serverUrl = sett.SCEServerAddress;
				if (string.IsNullOrEmpty(serverUrl))
					serverUrl = "http://api.shoppingcartelite.com/master/sceapi.asmx";

				var client = new srSCE2.sceApi();

				var auth = new srSCE2.AuthHeaderAPI { ApiAccessKey = sett.SCEAccessKey, ApiKey = sett.SCEAPIKey, ApiSecretKey = sett.SCEAPISecret };
				client.AuthHeaderAPIValue = auth;
				var r = client.ProductSearch("A", "A");
				return true;
			}
			catch (Exception err)
			{
				if (err.Message.Contains("Invalid Credentials"))
					MessagesPanelManager.PrintMessage("Please provide a valid SCE API Key", ImportanceLevel.High);
				else
					MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
				return false;
			}
		}

		public static void BatchUpdateFile(ScraperSettings sett, string fileUrl)
		{
			if (string.IsNullOrEmpty(fileUrl))
			{
				MessagesPanelManager.PrintMessage("SCE export file path is empty", ImportanceLevel.Mid);
				return;
			}

			var url = fileUrl.ToLower().Replace("ftp://ftp.", "http://www.");
			MessagesPanelManager.PrintMessage("Uploading file to SCE. " + url);
			var serverUrl = sett.SCEServerAddress;
			if (string.IsNullOrEmpty(serverUrl))
				serverUrl = "http://api.shoppingcartelite.com/master/sceapi.asmx";
			//var client = GetClient(serverUrl);
			var client = new srSCE2.sceApi();

			var auth = new srSCE2.AuthHeaderAPI{ ApiAccessKey = sett.SCEAccessKey, ApiKey = sett.SCEAPIKey, ApiSecretKey = sett.SCEAPISecret };
			client.AuthHeaderAPIValue = auth;
			var r = client.RunProductBatch(url);

			var fileInfo = new SCEFile { FileName = url, ModuleName = sett.Name, RequestID = r };
			var r2 = client.GetProductBatchErrors(r);
			fileInfo.ProcessResult = r2;
			AppSettings.Default.SCEFiles.Add(fileInfo);
			AppSettings.Default.SaveConfig();
		}

		public static void CheckResults(ScraperSettings sett)
		{
			var client = new srSCE2.sceApi();
			var auth = new srSCE2.AuthHeaderAPI { ApiAccessKey = sett.SCEAccessKey, ApiKey = sett.SCEAPIKey, ApiSecretKey = sett.SCEAPISecret };
			client.AuthHeaderAPIValue = auth;

			var files = GetModuleFiles(sett.Name);
			foreach (var file in files)
			{
				if (!file.Selected)
					continue;
				try
				{
					MessagesPanelManager.PrintMessage("Checking " + file.FileName);
					var res = client.GetProductBatchErrors(file.RequestID);
					file.ProcessResult = res;
				}
				catch (Exception err)
				{
					MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.Critical);
				}
			}
			AppSettings.Default.SaveConfig();
		}

		public static List<SCEFile> GetModuleFiles(string moduleName)
		{
			var r = AppSettings.Default.SCEFiles.Where(x => x.ModuleName == moduleName);
			return r.ToList();
		}
	}
}