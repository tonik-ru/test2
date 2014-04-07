using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using HtmlAgilityPack;
using Scraper.Lib.Main;
using Scraper.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

namespace WheelsScraper
{
	public class BaseScraper : ISimpleScraper/*, IFieldInfoProvider*/
	{
		int[] itemTypesAllowedOverLimit = new int[] { 1200, 1250, -100 };

		protected Timer imagesTimer;
		const int ImageItemType = 1200;
		const int UploadImageItemType = 1250;
		const int StartGetImagesItemType = -100;
		public virtual System.Windows.Forms.Control SettingsTab { get; protected set; }

		public bool IsRunning { get; protected set; }
		public bool RequireSCEAPIKey { get; set; }
		public int MaxRetries { get; set; }

		public int MaxThreads { get; set; }
		public bool Googlebot { get; set; }
		public int MaxRecords { get; set; }
		public bool UseLogin { get; set; }
		protected int BrandItemType { get; set; }
		public List<string> ImageFields { get; set; }

		public List<string> BrandsToLoad { get; protected set; }

		public ScraperSettings Settings { get; set; }

		public virtual object WareInfoList { get; protected set; }

		protected bool itemsLoades;

		public void Cancel()
		{
			cancel = true;
			MessagePrinter.PrintMessage("Cancel");
			StopImagesTimer();
		}

		protected bool cancel;

		protected SortedSet<string> knownImages { get; set; }

		public virtual WareInfo WareInfoType { get { return new WareInfo(); } }

		public BaseScraper()
		{
			System.Net.ServicePointManager.DefaultConnectionLimit = 100;
			LoginInfos = new List<LoginInfo>();
			BrandsToLoad = new List<string>();
			lstProcessQueue = new List<ProcessQueueItem>();
			Wares = new List<WareInfo>();
			ImageFields = new List<string>();

			PageRetriever = new PageRetriever();
			//WareInfoList = new List<WareInfo>();
			WareInfoList = Wares;
			PageRetriever.Referer = Url;
			BrandItemType = 1000;
			MaxRetries = 3;
		}

		protected HtmlDocument CreateDoc(string html)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(html);
			return doc;
		}

		protected void ImagesTimerCallback(object state)
		{
			StartImageDownloads();
		}

		public string Name { get; protected set; }

		public event EventHandler Autosave;

		protected virtual void OnAutosave()
		{
			if (Autosave != null)
				Autosave(this, null);
		}

		public string Url { get; set; }


		public List<LoginInfo> LoginInfos { get; set; }

		protected LoginInfo GetLoginInfo()
		{
			var login = LoginInfos.Where(x => x.IsActive).FirstOrDefault();
			return login;
		}

		protected virtual bool Login()
		{
			return true;
		}

		public event EventHandler ItemLoaded;
		public event EventHandler BrandLoaded;

		public PageRetriever PageRetriever { get; set; }

		public List<WareInfo> Wares { get; set; }

		protected List<ProcessQueueItem> lstProcessQueue;


		protected void OnItemLoaded(WareInfo wi)
		{
			if (ItemLoaded != null)
				ItemLoaded(wi, null);
		}

		protected void OnBrandLoaded(string name)
		{
			if (BrandLoaded != null)
				BrandLoaded(name, null);
		}

		protected virtual void RealStartProcess()
		{
			ProcessQueueItem pqi = new ProcessQueueItem { URL = Url, ItemType = 1 };
			lstProcessQueue.Add(pqi);
			StartOrPushPropertiesThread();
		}

		protected void OnProcessComplete()
		{
			IsRunning = false;
			MessagePrinter.PrintMessage("Complete");
			CompleteSuccessfully = Wares.Count > 0 && !cancel;
			StopImagesTimer();
		}

		protected virtual void OnBeforeRealStartProcess()
		{
		}

		public bool HasActiveLogin
		{
			get
			{
				return GetLoginInfo() != null;
			}
		}

		protected void AddImageDownloadTask()
		{
			lock (this)
				lstProcessQueue.Add(new ProcessQueueItem { ItemType = StartGetImagesItemType });
		}

