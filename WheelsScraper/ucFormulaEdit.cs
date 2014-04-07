using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace WheelsScraper
{
	public partial class ucFormulaEdit : XtraUserControl
	{
		public string Formula
		{
			get { return meFormula.Text; }
			set { meFormula.Text = value; }
		}
		List<string> _fields;
		public List<string> Fields
		{
			get { return _fields; }
			set { _fields = value; RefreshBindings(); }
		}
		

		public ucFormulaEdit()
		{
			InitializeComponent();
		}

		private void ucFormulaEdit_Load(object sender, EventArgs e)
		{
			RefreshBindings();
		}

		protected void RefreshBindings()
		{
			cbeFields.Properties.Items.Clear();
			if (Fields == null)
				return;			
			cbeFields.Properties.Items.AddRange(Fields);
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			(this.Parent as PopupContainerControl).OwnerEdit.ClosePopup();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			(this.Parent as PopupContainerControl).OwnerEdit.CancelPopup();
		}

		private void btnAddField_Click(object sender, EventArgs e)
		{
			meFormula.Text = meFormula.Text.Insert(meFormula.SelectionStart, "[" + cbeFields.SelectedItem + "]");
		}
	}
}