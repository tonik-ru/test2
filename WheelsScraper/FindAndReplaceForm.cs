using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WheelsScraper
{
	public partial class FindAndReplaceForm : XtraForm
	{
		public List<string> Columns { get; set; }
		public event EventHandler<DataReplaceEvent> ReplaceData;
		//public GridView Grid { get; set; }

		public FindAndReplaceForm()
		{
			InitializeComponent();
		}

		private void FindAndReplaceForm_Load(object sender, EventArgs e)
		{
			cbeColumns.Properties.Items.Add("All columns");
			foreach (var col in Columns)
			{
				cbeColumns.Properties.Items.Add(col);
			}

			cbeColumns.SelectedIndex = 0;
		}

		protected void OnReplaceData()
		{
			if (ReplaceData != null)
				ReplaceData(this, new DataReplaceEvent { Column = cbeColumns.SelectedItem.ToString(), MatchCase = ceMatchCase.Checked, ReplaceText = txtReplaceString.Text, SearchText = txtSearchString.Text, WholeCell = ceWholeCell.Checked, IsRegex = ceRegex.Checked });
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			OnReplaceData();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
