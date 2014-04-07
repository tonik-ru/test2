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
	public partial class ManageRulesForm : Form
	{
		public ManageRulesForm()
		{
			InitializeComponent();
			//repositoryItemImageComboBox1.Items.AddEnum(typeof(ProfileRuleType));
			var r = Enum.GetValues(typeof(ProfileRuleType));
			foreach (ProfileRuleType item in r)
			{
				var desc = item.GetDescription();
				repositoryItemImageComboBox1.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(desc, item));
			}
		}

		private void ManageRulesForm_Load(object sender, EventArgs e)
		{
			//profileRuleBindingSource.DataSource = AppSettings.Default.ProfileRules;
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			AppSettings.Default.SaveConfig();
			this.Close();
		}
	}
}
