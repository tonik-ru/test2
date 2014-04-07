using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Hosting;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UpdaterService" in code, svc and config file together.
public class UpdaterService : IUpdaterService
{
	private static readonly log4net.ILog log = log4net.LogManager.GetLogger("");

	public static log4net.ILog Log
	{
		get { return UpdaterService.log; }
	}

	public string GetFileHash(string filePath)
	{
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		StringBuilder sbHash = new StringBuilder();
		using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
		{
			md5.ComputeHash(fs);
			foreach (byte b in md5.Hash)
			{
				sbHash.Append(b.ToString("X2"));
			}
		}
		return sbHash.ToString();
	}

	public string GetRemoteIP()
	{
		var oc = System.ServiceModel.OperationContext.Current;
		var remoteIP = ((System.ServiceModel.Channels.RemoteEndpointMessageProperty)(oc.IncomingMessageProperties[System.ServiceModel.Channels.RemoteEndpointMessageProperty.Name])).Address;
		return remoteIP;
	}

	private void ValidateAppName(string appName)
	{
		if (string.IsNullOrEmpty(appName))
			throw new Exception("AppName = null");
		appName = appName.ToLower();
		var validApps = GetApplications();
		if (!validApps.Contains(appName))
			throw new Exception("Invalid AppName");
	}

	private void ValidateFileName(string appDir, string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
			throw new Exception("FilName = null");
		fileName = fileName.ToLower();
		//string appDir = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), appName);
		var files = Directory.GetFiles(appDir).Select(x => Path.GetFileName(x).ToLower()).ToList();
		if (!files.Contains(fileName))
			throw new Exception("Invalid FileName");
	}

	public List<UpdateFileInfo> GetAppFiles(string applicationName, int clientID, ref int release)
	{
		var remoteIP = GetRemoteIP();
		Log.InfoFormat("GetAppFiles. applicationName = {1}, ClientID = {0}, IP = {2}", clientID, applicationName, remoteIP);

		List<UpdateFileInfo> ufiFiles = new List<UpdateFileInfo>();
		try
		{
			ValidateAppName(applicationName);
			release = GetReleaseForClient(applicationName, clientID);
			string appDir = HostingEnvironment.MapPath("~/App_Data\\" + applicationName + "\\" + release);

			//string appDir = HostingEnvironment.MapPath("~/App_Data\\" + applicationName);
			DirectoryInfo di = new DirectoryInfo(appDir);
			var files = di.GetFiles("*.*");
			foreach (var file in files)
			{
				var fvi = FileVersionInfo.GetVersionInfo(file.FullName);
				UpdateFileInfo ufi = new UpdateFileInfo { Name = file.Name, Version = fvi.FileVersion, Hash = GetFileHash(file.FullName), Length = (int)file.Length };
				ufiFiles.Add(ufi);
			}
			Log.InfoFormat("GetAppFiles. applicationName = {1}, Shop = {0}, IP = {2}, Release = {3}", clientID, applicationName, remoteIP, release);
		}
		catch (Exception err)
		{
			Log.Error("GetAppFiles", err);
		}
		return ufiFiles;
	}

	private static List<string> GetApplications()
	{
		string appDir = HostingEnvironment.MapPath("~/App_Data");
		DirectoryInfo di = new DirectoryInfo(appDir);
		var dirs = di.GetDirectories().Select(x => x.Name.ToLower()).ToList();
		return dirs;
	}

	public Stream GetFile(string applicationName, string fileName, int clientID)
	{
		//System.ServiceModel.OperationContext.Current.IncomingMessageProperties.
		try
		{
			Log.InfoFormat("GetFile. applicationName = {1}, fileName = {2}, ClientID = {0}", clientID, applicationName, fileName);
			ValidateAppName(applicationName);
			//string appDir = HostingEnvironment.MapPath("~/App_Data\\" + applicationName);
			int release = GetReleaseForClient(applicationName, clientID);
			string appDir = HostingEnvironment.MapPath("~/App_Data\\" + applicationName + "\\" + release);

			ValidateFileName(appDir, fileName);

			var filePath = Path.Combine(appDir, fileName);
			var fvi = FileVersionInfo.GetVersionInfo(filePath);
			Log.InfoFormat("Sending file. applicationName = {1}, fileName = {2}, ClientID = {0}, version = {3}, Release = {4}", clientID, applicationName, fileName, fvi.FileVersion, release);
			//using (
			FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			{
				//var fileBytes = new byte[fs.Length];
				//fs.Read(fileBytes, 0, fileBytes.Length);
				return fs;
			}
		}
		catch (Exception err)
		{
			Log.Error("GetFile", err);
		}
		return null;
	}
	
	private int GetRelease(string appDir)
	{
		if (!Directory.Exists(appDir))
			return 0;
		int release = 0;
		DirectoryInfo di = new DirectoryInfo(appDir);
		var files = di.GetFiles("release*");
		foreach (var file in files)
		{
			using (var sr = new StreamReader(file.OpenRead()))
			{
				var str = sr.ReadLine();
				int.TryParse(str, out release);
			}
		}
		return release;
	}

	private int GetReleaseForClient(string applicationName, int clientID)
	{
		int specialRelease = 0;
		string appDir = HostingEnvironment.MapPath("~/App_Data\\" + applicationName + "\\Clients\\" + clientID);
		specialRelease = GetRelease(appDir);
		if (specialRelease == 0)
		{
			appDir = HostingEnvironment.MapPath("~/App_Data\\" + applicationName + "\\Clients\\All");
			specialRelease = GetRelease(appDir);
		}

		return specialRelease;
	}
}