		public virtual void ProcessAllData()
		{
			lock (this)
			{
				if (IsRunning)
				{
					MessagePrinter.PrintMessage("Already running");
					OnProcessComplete();
				}
				IsRunning = true;
			}

			//PageRetriever = new PageRetriever();
			knownImages = new SortedSet<string>();
			PageRetriever.Googlebot = Googlebot;
			((System.Collections.IList)WareInfoList).Clear();
			Wares.Clear();
			OnItemLoaded(null);

			lstProcessQueue.Clear();
			//LoadConfig();

			//IsRunning = true;
			cancel = false;
			lstPropertiesWH = new List<IAsyncResult>();
			inProcess = new List<ProcessQueueItem>();
			try
			{
				if (!HasActiveLogin && UseLogin)
					throw new Exception("No active login found");
				HtmlNode.ElementsFlags.Remove("form");
				PageRetriever.Proxies = Proxies;
				OnBeforeRealStartProcess();

				bool loginDone = false;
				if (UseLogin)
				{
					for (int i = 0; i < 3; i++)
					{
						if (cancel)
							break;
						loginDone = Login();
						if (loginDone)
							break;
						else
						{
							MessagePrinter.PrintMessage("Login error!", ImportanceLevel.High);
						}
					}
				}
				else
					loginDone = true;
				if (loginDone)
				{
					RealStartProcess();
					StartImagesTimer();
					lock (this)
						AddImageDownloadTask();
				}
			}
			catch (NoLoginException err)
			{
				MessagePrinter.PrintMessage(err.Message, ImportanceLevel.Critical);
			}
			catch (Exception err)
			{
				MessagePrinter.PrintMessage(err.Message, ImportanceLevel.Critical);
			}
			if (propThread != null)
				propThread.Join();
			var p1 = lstPropertiesWH.Select(x => x.AsyncWaitHandle).ToArray();
			if (p1.Length > 0)
				WaitHandle.WaitAll(p1);

			OnProcessComplete();
		}

		bool propertiesLoopIsRunning;
		protected Thread propThread;

		protected void StartOrPushPropertiesThread()
		{
			if (cancel)
				return;
			lock (this)
			{
				if (propertiesLoopIsRunning)
					return;
				ThreadStart threadDelegate = new ThreadStart(StartGetItemProperties);
				propThread = new Thread(threadDelegate);
				propThread.Start();
			}
		}

		protected List<IAsyncResult> lstPropertiesWH;
		protected List<ProcessQueueItem> inProcess;

		protected string[] imageExtensions = { ".png", ".jpeg", ".gif" };

