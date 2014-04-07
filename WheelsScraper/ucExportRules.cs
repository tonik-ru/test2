using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WheelsScraper.Export;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace WheelsScraper
{
	public partial class ucExportRules : UserControl
	{
		public DevExpress.XtraBars.Ribbon.RibbonControl Ribbon { get { return ribbonControl1; } }

		public ISimpleScraper Scrap { get; set; }
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

		public ucExportRules()
		{
			InitializeComponent();
		}

		private void ucExportRules_Load(object sender, EventArgs e)
		{
			ricbRuleType.Items.AddRange(Enum.GetNames(typeof(OperationType)));
		}

		protected void RefreshDataSources()
		{
			if (Sett == null)
				return;
			ricbFieldHeader.Items.Clear();
			var fieldsInType = Scraper.Lib.Main.PropHelper.GetProperties(Scrap);

			//var allFieldHeaders = Sett.ExportProfiles.SelectMany(x => x.Fields).Select(x => x.Header);
			//ricbFieldHeader.Items.AddRange(allFieldHeaders.ToArray());
			ricbFieldHeader.Items.AddRange(fieldsInType);

			exportFieldRuleBindingSource.DataSource = Sett.ExportRules;

			ucFormulaEdit1.Fields = fieldsInType;
		}

		private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
		{
			var rule = (Export.ExportFieldRule)e.Row;
			if (string.IsNullOrEmpty(rule.FieldName))
			{
				e.Valid = false;
				gridView1.SetColumnError(colFieldHeader, "Please select column");
			}
		}

		private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
		{
			e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
		}

		private void riFormula_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
		}

		private void riFormula_QueryPopUp(object sender, CancelEventArgs e)
		{
			var item = (ExportFieldRule)gridView1.GetFocusedRow();
			var val = ((DevExpress.XtraEditors.PopupBaseEdit)(sender)).EditValue;

			if (val != null)
				ucFormulaEdit1.Formula = val.ToString();
			else
				ucFormulaEdit1.Formula = "";
		}

		private void riFormula_QueryResultValue(object sender, DevExpress.XtraEditors.Controls.QueryResultValueEventArgs e)
		{
			e.Value = ucFormulaEdit1.Formula;
		}

		private void ucExportRules_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
				RefreshDataSources();
		}

		private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
		{
			gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["Enabled"], true);
		}

		private void RenumRows(List<ExportFieldRule> fields, GridView gw)
		{
			for (int i = 0; i < fields.Count; i++)
				fields[gw.GetDataSourceRowIndex(i)].Order = i;
		}

		protected void MoveDown(GridView gv)
		{
			var fields = Sett.ExportRules;

			RenumRows(fields, gv);
			var curRowPos = gv.FocusedRowHandle;

			if (curRowPos < 0)
				return;
			var p1 = gv.GetDataSourceRowIndex(curRowPos);
			var p2 = gv.GetDataSourceRowIndex(curRowPos + 1);
			if (p1 < 0 || p2 < 0)
				return;
			fields[p1].Order += 1;
			fields[p2].Order -= 1;
			gv.RefreshData();
		}

		protected void MoveUp(GridView gv)
		{
			var fields = Sett.ExportRules;

			RenumRows(fields, gv);
			var curRowPos = gv.FocusedRowHandle;

			if (curRowPos < 0)
				return;
			var p1 = gv.GetDataSourceRowIndex(curRowPos);
			var p2 = gv.GetDataSourceRowIndex(curRowPos - 1);
			if (p1 < 0 || p2 < 0)
				return;

			fields[p2].Order += 1;
			fields[p1].Order -= 1;
			gv.RefreshData();
		}


		protected void MoveToTop(GridView gv)
		{
			var curRowPos = gv.FocusedRowHandle;

			if (curRowPos < 0)
				return;

			var fields = Sett.ExportRules;

			var p1 = gv.GetDataSourceRowIndex(curRowPos);
			fields[p1].Order = -100000;
			gv.RefreshData();
			RenumRows(fields, gv);
		}

		protected void MoveToDown(GridView gv)
		{
			var curRowPos = gv.FocusedRowHandle;

			if (curRowPos < 0)
				return;

			var fields = Sett.ExportRules;

			var p1 = gv.GetDataSourceRowIndex(curRowPos);
			fields[p1].Order = 100000;
			gv.RefreshData();
			RenumRows(fields, gv);
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			MoveUp(gridView1);
		}

		private void btnTop_Click(object sender, EventArgs e)
		{
			MoveToTop(gridView1);
		}

		private void btnLow_Click(object sender, EventArgs e)
		{
			MoveToDown(gridView1);
		}

		private void gridView1_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
		{
			e.Handled = true;
			e.Result = System.Collections.Comparer.Default.Compare(((ExportFieldRule)e.RowObject1).Order, ((ExportFieldRule)e.RowObject2).Order);
			if (e.SortOrder == DevExpress.Data.ColumnSortOrder.Descending)
				e.Result = -e.Result;
		}

		private void exportFieldRuleBindingSource_AddingNew(object sender, AddingNewEventArgs e)
		{
			e.NewObject = new ExportFieldRule { Order = Sett.ExportRules.Count };
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			MoveDown(gridView1);
		}

		private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			ExportProfile();
		}

		protected void ExportProfile()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "All files (*.csv)|*.csv";
			sfd.RestoreDirectory = true;
			if (sfd.ShowDialog() != DialogResult.OK)
				return;
			DoExportProfile(sfd.FileName);
		}

		protected void AddField(StringBuilder sb, string val)
		{
			if (sb.Length > 0)
				sb.Append(",");
			var tmpVal = val.Replace("\"", "\"\"");
			sb.Append("\"" + tmpVal + "\"");
		}

		private void DoExportProfile(string fileName)
		{
			using (var sw = new StreamWriter(fileName, false))
			{
				string strHead = "Enabled,Field name,Rule type,Search string,Match case,Replace string";
				sw.WriteLine(strHead);

				foreach (var r in Sett.ExportRules.OrderBy(x => x.Order))
				{
					var sb = new StringBuilder();
					AddField(sb, r.Enabled.ToString());
					AddField(sb, r.FieldName);
					AddField(sb, r.RuleType.ToString());
					AddField(sb, r.SearchString);
					AddField(sb, r.MatchCase.ToString());
					AddField(sb, r.ReplaceString);
					sw.WriteLine(sb.ToString());
				}
			}
		}

		private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			ImportProfile();
		}

		private void ImportProfile()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "All files (*.csv)|*.csv";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				DoImportProfile(ofd.FileName);
			}
		}

		private void DoImportProfile(string fileName)
		{
			int order = 0;
			if (Sett.ExportRules.Any())
				order = Sett.ExportRules.Max(x => x.Order) + 1;
			using (var sw = File.OpenText(fileName))
			{
				using (var csv = new CsvReader(sw, true))
				{
					while (csv.ReadNextRecord())
					{
						if (string.IsNullOrEmpty(csv["Field name"]))
							continue;
						var rule = new ExportFieldRule();
						rule.FieldName = csv["Field name"];
						rule.RuleType = (OperationType)Enum.Parse(typeof(OperationType), csv["Rule type"]);
						rule.SearchString = csv["Search string"];
						rule.ReplaceString = csv["Replace string"];
						rule.MatchCase = bool.Parse(csv["Match case"]);
						rule.Enabled = bool.Parse(csv["Enabled"]); ;
						rule.Order = order++;
						Sett.ExportRules.Add(rule);
					}
				}
			}
			exportFieldRuleBindingSource.ResetBindings(false);
		}

		private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			if(!Sett.ExportRules.Any())
				return;
			if (MessageBox.Show("Delete all rules?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				return;
			Sett.ExportRules.Clear();
			exportFieldRuleBindingSource.ResetBindings(false);
		}
	}
}