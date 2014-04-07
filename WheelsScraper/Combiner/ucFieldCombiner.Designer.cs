namespace WheelsScraper.Combiner
{
	partial class ucFieldCombiner
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
			this.gridControl3 = new DevExpress.XtraGrid.GridControl();
			this.valuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.categoriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.categoryInfosBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.bsSett = new System.Windows.Forms.BindingSource(this.components);
			this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colVisible = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colValue = new DevExpress.XtraGrid.Columns.GridColumn();
			this.ripceFormula = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
			this.popupContainerControl1 = new DevExpress.XtraEditors.PopupContainerControl();
			this.ucFormulaEdit1 = new WheelsScraper.ucFormulaEdit();
			this.gridControl2 = new DevExpress.XtraGrid.GridControl();
			this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colOutputHeader = new DevExpress.XtraGrid.Columns.GridColumn();
			this.ricbHeaders = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.gridControl1 = new DevExpress.XtraGrid.GridControl();
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
			this.splitterItem2 = new DevExpress.XtraLayout.SplitterItem();
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridControl3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.valuesBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.categoriesBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.categoryInfosBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsSett)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ripceFormula)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
			this.popupContainerControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ricbHeaders)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).BeginInit();
			this.SuspendLayout();
			// 
			// layoutControl1
			// 
			this.layoutControl1.Controls.Add(this.gridControl3);
			this.layoutControl1.Controls.Add(this.gridControl2);
			this.layoutControl1.Controls.Add(this.gridControl1);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(710, 275, 250, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(444, 447);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			// 
			// gridControl3
			// 
			this.gridControl3.DataSource = this.valuesBindingSource;
			this.gridControl3.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.gridControl3.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.gridControl3.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.gridControl3.EmbeddedNavigator.Buttons.First.Visible = false;
			this.gridControl3.EmbeddedNavigator.Buttons.Last.Visible = false;
			this.gridControl3.EmbeddedNavigator.Buttons.Next.Visible = false;
			this.gridControl3.EmbeddedNavigator.Buttons.NextPage.Visible = false;
			this.gridControl3.EmbeddedNavigator.Buttons.Prev.Visible = false;
			this.gridControl3.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
			this.gridControl3.EmbeddedNavigator.TextLocation = DevExpress.XtraEditors.NavigatorButtonsTextLocation.None;
			this.gridControl3.Location = new System.Drawing.Point(164, 237);
			this.gridControl3.MainView = this.gridView3;
			this.gridControl3.Name = "gridControl3";
			this.gridControl3.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ripceFormula});
			this.gridControl3.Size = new System.Drawing.Size(278, 208);
			this.gridControl3.TabIndex = 6;
			this.gridControl3.UseEmbeddedNavigator = true;
			this.gridControl3.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
			// 
			// valuesBindingSource
			// 
			this.valuesBindingSource.DataMember = "Values";
			this.valuesBindingSource.DataSource = this.categoriesBindingSource;
			// 
			// categoriesBindingSource
			// 
			this.categoriesBindingSource.DataMember = "Categories";
			this.categoriesBindingSource.DataSource = this.categoryInfosBindingSource;
			this.categoriesBindingSource.CurrentItemChanged += new System.EventHandler(this.categoriesBindingSource_CurrentItemChanged);
			// 
			// categoryInfosBindingSource
			// 
			this.categoryInfosBindingSource.DataMember = "CategoryInfos";
			this.categoryInfosBindingSource.DataSource = this.bsSett;
			this.categoryInfosBindingSource.CurrentItemChanged += new System.EventHandler(this.categoryInfosBindingSource_CurrentItemChanged);
			// 
			// bsSett
			// 
			this.bsSett.DataSource = typeof(WheelsScraper.ScraperSettings);
			this.bsSett.CurrentChanged += new System.EventHandler(this.bsSett_CurrentChanged);
			// 
			// gridView3
			// 
			this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVisible,
            this.colDescription,
            this.colValue});
			this.gridView3.GridControl = this.gridControl3;
			this.gridView3.Name = "gridView3";
			this.gridView3.OptionsCustomization.AllowGroup = false;
			this.gridView3.OptionsCustomization.AllowSort = false;
			this.gridView3.OptionsView.ShowGroupPanel = false;
			// 
			// colVisible
			// 
			this.colVisible.FieldName = "Visible";
			this.colVisible.MaxWidth = 20;
			this.colVisible.Name = "colVisible";
			this.colVisible.Visible = true;
			this.colVisible.VisibleIndex = 0;
			this.colVisible.Width = 20;
			// 
			// colDescription
			// 
			this.colDescription.FieldName = "Description";
			this.colDescription.Name = "colDescription";
			this.colDescription.Visible = true;
			this.colDescription.VisibleIndex = 1;
			this.colDescription.Width = 122;
			// 
			// colValue
			// 
			this.colValue.ColumnEdit = this.ripceFormula;
			this.colValue.FieldName = "Value";
			this.colValue.Name = "colValue";
			this.colValue.Visible = true;
			this.colValue.VisibleIndex = 2;
			this.colValue.Width = 126;
			// 
			// ripceFormula
			// 
			this.ripceFormula.AutoHeight = false;
			this.ripceFormula.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ripceFormula.Name = "ripceFormula";
			this.ripceFormula.PopupControl = this.popupContainerControl1;
			this.ripceFormula.QueryResultValue += new DevExpress.XtraEditors.Controls.QueryResultValueEventHandler(this.ripceFormula_QueryResultValue);
			this.ripceFormula.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.ripceFormula_QueryPopUp);
			// 
			// popupContainerControl1
			// 
			this.popupContainerControl1.Controls.Add(this.ucFormulaEdit1);
			this.popupContainerControl1.Location = new System.Drawing.Point(353, 250);
			this.popupContainerControl1.Name = "popupContainerControl1";
			this.popupContainerControl1.Size = new System.Drawing.Size(344, 180);
			this.popupContainerControl1.TabIndex = 2;
			// 
			// ucFormulaEdit1
			// 
			this.ucFormulaEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucFormulaEdit1.Fields = null;
			this.ucFormulaEdit1.Formula = "";
			this.ucFormulaEdit1.Location = new System.Drawing.Point(0, 0);
			this.ucFormulaEdit1.Name = "ucFormulaEdit1";
			this.ucFormulaEdit1.Size = new System.Drawing.Size(344, 180);
			this.ucFormulaEdit1.TabIndex = 0;
			// 
			// gridControl2
			// 
			this.gridControl2.DataSource = this.categoryInfosBindingSource;
			this.gridControl2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
			this.gridControl2.EmbeddedNavigator.Buttons.Edit.Visible = false;
			this.gridControl2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
			this.gridControl2.EmbeddedNavigator.Buttons.First.Visible = false;
			this.gridControl2.EmbeddedNavigator.Buttons.Last.Visible = false;
			this.gridControl2.EmbeddedNavigator.Buttons.Next.Visible = false;
			this.gridControl2.EmbeddedNavigator.Buttons.NextPage.Visible = false;
			this.gridControl2.EmbeddedNavigator.Buttons.Prev.Visible = false;
			this.gridControl2.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
			this.gridControl2.EmbeddedNavigator.TextLocation = DevExpress.XtraEditors.NavigatorButtonsTextLocation.None;
			this.gridControl2.Location = new System.Drawing.Point(2, 2);
			this.gridControl2.MainView = this.gridView2;
			this.gridControl2.Name = "gridControl2";
			this.gridControl2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ricbHeaders});
			this.gridControl2.Size = new System.Drawing.Size(153, 443);
			this.gridControl2.TabIndex = 5;
			this.gridControl2.UseEmbeddedNavigator = true;
			this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
			// 
			// gridView2
			// 
			this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colOutputHeader});
			this.gridView2.GridControl = this.gridControl2;
			this.gridView2.Name = "gridView2";
			this.gridView2.OptionsCustomization.AllowGroup = false;
			this.gridView2.OptionsDetail.EnableMasterViewMode = false;
			this.gridView2.OptionsView.ShowGroupPanel = false;
			// 
			// colOutputHeader
			// 
			this.colOutputHeader.ColumnEdit = this.ricbHeaders;
			this.colOutputHeader.FieldName = "OutputHeader";
			this.colOutputHeader.Name = "colOutputHeader";
			this.colOutputHeader.Visible = true;
			this.colOutputHeader.VisibleIndex = 0;
			// 
			// ricbHeaders
			// 
			this.ricbHeaders.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
			this.ricbHeaders.AutoHeight = false;
			this.ricbHeaders.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.ricbHeaders.Name = "ricbHeaders";
			this.ricbHeaders.NullValuePrompt = "Select column header";
			this.ricbHeaders.NullValuePromptShowForEmptyValue = true;
			this.ricbHeaders.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			this.ricbHeaders.ValidateOnEnterKey = true;
			// 
			// gridControl1
			// 
			this.gridControl1.DataSource = this.categoriesBindingSource;
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
			this.gridControl1.Location = new System.Drawing.Point(164, 2);
			this.gridControl1.MainView = this.gridView1;
			this.gridControl1.Name = "gridControl1";
			this.gridControl1.Size = new System.Drawing.Size(278, 226);
			this.gridControl1.TabIndex = 4;
			this.gridControl1.UseEmbeddedNavigator = true;
			this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
			// 
			// gridView1
			// 
			this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colName});
			this.gridView1.GridControl = this.gridControl1;
			this.gridView1.Name = "gridView1";
			this.gridView1.OptionsCustomization.AllowGroup = false;
			this.gridView1.OptionsCustomization.AllowSort = false;
			this.gridView1.OptionsDetail.EnableMasterViewMode = false;
			this.gridView1.OptionsView.ShowGroupPanel = false;
			// 
			// colName
			// 
			this.colName.Caption = "Category name";
			this.colName.FieldName = "Name";
			this.colName.Name = "colName";
			this.colName.Visible = true;
			this.colName.VisibleIndex = 0;
			// 
			// layoutControlGroup1
			// 
			this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.splitterItem1,
            this.splitterItem2});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(444, 447);
			this.layoutControlGroup1.Text = "Root";
			this.layoutControlGroup1.TextVisible = false;
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.gridControl1;
			this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
			this.layoutControlItem1.Location = new System.Drawing.Point(162, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(282, 230);
			this.layoutControlItem1.Text = "layoutControlItem1";
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextToControlDistance = 0;
			this.layoutControlItem1.TextVisible = false;
			// 
			// layoutControlItem2
			// 
			this.layoutControlItem2.Control = this.gridControl2;
			this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(157, 447);
			this.layoutControlItem2.Text = "layoutControlItem2";
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextToControlDistance = 0;
			this.layoutControlItem2.TextVisible = false;
			// 
			// layoutControlItem3
			// 
			this.layoutControlItem3.Control = this.gridControl3;
			this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
			this.layoutControlItem3.Location = new System.Drawing.Point(162, 235);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(282, 212);
			this.layoutControlItem3.Text = "layoutControlItem3";
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextToControlDistance = 0;
			this.layoutControlItem3.TextVisible = false;
			// 
			// splitterItem1
			// 
			this.splitterItem1.AllowHotTrack = true;
			this.splitterItem1.CustomizationFormText = "splitterItem1";
			this.splitterItem1.Location = new System.Drawing.Point(157, 0);
			this.splitterItem1.Name = "splitterItem1";
			this.splitterItem1.Size = new System.Drawing.Size(5, 447);
			// 
			// splitterItem2
			// 
			this.splitterItem2.AllowHotTrack = true;
			this.splitterItem2.CustomizationFormText = "splitterItem2";
			this.splitterItem2.Location = new System.Drawing.Point(162, 230);
			this.splitterItem2.Name = "splitterItem2";
			this.splitterItem2.Size = new System.Drawing.Size(282, 5);
			// 
			// ucFieldCombiner
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.popupContainerControl1);
			this.Controls.Add(this.layoutControl1);
			this.Name = "ucFieldCombiner";
			this.Size = new System.Drawing.Size(444, 447);
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridControl3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.valuesBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.categoriesBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.categoryInfosBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsSett)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ripceFormula)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
			this.popupContainerControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ricbHeaders)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl1;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraGrid.GridControl gridControl2;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
		private DevExpress.XtraGrid.GridControl gridControl1;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
		private DevExpress.XtraGrid.GridControl gridControl3;
		private System.Windows.Forms.BindingSource valuesBindingSource;
		private System.Windows.Forms.BindingSource categoriesBindingSource;
		private System.Windows.Forms.BindingSource categoryInfosBindingSource;
		private System.Windows.Forms.BindingSource bsSett;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
		private DevExpress.XtraGrid.Columns.GridColumn colVisible;
		private DevExpress.XtraGrid.Columns.GridColumn colDescription;
		private DevExpress.XtraGrid.Columns.GridColumn colValue;
		private DevExpress.XtraGrid.Columns.GridColumn colOutputHeader;
		private DevExpress.XtraGrid.Columns.GridColumn colName;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
		private DevExpress.XtraLayout.SplitterItem splitterItem1;
		private DevExpress.XtraLayout.SplitterItem splitterItem2;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox ricbHeaders;
		private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit ripceFormula;
		private DevExpress.XtraEditors.PopupContainerControl popupContainerControl1;
		private ucFormulaEdit ucFormulaEdit1;
	}
}
