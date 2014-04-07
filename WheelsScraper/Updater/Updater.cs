using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace WheelsScraper
{
	public class FileTransferInfo : EventArgs
	{
		public string Name { get; set; }
		public int Length { get; set; }
		public int Read { get; set; }
	}

	public class UpdateCompleteEventArgs : EventArgs
	{
		public bool NeedRestart { get; set; }
	}

	public class ErrorEventArgs : EventArgs
	{
		public Exception Exception { get; set; }
	}

	public class UpdateInfo : EventArgs
	{
		public List<FileTransferInfo> FilesToTransfer;
		public UpdateInfo()
		{
			FilesToTransfer = new List<FileTransferInfo>();
		}
	}

	public class Updater
	{
		public event EventHandler<UpdateInfo> ReadyToUpdate;
		public event EventHandler<FileTransferInfo> StartFileTransfer;
		public event EventHandler<FileTransferInfo> FileTransfer;
		public event EventHandler<UpdateCompleteEventArgs> UpdateComplete;
		public event EventHandler<ErrorEventArgs> Error;

		SRUpdater.UpdaterServiceClient srUpdater;

		public static void CheckForUpdates(bool restart)
		{
			var frm = new AutoupdateForm();
			frm.ShowDialog();
			if (frm.NeedRestart)
			{
				if (restart)
					System.Windows.Forms.Application.Restart();
				else
					if(System.Windows.Forms.MessageBox.Show("Application updated. Do you want to restart app now?", "Easy Data Feed", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning)== System.Windows.Forms.DialogResult.Yes)
						System.Windows.Forms.Application.Restart(); ;
				return;
			}
		}
		
		
		public void Cancel()
		{
			_cancel = true;
			if (srUpdater != null)
				srUpdater.Abort();
			//MessagesPanelManager.PrintMessage("Autoupdate canceled", ImportanceLevel.Mid);
		}

		protected bool _cancel { get; set; }

		public static string GetFileHash(string filePath)
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

		const string AppName = "EDF2";

		SRUpdater.UpdaterServiceClient GetUpdaterServiceClient()
		{
			if (!string.IsNullOrEmpty(AppSettings.Default.UpdatesServer))
				return new SRUpdater.UpdaterServiceClient("BasicHttpBinding_IUpdaterService", AppSettings.Default.UpdatesServer);
			else
				return new SRUpdater.UpdaterServiceClient();
		}

		public bool Update()
		{
			bool gotError = false;
			//System.Threading.Thread.Sleep(5000);

			Program.Log.Info("Checking for updates");
			UpdateInfo updateInfo = new UpdateInfo();
			List<SRUpdater.UpdateFileInfo> filesToGet = new List<SRUpdater.UpdateFileInfo>();
			List<SRUpdater.UpdateFileInfo> runOnceFilesToGet = new List<SRUpdater.UpdateFileInfo>();
			int release = 0;
			try
			{
				var curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				srUpdater = GetUpdaterServiceClient();
				var filesOnServer = srUpdater.GetAppFiles(AppName, 0, ref release);

				foreach (var file in filesOnServer)
				{
					bool updateThisFile = false;
					var localFilePath = Path.Combine(curDir, file.Name);
					if (!File.Exists(localFilePath))
					{
						updateThisFile = true;
					}
					else
					{
						if (GetFileHash(localFilePath) != file.Hash)
							updateThisFile = true;
					}
					if (updateThisFile)
					{
						filesToGet.Add(file);
						updateInfo.FilesToTransfer.Add(new FileTransferInfo { Name = file.Name, Length = file.Length });
						Program.Log.InfoFormat("File {0} should be updated. Version on server: {1}", file.Name, file.Version);
					}
				}

				if (ReadyToUpdate != null)
					ReadyToUpdate(this, updateInfo);

				foreach (var file in filesToGet)
				{
					if (_cancel)
						break;

					FileTransferInfo fti = new FileTransferInfo { Name = file.Name, Length = file.Length, Read = 0 };
					if (StartFileTransfer != null)
						StartFileTransfer(this, fti);

					var fileBytes = new byte[65536];
					var fileStream = srUpdater.GetFile(AppName, file.Name, 0);
					var localFilePath = Path.Combine(curDir, "_new_" + file.Name);
					int pos = 0;
					using (var fs = new FileStream(localFilePath, FileMode.Create))
					{
						int read = 0;
						do
						{
							if (_cancel)
								break;
							read = fileStream.Read(fileBytes, 0, fileBytes.Length);
							pos += read;
							fs.Write(fileBytes, 0, read);

							fti.Read = pos;
							if (FileTransfer != null)
								FileTransfer(this, fti);
							//System.Threading.Thread.Sleep(10);

						} while (read != 0);
						fs.Close();
					}
					Program.Log.InfoFormat("Received file {0}. Size: {1}", fti.Name, fti.Read);
				}

				//List<string> filesToDelete=new List<string>();
				if (!_cancel)
				{
					foreach (var file in filesToGet)
					{
						var oldFilePath = Path.Combine(curDir, "_old_" + file.Name);
						if (File.Exists(oldFilePath))
							File.Delete(oldFilePath);
					}

					foreach (var file in filesToGet)
					{
						var localFilePath = Path.Combine(curDir, file.Name);
						var oldFilePath = Path.Combine(curDir, "_old_" + file.Name);
						if (File.Exists(localFilePath))
						{
							File.Move(localFilePath, oldFilePath);
						}
					}

					foreach (var file in filesToGet)
					{
						var localFilePath = Path.Combine(curDir, file.Name);
						var newFilePath = Path.Combine(curDir, "_new_" + file.Name);
						File.Move(newFilePath, localFilePath);
					}
				}
			}
			catch (EndpointNotFoundException err)
			{
				gotError = true;
				Program.Log.Error("Error updating app", err);
				if (Error != null)
					Error(this, new ErrorEventArgs { Exception = new Exception("Update server is not available now") });
			}
			catch (CommunicationObjectAbortedException err)
			{
				gotError = true;
				Program.Log.Error("Error updating app", err);
			}
			catch (Exception err)
			{
				gotError = true;
				Program.Log.Error("Error updating app", err);
				if (Error != null)
					Error(this, new ErrorEventArgs { Exception = err });
			}

			if (!_cancel && release != 0)
			{
				AppSettings.Default.ReleaseID = release;
				AppSettings.Default.SaveConfig();
			}
			if (UpdateComplete != null)
				UpdateComplete(this, new UpdateCompleteEventArgs { NeedRestart = filesToGet.Count() > 0 && !_cancel });

			Program.Log.Info("Update complete." + ((release != 0) ? " Release = " + release : ""));
			return filesToGet.Count() > 0 && !_cancel && !gotError;
		}

		private static Timer autoUpdateTimer;
		public static void StartPeriodicUpdates()
		{
			autoUpdateTimer = new Timer(AutoupdaterTimerProc, null, 10 * 60 * 1000, Timeout.Infinite);
		}

		public static event EventHandler ReleaseUpdated;

		private static void AutoupdaterTimerProc(object state)
		{
			Updater upd = new Updater();
			var needRestart = upd.Update();
			if (needRestart)
			{
				if (ReleaseUpdated != null)
					ReleaseUpdated(null, null);
				//if (System.Windows.Forms.MessageBox.Show("Получено обновление для АРМ-Кассира. Необходимо перезапустить программу.\nХотите выполнить перезапуск немедленно?", "Обновление АРМ-кассира", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
				//{
				//	System.Windows.Forms.Application.Restart();
				//}
			}
			else
				autoUpdateTimer.Change(10 * 60 * 1000, System.Threading.Timeout.Infinite);
		}
	}
}