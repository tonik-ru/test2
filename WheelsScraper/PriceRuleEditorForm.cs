using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Scraper.Shared;

namespace WheelsScraper
{
	public partial class PriceRuleEditorForm : Form
	{
		public ProfileRule Rule { get; set; }
		public PriceRuleEditorForm()
		{
			InitializeComponent();
			var r = Enum.GetValues(typeof(ProfileRuleType));
			foreach (ProfileRuleType item in r)
			{
				var desc = item.GetDescription();
				cbRules.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(desc, item));
			}
		}

		private void PriceRuleEditorForm_Load(object sender, EventArgs e)
		{
			bindingSource1.DataSource = Rule;
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void simpleButton2_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
