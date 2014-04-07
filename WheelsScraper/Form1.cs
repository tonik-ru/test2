using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using LumenWorks.Framework.IO.Csv;
using Ionic.Zip;
using WheelsScraper.srAuth;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Demos.RibbonSimplePad;
using System.Text.RegularExpressions;

namespace WheelsScraper
{
	public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm, IMainApp
	{
		protected bool allowGlobalSchedule;
		protected RuntimeManager rtMgr;
		public BindingList<InnerMessage> Messages;

		MRUArrayList arMRUList = null;

		public Form1()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
			AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
			InitializeComponent();
			InitSkinGallery();
			InitMessagesPanel();
			InitMostRecentFiles();
		}

		void AddFileToMRU(string filePath)
		{
			if (!AppSettings.Default.MRUFiles.Any(x => x.FileName.ToLower() == filePath.ToLower()))
				AppSettings.Default.MRUFiles.Insert(0, new MRUItem { FileName = filePath });
			if(!filePath.ToLower().StartsWith("http"))
				AddPathToMRU(filePath);
			InitMostRecentFiles();
		}

		void AddPathToMRU(string path)
		{
			var path2 = Path.GetDirectoryName(Path.GetFullPath(path).ToLower());
			if (!AppSettings.Default.MRUPaths.Any(x => x.FileName.ToLower() == path2))
				AppSettings.Default.MRUPaths.Insert(0, new MRUItem { FileName = path2 });
		}

		void InitMostRecentFiles()
		{
			//AppSettings.Default.MRUFiles.Add(new MRUItem { FileName = "Test.edf", Pinned = false });
			//AppSettings.Default.MRUFiles.Add(new MRUItem { FileName = "Test2.edf", Pinned = false });
			//string fileName = Application.StartupPath + "\\" + MRUArrayList.MRUFileName;
			//string folderName = Application.StartupPath + "\\" + MRUArrayList.MRUFolderName;
			//arMRUList.Init(fileName, "Document1.rtf");
			//recentItemsControl1.MRUFileList.Init(fileName, "Document1.rtf");
			//recentItemsControl1.MRUFolderList.Init(folderName, Application.StartupPath);
			recentItemsControl1.MRUFileList.Clear();
			foreach (var item in AppSettings.Default.MRUFiles.Take(30))
				recentItemsControl1.MRUFileList.InsertElement(item.FileName + "," + item.Pinned);
			recentItemsControl1.MRUFolderList.Clear();
			foreach (var item in AppSettings.Default.MRUPaths.Take(30))
				recentItemsControl1.MRUFolderList.InsertElement(item.FileName + "," + item.Pinned);
		}

