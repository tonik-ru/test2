namespace WheelsScraper
{
	partial class ucAdditionalSources
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
			this.extendedScraperSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.gridControl1 = new DevExpress.XtraGrid.GridControl();
			this.additionalDataSourceBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colFile = new DevExpress.XtraGrid.Columns.GridColumn();
			this.ribePickFile = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
			this.colBrandColumnName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.ricbColBrand = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.colPartNoColumnName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.ricbColPartNo = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.colSheetName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.ricbSheetName = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.extendedScraperSettingsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.additionalDataSourceBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ribePickFile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ricbColBrand)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ricbColPartNo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ricbSheetName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
			this.SuspendLayout();
			// 
			// layoutControl1
			// 
			this.layoutControl1.Controls.Add(this.checkEdit1);
			this.layoutControl1.Controls.Add(this.gridControl1);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(296, 278);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			// 
			// checkEdit1
			// 
			this.checkEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.extendedScraperSettingsBindingSource, "ValidateAgainstAddData", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkEdit1.Location = new System.Drawing.Point(2, 2);
			this.checkEdit1.Name = "checkEdit1";
			this.checkEdit1.Properties.Caption = "Use additional data sources";
			this.checkEdit1.Size = new System.Drawing.Size(292, 19);
			this.checkEdit1.StyleController = this.layoutControl1;
			this.checkEdit1.TabIndex = 5;
			this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
			// 
			// extendedScraperSettingsBindingSource
			// 
			this.extendedScraperSettingsBindingSource.DataSource = typeof(WheelsScraper.ExtendedScraperSettings);
			// 
			// gridControl1
			// 
			this.gridControl1.DataSource = this.additionalDataSourceBindingSource;
			this.gridControl1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.gridControl1.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.gridControl1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.gridControl1.EmbeddedNavigator.Buttons.First.Visible = false;
			this.gridControl1.EmbeddedNavigator.Buttons.Last.Visible = false;
			this.gridControl1.EmbeddedNavigator.Buttons.Next.Visible = false;
			this.gridControl1.EmbeddedNavigator.Buttons.NextPage.Visible = false;
			this.gridControl1.EmbeddedNavigator.Buttons.Prev.Visible = false;
			this.gridControl1.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
			this.gridControl1.EmbeddedNavigator.TextLocation = DevExpress.XtraEditors.NavigatorButtonsTextLocation.None;
			this.gridControl1.Location = new System.Drawing.Point(2, 25);
			this.gridControl1.MainView = this.gridView1;
			this.gridControl1.Name = "gridControl1";
			this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ribePickFile,
            this.ricbColBrand,
            this.ricbColPartNo,
            this.ricbSheetName});
			this.gridControl1.Size = new System.Drawing.Size(292, 251);
			this.gridControl1.TabIndex = 4;
			this.gridControl1.UseEmbeddedNavigator = true;
			this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
			// 
			// additionalDataSourceBindingSource
			// 
			this.additionalDataSourceBindingSource.DataSource = typeof(WheelsScraper.AdditionalDataSource);
			// 
			// gridView1
			// 
			this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFile,
            this.colBrandColumnName,
            this.colPartNoColumnName,
            this.colSheetName});
			this.gridView1.GridControl = this.gridControl1;
			this.gridView1.Name = "gridView1";
			this.gridView1.OptionsCustomization.AllowGroup = false;
			this.gridView1.OptionsView.ShowGroupPanel = false;
			this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colFile, DevExpress.Data.ColumnSortOrder.Ascending)});
			this.gridView1.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gridView1_ShowingEditor);
			// 
			// colFile
			// 
			this.colFile.Caption = "* File";
			this.colFile.ColumnEdit = this.ribePickFile;
			this.colFile.FieldName = "File";
			this.colFile.Name = "colFile";
			this.colFile.Visible = true;
			this.colFile.VisibleIndex = 0;
			// 
			// ribePickFile
			// 
			this.ribePickFile.AutoHeight = false;
			this.ribePickFile.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.ribePickFile.Name = "ribePickFile";
			this.ribePickFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ribePickFile_ButtonClick);
			// 
			// colBrandColumnName
			// 
			this.colBrandColumnName.ColumnEdit = this.ricbColBrand;
			this.colBrandColumnName.FieldName = "BrandColumnName";
			this.colBrandColumnName.Name = "colBrandColumnName";
			this.colBrandColumnName.Visible = true;
			this.colBrandColumnName.VisibleIndex = 2;
			// 
			// ricbColBrand
			// 
			this.ricbColBrand.AutoHeight = false;
			this.ricbColBrand.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ricbColBrand.Name = "ricbColBrand";
			// 
			// colPartNoColumnName
			// 
			this.colPartNoColumnName.Caption = "* Part #";
			this.colPartNoColumnName.ColumnEdit = this.ricbColPartNo;
			this.colPartNoColumnName.FieldName = "PartNoColumnName";
			this.colPartNoColumnName.Name = "colPartNoColumnName";
			this.colPartNoColumnName.Visible = true;
			this.colPartNoColumnName.VisibleIndex = 3;
			// 
			// ricbColPartNo
			// 
			this.ricbColPartNo.AutoHeight = false;
			this.ricbColPartNo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ricbColPartNo.Name = "ricbColPartNo";
			// 
			// colSheetName
			// 
			this.colSheetName.Caption = "Sheet";
			this.colSheetName.ColumnEdit = this.ricbSheetName;
			this.colSheetName.FieldName = "SheetName";
			this.colSheetName.Name = "colSheetName";
			this.colSheetName.Visible = true;
			this.colSheetName.VisibleIndex = 1;
			// 
			// ricbSheetName
			// 
			this.ricbSheetName.AutoHeight = false;
			this.ricbSheetName.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ricbSheetName.Name = "ricbSheetName";
			this.ricbSheetName.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			// 
			// layoutControlGroup1
			// 
			this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(296, 278);
			this.layoutControlGroup1.Text = "layoutControlGroup1";
			this.layoutControlGroup1.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.gridControl1;
			this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 23);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(296, 255);
			this.layoutControlItem1.Text = "layoutControlItem1";
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextToControlDistance = 0;
			this.layoutControlItem1.TextVisible = false;
			// 
			// layoutControlItem2
			// 
			this.layoutControlItem2.Control = this.checkEdit1;
			this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(296, 23);
			this.layoutControlItem2.Text = "layoutControlItem2";
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextToControlDistance = 0;
			this.layoutControlItem2.TextVisible = false;
			// 
			// ucAdditionalSources
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.layoutControl1);
			this.Name = "ucAdditionalSources";
			this.Size = new System.Drawing.Size(296, 278);
			this.Load += new System.EventHandler(this.ucAdditionalSources_Load);
			this.VisibleChanged += new System.EventHandler(this.ucAdditionalSources_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.extendedScraperSettingsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.additionalDataSourceBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ribePickFile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ricbColBrand)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ricbColPartNo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ricbSheetName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl1;
		private DevExpress.XtraGrid.GridControl gridControl1;
		private System.Windows.Forms.BindingSource additionalDataSourceBindingSource;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
		private DevExpress.XtraGrid.Columns.GridColumn colFile;
		private DevExpress.XtraGrid.Columns.GridColumn colBrandColumnName;
		private DevExpress.XtraGrid.Columns.GridColumn colPartNoColumnName;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit ribePickFile;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ricbColBrand;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ricbColPartNo;
		private DevExpress.XtraGrid.Columns.GridColumn colSheetName;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ricbSheetName;
		private DevExpress.XtraEditors.CheckEdit checkEdit1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
		private System.Windows.Forms.BindingSource extendedScraperSettingsBindingSource;
	}
}