		protected virtual void ProcessImage(ProcessQueueItem pqi)
		{
			MessagePrinter.PrintMessage("Processing " + pqi.URL);

			var url = pqi.URL;

			WareInfo wi = null;
			ImageProcessInfo ipi = null;
			if (pqi.Item is WareInfo)
				wi = (WareInfo)pqi.Item;
			else if (pqi.Item is ImageProcessInfo)
			{
				ipi = (ImageProcessInfo)pqi.Item;
				wi = ipi.WareInfo;
			}

			//var localFile = Path.Combine(LocalPath, HttpUtility.UrlEncode(Path.GetFileName(url)));
			var localFile = GetLocalImageFilename(url);

			bool fileExists = File.Exists(localFile);
			bool imageIsKnown = false;
			lock (knownImages)
				imageIsKnown = knownImages.Contains(url);

			var updatedUrl = GetFixedImageUrl(url, localFile);
			bool imageDownloaded = false;

			if ((!Settings.SkipExistingImages || (Settings.SkipExistingImages && !fileExists)) && !imageIsKnown)
			{
				if (url.StartsWith("http"))
				{
					MessagePrinter.PrintMessage("Downloading image " + url);
					try
					{
						PageRetriever.SaveFromServer(url, localFile, true);
						imageDownloaded = true;
					}
					catch (Exception err)
					{
						//MessagePrinter.PrintMessage(err.Message);
						if (pqi.NumberOfAttempts < 3)
							throw;
					}
				}
				else
				{
					try
					{
						if (File.Exists(url))
							File.Copy(url, localFile);
						imageDownloaded = true;
					}
					catch (Exception err)
					{
					}
				}
			}

			lock (knownImages)
				knownImages.Add(url);

			//var fileName = Path.GetFileName(url);
			//if (Settings.ConvertImages)
			//	fileName = System.IO.Path.GetFileNameWithoutExtension(fileName) + ".jpg";

			//if (!string.IsNullOrEmpty(Settings.ImagesPublicPath))
			//	updatedUrl =  new Uri(new Uri(Settings.ImagesPublicPath), fileName).ToString();			
			//updatedUrl = GetFixedImageUrl(url);
			fileExists = File.Exists(localFile);
			if (fileExists)
			{
				bool isPdf = url.ToLower().EndsWith(".pdf");
				if (isPdf && Settings.ConvertPDFToJpg)
				{
					bool onlyFirstPage = ipi.MergePdf;//for image search column
					var convertedPages = ConvertPdfToJpg(localFile, onlyFirstPage);
					localFile = convertedPages;
					var pages = convertedPages.Split(',');
					updatedUrl = "";
					foreach (var p in pages)
					{
						var updatedPageUrl = GetFixedImageUrl(url, p);
						if (!string.IsNullOrEmpty(updatedUrl))
							updatedUrl += ",";
						updatedUrl += updatedPageUrl;
					}
				}
				else if ((Settings.ConvertImages && !url.ToLower().EndsWith(".jpg")))
				{
					var origFile = localFile;
					var fileName = System.IO.Path.GetFileNameWithoutExtension(localFile) + ".jpg";
					localFile = Path.Combine(LocalPath, fileName);
					if (imageDownloaded)
						ConvertImage(origFile, localFile);
					updatedUrl = GetFixedImageUrl(url, localFile);
				}
				if (wi != null)//wi==null for image search columns
				{
					if (wi.LocalImageFile != null)
						wi.LocalImageFile += ",";
					wi.LocalImageFile += localFile;
				}
			}

			if (ipi != null)
			{
				lock (ipi.WareInfo)
				{
					var origFieldValue = PropHelper.GetPropValue(wi, ipi.FieldName).ToString();
					var updatedField = origFieldValue.Replace(ipi.Url, updatedUrl);
					PropHelper.SetPropValue(wi, ipi.FieldName, updatedField);
				}
			}

			if (Settings.UploadImagesToFtp && fileExists)
			{
				var filesToUpload = localFile.Split(',');
				lock (this)
				{
					foreach (var lf in filesToUpload)
					{
						if (Settings.SkipUploadOfExistingImages && !imageDownloaded)
							continue;
						if (!lstProcessQueue.Any(x => x.URL == lf && x.ItemType == UploadImageItemType))
							lstProcessQueue.Add(new ProcessQueueItem { ItemType = UploadImageItemType, URL = lf, Item = new FileUploadInfo { LocalPath = lf, Overwrite = !Settings.DontOverwriteImagesOnFtp } });
					}
				}
			}

			pqi.Processed = true;
		}

		protected virtual Action<ProcessQueueItem> GetImageItemProcessor(ProcessQueueItem item)
		{
			return ProcessImage;
		}

		private Action<ProcessQueueItem> IntGetItemProcessor(ProcessQueueItem item)
		{
			var act = GetItemProcessor(item);
			if (act == null && item.ItemType == StartGetImagesItemType)
				act = ProcessImagesDownload;
			if (act == null && item.ItemType == ImageItemType)
				act = GetImageItemProcessor(item);
			if (act == null && item.ItemType == UploadImageItemType)
				act = GetImageUploadProcessor(item);
			return act;
		}

		private Action<ProcessQueueItem> GetImageUploadProcessor(ProcessQueueItem item)
		{
			return ProcessUploadImagesToFtp;
		}

		protected virtual Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
		{
			return null;
		}

		private Dictionary<string, PropertyInfo> propsInType;

		public object GetPropValue(object src, string propName)
		{
			return PropHelper.GetPropValue(src, propName);
			//if (propsInType == null)
			//	propsInType = src.GetType().GetProperties().ToDictionary(x => x.Name);

			//if (string.IsNullOrEmpty(propName))
			//	return null;
			//if (src is IExtWareInfoProvider)
			//{
			//	return (src as IExtWareInfoProvider).GetFieldValue(propName);
			//}
			//try
			//{
			//	if (!propsInType.ContainsKey(propName))
			//		return null;
			//	else
			//		return propsInType[propName].GetValue(src, null);
			//}
			//catch (Exception err)
			//{
			//	return "";
			//}
		}

