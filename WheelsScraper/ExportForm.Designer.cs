namespace WheelsScraper
{
	partial class ExportForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.gridControl1 = new DevExpress.XtraGrid.GridControl();
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.wareInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.colPartNumber = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colManufacturerNumber = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colBrand = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colMSRP = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colCost = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colJobber = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colProcessed = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colImageUrl = new DevExpress.XtraGrid.Columns.GridColumn();
			this.colDescription = new DevExpress.XtraGrid.Columns.GridColumn();
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.wareInfoBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// gridControl1
			// 
			this.gridControl1.DataSource = this.wareInfoBindingSource;
			this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridControl1.Location = new System.Drawing.Point(0, 0);
			this.gridControl1.MainView = this.gridView1;
			this.gridControl1.Name = "gridControl1";
			this.gridControl1.Size = new System.Drawing.Size(643, 423);
			this.gridControl1.TabIndex = 0;
			this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
			// 
			// gridView1
			// 
			this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPartNumber,
            this.colManufacturerNumber,
            this.colName,
            this.colBrand,
            this.colMSRP,
            this.colCost,
            this.colJobber,
            this.colQuantity,
            this.colProcessed,
            this.colImageUrl,
            this.colDescription});
			this.gridView1.GridControl = this.gridControl1;
			this.gridView1.Name = "gridView1";
			// 
			// wareInfoBindingSource
			// 
			this.wareInfoBindingSource.DataSource = typeof(WheelsScraper.WareInfo);
			// 
			// colPartNumber
			// 
			this.colPartNumber.FieldName = "PartNumber";
			this.colPartNumber.Name = "colPartNumber";
			this.colPartNumber.Visible = true;
			this.colPartNumber.VisibleIndex = 0;
			this.colPartNumber.Width = 55;
			// 
			// colManufacturerNumber
			// 
			this.colManufacturerNumber.FieldName = "ManufacturerNumber";
			this.colManufacturerNumber.Name = "colManufacturerNumber";
			this.colManufacturerNumber.Visible = true;
			this.colManufacturerNumber.VisibleIndex = 1;
			this.colManufacturerNumber.Width = 55;
			// 
			// colName
			// 
			this.colName.FieldName = "Name";
			this.colName.Name = "colName";
			this.colName.Visible = true;
			this.colName.VisibleIndex = 2;
			this.colName.Width = 55;
			// 
			// colBrand
			// 
			this.colBrand.FieldName = "Brand";
			this.colBrand.Name = "colBrand";
			this.colBrand.Visible = true;
			this.colBrand.VisibleIndex = 3;
			this.colBrand.Width = 55;
			// 
			// colMSRP
			// 
			this.colMSRP.FieldName = "MSRP";
			this.colMSRP.Name = "colMSRP";
			this.colMSRP.Visible = true;
			this.colMSRP.VisibleIndex = 4;
			this.colMSRP.Width = 55;
			// 
			// colCost
			// 
			this.colCost.FieldName = "Cost";
			this.colCost.Name = "colCost";
			this.colCost.Visible = true;
			this.colCost.VisibleIndex = 5;
			this.colCost.Width = 55;
			// 
			// colJobber
			// 
			this.colJobber.FieldName = "Jobber";
			this.colJobber.Name = "colJobber";
			this.colJobber.Visible = true;
			this.colJobber.VisibleIndex = 6;
			this.colJobber.Width = 55;
			// 
			// colQuantity
			// 
			this.colQuantity.FieldName = "Quantity";
			this.colQuantity.Name = "colQuantity";
			this.colQuantity.Visible = true;
			this.colQuantity.VisibleIndex = 7;
			this.colQuantity.Width = 61;
			// 
			// colProcessed
			// 
			this.colProcessed.FieldName = "Processed";
			this.colProcessed.Name = "colProcessed";
			this.colProcessed.Visible = true;
			this.colProcessed.VisibleIndex = 8;
			this.colProcessed.Width = 72;
			// 
			// colImageUrl
			// 
			this.colImageUrl.FieldName = "ImageUrl";
			this.colImageUrl.Name = "colImageUrl";
			this.colImageUrl.Visible = true;
			this.colImageUrl.VisibleIndex = 9;
			this.colImageUrl.Width = 81;
			// 
			// colDescription
			// 
			this.colDescription.FieldName = "Description";
			this.colDescription.Name = "colDescription";
			this.colDescription.Visible = true;
			this.colDescription.VisibleIndex = 10;
			this.colDescription.Width = 90;
			// 
			// SaveGridForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(643, 423);
			this.Controls.Add(this.gridControl1);
			this.Name = "SaveGridForm";
			this.Text = "SaveGridForm";
			this.Load += new System.EventHandler(this.SaveGridForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.wareInfoBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraGrid.GridControl gridControl1;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
		private System.Windows.Forms.BindingSource wareInfoBindingSource;
		private DevExpress.XtraGrid.Columns.GridColumn colPartNumber;
		private DevExpress.XtraGrid.Columns.GridColumn colManufacturerNumber;
		private DevExpress.XtraGrid.Columns.GridColumn colName;
		private DevExpress.XtraGrid.Columns.GridColumn colBrand;
		private DevExpress.XtraGrid.Columns.GridColumn colMSRP;
		private DevExpress.XtraGrid.Columns.GridColumn colCost;
		private DevExpress.XtraGrid.Columns.GridColumn colJobber;
		private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
		private DevExpress.XtraGrid.Columns.GridColumn colProcessed;
		private DevExpress.XtraGrid.Columns.GridColumn colImageUrl;
		private DevExpress.XtraGrid.Columns.GridColumn colDescription;
	}
}