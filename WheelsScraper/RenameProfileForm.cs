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
	public partial class RenameProfileForm : Form
	{
		public string InputValue
		{
			get { return txtText.Text; }
			set { txtText.Text = value; }
		}
		public RenameProfileForm()
		{
			InitializeComponent();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}
	}
}