		public void SetPropValue(object src, string propName, object value)
		{
			PropHelper.SetPropValue(src, propName, value);
			//if (propsInType == null)
			//	propsInType = src.GetType().GetProperties().ToDictionary(x => x.Name);

			//if (string.IsNullOrEmpty(propName))
			//	return;
			//if (src is IExtWareInfoProvider)
			//{
			//	(src as IExtWareInfoProvider).SetFieldValue(propName, value);
			//	return;
			//}

			//try
			//{
			//	if (!propsInType.ContainsKey(propName))
			//		return;
			//	else
			//		propsInType[propName].SetValue(src, value, null);
			//}
			//catch (Exception err)
			//{
			//
			//}
		}

		protected virtual void ProcessImagesDownload(ProcessQueueItem pqi)
		{
			StartImageDownloads();
			pqi.Processed = true;
			//lstUrls = lstUrls.Distinct().ToList();
			//foreach (var url in lstUrls)
			//{
			//	lock (this)
			//	{
			//		lstProcessQueue.Add(new ProcessQueueItem { URL = url, ItemType = ImageItemType });
			//	}
			//}
			StartOrPushPropertiesThread();
		}

		public void SaveConfig()
		{
			ScrapRuntimeConfig scrCfg = new ScrapRuntimeConfig();
			scrCfg.Wares = Wares;
			scrCfg.LstProcessQueue = lstProcessQueue;
			scrCfg.BrandsToLoad = BrandsToLoad;

			var curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			var configFile = Path.Combine(curDir, Name + ".config");
			using (var fs = System.IO.File.Create(configFile))
			{
				var t = typeof(WareInfo);
				if (Wares.Count > 0)
					t = Wares[0].GetType();
				XmlSerializer xs = new XmlSerializer(typeof(ScrapRuntimeConfig), new Type[] { t });
				xs.Serialize(fs, scrCfg);
			}
		}

		public void LoadConfig()
		{
			var scrCfg = ReadConfig();
			if (scrCfg.Wares != null)
				this.Wares = scrCfg.Wares;
			if (scrCfg.LstProcessQueue != null)
				lstProcessQueue = scrCfg.LstProcessQueue;
			if (scrCfg.BrandsToLoad != null)
				BrandsToLoad = scrCfg.BrandsToLoad;

			lstProcessQueue.Where(x => !x.DataFound).ToList().ForEach(x => { x.Processed = false; x.NumberOfAttempts = 0; });
			if (WareInfoList is IList)
				Wares.ForEach(x => ((IList)WareInfoList).Add(x));
		}

		protected ScrapRuntimeConfig ReadConfig()
		{
			var curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			var configFile = Path.Combine(curDir, Name + ".config");
			if (File.Exists(configFile))
			{
				XmlSerializer xs = new XmlSerializer(typeof(ScrapRuntimeConfig));
				using (var sr = System.IO.File.OpenText(configFile))
				{
					var conf = (ScrapRuntimeConfig)xs.Deserialize(sr);
					return conf;
				}
			}
			return new ScrapRuntimeConfig();
		}

		private DateTime LastCheck;

		protected void ValidateRunLimit()
		{
			bool shouldFinish = false;

			if (LastCheck == DateTime.MinValue)
				LastCheck = DateTime.Now;

			if (DateTime.Now < Settings.StartDate)
				shouldFinish = true;
			if (LastCheck > DateTime.Now)
				shouldFinish = true;
			else
				LastCheck = DateTime.Now;

			if (Settings.StartDate + Settings.TimeLeft < DateTime.Now)
				shouldFinish = true;
			//shouldFinish = true;
			if (shouldFinish)
			{
				MessagePrinter.PrintMessage("Work limit reached.", ImportanceLevel.High);
				Pause();
				//lock (this)
				//	lstProcessQueue.ForEach(x => x.Processed = true);
			}
		}

