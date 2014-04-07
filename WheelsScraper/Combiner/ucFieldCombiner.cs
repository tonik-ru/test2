using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Scraper.Shared;

namespace WheelsScraper.Combiner
{
	public partial class ucFieldCombiner : XtraUserControl
	{
		public ISimpleScraper Scrap { get; set; }

		private ScraperSettings _sett;
		public ScraperSettings Sett
		{
			get
			{
				return _sett;
			}
			set
			{
				if (value == null)
					return;
				_sett = value;
				RefreshDataSources();
			}
		}

		public ucFieldCombiner()
		{
			InitializeComponent();
		}

		protected void RefreshDataSources()
		{
			if (Sett == null)
				return;
			bsSett.DataSource = Sett;
			ricbHeaders.Items.Clear();

			var headers = Sett.ExportProfiles.SelectMany(x => x.Fields).Select(x => x.Header).Where(x => !string.IsNullOrEmpty(x)).Distinct();
			ricbHeaders.Items.AddRange(headers.ToArray());

			PopulateFields();
		}

		protected void PopulateFields()
		{
			var fieldsInType = Scraper.Lib.Main.PropHelper.GetProperties(Scrap);
			ucFormulaEdit1.Fields = fieldsInType;
		}

		private void bsSett_CurrentChanged(object sender, EventArgs e)
		{
			EnableGrids();
		}

		private void categoryInfosBindingSource_CurrentItemChanged(object sender, EventArgs e)
		{
			EnableGrids();
		}

		protected void EnableGrids()
		{
			if (categoryInfosBindingSource.Current == null)
				gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
			else
				gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;

			if (categoriesBindingSource.Current == null)
				gridView3.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
			else
				gridView3.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
		}

		private void categoriesBindingSource_CurrentItemChanged(object sender, EventArgs e)
		{
			EnableGrids();
		}

		private void ripceFormula_QueryResultValue(object sender, DevExpress.XtraEditors.Controls.QueryResultValueEventArgs e)
		{
			e.Value = ucFormulaEdit1.Formula;
		}

		private void ripceFormula_QueryPopUp(object sender, CancelEventArgs e)
		{
			PopulateFields();
			var item = (ExportFieldSpcValueInfo)valuesBindingSource.Current;
			ucFormulaEdit1.Formula = item.Value;
		}
	}
}