namespace WheelsScraper
{
	partial class AutoupdateForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoupdateForm));
			this.label1 = new System.Windows.Forms.Label();
			this.pbProgr = new System.Windows.Forms.ProgressBar();
			this.lblFileName = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lvFiles = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.ForeColor = System.Drawing.Color.LightSkyBlue;
			this.label1.Location = new System.Drawing.Point(7, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(291, 31);
			this.label1.TabIndex = 1;
			this.label1.Text = "Checking for updates";
			// 
			// pbProgr
			// 
			this.pbProgr.Location = new System.Drawing.Point(14, 43);
			this.pbProgr.Name = "pbProgr";
			this.pbProgr.Size = new System.Drawing.Size(397, 23);
			this.pbProgr.TabIndex = 2;
			// 
			// lblFileName
			// 
			this.lblFileName.AutoSize = true;
			this.lblFileName.Location = new System.Drawing.Point(14, 69);
			this.lblFileName.Name = "lblFileName";
			this.lblFileName.Size = new System.Drawing.Size(97, 13);
			this.lblFileName.TabIndex = 3;
			this.lblFileName.Text = "Downloading file...";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(296, 211);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(114, 40);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lvFiles
			// 
			this.lvFiles.FormattingEnabled = true;
			this.lvFiles.Location = new System.Drawing.Point(14, 85);
			this.lvFiles.Name = "lvFiles";
			this.lvFiles.Size = new System.Drawing.Size(397, 121);
			this.lvFiles.TabIndex = 5;
			// 
			// AutoupdateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(421, 252);
			this.ControlBox = false;
			this.Controls.Add(this.lvFiles);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblFileName);
			this.Controls.Add(this.pbProgr);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "AutoupdateForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Startup";
			this.Load += new System.EventHandler(this.StartUpForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ProgressBar pbProgr;
		private System.Windows.Forms.Label lblFileName;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ListBox lvFiles;
	}
}