		private void StartGetItemProperties()
		{
			lock (this)
			{
				if (propertiesLoopIsRunning)
					return;
				propertiesLoopIsRunning = true;
			}
			MessagePrinter.PrintMessage("Starting new loop");
			do
			{
				if (cancel)
					break;
				if (IsPaused)
				{
					Thread.Sleep(5000);
					continue;
				}
				//ValidateRunLimit();
				if (IsPaused)
					break;
				WaitHandle[] p1 = new WaitHandle[0];
				DateTime dt1 = new DateTime(2013, 2, 11);
				bool threadsStarted = false;
				lock (this)
				{
					propertiesLoopIsRunning = true;
					int recordLimit = (MaxRecords == 0) ? int.MaxValue : MaxRecords - Wares.Count;//.Where(x => x.Processed).Count();
					recordLimit = Math.Max(0, recordLimit);

					var itemsToStart = lstProcessQueue.Where(x => !x.Processed && x.NumberOfAttempts < MaxRetries && !inProcess.Contains(x));//.OrderByDescending(x => x.ItemType).ThenBy(x => x.DateAdd);
					if (recordLimit == 0)
					{
						itemsToStart = itemsToStart.Where(x => itemTypesAllowedOverLimit.Contains(x.ItemType));
						recordLimit = itemsToStart.Count();
					}
					var limit2 = recordLimit;
					
					itemsToStart = itemsToStart.OrderByDescending(x => x.ItemType).ThenBy(x => x.DateAdd);

					if (BrandItemType != 1000)
					{
						var numOfWare = itemsToStart.Where(x => !x.Processed && x.ItemType > BrandItemType && !inProcess.Contains(x)).Count();
						//var numOfWare = lstProcessQueue.Where(x => !x.Processed && x.ItemType > BrandItemType && !inProcess.Contains(x)).Count();
						int numOfImages = itemsToStart.Where(x => !x.Processed && x.ItemType >= ImageItemType && !inProcess.Contains(x)).Count();

						if (numOfWare > 0)
						{
							limit2 = Math.Min(recordLimit, numOfWare);
							if (limit2 == 0)
							{
								//limit2 = numOfImages;
								limit2 = itemsToStart.Count();
								recordLimit = limit2;
							}
						}
						else
						{
							var inproc1 = inProcess.Where(x => x.ItemType >= BrandItemType).Count();
							if (inproc1 == 0)
								limit2 = 1;
							else
								limit2 = 0;
						}
					}

					//StartImageDownloads();

					var numThreads = Math.Min(Math.Min(MaxThreads - lstPropertiesWH.Count, recordLimit), limit2);
					var r1 = itemsToStart.Take(numThreads).ToList();
					//var r1 = lstProcessQueue.Where(x => !x.Processed && x.NumberOfAttempts < MaxRetries && !inProcess.Contains(x)).OrderByDescending(x => x.ItemType).ThenBy(x => x.DateAdd).Take(numThreads).ToList();

					threadsStarted = r1.Count > 0;

					if (r1.Count > 0)
						MessagePrinter.PrintMessage("Starting " + r1.Count + " threads");

					if (r1.Count == 0 && lstPropertiesWH.Count == 0)
						break;
					if (recordLimit == 0 && limit2 == 0)
						break;

					foreach (var item in r1)
					{
						if (cancel)
							break;
						var act = IntGetItemProcessor(item);
						//if (act == null && item.ItemType == StartGetImagesItemType)
						//	act = ProcessImagesDownload;
						//if (act == null && item.ItemType == ImageItemType)
						//	act = GetImageItemProcessor(item);
						//if (act == null && item.ItemType == UploadImageItemType)
						//	act = GetImageUploadProcessor(item);
						itemsLoades = true;


						inProcess.Add(item);
						item.NumberOfAttempts++;

						Action<Action<ProcessQueueItem>, ProcessQueueItem> act2 = ExecuteActionWithWait;
						if (item.ItemType == ImageItemType)
							act2 = ExecuteActionWithoutWait;

						var ar = act2.BeginInvoke(act, item, ItemLoadedCallback, item);
						lstPropertiesWH.Add(ar);
					}
					p1 = lstPropertiesWH.Select(x => x.AsyncWaitHandle).ToArray();
				}
				if (!threadsStarted)
					Thread.Sleep(300);
				if (p1 != null && p1.Length > 0)
					WaitHandle.WaitAny(p1);
			} while (true);

			lock (this)
				propertiesLoopIsRunning = false;
		}

		protected void ExecuteActionWithoutWait(Action<ProcessQueueItem> act, ProcessQueueItem pqi)
		{
			if (act == null)
			{
				MessagePrinter.PrintMessage("Unknown action. ItemType = " + pqi.ItemType, ImportanceLevel.High);
				pqi.Processed = true;
				return;
			}
			try
			{
				act(pqi);
			}
			catch (Exception err)
			{
				Log.Error(err);
				MessagePrinter.PrintMessage(err.Message, ImportanceLevel.High);
			}
		}

