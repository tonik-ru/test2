using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WheelsScraper;

namespace Databox.Libs.UniversalMapper
{
	public partial class ucExtSettings : XtraUserControl
	{
		public ExtSettings ExtSett
		{
			get
			{
				return (ExtSettings)Sett.SpecialSettings;
			}
		}

		ScraperSettings _sett;
		public ScraperSettings Sett
		{
			get { return _sett; }
			set { _sett = value; RefreshBindings(); }
		}

		public ucExtSettings()
		{
			InitializeComponent();
		}

		protected void RefreshBindings()
		{
			bsSett.DataSource = ExtSett;
		}

		private void beFilePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			var file = PickFile("CSV files (*.csv*;*.txt;)|*.csv*;*.txt");
			if (file != null)
				beFilePath.Text = file;
		}

		protected string PickFile(string filter)
		{
			var ofd = new OpenFileDialog();
			ofd.Filter = filter;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				return ofd.FileName;
			}
			return null;
		}

	}
}
