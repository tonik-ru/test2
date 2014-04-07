using Scraper.Shared;
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
	public partial class ImportResultsFromCsvForm : Form
	{
		public List<ExportProfile> Profiles { get; set; }
		public ExportProfile Profile { get; set; }
		public string FileName { get; set; }
		public ISimpleScraper Scraper { get; set; }

		public ImportResultsFromCsvForm()
		{
			InitializeComponent();
		}

		private void ImportResultsFromCsvForm_Load(object sender, EventArgs e)
		{
			if (Scraper is IFieldInfoProvider)
				cbProfile.Properties.Items.Add("Create new profile");
			else
				cbProfile.Properties.Items.Add("");
			foreach (var prof in Profiles)
			{
				cbProfile.Properties.Items.Add(prof.Name);
			}
			cbProfile.SelectedIndex = 1;
			beFileName.Text = FileName;
		}

		protected void CreateExportProfile()
		{
			if (!(Scraper is IFieldInfoProvider))
				return;
			ExportProfile prof = new ExportProfile();
			prof.Name = "New profile";

			var scrap = (IFieldInfoProvider)Scraper;
			Scraper.Url = FileName;
			var fields = scrap.GetFieldNames();
			var defHeaders = WheelsScraper.Properties.Resources.DefaulHeaders.Split(',');
			int i = 0;
			foreach (var h1 in defHeaders)
			{
				var h2 = fields.Where(x => x.ToLower() == h1.ToLower()).FirstOrDefault();
				//if (h2 != null)
				{
					prof.Fields.Add(new FieldInfo { FieldName = h2, Header = h1, Export = true, VisibleIndex = i++ });
				}
			}
			Profile = prof;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (cbProfile.SelectedIndex == 0)
				CreateExportProfile();
			else
				Profile = Profiles.Where(x => x.Name == (string)cbProfile.SelectedItem).FirstOrDefault();
			if (Profile == null)
			{
				MessageBox.Show("Please select profile");
				return;
			}
			FileName = beFileName.Text;
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		private void beFileName_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "csv files(*.csv)|*.csv";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				beFileName.Text = ofd.FileName;
			}
		}
	}
}