		protected void ExecuteActionWithWait(Action<ProcessQueueItem> act, ProcessQueueItem pqi)
		{
			WaitRandomReal();
			ExecuteActionWithoutWait(act, pqi);
		}

		protected double ParseDouble(string str)
		{
			if (str == null)
				return 0;
			str = str.Trim();
			if (str == "")
				return 0;
			var p1 = str.IndexOf(" ");
			if (p1 != -1)
				str = str.Substring(0, p1);
			str = str.Replace("$", "").Replace(":", "").Replace("MSRP", "");
			CultureInfo myCI = new CultureInfo(Thread.CurrentThread.CurrentCulture.LCID);
			myCI.NumberFormat.NumberDecimalSeparator = ".";
			myCI.NumberFormat.NumberGroupSeparator = ",";

			double val = 0;
			double.TryParse(str, NumberStyles.Number, myCI.NumberFormat, out val);
			return val;
		}

		protected double ParsePrice(string str)
		{
			if (string.IsNullOrEmpty(str))
				return 0;
			var isEur = str.IndexOf("EUR");
			var res = ParseDouble(str.Replace("$", "").Replace("EUR", ""));
			if (isEur != -1)
				res = res / 100;
			return res;
		}

		protected int ParseInt(string str)
		{
			if (str == null)
				return 0;
			str = str.Trim();
			if (str == "")
				return 0;
			CultureInfo myCI = new CultureInfo(Thread.CurrentThread.CurrentCulture.LCID);
			myCI.NumberFormat.NumberDecimalSeparator = ".";
			myCI.NumberFormat.NumberGroupSeparator = ",";

			int val = 0;
			int.TryParse(str, NumberStyles.Number, myCI.NumberFormat, out val);
			return val;
		}

		protected void ItemLoadedCallback(IAsyncResult ar)
		{
			lock (this)
			{
				//LogManager.PrintMessage("Thread complete");
				//ar.AsyncWaitHandle.SafeWaitHandle.
				lstPropertiesWH.Remove(ar);
				inProcess.Remove((ProcessQueueItem)ar.AsyncState);

				//var wi = (ProcessQueueItem)ar.AsyncState;
				var itemType = ((ProcessQueueItem)ar.AsyncState).ItemType;
				if (itemType == ImageItemType && inProcess.Where(x => !x.Processed).Any())
					return;
				if (itemType >= BrandItemType)
				{
					if (inProcess.Where(x => x.ItemType > BrandItemType).Count() + lstProcessQueue.Where(x => !x.Processed && x.ItemType > BrandItemType).Count() == 0)
					{
						var pqi = (ProcessQueueItem)ar.AsyncState;
						string brandName = null;
						if (pqi.Item is WareInfo)
							brandName = ((WareInfo)pqi.Item).Brand;
						else if (pqi.Item is string)
							brandName = (string)pqi.Item;
						else
							brandName = pqi.Name;
						OnBrandLoaded(brandName);
					}
				}
			}
		}


		public List<ProxyInfo> Proxies { get; set; }



		#region ISimpleScraper Members


		public void Login(string login, string password)
		{
			throw new NotImplementedException();
		}

		#endregion

		public log4net.ILog Log { get; set; }


		public IMessagePrinter MessagePrinter { get; set; }

		public int MaxDelay { get; set; }

		Random rnd = new Random();
		protected void WaitRandom()
		{
			return;
			//if (MaxDelay > 0)
			//{
			//	var delay = rnd.Next(MaxDelay * 1000);
			//	Thread.Sleep(delay);
			//}
		}

		protected void WaitRandomReal()
		{
			if (MaxDelay > 0)
			{
				var delay = rnd.Next(MaxDelay * 1000);
				Thread.Sleep(delay);
			}
		}

		public string FtpAddress { get; set; }
		public string FtpLogin { get; set; }
		public string FtpPassword { get; set; }

		public int ScheduleType { get; set; }

		public bool CompleteSuccessfully { get; protected set; }


		public void Pause()
		{
			StopImagesTimer();
			IsPaused = true;
			OnSomeEvent(this, "Pause", null);
		}

		protected void StartImagesTimer()
		{
			StopImagesTimer();
			imagesTimer = new Timer(ImagesTimerCallback, null, 20 * 1000, 60 * 1000);
		}

