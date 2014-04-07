using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WheelsScraper
{
	public partial class PickUrlForm : Form
	{
		public string Url
		{
			get
			{
				return (string)hlUrl.EditValue;
			}
		}

		public PickUrlForm()
		{
			InitializeComponent();
			hlUrl.EditValue = AppSettings.Default.EdfListUrl;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			hlUrl.Text = "";
		}
	}
}
