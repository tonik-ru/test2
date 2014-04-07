using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WheelsScraper
{
	public partial class ucBullets : UserControl
	{
		private ScraperSettings _sett;
		public ISimpleScraper Scrap;
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

		public ucBullets()
		{
			InitializeComponent();
		}

		protected void RefreshDataSources()
		{
			if (Sett == null)
				return;
			ricbFields.Items.Clear();
			ricbFields.Items.AddRange(Scraper.Lib.Main.PropHelper.GetProperties(Scrap));

			bulletsMoveInfoBindingSource.DataSource = Sett.BulletsMoveInfos;
		}

		private void ucBullets_Load(object sender, EventArgs e)
		{

		}

		private void ucBullets_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
				RefreshDataSources();
		}
	}
}