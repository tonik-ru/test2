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
	public partial class ExportForm : Form
	{
		public List<WareInfo> Data { get; set; }
		public string ExportFileName { get; set; }
		public string Fields { get; set; }
		public string Headers { get; set; }


		public ExportForm()
		{
			InitializeComponent();
		}

		public void Export()
		{
			try
			{
				if (!string.IsNullOrEmpty(Fields))
				{
					var fieldsSpl = Fields.ToLower().Split(';').ToList();
					var headersSpl = Headers.Split(';').ToList();
					foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView1.Columns)
					{
						var p1 = fieldsSpl.IndexOf(col.FieldName.ToLower());
						if (!fieldsSpl.Contains(col.FieldName.ToLower()))
							col.Visible = false;
						else
						{
							col.VisibleIndex = p1;
							if (!string.IsNullOrEmpty(Headers))
								col.Caption = headersSpl[p1];
						}
					}
				}								
				DevExpress.XtraPrinting.XlsxExportOptions o = new DevExpress.XtraPrinting.XlsxExportOptions();
				if (ExportFileName.ToLower().EndsWith(".xlsx"))
					gridControl1.ExportToXlsx(ExportFileName);
				else
					gridControl1.ExportToXls(ExportFileName);
			}
			catch (Exception err)
			{
				Program.Log.Error(err);
				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
			}
			this.Close();
		}

		private void SaveGridForm_Load(object sender, EventArgs e)
		{
			wareInfoBindingSource.DataSource = Data;
		}
	}
}