		protected void StopImagesTimer()
		{
			if (imagesTimer != null)
				imagesTimer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		public void Resume()
		{
			IsPaused = false;
			OnSomeEvent(this, "Resume", null);
			StartImagesTimer();
		}

		public bool IsPaused { get; protected set; }

		public bool ConvertImages { get; set; }
		public bool DownloadImages { get; set; }
		public bool UploadImagesToFtp { get; set; }
		public string ImagesPublicPath { get; set; }
		public string LocalPath { get; set; }


		private static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for (j = 0; j < encoders.Length; ++j)
			{
				if (encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}

		public string ConvertPdfToJpg(string srcFileName, bool onlyFirstPage)
		{
			int desired_x_dpi = 192;
			int desired_y_dpi = 192;

			var _lastInstalledVersion =
				GhostscriptVersionInfo.GetLastInstalledVersion(
						GhostscriptLicense.GPL | GhostscriptLicense.AFPL,
						GhostscriptLicense.GPL);

			var _rasterizer = new GhostscriptRasterizer();

			_rasterizer.Open(srcFileName, _lastInstalledVersion, false);

			string outputFiles = "";
			var dir = Path.GetDirectoryName(srcFileName);
			var fileName = Path.GetFileNameWithoutExtension(srcFileName);
			for (int pageNumber = 1; pageNumber <= _rasterizer.PageCount; pageNumber++)
			{
				var dstFileName = Path.Combine(dir, fileName);
				//var dstFileName = GetLocalImageFilename(srcFileName);
				string pageFilePath = dstFileName + (onlyFirstPage ? "" : "_" + pageNumber.ToString()) + ".jpg";

				var img = _rasterizer.GetPage(desired_x_dpi, desired_y_dpi, pageNumber);
				var myImageCodecInfo = GetEncoderInfo("image/jpeg");
				var myEncoderParameters = new EncoderParameters(2);
				var myEncoder = System.Drawing.Imaging.Encoder.Quality;
				var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
				myEncoderParameters.Param[0] = myEncoderParameter;
				myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionLZW);

				//img.Save(pageFilePath, ImageFormat.Tiff);
				img.Save(pageFilePath, myImageCodecInfo, myEncoderParameters);
				if (!string.IsNullOrEmpty(outputFiles))
					outputFiles += ",";
				outputFiles += pageFilePath;
				if (onlyFirstPage)
					break;
			}
			return outputFiles;
		}

		public static void ConvertImage(string srcFileName, string dstFileName)
		{
			using (Image img = new Bitmap(srcFileName))
			{
				var myImageCodecInfo = GetEncoderInfo("image/jpeg");
				var myEncoderParameters = new EncoderParameters(1);
				var myEncoder = System.Drawing.Imaging.Encoder.Quality;
				var myEncoderParameter = new EncoderParameter(myEncoder, 100L);
				myEncoderParameters.Param[0] = myEncoderParameter;

				var img2 = new Bitmap(img.Width, img.Height);
				var g = Graphics.FromImage(img2);
				g.FillRegion(Brushes.White, new Region(new Rectangle(0, 0, img.Width, img.Height)));
				g.DrawImageUnscaled(img, 0, 0);

				if (System.IO.File.Exists(dstFileName))
					System.IO.File.Delete(dstFileName);
				img2.Save(dstFileName, myImageCodecInfo, myEncoderParameters);
			}
		}

		public virtual Type[] GetTypesForXmlSerialization()
		{
			return new Type[0];
		}

		public object SpecialSettings { get; set; }

		public virtual List<string> GetFieldNames()
		{
			if (propsInType == null)
				propsInType = WareInfoType.GetType().GetProperties().Where(x => x.PropertyType == typeof(string)).ToDictionary(x => x.Name);
			return propsInType.Select(x => x.Key).ToList();
		}

		public virtual void OnPostProcess()
		{

		}

		public virtual void OnFileUploaded(string fileName)
		{

		}

		public virtual void OnResultsLoaded()
		{

		}

		public event Action<object, string, object> SomeEvent;

		public void OnSomeEvent(object sender, string eventName, object data)
		{
			if (SomeEvent != null)
				SomeEvent(sender, eventName, data);
		}

		protected void ProcessUploadImagesToFtp(ProcessQueueItem pqi)
		{
			var sett = Settings;
			var imgLocalFile = pqi.URL;
			if (string.IsNullOrEmpty(imgLocalFile))
				return;

			bool overwrite = true;
			var fui = (FileUploadInfo)pqi.Item;
			if (fui != null)
				overwrite = fui.Overwrite;
			var fileName = System.IO.Path.GetFileName(imgLocalFile);
			FtpHelper.UploadFileToFtp(sett.FtpAddress, sett.FtpUsername, sett.FtpPassword, fileName, imgLocalFile, overwrite);
			pqi.Processed = true;
		}

		protected string GetFixedImageUrl(string url, string localFileName)
		{
			var updatedUrl = url;
			var fileName = Path.GetFileName(localFileName);
			if (Settings.ConvertImages)
				fileName = System.IO.Path.GetFileNameWithoutExtension(fileName) + ".jpg";

			if (!string.IsNullOrEmpty(Settings.ImagesPublicPath))
				updatedUrl = new Uri(new Uri(Settings.ImagesPublicPath), fileName).ToString();

			return updatedUrl;
		}

		protected string GetLocalImageFilename(string url)
		{
			var filePath = url;
			string fileName = "";
			var ext = Path.GetExtension(Path.GetFileName(url));
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			StringBuilder sbHash = new StringBuilder();
			var buf = Encoding.Default.GetBytes(url);

			md5.ComputeHash(buf);
			foreach (byte b in md5.Hash)
			{
				sbHash.Append(b.ToString("X2"));
			}
			fileName = sbHash.ToString();
			var tmp = Path.Combine(LocalPath, fileName);
			if (!string.IsNullOrEmpty(ext))
				tmp += ext;
			return tmp;
		}
		
		public void StartImageDownloads()
		{
			if (!DownloadImages)
				return;

			var lstUrls = new List<string>();
			
			var imageSearchFields = Settings.ExportProfiles.SelectMany(x => x.Fields).Where(x => x.SearchForImage).Select(x => x.FieldName).ToList();

			lock (this)
			{
				foreach (var wi in Wares.Where(x => !x.ImageProcessed))
				{
					foreach (var field in ImageFields)
					{
						var path = GetPropValue(wi, field);
						if (path == null)
							continue;
						var urls = path.ToString().Split(',');

						string updatedUrls = null;
						foreach (var url in urls)
						{
							if (string.IsNullOrEmpty(url))
								continue;
							//var updatedUrl = url;
							//bool skipDownload = lstProcessQueue.Any(x => x.URL == url);
							//bool isPdf = url.ToLower().EndsWith(".pdf");
							//if (!lstProcessQueue.Any(x => x.URL == url))
							{
								var ipi = new ImageProcessInfo { FieldName = field, Url = url, WareInfo = wi };
								lstProcessQueue.Add(new ProcessQueueItem { URL = url, ItemType = ImageItemType, Item = ipi });
							}

							//if (!url.ToLower().EndsWith(".pdf"))
							//	updatedUrl = GetFixedImageUrl(url, url);
							//if (updatedUrls != null)
							//	updatedUrls += ",";
							//updatedUrls += updatedUrl;
						}
						//SetPropValue(wi, field, updatedUrls);
					}

					foreach (var field in imageSearchFields)
					{
						bool imageFound = false;
						var htmlO = GetPropValue(wi, field);
						if (htmlO == null)
							continue;

						var html = htmlO.ToString();
						var doc = new HtmlDocument();
						doc.LoadHtml(html);
						var images = doc.DocumentNode.SelectNodes("//img[@src]");
						if (images != null)
						{
							foreach (var img in images)
							{
								var url = img.AttributeOrNull("src");
								if (!(url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
									continue;
								
								//var fixedUrl = GetFixedImageUrl(url, url);
								//img.Attributes["src"].Value = fixedUrl;
								imageFound = true;

								var ipi = new ImageProcessInfo { FieldName = field, Url = url, WareInfo = wi, MergePdf = true };

								//if (!lstProcessQueue.Any(x => x.URL == url))
								lstProcessQueue.Add(new ProcessQueueItem { URL = url, ItemType = ImageItemType, Item = ipi });
							}
						}
						if (imageFound)
							PropHelper.SetPropValue(wi, field, doc.DocumentNode.OuterHtml);
					}


					wi.ImageProcessed = true;
				}
			}
		}
	}
}