		void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			//	throw new NotImplementedException();
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string curFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			Program.Log.Debug(args.Name);
			if (args.Name.Contains("Databox.Libs."))
			{
				var asm = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName == args.Name).FirstOrDefault();
				return asm;
				//return System.Reflection.Assembly.GetExecutingAssembly();
			}
			return null;
		}

		private void InitMessagesPanel()
		{
			MessagesPanelManager.Init(this);
			Messages = new BindingList<InnerMessage>();
			gridControlMessages.DataSource = Messages;
			Messages.ListChanged += (x, o) =>
			{
				gridView1.MoveLast();
			};
		}
		private void button1_Click(object sender, EventArgs e)
		{

		}

		List<ISimpleScraper> scrapers = new List<ISimpleScraper>();

		DateTime lastUpdatedView;
		void wheelsScr_ItemLoaded(object sender, EventArgs e)
		{
			if ((DateTime.Now - lastUpdatedView).TotalSeconds < 5)
				return;
			if (this.InvokeRequired)
			{
				this.Invoke(new Action<object, EventArgs>(wheelsScr_ItemLoaded), sender, e);
			}
			else
			{
				lastUpdatedView = DateTime.Now;
				var sr = gridView2.FocusedRowHandle;
				//gridView2.RefreshData();
				gridView2.FocusedRowHandle = sr;
				wareInfoBindingSource.ResetBindings(false);
				//gridControl1.Refresh();
			}
		}



		ScraperSettings curSettings;
		ISimpleScraper curScraper;
		ExportProfile curProfile;

		protected void RefreshBindings()
		{
			if (this.InvokeRequired)
				this.Invoke(new Action(RefreshBindings));
			else
			{
				bsScrapProps.ResetCurrentItem();
				var scraperName = (string)cbScraper.SelectedItem;
				if (scraperName == null)
				{
					EnableControls(false);
					return;
				}
				var curSet = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraperName).FirstOrDefault();
				curSettings = curSet;

				sCEFileBindingSource.DataSource = WheelsScraper.SCE.SCEManager.GetModuleFiles(curSet.Name);

				ricbProfiles.Items.Clear();
				ricbProfiles.Items.AddRange(curSet.ExportProfiles.Select(x => x.Name).ToList());

				ricbProfileName2.Items.Clear();
				ricbProfileName2.Items.AddRange(curSet.ExportProfiles.Select(x => x.Name).ToList());

				var br = curSet.BrandsStr; ;
				ccbeBrands.Properties.Items.Clear();
				curSet.Brands.ForEach(x => ccbeBrands.Properties.Items.Add(x.Name));
				ccbeBrands.Text = br;

				//AppSettings.Default.ScrapSettings.IndexOf()
				var pos = AppSettings.Default.ScrapSettings.IndexOf(curSet);
				bsScrapProps.Position = pos;
				bsScrapProps.ResetBindings(false);
				//bbiStartCurrent.Caption = "Start " + scraperName;
				var scrap = scrapers.Where(x => x.Name == scraperName).First();
				curScraper = scrap;
				//btnStartScrap.Enabled = !scrap.IsRunning;

				EnableControls(!scrap.IsRunning);
				if (!scrap.IsRunning)
					bbiPauseCurrent.Enabled = false;
				else
				{
					bbiPauseCurrent.Enabled = true;
					if (scrap.IsPaused)
						bbiPauseCurrent.Caption = "Resume";
					else
						bbiPauseCurrent.Caption = "Pause";
				}
				bbiStopCurrent.Enabled = scrap.IsRunning;

				//wareInfoBindingSource.DataSource = scrap.Wares;
				var defProfile = curSet.ExportProfiles.Where(x => x.IsDefault).FirstOrDefault();
				if (defProfile == null)
					defProfile = curSet.ExportProfiles[0];
				curProfile = defProfile;

				var fieldsSet = defProfile.Fields.OrderBy(x => x.VisibleIndex);

				wareInfoBindingSource.DataSource = null;

				if (scrap is ISimpleScraper)
				{
					gridView2.Columns.Clear();
					//var fields = (scrap as IFieldInfoProvider).GetFieldNames();
					var fields = Scraper.Lib.Main.PropHelper.GetProperties(scrap);
					int i = 0;

					foreach (var f in fields)
					{
						var f2 = fieldsSet.Where(x => x.FieldName == f && x.Export).FirstOrDefault();
						if (f2 != null)
						{
							var col = gridView2.Columns.AddVisible("__" + f2.FieldName, f2.Header);
							if (string.IsNullOrEmpty(col.Caption))
								col.Caption = f2.FieldName;
							col.VisibleIndex = -100;
							col.VisibleIndex = i++;
							col.UnboundType = DevExpress.Data.UnboundColumnType.String;
						}
					}
				}
				else
				{
					//gridView2.PopulateColumns(scrap.WareInfoList);
					//var lstColsToDelete = new List<DevExpress.XtraGrid.Columns.GridColumn>();

					//foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView2.Columns)
					//{
					//	var f1 = fieldsSet.Where(x => x.FieldName == col.FieldName && x.Export).FirstOrDefault();
					//	if (f1 != null)
					//	{
					//		col.VisibleIndex = f1.VisibleIndex;
					//		col.Caption = f1.Header;
					//	}
					//	else
					//		lstColsToDelete.Add(col);
					//}
					//lstColsToDelete.ForEach(x => gridView2.Columns.Remove(x));
				}

				int idx = 0;
				foreach (var field in fieldsSet)
				{
					var f1 = gridView2.Columns.Cast<DevExpress.XtraGrid.Columns.GridColumn>()
						.Where(x => x.Caption == field.Header /*&& x.FieldName == field.FieldName*/).FirstOrDefault();
					if (f1 != null)
						f1.VisibleIndex = idx++;
				}

				foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView2.Columns)
				{
					if (col.VisibleIndex == 0)
					{
						col.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
	            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
					}
					if (col.VisibleIndex == 1)
					{
						col.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
	            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, null, "Total records")});
						//gridView2.Columns[0].Summary.Add(new DevExpress.XtraGrid.GridSummaryItem(DevExpress.Data.SummaryItemType.Count, gridView2.Columns[0].FieldName, "n"));
					}
				}
				wareInfoBindingSource.DataSource = scrap.WareInfoList;
			}
		}


		protected void ProcessComplete(IAsyncResult ar)
		{
			//var scrap = (ISimpleScraper)ar.AsyncState;
			//scrapers.Remove(scrap);
			//EnableControls(true);
			RefreshBindings();
			var scrap = (ISimpleScraper)ar.AsyncState;
			if (scrap.CompleteSuccessfully)
				SaveResult(scrap);
			var sett = AppSettings.Default.ScrapSettings.Where(x => x.Name == scrap.Name).FirstOrDefault();
			if (scrapersQueue.Contains(sett))
				scrapersQueue.Remove(sett);
			sett.LastRun = DateTime.Now;
			if (scrapersQueue.Count == 0)
				globaLastRun = DateTime.Now;

			scrap.OnPostProcess();
			sett.IsRunning = false;

			SCE.LicenseValidator.ReportUsage(sett.SCEAPIKey, sett.Name, sett.StartDate, DateTime.Now - sett.StartDate);

			RefreshBindings();
		}

		protected void UpdateNextRunGlobal()
		{
			if (!AppSettings.Default.EnableSchedule)
				this.Invoke(new Action(() => { lblNextRun.Text = " "; }));
			else
			{
				var nextRun = globaLastRun.AddMinutes(AppSettings.Default.IntervalMinutes);
				if (DateTime.Now > DateTime.Today.AddTicks(AppSettings.Default.EndAt.Ticks))
					nextRun = DateTime.Today.AddDays(1).AddTicks(AppSettings.Default.StartAt.Ticks);
				if (nextRun > DateTime.Today.AddTicks(AppSettings.Default.EndAt.Ticks))
					nextRun = DateTime.Today.AddDays(1).AddTicks(AppSettings.Default.StartAt.Ticks);
				if (nextRun < DateTime.Today.AddTicks(AppSettings.Default.StartAt.Ticks))
					nextRun = DateTime.Today.AddTicks(AppSettings.Default.StartAt.Ticks);
				var NextRunIn = nextRun - DateTime.Now;
				if (NextRunIn.TotalMinutes < 0)
					NextRunIn = new TimeSpan();
				NextRunIn = new TimeSpan(NextRunIn.Hours, NextRunIn.Minutes, NextRunIn.Seconds);
				var r1 = string.Format("Next run: {0:D2}:{1:D2}:{2:D2}", NextRunIn.Hours, NextRunIn.Minutes, NextRunIn.Seconds);
				if (NextRunIn.TotalSeconds == 0 && scrapersQueue.Count != 0)
					r1 = "Waiting to complete";
				this.Invoke(new Action(() => { lblNextRun.Text = r1; }));
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			StopCurrentFeed();
		}

		protected void StopCurrentFeed()
		{
			if (MessageBox.Show("Stop current Datafeed?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
				return;
			var scrap = GetCurrentScraper();
			scrap.Cancel();
			var sett = GetCurrentConfig();
			if (sett.EnableSchedule)
			{
				if (MessageBox.Show("Disable the Schedule?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
					return;
				sett.EnableSchedule = false;
				RefreshBindings();
				UpdateLimit();
			}
		}

		protected void CancelProcessor(string name)
		{
			var scrap = scrapers.Where(x => x.Name == name).FirstOrDefault();
			if (scrap == null)
				return;
			scrap.Cancel();
		}

		protected void CancelCurrentProcessor()
		{
			CancelProcessor((string)cbScraper.SelectedItem);
		}

		protected void RefreshControlsEnabled()
		{
			var scrap = GetCurrentScraper();
			var sett = GetCurrentConfig();
			var mgr = EDFManager.GetEDFManager(scrap);
			bbiUndo.Enabled = mgr.UndoMgr.HasUndoSteps();
			bbiRedo.Enabled = mgr.UndoMgr.HasRedoSteps();

			bbiForceUpload.Enabled = sett.FtpExportSettings.Any();
			bbiForceUpload.Enabled = sett.LocalExportSettings.Any();
		}

		protected void EnableControls(bool enable)
		{
			if (this.InvokeRequired)
				this.Invoke(new Action<bool>(EnableControls), enable);
			else
			{
				//tabExportSettings.Enabled = enable;
				tabFtpSettings.Enabled = enable;
				tabLocalSettigs.Enabled = enable;
				lcgLogin.Enabled = enable;
				tabSchedule.Enabled = enable;
				lcgSett.Enabled = enable;
				tabImages.Enabled = enable;
				bbiStartCurrent.Enabled = enable;
				//btnClearData.Enabled = enable;
				bbiPauseCurrent.Enabled = !enable;
				//lcgSpeed.Enabled = enable;
				lcgProxy.Enabled = enable;
				bbiForceUpload.Enabled = enable;
			}
		}

		private void btnSaveResults_Click(object sender, EventArgs e)
		{
			SaveFile();
		}

		private void SaveFile()
		{
			var scrap = GetCurrentScraper();
			if (scrap == null)
				return;
			if (scrap.Wares.Count == 0)
			{
				MessagesPanelManager.PrintMessage("Nothing to save");
				return;
			}
			var conf = GetCurrentConfig();
			try
			{
				SaveFileDialog sfd = new SaveFileDialog();

				//	sfd.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx";
				sfd.Filter = "CSV files (*.csv)|*.csv";
				sfd.RestoreDirectory = true;
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					var defProfile = conf.ExportProfiles.Where(x => x.IsDefault).FirstOrDefault();
					if (defProfile == null)
						defProfile = conf.ExportProfiles[0];
					curProfile = defProfile;
					var csvExporter = new ScrapResultCsvExporter(scrap, AppSettings.Default.GetExtSettings(scrap.Name).ValidateAgainstAddData, conf);

					var addDataMgr = new AdditionalDataSourceManager(scrap);
					List<WareInfo> badWares;
					csvExporter.Export(scrap.Wares, defProfile.Fields, sfd.FileName, false, out badWares, false, false, defProfile);
				}
			}
			catch (Exception err)
			{
				Program.Log.Error(err);
				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
			}
		}


		private bool ignoreSettingsChangedEvent;
		private ISimpleScraper currentScraper;

		private void cbScraper_SelectedIndexChanged(object sender, EventArgs e)
		{
			//if (cbScraper.SelectedItem == null)
			//{
			//	return;
			//}
			//var scraperName = (string)cbScraper.SelectedItem;
			ignoreSettingsChangedEvent = true;
			RefreshBindings();
			UpdateFtpSettings();
			UpdateLocalSettings();
			UpdateLimit();
			ignoreSettingsChangedEvent = false;
			UpdateSpecialSettings();
			UpdateSCEConnector();

			currentScraper = GetCurrentScraper();

			ucAdditionalSources1.Sett = GetCurrentConfig();
			ucExportRules1.Scrap = GetCurrentScraper();
			ucExportRules1.Sett = GetCurrentConfig();
			ucBullets1.Scrap = GetCurrentScraper();
			ucBullets1.Sett = GetCurrentConfig();

			//setup profiles tab
			exportFieldsSelector1.Sett = GetCurrentConfig();
			exportFieldsSelector1.Scraper = GetCurrentScraper();
			exportFieldsSelector1.FieldType = GetCurrentScraper().WareInfoType;
			exportFieldsSelector1.SelectedProfile = null;

			ucFieldCombiner1.Scrap = GetCurrentScraper();
			ucFieldCombiner1.Sett = GetCurrentConfig();

			edfMgr = EDFManager.GetEDFManager(currentScraper);

			RefreshControlsEnabled();
		}

		protected void UpdateSCEConnector()
		{
			var scrap = GetCurrentScraper();
			bool allowSCE = false;
			if (scrap is ISceConnector)
				allowSCE = (scrap as ISceConnector).AllowConnector;

			allowSCE = true;
			bbiSCE.Visibility = allowSCE ? BarItemVisibility.Always : BarItemVisibility.Never;
			//tabSCEResults.PageVisible = allowSCE;
		}

		protected void UpdateSpecialSettings()
		{
			lcSpecialSettings.Clear();
			var scrap = GetCurrentScraper();
			if (scrap == null)
				return;
			var sett = GetCurrentConfig();
			if (scrap.SettingsTab != null)
			{
				var item = lcSpecialSettings.AddItem();
				item.Control = scrap.SettingsTab;
				item.TextVisible = false;
				//item.
			}
		}

		protected void DoLoadProcessorConfig(string edfFileName, string libFileName = null)
		{
			{
				if (!File.Exists(edfFileName))
				{
					MessageBox.Show("File doesnt exist", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
					throw new Exception("File doesnt exist");
					//return;
				}
				string dllFileName = null;
				ScraperSettings prevLoadedSetting = null;
				ScraperSettings conf = null;
				conf = ScraperSettings.ReadConfig(edfFileName);

				if (libFileName != null)
					conf.ProcessAssembly = libFileName;

				prevLoadedSetting = AppSettings.Default.ScrapSettings.Where(x => x.Name == conf.Name).FirstOrDefault();
				if (prevLoadedSetting != null)
				{
					MessageBox.Show("Datafeed " + prevLoadedSetting.Name + " already loaded");
					return;
					//if (MessageBox.Show("Datafeed " + prevLoadedSetting.Name + " already loaded. Do you want to load new feed?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
					//	return;
					var scr1 = scrapers.Where(x => x.Name == prevLoadedSetting.Name).FirstOrDefault();
					if (scr1 != null)
						scrapers.Remove(scr1);
				}

				if (conf.ProcessAssembly == null)
					return;

				var curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				if (!conf.ProcessAssembly.StartsWith("http"))
				{
					var dllFile = conf.ProcessAssembly;
					if (!Path.IsPathRooted(dllFile))
					{
						var curDir2 = Path.GetDirectoryName(edfFileName);
						dllFileName = Path.Combine(curDir2, conf.ProcessAssembly);
						if (!File.Exists(dllFileName))
						{
							dllFileName = Path.Combine(curDir, dllFileName);
						}
					}
					else
						dllFileName = dllFile;
				}

				if (dllFileName != null && !File.Exists(dllFileName))
				{
					MessagesPanelManager.PrintMessage("File " + dllFileName + " not found", ImportanceLevel.High);
					return;
				}
				var scrapIfaceType = typeof(ISimpleScraper);

				Assembly assembly;
				if (conf.ProcessAssembly.StartsWith("http"))
				{
					PageRetriever pr = new PageRetriever();
					string tmpAsmPath = Path.GetTempFileName() + ".dll";
					pr.SaveFromServer(conf.ProcessAssembly, tmpAsmPath);
					assembly = Assembly.LoadFile(tmpAsmPath);
					//assembly = Assembly.Load(PageRetriever.GetFromServer(conf.ProcessAssembly));
				}
				else
					assembly = Assembly.LoadFile(dllFileName);
				var scrapClasses = assembly.GetTypes().Where(x => x.IsClass && x.GetInterfaces().Contains(scrapIfaceType) && !x.Name.Contains("BaseScraper"));

				ignoreSettingsChangedEvent = true;
				if (prevLoadedSetting != null)
					AppSettings.Default.ScrapSettings.Remove(prevLoadedSetting);

				List<ISimpleScraper> lstNewScrapers = new List<ISimpleScraper>();
				foreach (var scrapT in scrapClasses)
				{
					var scraper = (ISimpleScraper)Activator.CreateInstance(scrapT);
					if (scrapers.Any(x => x.Name == scraper.Name))
						continue;
					//else
					//	AppSettings.Default.ScrapSettings.Add(conf);
					lstNewScrapers.Add(scraper);
					scrapers.Add(scraper);
					scraper.MessagePrinter = new MessagePrinter { AppName = scraper.Name };
					scraper.Log = Program.Log;
					scraper.ItemLoaded += wheelsScr_ItemLoaded;
					scraper.BrandLoaded += scrap_BrandLoaded;
					scraper.SomeEvent += scraper_SomeEvent;
					if (scraper is IAcceptMainApp)
						(scraper as IAcceptMainApp).MainApp = this;
				}

				//AppSettings.Default.ScrapSettings.Add(conf);

				//cbScraper.Properties.Items.Clear();
				foreach (var scraper in lstNewScrapers)
				{
					cbScraper.Properties.Items.Add(scraper.Name);
					var settings = AppSettings.Default.ScrapSettings;
					if (!settings.Any(x => x.Name == scraper.Name))
					{
						if (conf == null)
						{
							var set = new ScraperSettings { Name = scraper.Name, MaxThreads = 1, MaxDelay = 10, FileName = scraper.Name.Replace(".", ""), FtpFileName = scraper.Name.Replace(".", ""), Url = scraper.Url, Googlebot = true, ProcessAssembly = Path.GetFileName(dllFileName) };
							AppSettings.Default.ScrapSettings.Add(set);
							conf = set;
						}
						else
							AppSettings.Default.ScrapSettings.Add(conf);
					}
					var sett = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraper.Name).FirstOrDefault();
					var conf2 = ScraperSettings.ReadConfig(edfFileName, scraper.GetTypesForXmlSerialization());
					AppSettings.Default.ScrapSettings.Remove(sett);
					AppSettings.Default.ScrapSettings.Add(conf2);
					if (conf2.SpecialSettings == null)
						conf2.SpecialSettings = scraper.SpecialSettings;
					scraper.Settings = conf2;

					//nbgFeeds.ItemLinks.Add(new DevExpress.XtraNavBar.NavBarItem(scraper.Name));
				}
				ClearLog();
				bsScrapProps.DataSource = AppSettings.Default.ScrapSettings;
				cbScraper.SelectedItem = conf.Name;
				MessagesPanelManager.PrintMessage("Data feed " + conf.Name + " loaded");
			}
			//catch (Exception err)
			//{
			//	MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.Critical);
			//}
			//ignoreSettingsChangedEvent = false;
		}

		void scraper_SomeEvent(object sender, string eventName, object data)
		{
			var scr = sender as ISimpleScraper;
			if (scr == null)
				return;
			if (eventName == "Pause" || eventName == "Resume")
			{
				RefreshBindings();
			}
		}

		protected void StartResumeTimer(ISimpleScraper scrap)
		{

		}

		protected void LoadProcessor()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select one or multiple EDF files to load";
			ofd.Filter = "Easy data feeds (*.edf)|*.edf";
			ofd.Multiselect = true;
			//ofd.Filter = "Easy data feeds (*.config)|EasyDataFeed.Modules.*.config|All files|*.*";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				foreach (var fn in ofd.FileNames)
					OpenEDF(fn);
			}
		}

		private void btnSaveSettings_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		protected void SaveLibToFile(ISimpleScraper scrap, string dstPath)
		{
			try
			{
				File.Copy(scrap.GetType().Assembly.Location, dstPath);
			}
			catch (Exception err)
			{
				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
			}
		}

		private void SaveSettings()
		{
			bsScrapProps.EndEdit();
			//this is to trigged EndEdit for all binding sources
			gridView1.Focus();
			var sett = GetCurrentConfig();
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "EasyDataFeed (*.edf)|*.edf";
			sfd.FileName = sett.Name + ".edf";
			sfd.RestoreDirectory = true;
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				SaveBrandsToConfig();

				var fName = Path.GetFileName(sett.ProcessAssembly);
				sett.ProcessAssembly = fName;
				var dstFileName = Path.Combine(Path.GetDirectoryName(sfd.FileName), sett.ProcessAssembly);
				SaveLibToFile(GetCurrentScraper(), dstFileName);
				sett.SaveConfig(sfd.FileName);
			}

			//AppSettings.Default.SaveConfig();
			settingsChanged = false;
			EnableSaveSettingsButton();
			MessagesPanelManager.PrintMessage("Saved");
		}

		private void btnStartScrap_Click(object sender, EventArgs e)
		{
			StartCurrentProcessor();
		}

		private void StartCurrentProcessor()
		{
			StartProcessor((string)cbScraper.SelectedItem);
			ShowTabs("Data,Log");
			tcgMain.SelectedTabPage = lcgLog;
		}

		protected List<ProxyInfo> ReadProxies(string filePath)
		{
			List<ProxyInfo> lstProxies = new List<ProxyInfo>();
			StreamReader sr = null;
			try
			{
				if (filePath.ToLower().StartsWith("http://") || filePath.ToLower().StartsWith("https://"))
				{
					var url = filePath;
					var r = (HttpWebRequest)HttpWebRequest.Create(url);

					var resp = r.GetResponse();
					var respStream = resp.GetResponseStream();
					sr = new StreamReader(respStream, Encoding.UTF8);
				}
				else
				{
					if (!File.Exists(filePath))
						return null;
					sr = new StreamReader(filePath);
				}
				string str = null;
				while ((str = sr.ReadLine()) != null)
				{
					str = str.Trim();
					if (str.Length == 0)
						continue;
					ProxyInfo pi = new ProxyInfo { Address = str };
					lstProxies.Add(pi);
				}
			}
			catch (Exception err)
			{
				Program.Log.Error(err);
				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
			}
			finally
			{
				if (sr != null)
					sr.Close();
			}
			return lstProxies;
		}

		private void SetupProcessor(ISimpleScraper scrap, ScraperSettings sett)
		{
			scrap.MaxDelay = sett.MaxDelay;
			scrap.MaxThreads = Math.Min(sett.MaxThreads, sett.ThreadsLimit);
			scrap.LoginInfos = sett.Logins;
			scrap.Googlebot = sett.Googlebot;
			scrap.MaxRecords = sett.LimitRecords ? sett.MaxRecords : 0;
			scrap.UseLogin = sett.UseLogin;
			scrap.DownloadImages = sett.DownloadImages;
			scrap.LocalPath = sett.LocalPath;
			scrap.Settings = sett;
			scrap.SpecialSettings = sett.SpecialSettings;

			var imageFields = sett.ExportProfiles.SelectMany(x => x.Fields).Where(x => x.IsImageColumn).Select(x => x.FieldName).ToList();
			scrap.ImageFields = imageFields;

			if (!string.IsNullOrEmpty(sett.Url))
				scrap.Url = sett.Url;
			if (sett.UseProxy)
			{
				List<ProxyInfo> lstP = new List<ProxyInfo>();
				if (!string.IsNullOrEmpty(sett.ProxyFilePath))
					lstP.AddRange(ReadProxies(sett.ProxyFilePath));
				if (!string.IsNullOrEmpty(sett.ProxyListUrl))
					lstP.AddRange(ReadProxies(sett.ProxyFilePath));
				scrap.Proxies = lstP;
			}
			else
				scrap.Proxies = null;

			var selectedBrands = ccbeBrands.Text.Split(new string[] { "" + ccbeBrands.Properties.SeparatorChar + " " }, StringSplitOptions.RemoveEmptyEntries);
			scrap.BrandsToLoad.Clear();
			scrap.BrandsToLoad.AddRange(selectedBrands);
		}

		void scrap_BrandLoaded(object sender, EventArgs e)
		{
			ForceSave(false, (string)sender);
		}

		private void StartProcessor(string name)
		{
			lock (this)
			{
				var scrap = scrapers.Where(x => x.Name == name).FirstOrDefault();
				if (scrap == null)
					return;
				MessagesPanelManager.PrintMessage("Starting " + name);
				if (scrap.IsRunning)
				{
					MessagesPanelManager.PrintMessage(scrap.Name + " is already running", ImportanceLevel.Mid);
					return;
				}

				var sett = AppSettings.Default.ScrapSettings.Where(x => x.Name == scrap.Name).First();
				if (!CheckIfCanStart(scrap, sett, true))
					return;

				if (!CheckFtpConnection(scrap))
					return;

				if (sett.SaveLocally && string.IsNullOrEmpty(sett.LocalPath))
				{
					MessagesPanelManager.PrintMessage("Local path is not set", ImportanceLevel.Mid);
					return;
				}
				SetupProcessor(scrap, sett);
				/*
				scrap.MaxDelay = sett.MaxDelay;
				scrap.MaxThreads = sett.MaxThreads;
				scrap.ItemLoaded += wheelsScr_ItemLoaded;
				//scrap.MessagePrinter = new MessagePrinter { AppName = scrap.Name };
				scrap.LoginInfos = sett.Logins;
				scrap.Googlebot = sett.Googlebot;
				scrap.MaxRecords = sett.LimitRecords ? sett.MaxRecords : 0;
				scrap.UseLogin = sett.UseLogin;
				scrap.DownloadImages = sett.DownloadImages;
				scrap.LocalPath = sett.LocalPath;
				scrap.Settings = sett;

				if (!string.IsNullOrEmpty(sett.Url))
					scrap.Url = sett.Url;
				if (sett.UseProxy)
				{
					List<ProxyInfo> lstP = new List<ProxyInfo>();
					if (!string.IsNullOrEmpty(sett.ProxyFilePath))
						lstP.AddRange(ReadProxies(sett.ProxyFilePath));
					if (!string.IsNullOrEmpty(sett.ProxyListUrl))
						lstP.AddRange(ReadProxies(sett.ProxyFilePath));
					scrap.Proxies = lstP;
				}
				else
					scrap.Proxies = null;
				*/
				//scrap.Wares.Clear();
				gridView2.RefreshData();
				//btnStartScrap.Enabled = false;
				EnableControls(false);
				sett.IsRunning = true;
				RefreshBindings();


				this.Invoke(new Action(() => { bbiPauseCurrent.Enabled = true; bbiStopCurrent.Enabled = true; bbiStartCurrent.Enabled = false; }));



				//btnPause.Enabled = true;
				var a = new Action(scrap.ProcessAllData);
				a.BeginInvoke(ProcessComplete, scrap);
			}
		}

		private void tsmiLoadProc_Click(object sender, EventArgs e)
		{
			LoadProcessor();
		}

		protected List<FieldInfo> GetFields(ScraperSettings sett)
		{
			if (scrapersQueue.Any(x => x.Name == sett.Name) && AppSettings.Default.InventoryOnly)
			{
				List<string> strs = new List<string>{"Supplier",
				"Qty",
				"Brand",
				"Part Number",
				"Warehouse",
				"Product Title"};
				var res = sett.Fields.Where(x => strs.Contains(x.Header));
				return res.ToList();
			}
			else if (scrapersQueue.Any(x => x.Name == sett.Name) && AppSettings.Default.PricesOnly)
			{
				List<string> strs = new List<string>{"Supplier",
				"Brand",
				"Part Number",
				"Warehouse",
				"Product Title",
				"Cost",
				"MSRP",
				"Jobber",
				"ListPrice"};
				var res = sett.Fields.Where(x => strs.Contains(x.Header));
				return res.ToList();
			}
			return sett.Fields;
		}

		protected List<FieldInfo> GetFields2(ScraperSettings sett, ExportProfile profile)
		{
			if (scrapersQueue.Any(x => x.Name == sett.Name) && AppSettings.Default.InventoryOnly)
			{
				List<string> strs = new List<string>{"Supplier",
				"Qty",
				"Brand",
				"Part Number",
				"Warehouse",
				"Product Title"};
				//var res = sett.Fields.Where(x => strs.Contains(x.Header));
				var res = profile.Fields.Where(x => strs.Contains(x.Header));
				return res.ToList();
			}
			else if (scrapersQueue.Any(x => x.Name == sett.Name) && AppSettings.Default.PricesOnly)
			{
				List<string> strs = new List<string>{"Supplier",
				"Brand",
				"Part Number",
				"Warehouse",
				"Product Title",
				"Cost",
				"MSRP",
				"Jobber",
				"ListPrice"};
				//var res = sett.Fields.Where(x => strs.Contains(x.Header));
				var res = profile.Fields.Where(x => strs.Contains(x.Header));
				return res.ToList();
			}
			//return sett.Fields;
			return profile.Fields;
		}

		protected Dictionary<string, List<string>> SaveResultsLocaly2(ISimpleScraper scraper, ExportSettings ex, bool byBrand, string brandName, bool zip, out string fullResultsFile)
		{
			var dicFiles = new Dictionary<string, List<string>>();
			fullResultsFile = null;
			var scrS = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraper.Name).First();

			var profile = scrS.ExportProfiles.Where(x => x.Name == ex.ProfileName).FirstOrDefault();
			if (profile == null)
			{
				MessagesPanelManager.PrintMessage("Profile \"" + ex.ProfileName + "\" not found!", ImportanceLevel.High);
				return dicFiles;
			}

			var wares = scraper.Wares.Where(x => x.Brand == brandName || brandName == null).ToList();
			//if (wares.Count() == 0)
			//	wares = scraper.Wares;
			var brByGr = wares.GroupBy(x => (x.Brand == null || !byBrand) ? "" : x.Brand.Replace("/", "_").Replace("\\", "_").Replace("&", "_").Replace("|", "_").ToLower());
			var csvExporter = new ScrapResultCsvExporter(scraper, AppSettings.Default.GetExtSettings(scraper.Name).ValidateAgainstAddData, scrS);
			foreach (var brGr in brByGr)
			{
				dicFiles.Add(brGr.Key, new List<string>());
				var maxRecords = ex.MaxRecords > 0 ? ex.MaxRecords : int.MaxValue;
				int numPages = (int)Math.Ceiling((double)brGr.Count() / maxRecords);
				List<WareInfo> badWares = new List<WareInfo>(); ;
				List<WareInfo> tmpBadWares;

				for (int i = 0; i < numPages; i++)
				{
					var dataRecords = brGr.Skip(i * maxRecords).Take(maxRecords).ToList();
					var tempFile = Path.GetTempFileName();
					try
					{
						csvExporter.Export(dataRecords, GetFields2(scrS, profile), tempFile, zip, out tmpBadWares, scrS.OutputBadDataToError, false, profile);
						dicFiles[brGr.Key].Add(tempFile);

						if (tmpBadWares.Count > 0)
							badWares.AddRange(tmpBadWares);
					}
					catch (Exception err)
					{
						Program.Log.Error(err.Message);
						MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
					}
				}
				if (badWares.Count > 0 && scrS.OutputBadDataToError)
				{
					var key = brGr.Key + "ERROR_";
					dicFiles.Add(key, new List<string>());
					var tempFile = Path.GetTempFileName();
					csvExporter.Export(badWares, GetFields2(scrS, profile), tempFile, zip, out tmpBadWares, true, true, profile);
					dicFiles[key].Add(tempFile);
				}
			}
			return dicFiles;
		}


		//protected Dictionary<string, string> SaveResultsLocaly(ISimpleScraper scraper, string brandName, out string fullResultsFile)
		//{
		//	Dictionary<string, string> files = new Dictionary<string, string>();
		//	fullResultsFile = null;
		//	var scrS = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraper.Name).First();

		//	if (scrS.LocalSaveByBrand || scrS.FtpSaveByBrand)
		//	{
		//		var wares = scraper.Wares.Where(x => x.Brand == brandName || brandName == null).ToList();
		//		//if (wares.Count() == 0)
		//		//	wares = scraper.Wares;
		//		var brByGr = wares.GroupBy(x => x.Brand == null ? "" : x.Brand.Replace("/", "_").Replace("\\", "_").Replace("&", "_").ToLower());
		//		foreach (var brGr in brByGr)
		//		{
		//			var tempFile = Path.GetTempFileName();
		//			try
		//			{
		//				ScrapResultCsvExporter.Export(brGr.Select(x => x).ToList(), GetFields(scrS), tempFile);
		//				files.Add(brGr.Key, tempFile);
		//			}
		//			catch (Exception err)
		//			{
		//				Program.Log.Error(err.Message);
		//				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
		//			}
		//		}
		//	}
		//	if ((scrS.UseFtp && !scrS.FtpSaveByBrand) || (scrS.SaveLocally && !scrS.LocalSaveByBrand))
		//	{
		//		fullResultsFile = Path.GetTempFileName();
		//		try
		//		{
		//			ScrapResultCsvExporter.Export(scraper.Wares, GetFields(scrS), fullResultsFile);
		//		}
		//		catch (Exception err)
		//		{
		//			Program.Log.Error(err.Message);
		//			MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
		//		}
		//	}
		//	return files;
		//}

		//protected void SaveResult(ISimpleScraper scraper)
		//{
		//	var scrS = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraper.Name).First();
		//	string localFile = scrS.FileName;
		//	var tempFile = Path.GetTempFileName();

		//	if (string.IsNullOrEmpty(scrS.FileName))
		//		localFile = scraper.Name + ".csv";

		//	if (!scrS.OverwriteFile)
		//	{
		//		localFile = Path.GetFileNameWithoutExtension(localFile) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(localFile);
		//	}
		//	if (scrS.LocalPath != null)
		//		localFile = Path.Combine(scrS.LocalPath, localFile);

		//	var ftpFile = scrS.FtpFileName;
		//	if (!scrS.OverwriteFtpFile)
		//	{
		//		ftpFile = Path.GetFileNameWithoutExtension(ftpFile) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")  + Path.GetExtension(localFile);
		//	}

		//	try
		//	{
		//		ScrapResultCsvExporter.Export(scraper.Wares, scrS.Fields, tempFile);
		//		if (scrS.SaveLocally)
		//		{
		//			try
		//			{
		//				File.Copy(tempFile, localFile, true);
		//			}
		//			catch (Exception err)
		//			{
		//				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
		//			}
		//		}
		//		if(scrS.UseFtp)
		//			UploadResultToFtp(scraper, tempFile, ftpFile);
		//	}
		//	catch (Exception err)
		//	{
		//		MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
		//	}			
		//}

		protected bool CheckFtpConnection(ISimpleScraper scraper)
		{
			try
			{
				var sett = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraper.Name).First();
				if (!sett.UseFtp)
					return true;
				MessagesPanelManager.PrintMessage("Checking ftp server");
				var res = FtpHelper.CheckConnection(sett.FtpAddress, sett.FtpUsername, sett.FtpPassword);
				MessagesPanelManager.PrintMessage("Ftp server OK");
				return res;
			}
			catch (Exception err)
			{
				Program.Log.Error(err.Message);
				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
				return false;
			}
		}

		protected void SaveResult2(ISimpleScraper scraper, bool onlyFtp = false, string brandName = null)
		{
			var scrS = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraper.Name).First();
			var tempFile = Path.GetTempFileName();
			string localFile = scrS.FileName;
			var ftpFile = scrS.FtpFileName;

			string fullResultsFile = null;

			//moved to BaseScraper
			//foreach (var item in scraper.Wares)
			//	ConvertAndUpload(item, scrS);

			if (scrS.SaveLocally)
			{
				foreach (var ex in scrS.LocalExportSettings)
				{
					var lstFilesToSave = SaveResultsLocaly2(scraper, ex, scrS.LocalSaveByBrand, brandName, ex.Zip, out fullResultsFile);
					CopyFiles(scraper, lstFilesToSave, ex.FileName, scrS.OverwriteFile, scrS.LocalPath, false, true, ex.Zip);
				}
			}

			if (scrS.UseFtp)
			{
				foreach (var ex in scrS.FtpExportSettings)
				{
					var lstFilesToSave = SaveResultsLocaly2(scraper, ex, scrS.FtpSaveByBrand, brandName, ex.Zip, out fullResultsFile);
					CopyFiles(scraper, lstFilesToSave, ex.FileName, scrS.OverwriteFtpFile, null, true, false, ex.Zip);
				}
			}
		}

		protected void CopyFiles(ISimpleScraper scraper, Dictionary<string, List<string>> dicFiles, string fileName, bool overwriteFile, string path, bool ftpSave, bool localSave, bool zip)
		{
			foreach (var br in dicFiles)
			{
				int pageNum = 0;
				foreach (var savedFile in br.Value)
				{
					var srcFile = savedFile;
					pageNum++;
					string pageNumSrt = br.Value.Count > 1 ? string.Format("_{0}", pageNum) : "";
					string localFile = fileName;
					if (!string.IsNullOrEmpty(br.Key))
						localFile += "_" + br.Key;
					localFile += pageNumSrt;

					if (!overwriteFile)
						localFile = localFile + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
					if (path != null)
						localFile = Path.Combine(path, localFile);
					localFile += ".csv";
					if (zip)
						localFile += ".zip";

					if (zip)
					{
						var zipFileName = Path.GetTempFileName();
						var zf = new ZipFile();
						var ze = zf.AddFile(savedFile);
						ze.FileName = Path.GetFileNameWithoutExtension(localFile);
						zf.Save(zipFileName);
						srcFile = zipFileName;
						File.Delete(savedFile);
					}
					if (localSave)
					{
						try
						{
							File.Copy(srcFile, localFile, true);
							MessagesPanelManager.PrintMessage("File saved: " + localFile);
						}
						catch (Exception err)
						{
							Program.Log.Error(err.Message);
							MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
						}
					}
					if (ftpSave)
					{
						UploadResultToFtp(scraper, srcFile, localFile);
					}
					File.Delete(srcFile);
				}
			}
		}

		protected void SaveResult(ISimpleScraper scraper, bool onlyFtp = false, string brandName = null)
		{
			SaveResult2(scraper, onlyFtp, brandName);
			return;

			//var scrS = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraper.Name).First();
			//var tempFile = Path.GetTempFileName();
			//string localFile = scrS.FileName;
			//var ftpFile = scrS.FtpFileName;

			//string fullResultsFile = null;

			//var lstFiles = SaveResultsLocaly(scraper, brandName, out fullResultsFile);
			//try
			//{
			//	foreach (var br in lstFiles)
			//	{
			//		//if (scrS.SaveLocally && scrS.LocalSaveByBrand)
			//		{
			//			localFile = scrS.FileName;
			//			if (string.IsNullOrEmpty(scrS.FileName))
			//				localFile = scraper.Name;
			//			localFile += "_" + br.Key;

			//			if (!scrS.OverwriteFile)
			//				localFile = localFile + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
			//			if (scrS.LocalPath != null)
			//				localFile = Path.Combine(scrS.LocalPath, localFile);
			//			localFile += ".csv";

			//			ftpFile = scrS.FtpFileName;
			//			ftpFile += "_" + br.Key;
			//			if (!scrS.OverwriteFtpFile)
			//				ftpFile = ftpFile + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
			//			ftpFile += ".csv";

			//			if (scrS.LocalSaveByBrand && scrS.SaveLocally && !onlyFtp)
			//			{
			//				try
			//				{
			//					File.Copy(br.Value, localFile, true);
			//					MessagesPanelManager.PrintMessage("File saved: " + localFile);
			//				}
			//				catch (Exception err)
			//				{
			//					Program.Log.Error(err.Message);
			//					MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
			//				}
			//			}
			//			if (scrS.UseFtp && scrS.FtpSaveByBrand)
			//				UploadResultToFtp(scraper, br.Value, ftpFile);
			//		}
			//	}

			//	localFile = scrS.FileName;
			//	if (string.IsNullOrEmpty(scrS.FileName))
			//		localFile = scraper.Name + ".csv";
			//	if (!scrS.OverwriteFile)
			//		localFile = localFile + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
			//	if (scrS.LocalPath != null)
			//		localFile = Path.Combine(scrS.LocalPath, localFile);
			//	localFile += ".csv";
			//	if (scrS.SaveLocally && !scrS.LocalSaveByBrand && fullResultsFile != null && !onlyFtp)
			//	{
			//		File.Copy(fullResultsFile, localFile, true);
			//		MessagesPanelManager.PrintMessage("File saved: " + localFile);
			//	}

			//	ftpFile = scrS.FtpFileName;
			//	if (!scrS.OverwriteFtpFile)
			//		ftpFile = Path.GetFileNameWithoutExtension(ftpFile) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(localFile);
			//	if (scrS.UseFtp && !scrS.FtpSaveByBrand && fullResultsFile != null)
			//		UploadResultToFtp(scraper, fullResultsFile, ftpFile);

			//	//foreach (var item in scraper.Wares)
			//	//	ConvertAndUpload(item, scrS);
			//}
			//catch (Exception err)
			//{
			//	Program.Log.Error(err.Message);
			//	MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
			//}
		}

		protected void ConvertAndUpload(WareInfo wi, ScraperSettings sett)
		{
			try
			{
				if (string.IsNullOrEmpty(wi.LocalImageFile))
					return;

				var imgLocalFiles = wi.LocalImageFile.Split(',');
				foreach (var imgLocalFile in imgLocalFiles)
				{
					if (sett.UseFtp && sett.UploadImagesToFtp)
					{
						var fileName = System.IO.Path.GetFileName(imgLocalFile);
						FtpHelper.UploadFileToFtp(sett.FtpAddress, sett.FtpUsername, sett.FtpPassword, fileName, imgLocalFile);
					}

					string ext = System.IO.Path.GetExtension(imgLocalFile).ToLower();
					if (sett.ConvertImages && ext != ".jpg" && ext != ".pdf")
					{
						var convertedFileName = System.IO.Path.GetFileNameWithoutExtension(imgLocalFile) + ".jpg";
						var convertedFilePath = convertedFileName;
						if (sett.LocalPath != null)
							convertedFilePath = System.IO.Path.Combine(sett.LocalPath, convertedFileName);
						BaseScraper.ConvertImage(imgLocalFile, convertedFilePath);
						if (sett.UseFtp && sett.UploadImagesToFtp)
							FtpHelper.UploadFileToFtp(sett.FtpAddress, sett.FtpUsername, sett.FtpPassword, convertedFileName, convertedFilePath);
					}
				}
			}
			catch (Exception err)
			{
				Program.Log.Error(err.Message);
				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
			}
		}

		protected void UploadResultToFtp(ISimpleScraper scraper, string localFile, string ftpFile)
		{
			var scrS = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraper.Name).First();
			if (string.IsNullOrEmpty(scrS.FtpAddress))
				return;
			try
			{
				if (!File.Exists(localFile))
					return;
				MessagesPanelManager.PrintMessage("Uploading to ftp");
				var ftpFilePath = FtpHelper.UploadFileToFtp(scrS.FtpAddress, scrS.FtpUsername, scrS.FtpPassword, ftpFile, localFile);
				MessagesPanelManager.PrintMessage("Uploaded to ftp");
				//var ftpFilePath = scrS.FtpAddress + ftpFile;
				scraper.OnFileUploaded(ftpFilePath);

				if (scrS.SCEBatchProcess)
					WheelsScraper.SCE.SCEManager.BatchUpdateFile(scrS, ftpFilePath);
			}
			catch (Exception err)
			{
				Program.Log.Error(err);
				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
			}
		}

		private void gridView3_KeyPress(object sender, KeyPressEventArgs e)
		{
			//if(e.)
		}

		private void gridControl2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				gridView3.DeleteSelectedRows();
				e.Handled = true;
			}
		}

		private void gridControl3_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				gridView4.DeleteSelectedRows();
				e.Handled = true;
			}
		}

		private void tsmiExit_Click(object sender, EventArgs e)
		{
			ExitApp();
		}

		void ExitApp()
		{
			if (scrapers.Any(x => x.IsRunning))
				if (MessageBox.Show("Some processors are runnig. Close the application?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					return;
			this.Close();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			ribbonControl1.ForceInitialize();
			RefreshSkins();
			ShowTabs("Data,Log");
			
			layoutControl2.Dock = DockStyle.Fill;
			rtMgr = new RuntimeManager();
			rtMgr.ResumeScraper = PauseScraper;

			scrapersQueue = new List<ScraperSettings>();
			FtpHelper.MessagePrinter = new MessagePrinter { AppName = "" };
			tmrSchedule = new System.Threading.Timer(TimerProc, null, 10 * 1000, 1 * 1000);
			MessagesPanelManager.PrintMessage("Starting scheduler in 10 seconds");
			List<SyncInterval> lstSync = new List<SyncInterval>();
			lstSync.Add(new SyncInterval { Name = "Every 15 minutes", Interval = 15 });
			lstSync.Add(new SyncInterval { Name = "Every 30 minutes", Interval = 30 });
			lstSync.Add(new SyncInterval { Name = "Every 1 hour", Interval = 60 });
			lstSync.Add(new SyncInterval { Name = "Every 6 hours", Interval = 60 * 6 });
			lstSync.Add(new SyncInterval { Name = "Every 12 hours", Interval = 60 * 12 });
			lstSync.Add(new SyncInterval { Name = "Every Day", Interval = 60 * 24 });
			lstSync.Add(new SyncInterval { Name = "Every Week", Interval = 60 * 24 * 7 });
			lstSync.Add(new SyncInterval { Name = "Every Month", Interval = 60 * 24 * 30 });
			repositoryItemLookUpEdit1.DataSource = lstSync;

			List<SyncInterval> lstSync2 = new List<SyncInterval>();
			lstSync2.Add(new SyncInterval { Name = "15 minutes", Interval = 15 });
			lstSync2.Add(new SyncInterval { Name = "30 minutes", Interval = 30 });
			lstSync2.Add(new SyncInterval { Name = "1 hour", Interval = 60 });
			lstSync2.Add(new SyncInterval { Name = "6 hours", Interval = 60 * 6 });
			lstSync2.Add(new SyncInterval { Name = "12 hours", Interval = 60 * 12 });
			lstSync2.Add(new SyncInterval { Name = "24 hours", Interval = 60 * 24 });
			leInterval.Properties.DataSource = lstSync2;

			RefreshBindings();
			EnableSaveSettingsButton();
			bbiStopCurrent.Enabled = false;
			//btnClearData.Enabled = false;
			bbiPauseCurrent.Enabled = false;
			AssociateExtension();
			//cbEnableSchedule.Checked = AppSettings.Default.EnableSchedule;
			//deStartAt.EditValue = AppSettings.Default.StartAt;
			//deEnd.EditValue = AppSettings.Default.EndAt;
			//spMaxProcs.EditValue = AppSettings.Default.MaxProcessors;
			bsProp.DataSource = AppSettings.Default;
			//EnableSchedule();
			var arguments = Environment.GetCommandLineArgs();
			if (arguments.Length > 1)
			{
				var edfToLoad = arguments[1].ToLower();
				if (edfToLoad.EndsWith(".edf"))
					DoLoadProcessorConfig(edfToLoad);
			}
			else
				LoadAllDatafeeds();
		}

		System.Threading.Timer tmrSchedule;

		DateTime globaLastRun;
		private void CheckSchedule()
		{
			lock (this)
			{
				//if (allowGlobalSchedule)
				{
					UpdateNextRunGlobal();
					if (AppSettings.Default.EnableSchedule)
					{
						if (DateTime.Now > globaLastRun.AddMinutes(AppSettings.Default.IntervalMinutes) && DateTime.Now.TimeOfDay >= AppSettings.Default.StartAt.TimeOfDay && DateTime.Now.TimeOfDay < AppSettings.Default.EndAt.TimeOfDay)
						{
							if (scrapersQueue.Count == 0)
							{
								var scrs = AppSettings.Default.ScrapSettings.Join(scrapers.Where(z => !z.IsRunning), x => x.Name, y => y.Name, (x, y) => x);
								scrapersQueue.AddRange(scrs);
								lblNextRun.Text = " ";
							}
							//RunAll();
							//return;
						}
						RunAll();
					}
					if (AppSettings.Default.EnableSchedule)
						return;
				}
				RunAll();
				//
				NormaliseSchedule();
				var procs = AppSettings.Default.ScrapSettings.Where(x => x.EnableSchedule && scrapers.Where(y => !y.IsRunning).Any(y => y.Name == x.Name)).ToList();
				foreach (var proc in procs)
				{
					foreach (var sc in proc.Schedule)
					{
						//var r1 = DateTime.Today.AddTicks(sc.Start.Ticks);
						//var r2 = DateTime.Today.AddTicks(sc.End.Ticks);
						//var r3 = proc.LastRun.AddMinutes(sc.Interval);
						//if (sc.Enabled)
						{
							var nextRun = proc.LastRun.AddMinutes(sc.Interval);
							if (DateTime.Now > DateTime.Today.AddTicks(sc.End.Ticks))
								nextRun = DateTime.Today.AddDays(1).AddTicks(sc.Start.Ticks);
							if (nextRun > DateTime.Today.AddTicks(sc.End.Ticks))
								nextRun = DateTime.Today.AddDays(1).AddTicks(sc.Start.Ticks);
							if (nextRun < DateTime.Today.AddTicks(sc.Start.Ticks))
								nextRun = DateTime.Today.AddTicks(sc.Start.Ticks);
							sc.NextRunIn = nextRun - DateTime.Now;
							if (sc.NextRunIn.TotalMinutes < 0)
								sc.NextRunIn = new TimeSpan();
							sc.NextRunIn = new TimeSpan(sc.NextRunIn.Hours, sc.NextRunIn.Minutes, sc.NextRunIn.Seconds);
							this.Invoke(new Action(() => { scheduleItemBindingSource.ResetBindings(false); }));
						}
					}
					if (proc.Schedule.Any(s => DateTime.Today.AddTicks(s.Start.Ticks) <= DateTime.Now && DateTime.Today.AddTicks(s.End.Ticks) >= DateTime.Now && proc.LastRun.AddMinutes(s.Interval) <= DateTime.Now))
					{
						Action<string> act = new Action<string>(StartProcessor);
						this.Invoke(act, proc.Name);
						//proc.LastRun = DateTime.Now;
					}

				}
			}
		}

		private void NormaliseSchedule()
		{
			AppSettings.Default.ScrapSettings.SelectMany(x => x.Schedule).ToList().ForEach(x =>
			{
				x.Start = new DateTime(1, 1, 1, x.Start.Hour, x.Start.Minute, x.Start.Second);
				x.End = new DateTime(1, 1, 1, x.End.Hour, x.End.Minute, x.End.Second);
			});
		}

		private void TimerProc(object state)
		{
			try
			{
				this.Invoke(new Action(CheckSchedule));
			}
			catch (Exception err)
			{
			}
		}

		private void txtLocalPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				txtLocalPath.Text = fbd.SelectedPath;
			}
		}

		protected ScraperSettings GetCurrentConfig()
		{
			var scraperName = (string)cbScraper.SelectedItem;
			var curSet = AppSettings.Default.ScrapSettings.Where(x => x.Name == scraperName).FirstOrDefault();
			return curSet;
		}

		EDFManager edfMgr;

		protected ISimpleScraper GetCurrentScraper()
		{
			var scraperName = (string)cbScraper.SelectedItem;
			var scrap = scrapers.Where(x => x.Name == scraperName).FirstOrDefault();
			return scrap;
		}

		protected bool EditExportProfiles(ExportProfile profile = null)
		{
			var curSett = GetCurrentConfig();
			if (curSett == null)
				return false;
			ExportFieldsSelector ef = new ExportFieldsSelector();

			SetupCurrentProcessor();
			ef.FieldType = GetCurrentScraper().WareInfoType;
			ef.Sett = curSett;
			ef.Scraper = GetCurrentScraper();
			ef.SelectedProfile = profile;
			//if (ef.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			//{
			//	RefreshBindings();
			//	return true;
			//}
			return false;
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			EditExportProfiles();
		}

		private void loginInfosBindingSource_AddingNew(object sender, AddingNewEventArgs e)
		{
			e.NewObject = new LoginInfo { IsActive = true };
		}

		private void beProxies_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)
				PickProxiesFile();
			if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
				beProxies.EditValue = "";
		}

		protected void PickProxiesFile()
		{
			OpenFileDialog ofd = new OpenFileDialog();

			ofd.Filter = "Proxies list (*.txt)|*.txt";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				beProxies.Text = ofd.FileName;
			}
		}

		private void checkEdit4_CheckedChanged(object sender, EventArgs e)
		{
			UpdateFtpSettings();
		}

		protected void UpdateFtpSettings()
		{
			txtFtpServer.Enabled = cbUseFtp.Checked;
			txtFtpPassword.Enabled = cbUseFtp.Checked;
			txtFtpLogin.Enabled = cbUseFtp.Checked;
			checkEdit1.Enabled = cbUseFtp.Checked;
			gridControl5.Enabled = cbUseFtp.Checked;
			cbFtpByBrand.Enabled = cbUseFtp.Checked;
			btnCheckFtp.Enabled = cbUseFtp.Checked;
			UpdateLimit();
		}

		private void cbSaveLocally_CheckedChanged(object sender, EventArgs e)
		{
			UpdateLocalSettings();
		}

		protected void UpdateLocalSettings()
		{
			checkEdit2.Enabled = cbSaveLocally.Checked;
			txtLocalPath.Enabled = cbSaveLocally.Checked;
			cbLocalByBrand.Enabled = cbSaveLocally.Checked;
			gridControl4.Enabled = cbSaveLocally.Checked;
		}

		private void btnLoad_Click(object sender, EventArgs e)
		{
			LoadLocalFeed();
		}

		protected void LoadLocalFeed()
		{
			if (settingsChanged)
				if (MessageBox.Show("Save existing settings?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
					SaveSettings();
			LoadProcessor();
			EnableSaveSettingsButton();
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://shoppingcartelite.wufoo.com/forms/w7w0s1/");
		}

		private bool settingsChanged;

		private void bsScrapProps_CurrentItemChanged(object sender, EventArgs e)
		{
			if (!ignoreSettingsChangedEvent)
			{
				settingsChanged = true;
				EnableSaveSettingsButton();
			}
		}

		private void EnableSaveSettingsButton()
		{
			UpdateLimit();
			//btnSaveSettings.Enabled = GetCurrentConfig() != null;
		}

		private void loginInfosBindingSource_CurrentItemChanged(object sender, EventArgs e)
		{
			//if (!ignoreSettingsChangedEvent)
			//{
			//	settingsChanged = true;
			//	EnableSaveSettingsButton();
			//}
		}

		private void scheduleItemBindingSource_CurrentItemChanged(object sender, EventArgs e)
		{
			//if (!ignoreSettingsChangedEvent)
			//{
			//	settingsChanged = true;
			//	EnableSaveSettingsButton();
			//}
		}

		private void gridView3_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
		{
			settingsChanged = true;
			EnableSaveSettingsButton();
		}

		private void gridView4_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
		{
			settingsChanged = true;
			EnableSaveSettingsButton();
		}

		private void cbLimitRecords_CheckedChanged(object sender, EventArgs e)
		{
			UpdateLimit();
		}

		protected void UpdateLimit()
		{
			var sett = GetCurrentConfig();
			if (sett == null)
				return;

			txtMaxRecords.Enabled = cbLimitRecords.Checked;
			gridControl2.Enabled = cbUseLogin.Checked;
			beProxies.Enabled = cbUseProxy.Checked;
			txtProxyUrl.Enabled = cbUseProxy.Checked;
			gridControl3.Enabled = cbSchedule.Checked;
			cbConvertToJpg.Enabled = cbGetImages.Checked;
			cbUploadImageToFtp.Enabled = cbGetImages.Checked && cbUseFtp.Checked;
			txtImgPubPrefix.Enabled = cbGetImages.Checked;

			ceSkipExisting.Enabled = sett.DownloadImages;
			ceDontUpload.Enabled = sett.DownloadImages && sett.SkipExistingImages;
			ceDontOverwrite.Enabled = sett.DownloadImages;
			ceConvertPDF.Enabled = sett.DownloadImages;
		}

		private void checkEdit5_CheckedChanged(object sender, EventArgs e)
		{
			UpdateLimit();
		}

		private void checkEdit3_CheckedChanged(object sender, EventArgs e)
		{
			UpdateLimit();
		}

		private void button1_Click_2(object sender, EventArgs e)
		{
			CheckFtpConnection(GetCurrentScraper());
		}

		private void AssociateExtension()
		{
			try
			{
				var curExe = System.Reflection.Assembly.GetExecutingAssembly().Location;
				FileAssociation.Associate(".edf", "EasyDataFeed", "EasyDataFeed settins file", curExe, curExe);
			}
			catch (Exception err)
			{
				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.Mid);
			}
		}

		private void ClearLog()
		{
			Messages.Clear();
		}

		private void btnClearLog_Click(object sender, EventArgs e)
		{
			ClearLog();
		}

		private void btnPause_Click(object sender, EventArgs e)
		{
			PauseCurrentScraper();
		}

		protected void PauseCurrentScraper()
		{
			var scrap = GetCurrentScraper();
			PauseScraper(scrap);
		}

		public bool CheckIfCanStart(ISimpleScraper scrap, ScraperSettings sett, bool validateAPIKey)
		{
			if (validateAPIKey)
				if (!SCE.SCEManager.ValidateLicense(sett, scrap.RequireSCEAPIKey))
					return false;
			var lv = new WheelsScraper.SCE.LicenseValidator();
			AuthInfo authInfo;
			var canStart = lv.Authorize(sett.SCEAPIKey, scrap.Name, out authInfo);
			if (!canStart)
				return false;
			sett.ThreadsLimit = authInfo.MaxThread;
			sett.StartDate = DateTime.Now;
			sett.TimeLeft = authInfo.TimeLeft;
			sett.MaxDelay = Math.Max(sett.MaxDelay, authInfo.Delay);
			rtMgr.StartScraperResumeTimer(scrap, authInfo);
			return true;
		}

		protected void PauseScraper(ISimpleScraper scrap)
		{
			//var scrap = GetCurrentScraper();
			if (scrap == null)
				return;
			//if (!scrap.IsRunning)
			//	return;
			if (scrap.IsPaused)
			{
				var sett = AppSettings.Default.ScrapSettings.Where(x => x.Name == scrap.Name).First();
				if (!CheckIfCanStart(scrap, sett, true))
					return;
				scrap.Resume();
			}
			else
			{
				scrap.Pause();
				var sett = AppSettings.Default.ScrapSettings.Where(x => x.Name == scrap.Name).First();
				SCE.LicenseValidator.ReportUsage(sett.SCEAPIKey, sett.Name, sett.StartDate, DateTime.Now - sett.StartDate);
			}
			RefreshBindings();
		}

		private void cbSchedule_CheckedChanged(object sender, EventArgs e)
		{
			UpdateLimit();
		}

		private void btnClearData_Click(object sender, EventArgs e)
		{
		}

		private void ClearData()
		{
			var scrap = GetCurrentScraper();
			if (scrap == null)
				return;
			if (MessageBox.Show("Clear data?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
				return;
			scrap.Wares.Clear();
			((System.Collections.IList)scrap.WareInfoList).Clear();
			wareInfoBindingSource.ResetBindings(false);
		}

		protected void LoadLocalDatafeeds()
		{
			var curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			var files = Directory.GetFiles(curDir, "*.edf");
			foreach (var file in files)
				DoLoadProcessorConfig(file);
		}

		protected void LoadDatafeedFromWeb(string urlList)
		{
			if (urlList == null)
				urlList = AppSettings.Default.EdfListUrl;

			var strsUrls = urlList.Split('\n');
			foreach (var url3 in strsUrls)
			{
				var url = url3.Trim();
				try
				{
					PageRetriever pr = new PageRetriever();
					if (!url.StartsWith("https://dl.dropbox.") && url.StartsWith("https://www.dropbox."))
						url = url.Replace("https://www.", "https://dl.");

					MessagesPanelManager.PrintMessage("Loading data feed list from " + url);
					var list = pr.ReadFromServer(url);
					var strs = list.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var url2 in strs)
					{
						var localFile = Path.GetTempFileName();
						pr.SaveFromServer(url2, localFile);
						DoLoadProcessorConfig(localFile);
						File.Delete(localFile);
					}
				}
				catch (Exception err)
				{
					Program.Log.Error(err.Message);
					MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
				}
			}
		}

		public void OpenEDF(string fileName)
		{
			if (fileName.StartsWith("http"))
				LoadDatafeedFromWeb(fileName);
			else
				LoadEDF(fileName, null);
			AddFileToMRU(fileName);
		}

		protected void LoadAllDatafeeds()
		{
			try
			{
				if (string.IsNullOrEmpty(AppSettings.Default.EdfListUrl))
				{
					LoadLocalDatafeeds();
					return;
				}
				LoadDatafeedFromWeb(null);
			}
			catch (Exception err)
			{
				Program.Log.Error(err.Message);
				MessagesPanelManager.PrintMessage(err.Message, ImportanceLevel.High);
			}
		}

		private void llDB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://db.tt/5nXZMIP");
		}

		private void btnForceUpload_Click(object sender, EventArgs e)
		{
			ForceFTPUpload();
		}

		private void ForceFTPUpload()
		{
			var scrap = GetCurrentScraper();
			if (scrap == null)
				return;
			SaveResult(scrap, true);
		}

		private void tsmiLoadFromWeb_Click(object sender, EventArgs e)
		{
			LoadEDFFromWeb();
		}

		private void LoadEDFFromWeb()
		{
			var frm = new PickUrlForm();
			if (frm.ShowDialog() == DialogResult.OK)
			{
				AppSettings.Default.EdfListUrl = frm.Url;
				AppSettings.Default.SaveConfig();
				LoadDatafeedFromWeb(frm.Url);
			}
		}

		private void cbGetImages_CheckedChanged(object sender, EventArgs e)
		{
			UpdateLimit();
		}

		private void txtFtpServer_EditValueChanged(object sender, EventArgs e)
		{
			var newUrl = txtFtpServer.Text.Replace("ftp://", "http://").Replace("ftp.", "www.");

			if (txtFtpServer.Text.Length > 10 && (txtImgPubPrefix.Text == "" || txtImgPubPrefix.Text.StartsWith(newUrl) || newUrl.StartsWith(txtImgPubPrefix.Text)))
			{
				var sett = (ScraperSettings)bsScrapProps.Current;
				sett.ImagesPublicPath = newUrl;
			}
		}

		private void tsmiExit_Click_1(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnRunAll_Click(object sender, EventArgs e)
		{
			RunAllModules();
		}

		private void RunAllModules()
		{
			scrapersQueue.Clear();
			scrapersQueue.AddRange(AppSettings.Default.ScrapSettings);
			RunAll();
		}

		List<ScraperSettings> scrapersQueue;
		List<ScraperSettings> scrapInProcess;

		private void RunAll()
		{
			var numOfRunning = scrapers.Where(x => x.IsRunning).Count();
			if (numOfRunning >= AppSettings.Default.MaxProcessors)
				return;
			var r2 = scrapersQueue.Where(x => !x.IsRunning).Join(scrapers.Where(x => !x.IsRunning), x => x.Name, y => y.Name, (x, y) => x)
				.Take(AppSettings.Default.MaxProcessors - numOfRunning).ToList();
			r2.ForEach(x => StartProcessor(x.Name));
		}

		private void cbEnableSchedule_CheckedChanged(object sender, EventArgs e)
		{
			//EnableSchedule();
		}

		private void EnableSchedule()
		{
			deStartAt.Enabled = cbEnableSchedule.Checked;
			deEnd.Enabled = cbEnableSchedule.Checked;
			spMaxProcs.Enabled = cbEnableSchedule.Checked;
			leInterval.Enabled = cbEnableSchedule.Checked;
			cbInventoryOnly.Enabled = cbEnableSchedule.Checked;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			//AppSettings.Default.EnableSchedule = cbEnableSchedule.Checked;
			//AppSettings.Default.StartAt = (DateTime)deStartAt.EditValue;
			//AppSettings.Default.EndAt = (DateTime)deEnd.EditValue;
			//AppSettings.Default.MaxProcessors = (int)(decimal)spMaxProcs.EditValue;
			AppSettings.Default.SaveConfig();
			if (tmrSchedule != null)
				tmrSchedule.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
			CancelAll();
		}

		private void spMaxProcs_EditValueChanged(object sender, EventArgs e)
		{
			//AppSettings.Default.MaxProcessors = (int)(decimal)spMaxProcs.EditValue;
		}

		private void btnCancelAll_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Cancel ALL Datafeeds?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
				return;
			CancelAll();
		}

		protected void CancelAll()
		{
			scrapers.ForEach(x => x.Cancel());
			scrapersQueue.Clear();
		}

		private void ccbeBrands_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			if (e.Button.Caption == "Load")
			{
				var scrap = GetCurrentScraper();
				if (scrap is IBrandProvider)
				{
					var sett = GetCurrentConfig();
					SetupProcessor(scrap, sett);
					var dicBrands = (scrap as IBrandProvider).GetBrands();
					if (dicBrands != null)
					{
						ccbeBrands.Properties.Items.Clear();
						sett.Brands.Clear();
						foreach (var br in dicBrands)
						{
							ccbeBrands.Properties.Items.Add(br.Key);
							sett.Brands.Add(new BrandListItem { Name = br.Key, Url = br.Value });
						}
					}
					ccbeBrands.ShowPopup();
				}
				else
					MessagesPanelManager.PrintMessage("This datafeed doesn't support brand loading");
			}
		}

		private void btnApplyThreads_Click(object sender, EventArgs e)
		{
			SetupCurrentProcessor();
		}

		protected void SetupCurrentProcessor()
		{
			SetupProcessor(GetCurrentScraper(), GetCurrentConfig());
		}

		private void btnStartGlobal_Click(object sender, EventArgs e)
		{
			allowGlobalSchedule = true;
		}

		private void btnForceSave_Click(object sender, EventArgs e)
		{
			ForceSave(false);
		}

		protected void ForceSave(bool onlyFtp, string brandName = null)
		{
			var scrap = GetCurrentScraper();
			if (scrap == null)
				return;
			SaveResult(scrap, onlyFtp, brandName);
		}

		private void SaveBrandsToConfig()
		{
			var sett = GetCurrentConfig();
			var selectedBrands = ccbeBrands.Text.Split(new string[] { "" + ccbeBrands.Properties.SeparatorChar + " " }, StringSplitOptions.RemoveEmptyEntries);
			sett.Brands.ForEach(x => x.Checked = selectedBrands.Contains(x.Name));
			sett.BrandsStr = ccbeBrands.Text;
		}

		private void ccbeBrands_EditValueChanged(object sender, EventArgs e)
		{
			SaveBrandsToConfig();
		}

		private void tutorialToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.shoppingcartelite.com/edf");
		}

		private void tsmiLoadDataLocalFeed_Click(object sender, EventArgs e)
		{
			LoadLocalFeed();
		}

		private void tsmiSave_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void tutorialToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.shoppingcartelite.com/edf");
		}

		private void tsmiLoadResults_Click(object sender, EventArgs e)
		{
			LoadResults();
		}

		protected void LoadResults()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "csv files(*.csv)|*.csv";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				var scrap = GetCurrentScraper();
				var frm = new ImportResultsFromCsvForm();

				frm.Profiles = GetCurrentConfig().ExportProfiles;
				frm.FileName = ofd.FileName;
				frm.Scraper = GetCurrentScraper();
				if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					var sett = GetCurrentConfig();
					if (!sett.ExportProfiles.Contains(frm.Profile))
						sett.ExportProfiles.Add(frm.Profile);
					if (EditExportProfiles(frm.Profile))
					{
						var sri = new ScrapResultsImporter();
						sri.ImportResults(scrap, frm.FileName, frm.Profile);
						scrap.OnResultsLoaded();
						RefreshBindings();
					}
				}
			}
		}

		private void gridView2_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
		{
			return;
		}

		public void LoadEDF(string edfFileName, string libFileName)
		{
			if (InvokeRequired)
				this.Invoke(new Action<string, string>(LoadEDF), edfFileName, libFileName);
			else
				DoLoadProcessorConfig(edfFileName, libFileName);
		}

		private void btnCheckSCEResults_Click(object sender, EventArgs e)
		{
			CheckSCEBatchUpdate();
		}

		protected void CheckSCEBatchUpdate()
		{
			var scrS = GetCurrentConfig();
			WheelsScraper.SCE.SCEManager.CheckResults(scrS);
			sCEFileBindingSource.ResetBindings(false);
		}

		private void tsmiCheckUpdates_Click(object sender, EventArgs e)
		{
			Updater.CheckForUpdates(false);
		}

		private void gridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
		{

		}

		private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
		{
			if (e.RowHandle == gridView2.FocusedRowHandle) return;

			var item = gridView2.GetRow(e.RowHandle);
			var ci = item as IColoredItem;
			if (ci != null && !ci.Color.IsEmpty)
			{
				e.Appearance.BackColor = ci.Color;
			}
		}

		private void RefreshSkins()
		{
			defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2013";
			//var skinName = AppSettings.Default.SkinName;
			//bsiSkins.ItemLinks.Clear();

			//var newBarCheckItem = new BarCheckItem(barManager1);
			//newBarCheckItem.Caption = "Reset to default";
			//newBarCheckItem.Tag = "";
			////newBarCheckItem.Checked = (string)newBarCheckItem.Tag == skinName;
			//newBarCheckItem.ItemClick += new ItemClickEventHandler(newBarCheckItem_ItemClick);
			//bsiSkins.AddItem(newBarCheckItem);

			//var skins = DevExpress.Skins.SkinManager.Default.Skins;
			////DevExpress.Skins.SkinManager.Default.RegisterAssembly(Assembly.LoadFile("))

			//for (int i = 0; i < skins.Count; i++)
			//{
			//	newBarCheckItem = new BarCheckItem(barManager1);
			//	newBarCheckItem.Caption = skins[i].SkinName;
			//	newBarCheckItem.Tag = skins[i].SkinName;
			//	newBarCheckItem.Checked = (string)newBarCheckItem.Tag == skinName;
			//	newBarCheckItem.ItemClick += new ItemClickEventHandler(newBarCheckItem_ItemClick);
			//	bsiSkins.AddItem(newBarCheckItem);
			//}
			//defaultLookAndFeel1.LookAndFeel.SkinName = skinName;
		}


		void newBarCheckItem_ItemClick(object sender, ItemClickEventArgs e)
		{
			var skinName = (string)e.Item.Tag;
			AppSettings.Default.SkinName = skinName;
			RefreshSkins();
		}

		protected void ShowTutor()
		{
			System.Diagnostics.Process.Start("http://www.easydatafeed.com/edf");
		}

		private void biTutor_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTutor();
		}

		private void navBarControl1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
		{

		}

		private void navBarControl1_SelectedLinkChanged(object sender, DevExpress.XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs e)
		{
			LinkClicked(e.Link.Caption, e.Group.Caption);
		}

		protected void LinkClicked(string linkText, string groupText)
		{
			if (groupText == "Feeds")
			{
				ShowTabs("Data,Log");
			}
			if (groupText == "Settings")
			{
				ShowTabs(linkText);
			}
		}

		protected void ShowTabs(string tabsToShow)
		{
			bsScrapProps.EndEdit();
			bool showHeader = tabsToShow.Contains(",");
			showHeader = true;
			tcgMain.ShowTabHeader = showHeader ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
			var tabNames = tabsToShow.Split(',');
			foreach (DevExpress.XtraLayout.LayoutControlGroup tab in tcgMain.TabPages)
			{
				if (tabNames.Contains(tab.Text))
					tab.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
				else
					tab.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			}
			if (showHeader)
				tcgMain.SelectedTabPageIndex = 0;
		}

		private void navBarControl1_GroupExpanded(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
		{
			LinkClicked(e.Group.SelectedLink.Item.Caption, e.Group.Caption);
		}

		void InitSkinGallery()
		{
			DevExpress.XtraBars.Helpers.SkinHelper.InitSkinGallery(rgbiSkins, true);
		}

		private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Local settings");
		}

		private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("FTP settings");
		}

		private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Images");
		}

		private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Merge data");
		}

		private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("API,SCE batch update results");
		}

		private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
		{
			RunAllModules();
		}

		private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
		{
			StartCurrentProcessor();
		}

		private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
		{
			PauseCurrentScraper();
		}

		private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
		{
			CancelCurrentProcessor();
		}

		private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
		{
			CancelAll();
		}

		private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Data,Log");
			tcgMain.SelectedTabPage = lcgData;
			ribbonControl1.SelectedPage = ribbonPage1;
		}

		private void biOpenFromWeb_ItemClick(object sender, ItemClickEventArgs e)
		{
			LoadEDFFromWeb();
		}

		private void biOpenLocal_ItemClick(object sender, ItemClickEventArgs e)
		{
			LoadLocalFeed();
		}

		private void biSave_ItemClick(object sender, ItemClickEventArgs e)
		{
			SaveSettings();
		}

		private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
		{
			ForceSave(false);
		}

		private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
		{
			SaveFile();
		}

		private void bbiForceUpload_ItemClick(object sender, ItemClickEventArgs e)
		{
			ForceFTPUpload();
		}

		private void barButtonItem5_ItemClick_1(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Merge data");
		}

		private void barButtonItem6_ItemClick_1(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Schedule");
		}

		private void barButtonItem4_ItemClick_1(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Special settings");
		}

		private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Limits");
		}

		private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Speed");
		}

		private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Proxy");
		}

		private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Login");
		}

		private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Global schedule");
		}

		private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
		{
			//EditExportProfiles();
			ShowTabs("Profiles");
		}

		protected void GetNewDatafeed()
		{
			System.Diagnostics.Process.Start("https://shoppingcartelite.wufoo.com/forms/w7w0s1/");
		}

		private void backstageViewButtonItem2_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
		{
			GetNewDatafeed();
		}

		void SaveMRU()
		{

		}

		private void backstageViewButtonItem3_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
		{
			ExitApp();
		}

		private void backstageViewButtonItem4_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
		{
			ShowTutor();
		}

		private void backstageViewButtonItem1_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
		{
			SaveSettings();
		}

		private void gridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
		{
			var r = e.Row;
			var curFieldInfo = curProfile.Fields.Where(x => x.Header == e.Column.Caption).FirstOrDefault();
			var fieldVal = e.Value;

			if (curFieldInfo == null)
			{
				var fieldName = e.Column.FieldName;
				if (fieldName != null && fieldName.StartsWith("__"))
					fieldName = fieldName.Substring(2);
				curFieldInfo = curProfile.Fields.Where(x => x.FieldName == fieldName).FirstOrDefault();
			}

			if (curFieldInfo == null)
				return;

			if (e.IsGetData)
			{
				//if (r is IExtWareInfoProvider)
				//	fieldVal = (r as IExtWareInfoProvider).GetFieldValue(e.Column.FieldName);
				//fieldVal = Scraper.Lib.Main.PropHelper.GetPropValue(r, e.Column.FieldName);
				fieldVal = ResultsVisualizer.GetPropValueWithRulesApplied(r, e.Column.FieldName, curSettings, curFieldInfo);
				if (curFieldInfo != null && curFieldInfo.ProfileRule != null)
					fieldVal = Scraper.Lib.Main.PropHelper.ApplyRule(fieldVal, curFieldInfo.ProfileRule);
				if (fieldVal != null)
					e.Value = fieldVal.ToString();
			}
			if (e.IsSetData)
			{
				var curVal = Scraper.Lib.Main.PropHelper.GetPropValue(r, curFieldInfo.FieldName);
				if (curVal != e.Value)
				{
					var edfMgr = EDFManager.GetEDFManager(GetCurrentScraper());
					var undo = edfMgr.UndoMgr.CreateUndo();
					undo.Name = "Manual edit";
					undo.AddItem(r, curFieldInfo.FieldName, curVal, e.Value);
					Scraper.Lib.Main.PropHelper.SetPropValue(r, curFieldInfo.FieldName, e.Value);
					RefreshControlsEnabled();
				}
			}
		}

		private void gridView2_ShowingEditor(object sender, CancelEventArgs e)
		{
			var fieldName = gridView2.FocusedColumn.FieldName ?? "";
			if (fieldName.StartsWith("__"))
				fieldName = fieldName.Substring(2);
			if (curSettings.ExportRules.Any(x => x.FieldName == fieldName && x.Enabled))
				e.Cancel = true;
			if (curSettings.BulletsMoveInfos.Any(x => x.DstField == fieldName && x.Enabled))
				e.Cancel = true;
		}

		private void barButtonItem15_ItemClick_1(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Bullets");
		}

		private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
		{
            ShowTabs("Rules");
		}

		private void lcgProfiles_Hidden(object sender, EventArgs e)
		{
			ribbonControl1.UnMergeRibbon();
		}

		private void lcgProfiles_Shown(object sender, EventArgs e)
		{
			ribbonControl1.MergeRibbon(exportFieldsSelector1.Ribbon);
		}

		private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
		{
            ShowFindAndReplace();
		}


		FindAndReplaceForm frmReplace;
		protected void ShowFindAndReplace()
		{
			if (frmReplace == null || frmReplace.IsDisposed)
				frmReplace = new FindAndReplaceForm();
			frmReplace.Columns = gridView2.Columns.Cast<DevExpress.XtraGrid.Columns.GridColumn>().Select(x => x.Caption).ToList();
			frmReplace.ReplaceData += frm_ReplaceData;
			frmReplace.Show();
		}

		void frm_ReplaceData(object sender, DataReplaceEvent e)
		{
			var data = (IEnumerable)GetCurrentScraper().WareInfoList;

			DataReplacer.FindAndReplace(data, e, curProfile, gridView2.Columns, edfMgr);

			RefreshControlsEnabled();
			RefreshData();
		}

		protected void RefreshData()
		{
			if (this.InvokeRequired)
				this.Invoke(new Action(RefreshData));
			else
			{
				gridView2.RefreshData();
			}
		}

		private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
		{
			RefreshData();
		}

		private void bbLoadLocal_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
		{
			LoadLocalFeed();
		}

		private void bbLoadWeb_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
		{
            LoadEDFFromWeb();
		}

		private void barButtonItem2_ItemClick_1(object sender, ItemClickEventArgs e)
		{
			ClearLog();
		}

		private void barButtonItem3_ItemClick_1(object sender, ItemClickEventArgs e)
		{
			ClearData();
		}

		private void barButtonItem24_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Combine");
		}

		private void barButtonItem25_ItemClick(object sender, ItemClickEventArgs e)
		{
			ShowTabs("Data,Log");
			tcgMain.SelectedTabPage = lcgLog;
		}

		protected void UndoLastEdit()
		{
			edfMgr.UndoMgr.UndoLastStep();
			RefreshControlsEnabled();
			RefreshData();
		}

		protected void RedoLastEdit()
		{
			edfMgr.UndoMgr.RedoLastStep();
			RefreshControlsEnabled();
			RefreshData();
		}

		private void bbiUndo_ItemClick(object sender, ItemClickEventArgs e)
		{
			UndoLastEdit();
		}

		private void bbiRedo_ItemClick(object sender, ItemClickEventArgs e)
		{
			RedoLastEdit();
		}

		private void lcgRules_Shown(object sender, EventArgs e)
		{
            ribbonControl1.MergeRibbon(ucExportRules1.Ribbon);
		}

		private void lcgRules_Hidden(object sender, EventArgs e)
		{
			ribbonControl1.UnMergeRibbon();
		}

		private void bsvbUpdate_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
		{
			Updater.CheckForUpdates(false);
		}
	}
}