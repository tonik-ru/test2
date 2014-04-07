namespace WheelsScraper
{
	partial class FindAndReplaceForm
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
			this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
			this.btnClose = new DevExpress.XtraEditors.SimpleButton();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.txtSearchString = new DevExpress.XtraEditors.TextEdit();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.txtReplaceString = new DevExpress.XtraEditors.TextEdit();
			this.ceMatchCase = new DevExpress.XtraEditors.CheckEdit();
			this.ceWholeCell = new DevExpress.XtraEditors.CheckEdit();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			this.cbeColumns = new DevExpress.XtraEditors.ComboBoxEdit();
			this.ceRegex = new DevExpress.XtraEditors.CheckEdit();
			((System.ComponentModel.ISupportInitialize)(this.txtSearchString.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtReplaceString.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ceMatchCase.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ceWholeCell.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cbeColumns.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ceRegex.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// simpleButton1
			// 
			this.simpleButton1.Location = new System.Drawing.Point(13, 161);
			this.simpleButton1.Name = "simpleButton1";
			this.simpleButton1.Size = new System.Drawing.Size(75, 23);
			this.simpleButton1.TabIndex = 5;
			this.simpleButton1.Text = "Replace all";
			this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(340, 161);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// labelControl1
			// 
			this.labelControl1.Location = new System.Drawing.Point(13, 13);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(24, 13);
			this.labelControl1.TabIndex = 2;
			this.labelControl1.Text = "Find:";
			// 
			// txtSearchString
			// 
			this.txtSearchString.Location = new System.Drawing.Point(102, 13);
			this.txtSearchString.Name = "txtSearchString";
			this.txtSearchString.Size = new System.Drawing.Size(313, 20);
			this.txtSearchString.TabIndex = 0;
			// 
			// labelControl2
			// 
			this.labelControl2.Location = new System.Drawing.Point(13, 50);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(65, 13);
			this.labelControl2.TabIndex = 4;
			this.labelControl2.Text = "Replace with:";
			// 
			// txtReplaceString
			// 
			this.txtReplaceString.Location = new System.Drawing.Point(102, 47);
			this.txtReplaceString.Name = "txtReplaceString";
			this.txtReplaceString.Size = new System.Drawing.Size(313, 20);
			this.txtReplaceString.TabIndex = 1;
			// 
			// ceMatchCase
			// 
			this.ceMatchCase.Location = new System.Drawing.Point(11, 83);
			this.ceMatchCase.Name = "ceMatchCase";
			this.ceMatchCase.Properties.Caption = "Match case:";
			this.ceMatchCase.Size = new System.Drawing.Size(75, 19);
			this.ceMatchCase.TabIndex = 2;
			// 
			// ceWholeCell
			// 
			this.ceWholeCell.Location = new System.Drawing.Point(92, 83);
			this.ceWholeCell.Name = "ceWholeCell";
			this.ceWholeCell.Properties.Caption = "Whole cell";
			this.ceWholeCell.Size = new System.Drawing.Size(75, 19);
			this.ceWholeCell.TabIndex = 3;
			// 
			// labelControl3
			// 
			this.labelControl3.Location = new System.Drawing.Point(13, 124);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(37, 13);
			this.labelControl3.TabIndex = 8;
			this.labelControl3.Text = "Look in:";
			// 
			// cbeColumns
			// 
			this.cbeColumns.Location = new System.Drawing.Point(102, 121);
			this.cbeColumns.Name = "cbeColumns";
			this.cbeColumns.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.cbeColumns.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			this.cbeColumns.Size = new System.Drawing.Size(155, 20);
			this.cbeColumns.TabIndex = 4;
			// 
			// ceRegex
			// 
			this.ceRegex.Location = new System.Drawing.Point(173, 83);
			this.ceRegex.Name = "ceRegex";
			this.ceRegex.Properties.Caption = "Regex";
			this.ceRegex.Size = new System.Drawing.Size(75, 19);
			this.ceRegex.TabIndex = 9;
			// 
			// FindAndReplaceForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(427, 194);
			this.Controls.Add(this.ceRegex);
			this.Controls.Add(this.cbeColumns);
			this.Controls.Add(this.labelControl3);
			this.Controls.Add(this.ceWholeCell);
			this.Controls.Add(this.ceMatchCase);
			this.Controls.Add(this.txtReplaceString);
			this.Controls.Add(this.labelControl2);
			this.Controls.Add(this.txtSearchString);
			this.Controls.Add(this.labelControl1);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.simpleButton1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindAndReplaceForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Find and replace";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FindAndReplaceForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.txtSearchString.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtReplaceString.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ceMatchCase.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ceWholeCell.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cbeColumns.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ceRegex.Properties)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.SimpleButton simpleButton1;
		private DevExpress.XtraEditors.SimpleButton btnClose;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.TextEdit txtSearchString;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.TextEdit txtReplaceString;
		private DevExpress.XtraEditors.CheckEdit ceMatchCase;
		private DevExpress.XtraEditors.CheckEdit ceWholeCell;
		private DevExpress.XtraEditors.LabelControl labelControl3;
		private DevExpress.XtraEditors.ComboBoxEdit cbeColumns;
		private DevExpress.XtraEditors.CheckEdit ceRegex;
	}
}