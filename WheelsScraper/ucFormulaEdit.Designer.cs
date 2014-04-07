namespace WheelsScraper
{
	partial class ucFormulaEdit
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
			this.meFormula = new DevExpress.XtraEditors.MemoEdit();
			this.cbeFields = new DevExpress.XtraEditors.ComboBoxEdit();
			this.btnAddField = new DevExpress.XtraEditors.SimpleButton();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.meFormula.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cbeFields.Properties)).BeginInit();
			this.SuspendLayout();
			// 
			// meFormula
			// 
			this.meFormula.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.meFormula.Location = new System.Drawing.Point(3, 29);
			this.meFormula.Name = "meFormula";
			this.meFormula.Size = new System.Drawing.Size(315, 113);
			this.meFormula.TabIndex = 0;
			// 
			// cbeFields
			// 
			this.cbeFields.Location = new System.Drawing.Point(4, 3);
			this.cbeFields.Name = "cbeFields";
			this.cbeFields.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.cbeFields.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
			this.cbeFields.Size = new System.Drawing.Size(173, 20);
			this.cbeFields.TabIndex = 1;
			// 
			// btnAddField
			// 
			this.btnAddField.Location = new System.Drawing.Point(183, 1);
			this.btnAddField.Name = "btnAddField";
			this.btnAddField.Size = new System.Drawing.Size(75, 23);
			this.btnAddField.TabIndex = 2;
			this.btnAddField.Text = "Add field";
			this.btnAddField.Click += new System.EventHandler(this.btnAddField_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(243, 148);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(162, 148);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// ucFormulaEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAddField);
			this.Controls.Add(this.cbeFields);
			this.Controls.Add(this.meFormula);
			this.Name = "ucFormulaEdit";
			this.Size = new System.Drawing.Size(321, 176);
			this.Load += new System.EventHandler(this.ucFormulaEdit_Load);
			((System.ComponentModel.ISupportInitialize)(this.meFormula.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cbeFields.Properties)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.MemoEdit meFormula;
		private DevExpress.XtraEditors.ComboBoxEdit cbeFields;
		private DevExpress.XtraEditors.SimpleButton btnAddField;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
	}
}
