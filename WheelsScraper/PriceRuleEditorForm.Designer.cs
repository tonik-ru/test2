﻿namespace WheelsScraper
{
	partial class PriceRuleEditorForm
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
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.cbRules = new DevExpress.XtraEditors.ImageComboBoxEdit();
			this.txtName = new DevExpress.XtraEditors.TextEdit();
			this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
			this.btnOK = new DevExpress.XtraEditors.SimpleButton();
			this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
			this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cbRules.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
			this.SuspendLayout();
			// 
			// layoutControl1
			// 
			this.layoutControl1.Controls.Add(this.btnCancel);
			this.layoutControl1.Controls.Add(this.btnOK);
			this.layoutControl1.Controls.Add(this.textEdit1);
			this.layoutControl1.Controls.Add(this.txtName);
			this.layoutControl1.Controls.Add(this.cbRules);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(642, 238, 250, 350);
			this.layoutControl1.OptionsView.UseDefaultDragAndDropRendering = false;
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(359, 146);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			// 
			// layoutControlGroup1
			// 
			this.layoutControlGroup1.CustomizationFormText = "Root";
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Size = new System.Drawing.Size(359, 146);
			this.layoutControlGroup1.Text = "Root";
			this.layoutControlGroup1.TextVisible = false;
			// 
			// cbRules
			// 
			this.cbRules.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSource1, "RuleType", true));
			this.cbRules.Location = new System.Drawing.Point(47, 36);
			this.cbRules.Name = "cbRules";
			this.cbRules.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.cbRules.Size = new System.Drawing.Size(130, 20);
			this.cbRules.StyleController = this.layoutControl1;
			this.cbRules.TabIndex = 4;
			// 
			// txtName
			// 
			this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "Name", true));
			this.txtName.Location = new System.Drawing.Point(47, 12);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(300, 20);
			this.txtName.StyleController = this.layoutControl1;
			this.txtName.TabIndex = 5;
			// 
			// textEdit1
			// 
			this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSource1, "Value", true));
			this.textEdit1.Location = new System.Drawing.Point(216, 36);
			this.textEdit1.Name = "textEdit1";
			this.textEdit1.Properties.Mask.EditMask = "f";
			this.textEdit1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
			this.textEdit1.Properties.Mask.UseMaskAsDisplayFormat = true;
			this.textEdit1.Size = new System.Drawing.Size(131, 20);
			this.textEdit1.StyleController = this.layoutControl1;
			this.textEdit1.TabIndex = 6;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(12, 60);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(165, 74);
			this.btnOK.StyleController = this.layoutControl1;
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.simpleButton1_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(181, 60);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(166, 74);
			this.btnCancel.StyleController = this.layoutControl1;
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.simpleButton2_Click);
			// 
			// bindingSource1
			// 
			this.bindingSource1.DataSource = typeof(WheelsScraper.ProfileRule);
			// 
			// layoutControlItem1
			// 
			this.layoutControlItem1.Control = this.cbRules;
			this.layoutControlItem1.CustomizationFormText = "Type:";
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(169, 24);
			this.layoutControlItem1.Text = "Type:";
			this.layoutControlItem1.TextSize = new System.Drawing.Size(31, 13);
			// 
			// layoutControlItem2
			// 
			this.layoutControlItem2.Control = this.txtName;
			this.layoutControlItem2.CustomizationFormText = "Name:";
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(339, 24);
			this.layoutControlItem2.Text = "Name:";
			this.layoutControlItem2.TextSize = new System.Drawing.Size(31, 13);
			// 
			// layoutControlItem3
			// 
			this.layoutControlItem3.Control = this.textEdit1;
			this.layoutControlItem3.CustomizationFormText = "Value:";
			this.layoutControlItem3.Location = new System.Drawing.Point(169, 24);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(170, 24);
			this.layoutControlItem3.Text = "Value:";
			this.layoutControlItem3.TextSize = new System.Drawing.Size(31, 13);
			// 
			// layoutControlItem4
			// 
			this.layoutControlItem4.Control = this.btnOK;
			this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
			this.layoutControlItem4.Location = new System.Drawing.Point(0, 48);
			this.layoutControlItem4.MinSize = new System.Drawing.Size(29, 26);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(169, 78);
			this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem4.Text = "layoutControlItem4";
			this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem4.TextToControlDistance = 0;
			this.layoutControlItem4.TextVisible = false;
			// 
			// layoutControlItem5
			// 
			this.layoutControlItem5.Control = this.btnCancel;
			this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
			this.layoutControlItem5.Location = new System.Drawing.Point(169, 48);
			this.layoutControlItem5.MinSize = new System.Drawing.Size(47, 26);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Size = new System.Drawing.Size(170, 78);
			this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem5.Text = "layoutControlItem5";
			this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem5.TextToControlDistance = 0;
			this.layoutControlItem5.TextVisible = false;
			// 
			// PriceRuleEditorForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(359, 146);
			this.ControlBox = false;
			this.Controls.Add(this.layoutControl1);
			this.Name = "PriceRuleEditorForm";
			this.ShowInTaskbar = false;
			this.Text = "Edit price rule";
			this.Load += new System.EventHandler(this.PriceRuleEditorForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cbRules.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraLayout.LayoutControl layoutControl1;
		private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
		private DevExpress.XtraEditors.TextEdit txtName;
		private DevExpress.XtraEditors.ImageComboBoxEdit cbRules;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
		private DevExpress.XtraEditors.TextEdit textEdit1;
		private System.Windows.Forms.BindingSource bindingSource1;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
		private DevExpress.XtraEditors.SimpleButton btnOK;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
		private DevExpress.XtraEditors.SimpleButton btnCancel;
		private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
	}
}