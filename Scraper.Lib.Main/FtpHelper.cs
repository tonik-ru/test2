using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WheelsScraper
{
	public static class FtpHelper
	{
		public static IMessagePrinter MessagePrinter { get; set; }

		private static FtpWebRequest GetFtp(string url, string userName, string password)
		{
			FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(url);
			//ftp.Method = WebRequestMethods.Ftp.UploadFile;
			ftp.Timeout = 30000;

			ftp.Credentials = new NetworkCredential(userName, password);
			return ftp;
		}

		public static void CreatePath(string ftpFilePath, string userName, string password)
		{
			var ftp = GetFtp(ftpFilePath, userName, password);
			ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
		}

		public static void DeleteFile(string ftpFilePath, string userName, string password)
		{
			try
			{
				var ftp = GetFtp(ftpFilePath, userName, password);
				ftp.Method = WebRequestMethods.Ftp.DeleteFile;

				using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
				{
				}
			}
			catch (Exception err)
			{
				//MessagePrinter.PrintMessage(err.Message, ImportanceLevel.High);
			}
		}

		public static bool CheckIfFileExists(string ftpFilePath, string userName, string password)
		{
			try
			{
				var ftp = GetFtp(ftpFilePath, userName, password);
				ftp.Method = WebRequestMethods.Ftp.GetFileSize;

				using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
				{
				}
				return true;
			}
			catch (Exception err)
			{
				
			}
			return false;
		}

		public static bool CheckConnection(string ftpAddress, string userName, string password)
		{
			/*
			try
			{
				var ftp = GetFtp(ftpAddress, userName, password);

				ftp.Method = WebRequestMethods.Ftp.ListDirectory;
				using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
				{
				}
				MessagePrinter.PrintMessage("Trying to create folder");
			}
			catch (Exception err)
			{
				MessagePrinter.PrintMessage(err.Message, ImportanceLevel.High);
			}
			*/
			try
			{
				Uri ftpUri = new Uri(ftpAddress);
				var root = "ftp://" + ftpUri.Host + ":" + ftpUri.Port;
				var ftp = GetFtp(root, userName, password);
				ftp.Method = WebRequestMethods.Ftp.ListDirectory;
				using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
				{
					MessagePrinter.PrintMessage("Login success. Checking folders");
				}

				var folders = ftpUri.Segments;
				if (folders.Length > 1)
				{
					var ftpPath = root + "/";
					for (int i = 1; i < folders.Length; i++)
					{
						ftpPath += folders[i];
						ftp = GetFtp(ftpPath, userName, password);

						ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
						try
						{
							using (FtpWebResponse response = (FtpWebResponse)ftp.GetResponse())
							{
								MessagePrinter.PrintMessage("Folder " + folders[i] + " created");
							}
						}
						catch (Exception err)
						{
						}
					}
				}

				return true;
			}
			catch (Exception err)
			{
				MessagePrinter.PrintMessage(err.Message, ImportanceLevel.Critical);
			}

			return false;
		}

		public static string UploadFileToFtp(string ftpAddress, string userName, string password, string fileName, string localFile, bool overwrite = true)
		{
			if (!ftpAddress.EndsWith("/"))
				ftpAddress += "/";
			var ftpFilePath = ftpAddress + fileName;

			for (int i = 0; i < 10; i++)
			{
				try
				{
					if (overwrite)
						DeleteFile(ftpFilePath, userName, password);
					else if (CheckIfFileExists(ftpFilePath, userName, password))
						return ftpFilePath;
					//var ftpFilePath = ftpAddress + Path.GetFileName(filePath);

					var ftp = GetFtp(ftpFilePath, userName, password);
					ftp.KeepAlive = true;
					ftp.UseBinary = true;
					ftp.Method = WebRequestMethods.Ftp.UploadFile;
					FileStream fs = File.OpenRead(localFile);
					byte[] buffer = new byte[fs.Length];
					fs.Read(buffer, 0, buffer.Length);
					fs.Close();
					Stream ftpstream = ftp.GetRequestStream();
					ftpstream.Write(buffer, 0, buffer.Length);
					ftpstream.Close();
					MessagePrinter.PrintMessage("File uploaded to: " + ftpFilePath);
					return ftpFilePath;
					break;
				}
				catch (Exception err)
				{
					MessagePrinter.PrintMessage(err.Message, ImportanceLevel.Mid);
				}
			}
			return null;
		}
	}
}
