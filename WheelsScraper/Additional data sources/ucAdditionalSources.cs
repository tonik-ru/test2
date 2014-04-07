using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace WheelsScraper
{
	public partial class ucAdditionalSources : XtraUserControl
	{
		private ScraperSettings _sett;
		public ScraperSettings Sett
		{
			get
			{
				return _sett;
			}
			set
			{
				if (value == null)
					return;
				_sett = value;
				RefreshDataSources();
			}
		}

		private ExtendedScraperSettings ExtSettings { get; set; }

		public ucAdditionalSources()
		{
			InitializeComponent();
		}

		private void ucAdditionalSources_Load(object sender, EventArgs e)
		{
			RefreshDataSources();
		}

		protected void RefreshDataSources()
		{
			if (Sett == null)
				return;
			ExtSettings = AppSettings.Default.GetExtSettings(Sett.Name);

			gridControl1.Enabled = ExtSettings.ValidateAgainstAddData;

			extendedScraperSettingsBindingSource.DataSource = ExtSettings;
			additionalDataSourceBindingSource.DataSource = ExtSettings.AddDataSourceFiles;
		}

		private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
		{
			var row = (AdditionalDataSource)gridView1.GetFocusedRow();
			if (gridView1.FocusedColumn == colFile)
			{
				if (!string.IsNullOrEmpty(row.File))
					InitSheetsNamesCB(row.File);
			}
			if (gridView1.FocusedColumn == colBrandColumnName || gridView1.FocusedColumn == colPartNoColumnName)
			{
				if (!string.IsNullOrEmpty(row.File))
					InitColumnNamesCB(row.File, row.SheetName);
			}
			if (gridView1.FocusedColumn == colSheetName)
			{
				if (!string.IsNullOrEmpty(row.File))
					InitSheetsNamesCB(row.File);
			}
			//if (gridView1_ShowingEditor.FocusedColumn == colPartNoColumnName)
			//{
			//	InitPKFields(row);
			//}
			//if (gridView4.FocusedColumn == colFK)
			//{
			//	if (!string.IsNullOrEmpty(row.FileName))// && !string.IsNullOrEmpty(row.SheetName))
			//		InitFKFields(row.FileName, row.SheetName);
			//}
		}

		private void InitColumnNamesCB(string fileName, string sheetName)
		{
			ricbColBrand.Items.Clear();
			ricbColBrand.Items.AddRange(Databox.Libs.Common.XlsReader.GetSheetColumnNames(fileName, sheetName));

			ricbColPartNo.Items.Clear();
			ricbColPartNo.Items.AddRange(Databox.Libs.Common.XlsReader.GetSheetColumnNames(fileName, sheetName));
		}

		private void InitSheetsNamesCB(string fileName)
		{
			ricbSheetName.Items.Clear();
			ricbSheetName.Items.AddRange(Databox.Libs.Common.XlsReader.GetSheetNames(fileName));
		}

		private void ribePickFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			var file = PickFile("Excel files (*.xls*;*.csv)|*.xls*;*.csv");
			if (file != null)
				((DevExpress.XtraEditors.ButtonEdit)(sender)).Text = file;
		}

		protected string PickFile(string filter)
		{
			var ofd = new OpenFileDialog();
			ofd.Filter = filter;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				return ofd.FileName;
			}
			return null;
		}

		private void checkEdit1_CheckedChanged(object sender, EventArgs e)
		{
			gridControl1.Enabled = ExtSettings.ValidateAgainstAddData;
		}

		private void ucAdditionalSources_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
				RefreshDataSources();
		}
	}
}