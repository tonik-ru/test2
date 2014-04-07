using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;

namespace WheelsScraper
{
	public partial class AutoupdateForm : DevExpress.XtraEditors.XtraForm
	{
		public bool NeedRestart { get; set; }

		public AutoupdateForm()
		{
			InitializeComponent();
		}

		Updater updater;

		void UpdateComplete(IAsyncResult result)
		{
			AsyncResult ar = (AsyncResult)result;
			Func<bool> fUpdated = (Func<bool>)ar.AsyncDelegate;

			if (!this.Visible)
				return;
			NeedRestart = fUpdated.EndInvoke(result);
			MethodInvoker mi = new MethodInvoker(this.Close);
			this.Invoke(mi);
		}

		private void StartUpForm_Load(object sender, EventArgs e)
		{
			updater = new Updater();
			updater.ReadyToUpdate += new EventHandler<UpdateInfo>(updater_ReadyToUpdate);
			updater.StartFileTransfer += new EventHandler<FileTransferInfo>(updater_StartFileTransfer);
			updater.FileTransfer += new EventHandler<FileTransferInfo>(updater_FileTransfer);
			updater.UpdateComplete += new EventHandler<UpdateCompleteEventArgs>(updater_UpdateComplete);
			updater.Error += new EventHandler<ErrorEventArgs>(updater_Error);
			Func<bool> fUpdate = updater.Update;
			fUpdate.BeginInvoke(UpdateComplete, null);
		}

		void updater_Error(object sender, ErrorEventArgs e)
		{
			this.Invoke(new Action<string>(ShowErrorMessage), e.Exception.Message);
		}

		private void ShowErrorMessage(string errMsg)
		{
			MessageBox.Show(errMsg);
		}

		void updater_UpdateComplete(object sender, UpdateCompleteEventArgs e)
		{
			//this.NeedRestart = e.NeedRestart;
			//MethodInvoker mi = new MethodInvoker(this.Close);
			//this.Invoke(mi);
		}

		void updater_FileTransfer(object sender, FileTransferInfo e)
		{
			SetPbPosition(e.Length, e.Read);
		}

		void updater_StartFileTransfer(object sender, FileTransferInfo e)
		{
			SetPbPosition(e.Length, 0);
			SetFileLabelText(string.Format("Dowloading file: {0}. Size {1}", e.Name, e.Length));
		}

		void updater_ReadyToUpdate(object sender, UpdateInfo e)
		{
			foreach (var file in e.FilesToTransfer)
				AddFileToList(file.Name);
		}

		void SetPbPosition(int maximum, int position)
		{
			if (this.InvokeRequired)
			{
				Action<int, int> wsInfoDlg = SetPbPosition;
				this.Invoke(wsInfoDlg, maximum, position);
			}
			else
			{
				pbProgr.Maximum = maximum;
				pbProgr.Value = position;
			}
		}

		void SetFileLabelText(string text)
		{
			if (this.InvokeRequired)
			{
				Action<string> wsInfoDlg = SetFileLabelText;
				this.Invoke(wsInfoDlg, text);
			}
			else
			{
				lblFileName.Text = text;
			}
		}

		void AddFileToList(string fileName)
		{
			if (this.InvokeRequired)
			{
				Action<string> wsInfoDlg = AddFileToList;
				this.Invoke(wsInfoDlg, fileName);
			}
			else
			{
				lvFiles.Items.Add(fileName);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			CancelUpdate();
		}

		private void CancelUpdate()
		{
			updater.Cancel();
		}
	}
}