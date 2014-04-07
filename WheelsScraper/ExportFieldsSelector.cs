using DevExpress.XtraGrid.Views.Grid;
using LumenWorks.Framework.IO.Csv;
using Scraper.Lib.Main;
using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WheelsScraper
{
	public partial class ExportFieldsSelector : DevExpress.XtraEditors.XtraUserControl
	{
		public ScraperSettings Sett { get; set; }
		public List<FieldInfo> Fields;
		public Object FieldType { get; set; }

		public DevExpress.XtraBars.Ribbon.RibbonControl Ribbon { get { return ribbonControl1; } }

		protected ISimpleScraper _scraper;
		public ISimpleScraper Scraper
		{
			get
			{
				return _scraper;
			}
			set
			{
				_scraper = value;
				RefreshFields();
			}
		}
		public ExportProfile SelectedProfile { get; set; }

		public ExportFieldsSelector()
		{
			InitializeComponent();
			//Fields = new List<FieldInfo>();
		}

		private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
		{
			var item = fieldInfoBindingSource.Current as FieldInfo;
			//if (item != null)
			{
				//if (!string.IsNullOrEmpty(item.FieldName) && gridView1.FocusedColumn != colExport)
				//{
				//	//e.Cancel = true;
				//}

				if (gridView1.FocusedColumn == colFieldName)
				{
					InitFieldNames();
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var item = (FieldInfo)fieldInfoBindingSource.AddNew();
			item.VisibleIndex = Fields.Count - 1;
			gridView1.RefreshData();
			//RenumRows();
		}

		private void RenumRows(List<FieldInfo> fields, GridView gw)
		{
			for (int i = 0; i < fields.Count; i++)
				fields[gw.GetDataSourceRowIndex(i)].VisibleIndex = i;
		}

		protected void MoveUp(GridView gv)
		{
			var fields = Fields;
			if (gv == gridView2)
				fields = currentProfile.AdditionalStaticFields;
			RenumRows(fields, gv);
			var curRowPos = gv.FocusedRowHandle;

			if (curRowPos < 0)
				return;
			var p1 = gv.GetDataSourceRowIndex(curRowPos);
			var p2 = gv.GetDataSourceRowIndex(curRowPos - 1);
			if (p1 < 0 || p2 < 0)
				return;

			fields[p2].VisibleIndex += 1;
			fields[p1].VisibleIndex -= 1;
			gv.RefreshData();
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			MoveUp(gridView1);
		}

		private void gridView1_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
		{
			e.Handled = true;
			e.Result = System.Collections.Comparer.Default.Compare(((FieldInfo)e.RowObject1).VisibleIndex, ((FieldInfo)e.RowObject2).VisibleIndex);
			if (e.SortOrder == DevExpress.Data.ColumnSortOrder.Descending)
				e.Result = -e.Result;
		}

		private void ExportFieldsSelector_Load(object sender, EventArgs e)
		{
			ricbUnitConv.Items.Add("");
			ricbUnitConv.Items.AddRange(Databox.UnitConverters.UnitConverter.GetConvertions());
			ricbCase.Items.AddRange(Enum.GetNames(typeof(Scraper.Shared.Export.Case)));
		}

		protected void RefreshFields()
		{
			if (Scraper == null)
				return;
			if (!Sett.ExportProfiles.Where(x => x.IsDefault).Any())
				Sett.ExportProfiles[0].IsDefault = true;

			bsProfiles.DataSource = Sett;
			bsProfiles.Position = 0;
			cbrProfiles.Refresh();

			fieldInfoBindingSource.ResetBindings(false);
			bsProfiles.ResetBindings(false);
			var r = bsProfiles.Current;
			cbrProfiles.ItemIndex = 0;

			if (Sett.ExportProfiles.Count == 1 && Sett.ExportProfiles[0].Fields.Count == 0)
				CreateNewDefaultProfile(Sett.ExportProfiles[0]);

			SelectedProfile = Sett.ExportProfiles[0];
			if (SelectedProfile != null)
			{
				cbrProfiles.ItemIndex = Sett.ExportProfiles.IndexOf(SelectedProfile);
				ShowFields();
			}
		}

		protected void ShowFields()
		{
			//var profile = (ExportProfile)cbrProfiles.EditValue;
			//if (cbrProfiles.ItemIndex == -1)
			//	profile = null;
			var profile = (ExportProfile)bsProfiles.Current;
			currentProfile = profile;
			if (profile == null)
			{
				fieldInfoBindingSource.DataSource = null;
			}
			else
			{
				fieldInfoBindingSource.DataSource = profile.Fields;
				Fields = profile.Fields;
			}
			fieldInfoBindingSource.DataSource = profile.Fields;
			fieldInfoBindingSource.ResetBindings(false);
		}

		private void InitFieldNamesForUniversal()
		{
			cbFieldNames.Items.Clear();
			cbFieldNames.Items.Add("");
			var fieldsInType = (Scraper as IFieldInfoProvider).GetFieldNames();
			var unusedFields = fieldsInType.Where(x => !Fields.Select(y => y.FieldName).Contains(x));
			foreach (var field in unusedFields)
			{
				cbFieldNames.Items.Add(field);
			}
			var usedFields = fieldsInType.Where(x => Fields.Select(y => y.FieldName).Contains(x));
			if (usedFields.Count() > 0)
			{
				cbFieldNames.Items.Add("<--BELOW HEADERS ARE SELECTED-->");

				foreach (var field in usedFields)
				{
					cbFieldNames.Items.Add(field);
				}
			}
		}

		private void InitFieldNames()
		{
			if (Scraper is IFieldInfoProvider)
			{
				InitFieldNamesForUniversal();
				return;
			}
			var fieldsInType = FieldType.GetType().GetProperties();
			cbFieldNames.Items.Clear();
			cbFieldNames.Items.Add("");
			var unusedFields = fieldsInType.Where(x => !Fields.Select(y => y.FieldName).Contains(x.Name));
			foreach (var field in unusedFields)
			{
				cbFieldNames.Items.Add(field.Name);
			}
			var usedFields = fieldsInType.Where(x => Fields.Select(y => y.FieldName).Contains(x.Name));
			if (usedFields.Count() > 0)
			{
				cbFieldNames.Items.Add("<--BELOW HEADERS ARE SELECTED-->");

				foreach (var field in usedFields)
				{
					cbFieldNames.Items.Add(field.Name);
				}
			}
		}

		protected void MoveDown(GridView gv)
		{
			var fields = Fields;
			if (gv == gridView2)
				fields = currentProfile.AdditionalStaticFields;
			RenumRows(fields, gv);
			var curRowPos = gv.FocusedRowHandle;

			if (curRowPos < 0)
				return;
			var p1 = gv.GetDataSourceRowIndex(curRowPos);
			var p2 = gv.GetDataSourceRowIndex(curRowPos + 1);
			if (p1 < 0 || p2 < 0)
				return;
			fields[p1].VisibleIndex += 1;
			fields[p2].VisibleIndex -= 1;
			gv.RefreshData();
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			MoveDown(gridView1);
		}

		private void btnDel_Click(object sender, EventArgs e)
		{
			var item = fieldInfoBindingSource.Current as FieldInfo;
			if (item == null)
				return;
			if (!string.IsNullOrEmpty(item.FieldName))
				return;
			fieldInfoBindingSource.RemoveCurrent();
		}

		private void fieldInfoBindingSource_CurrentChanged(object sender, EventArgs e)
		{
			var item = (FieldInfo)fieldInfoBindingSource.Current;
			if (item == null)
				return;
			//btnDel.Enabled = item.FieldName == null;
		}

		private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
		{
			if (gridView1.GetDataSourceRowIndex(e.RowHandle) < 0)
				return;
			var item = Fields[gridView1.GetDataSourceRowIndex(e.RowHandle)];
			if (item == null)
				return;

			if (e.Column == colCustomValue)
			{
				{
					if (item.FieldName == null)
						e.Appearance.BackColor = Color.Yellow;
					else
						e.Appearance.BackColor = Color.LightGray;
				}
			}
			if (e.Column == colFieldName)
			{
				{
					if (item.FieldName == null)
					{
						e.Appearance.BackColor = Color.LightGray;
					}
				}
			}
		}

		private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
		{
			var item = (FieldInfo)e.Row;
			if (!string.IsNullOrEmpty(item.CustomValue) && !string.IsNullOrEmpty(item.FieldName))
			{
				e.Valid = false;
				gridView1.SetColumnError(colCustomValue, "If \"Field Name\" is set, you can't use \"Custom Value\"");
				gridView1.SetColumnError(colFieldName, "If \"Field Name\" is set, you can't use \"Custom Value\"");
			}
		}

		private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
		{
			e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
		}

		private void fieldInfoBindingSource_AddingNew(object sender, AddingNewEventArgs e)
		{
			e.NewObject = new FieldInfo { VisibleIndex = Fields.Count - 1, Export = true };
		}

		private void AddAllFields()
		{
			var fieldsInType = PropHelper.GetProperties(Scraper);
			int i = 0;
			foreach (var field in fieldsInType)
			{
				var f1 = Fields.Where(x => string.Compare(x.FieldName, field, true) == 0).FirstOrDefault();
				if (f1 != null)
					continue;
				f1 = Fields.Where(x => string.Compare(x.Header, field, true) == 0).FirstOrDefault();
				if (f1 != null)
				{ f1.CustomValue = null; f1.FieldName = field; }
				else
					Fields.Add(new FieldInfo { Export = true, FieldName = field, Header = field, VisibleIndex = Fields.Count });
			}
			fieldInfoBindingSource.ResetBindings(false);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			AddAllFields();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			RenumRows(currentProfile.Fields, gridView1);
			RenumRows(currentProfile.AdditionalStaticFields, gridView2);
			//this.DialogResult = System.Windows.Forms.DialogResult.OK;
			//this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			//this.Close();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			RemoveAllFields();
		}

		private void RemoveAllFields()
		{
			//if (currentProfile.IsDefault)
			//{
			//	MessageBox.Show("You can't clear all fields for default profile");
			//	return;
			//}
			if (MessageBox.Show("Clear all fields?", "", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
				return;
			Fields.Clear();
			fieldInfoBindingSource.ResetBindings(false);
		}

		protected void MoveToTop(GridView gv)
		{
			var curRowPos = gv.FocusedRowHandle;

			if (curRowPos < 0)
				return;

			var fields = Fields;
			if (gv == gridView2)
				fields = currentProfile.AdditionalStaticFields;

			var p1 = gv.GetDataSourceRowIndex(curRowPos);
			fields[p1].VisibleIndex = -100000;
			gv.RefreshData();
			RenumRows(fields, gv);
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			MoveToTop(gridView1);
		}

		protected void MoveToDown(GridView gv)
		{
			var curRowPos = gv.FocusedRowHandle;

			if (curRowPos < 0)
				return;

			var fields = Fields;
			if (gv == gridView2)
				fields = currentProfile.AdditionalStaticFields;

			var p1 = gv.GetDataSourceRowIndex(curRowPos);
			fields[p1].VisibleIndex = 100000;
			gv.RefreshData();
			RenumRows(fields, gv);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			MoveToDown(gridView1);
		}

		private void btnCheckAll_Click(object sender, EventArgs e)
		{
			CheckAll();
		}

		private void CheckAll()
		{
			if (Fields.Count == 0)
				return;
			bool check = !Fields[0].Export;
			Fields.ForEach(x => x.Export = check);
			fieldInfoBindingSource.ResetBindings(false);
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			fieldInfoBindingSource.AddNew();
		}

		protected void DeleteCurrentField()
		{
			if (fieldInfoBindingSource.Current != null)
				fieldInfoBindingSource.RemoveCurrent();
		}

		private void btnDel_Click_1(object sender, EventArgs e)
		{
			DeleteCurrentField();
		}

		private void bsProfiles_CurrentChanged(object sender, EventArgs e)
		{
			//cbrProfiles.EditValue = bsProfiles.Current;
			ShowFields();
		}

		private void btnAddProfile_Click(object sender, EventArgs e)
		{
			AddProfile();
		}

		protected void CreateNewDefaultProfile(ExportProfile prof)
		{
			ProfileManager.CreateExportProfile(Scraper, Sett, prof);
			SelectedProfile = prof;
		}

		protected void AddProfile()
		{
			//RenameProfileForm frm = new RenameProfileForm();
			//frm.Text = "Add new profile";
			//if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				ExportProfile prof = new ExportProfile { Name = "New profile" };

				if (Scraper is IFieldInfoProvider)
					prof = ProfileManager.CreateExportProfile(Scraper, Sett);

				if (prof != null)
					bsProfiles.Add(prof);
				SelectedProfile = prof;
			}
		}

		private void cbrProfiles_EditValueChanged(object sender, EventArgs e)
		{
			var p1 = bsProfiles.IndexOf(cbrProfiles.EditValue);
			if (p1 != -1)
				bsProfiles.Position = p1;
			//ShowFields();
		}

		private void btnDelProfile_Click(object sender, EventArgs e)
		{
			DeleteCurrentProfile();
		}

		ExportProfile currentProfile;

		protected void DeleteCurrentProfile()
		{
			if (Sett.ExportProfiles.Count <= 1)
			{
				MessageBox.Show("You can't delete the last profile");
				return;
			}

			if (MessageBox.Show("Delete profile \"" + currentProfile.Name + "\"?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
			{
				var name = currentProfile.Name;
				var isDefault = currentProfile.IsDefault;
				bsProfiles.Remove(currentProfile);
				cbrProfiles.ItemIndex = 0;

				ShowFields();
				if (isDefault)
					SetCurrentProfileAdDefault(true);
				bsProfiles.ResetCurrentItem();

				var curDefName = currentProfile.Name;
				Sett.LocalExportSettings.Where(x => x.ProfileName == name).ToList()
					.ForEach(x => x.ProfileName = curDefName);
				Sett.FtpExportSettings.Where(x => x.ProfileName == name).ToList()
					.ForEach(x => x.ProfileName = curDefName);
			}
		}

		private void btnRenameProfile_Click(object sender, EventArgs e)
		{
			RenameProfile();
		}

		private void RenameProfile()
		{

			if (currentProfile == null)
				return;
			RenameProfileForm frm = new RenameProfileForm();
			frm.Text = "Rename profile";
			frm.InputValue = currentProfile.Name;
			if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Sett.LocalExportSettings.Where(x => x.ProfileName == currentProfile.Name).ToList()
					.ForEach(x => x.ProfileName = frm.InputValue);
				Sett.FtpExportSettings.Where(x => x.ProfileName == currentProfile.Name).ToList()
					.ForEach(x => x.ProfileName = frm.InputValue);
				currentProfile.Name = frm.InputValue;
			}
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			ImportProfile();
		}

		private void ImportProfile()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "All files (*.*)|*.*";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				DoImportProfile(ofd.FileName);
			}
		}

		private void DoImportProfile(string fileName)
		{
			try
			{
				using (var sw = File.OpenText(fileName))
				{
					string strHead = sw.ReadLine();
					string strVals = sw.ReadLine();
					if (strHead == null)// || strVals == null)
					{
						MessagesPanelManager.PrintMessage("Incorrect file format", ImportanceLevel.High);
						return;
					}

					var headers = strHead.Split(',');
					string[] fieldVals = new string[headers.Length];
					if (strVals != null)
						fieldVals = strVals.Split(',');
					if (headers.Length != fieldVals.Length)
					{
						MessagesPanelManager.PrintMessage("Incorrect file format: number of headers and number of values are different", ImportanceLevel.High);
						return;
					}
					var fieldsInType = PropHelper.GetProperties(Scraper);
					Fields.Clear();
					for (int i = 0; i < headers.Length; i++)
					{
						var fn = (fieldVals[i] ?? "").Trim();
						var header = headers[i].Trim();
						var foundField = fieldsInType.Where(x => string.Compare(x, header, true) == 0).FirstOrDefault();
						string field = null;
						string customVal = null;
						if (foundField != null)
							field = foundField;
						else
							customVal = fn;
						Fields.Add(new FieldInfo { Export = true, Header = header, VisibleIndex = i, CustomValue = customVal, FieldName = field });
					}
					fieldInfoBindingSource.ResetBindings(false);
				}
			}
			catch (Exception err)
			{
				Program.Log.Error(err);
				MessagesPanelManager.PrintMessage(err.ToString(), ImportanceLevel.High);
			}
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

		private void DoExportProfile(string fileName)
		{
			try
			{
				using (var sw = new StreamWriter(fileName, false))
				{
					string strHead = null;
					string strVals = null;

					foreach (var field in currentProfile.Fields)
					{
						if (strHead != null)
							strHead += ",";
						strHead += field.Header;
						if (strVals != null)
							strVals += ",";
						if (!string.IsNullOrEmpty(field.CustomValue))
							strVals += field.CustomValue;
						else
							strVals += field.FieldName;
					}
					sw.WriteLine(strHead);
					sw.WriteLine(strVals);
				}
			}
			catch (Exception err)
			{
				Program.Log.Error(err);
				MessagesPanelManager.PrintMessage(err.ToString(), ImportanceLevel.High);
			}
		}

		private void tsmiAddProfile_Click(object sender, EventArgs e)
		{
			AddProfile();
		}

		private void tsmiRenameProfile_Click(object sender, EventArgs e)
		{
			RenameProfile();
		}

		private void tsmiDelProfile_Click(object sender, EventArgs e)
		{
			DeleteCurrentProfile();
		}

		private void tsmiImportHeaders_Click(object sender, EventArgs e)
		{
			ImportProfile();
		}

		private void tsmiExportHeaders_Click(object sender, EventArgs e)
		{
			ExportProfile();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{

		}

		protected void SetCurrentProfileAdDefault(bool setDefault)
		{
			if (setDefault)
			{
				Sett.ExportProfiles.ForEach(x => { x.IsDefault = false; });
				currentProfile.IsDefault = true;
			}
			else
			{
				Sett.ExportProfiles[0].IsDefault = true;
			}
		}

		private void checkBox1_Click(object sender, EventArgs e)
		{
			SetCurrentProfileAdDefault(checkBox1.Checked);
		}

		private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			PriceRuleEditorForm frm = new PriceRuleEditorForm();
			var curField = (FieldInfo)fieldInfoBindingSource.Current;
			if (curField == null)
				return;

			ProfileRule rule = new ProfileRule();
			if (curField.ProfileRule != null)
			{
				rule.Name = curField.ProfileRule.Name;
				rule.RuleType = curField.ProfileRule.RuleType;
				rule.Value = curField.ProfileRule.Value;
			}
			frm.Rule = rule;
			if (frm.ShowDialog() == DialogResult.OK)
			{
				curField.ProfileRule = frm.Rule;
				gridView1.UpdateCurrentRow();
			}
		}

		private void resetToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetToDefault();
		}

		protected void ResetToDefault()
		{
			//if(MessageBox.Show("Reset profile to defaults?", );)
		}

		private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			AddProfile();
		}

		private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			RenameProfile();
		}

		private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			DeleteCurrentProfile();
		}

		private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			ImportProfile();
		}

		private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			ExportProfile();
		}

		protected void AddField()
		{
			fieldInfoBindingSource.AddNew();
		}

		private void bbiAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			AddField();
		}

		private void bbiDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			DeleteCurrentField();
		}

		private void bbiAddAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			AddAllFields();
		}

		private void bbiClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			RemoveAllFields();
		}

		private void bsProfiles_CurrentItemChanged(object sender, EventArgs e)
		{
			currentProfile = (ExportProfile)bsProfiles.Current;
			RefreshBindings();
		}

		protected void RefreshBindings()
		{
			if (currentProfile.UseStaticFields)
			{
				lcgStatic.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
			}
			else
			{
				lcgStatic.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			}
			bbiClearAdd.Enabled = currentProfile.UseStaticFields;
			bbiLoadAddColumns.Enabled = currentProfile.UseStaticFields;
		}

		private void button2_Click_1(object sender, EventArgs e)
		{
			MoveUp(gridView2);
		}

		private void button3_Click_1(object sender, EventArgs e)
		{
			MoveToTop(gridView2);
		}

		private void button5_Click(object sender, EventArgs e)
		{
			MoveToDown(gridView2);
		}

		private void button6_Click(object sender, EventArgs e)
		{
			MoveDown(gridView2);
		}

		private void ExportFieldsSelector_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible)
				RefreshFields();
		}

		private void gridView2_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
		{
			e.Handled = true;
			e.Result = System.Collections.Comparer.Default.Compare(((FieldInfo)e.RowObject1).VisibleIndex, ((FieldInfo)e.RowObject2).VisibleIndex);
			if (e.SortOrder == DevExpress.Data.ColumnSortOrder.Descending)
				e.Result = -e.Result;
		}

		private void bbiLoadAddColumns_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			LoadAddColumns();
		}

		protected void LoadAddColumns()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "All files (*.*)|*.*";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() != DialogResult.OK)
				return;

			var filePath = ofd.FileName;
			using (var sr = new StreamReader(filePath))
			{
				var csv = new CsvReader(sr, true, ',');
				var columns = csv.GetFieldHeaders();
				if (csv.ReadNextRecord())
				{
					for (int i = 0; i < columns.Length - 1; i += 2)
					{
						var colName = csv[i];
						var colVal = csv[i + 1];

						var foundCol = currentProfile.AdditionalStaticFields.Where(x => string.Compare(x.Header, colName, true) == 0).FirstOrDefault();
						if (foundCol == null)
						{
							foundCol = new FieldInfo();
							currentProfile.AdditionalStaticFields.Add(foundCol);
							foundCol.VisibleIndex = currentProfile.AdditionalStaticFields.Max(x => x.VisibleIndex) + 1;
							foundCol.Header = colName;
							foundCol.Export = true;
						}
						foundCol.CustomValue = colVal;
					}
				}
				else
					throw new Exception("Incorrect file format. No data found");
			}
			bsProfiles.ResetBindings(false);
		}

		private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			if (MessageBox.Show("Clear all additional static fields?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
				return;
			ClearAddFields();
		}

		protected void ClearAddFields()
		{
			currentProfile.AdditionalStaticFields.Clear();
			bsProfiles.ResetBindings(false);
		}
	}
}