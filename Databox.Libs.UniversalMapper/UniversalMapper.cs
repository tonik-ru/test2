using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Net;
using Scraper.Shared;
using System.Web;
using Databox.Libs.UniversalMapper;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace WheelsScraper
{
	public class UniversalMapper : BaseScraper, IFieldInfoProvider
	{
		protected List<string> FieldNames { get; private set; }
		public UniversalMapper()
		{
			Name = "UniversalMapper";
			PageRetriever.Referer = Url;
			WareInfoList = new List<ExtWareInfo>();
			SpecialSettings = new ExtSettings();
			Wares.Clear();
			FieldNames = new List<string>();
		}

		private ExtSettings extSett
		{
			get
			{
				return (ExtSettings)Settings.SpecialSettings;
			}
		}

		public override WareInfo WareInfoType { get { return new ExtWareInfo(); } }

		protected override bool Login()
		{
			return true;
		}


		public override Type[] GetTypesForXmlSerialization()
		{
			return new Type[] { typeof(ExtSettings) };
		}

		public override System.Windows.Forms.Control SettingsTab
		{
			get
			{
				var frm = new ucExtSettings();
				frm.Sett = Settings;
				return frm;
			}
		}

		protected override void RealStartProcess()
		{
			//if (!string.IsNullOrEmpty(extSett.FilePath))
			//	Url = extSett.FilePath;
			if (string.IsNullOrEmpty(extSett.FilePath))
				throw new Exception("Url is not set");
			ProcessQueueItem pqi = new ProcessQueueItem { URL = extSett.FilePath, ItemType = 1 };
			lstProcessQueue.Add(pqi);

			StartOrPushPropertiesThread();
		}

		protected void AddWareInfo(ExtWareInfo wi)
		{
			lock (this)
			{
				Wares.Add(wi);
				((IList)WareInfoList).Add(wi);
			}
		}

		protected void RemoveWareInfo(ExtWareInfo wi)
		{
			lock (this)
			{
				Wares.Remove(wi);
				((IList)WareInfoList).Remove(wi);
			}
		}

		protected Stream GetFile(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				return null;
			Stream stream = null;
			string tmpPath = filePath;
			if (filePath.StartsWith("http"))
			{
				tmpPath = Path.GetTempFileName();
				PageRetriever.SaveFromServer(filePath, tmpPath);
			}
			stream = File.Open(tmpPath, FileMode.Open, FileAccess.Read);
			return stream;
		}

		protected void ProcessFile(ProcessQueueItem pqi)
		{
			try
			{
				string curUrl = pqi.URL;
				using (var sr = new StreamReader(GetFile(curUrl)))
				{
					CsvReader csv = new CsvReader(sr, true, ',');
					var headers = csv.GetFieldHeaders();
					FieldNames.Clear();
					FieldNames.AddRange(headers);

					int fieldCount = csv.FieldCount;
					while (csv.ReadNextRecord())
					{
						var wareInfo = new ExtWareInfo();
						foreach (var fieldName in FieldNames)
						{
							var v1 = csv[fieldName] ?? "";
							var v2 = v1.ToString();
							wareInfo[fieldName] = v2;
						}
						Wares.Add((WareInfo)wareInfo);
						((IList)WareInfoList).Add(wareInfo);
					}
				}
				OnItemLoaded(null);
				pqi.Processed = true;
			}
			catch (Exception err)
			{
				Log.Error(err);
				MessagePrinter.PrintMessage(err.Message, ImportanceLevel.High);
			}
		}

		protected override Action<ProcessQueueItem> GetItemProcessor(ProcessQueueItem item)
		{
			Action<ProcessQueueItem> act = null;
			if (item.ItemType == 1)
				act = new Action<ProcessQueueItem>(ProcessFile);
			//		else if (item.ItemType == 2)
			//		act = new Action<ProcessQueueItem>(ProcessWareListPage);
			//else if (item.ItemType == 3)
			//act = new Action<ProcessQueueItem>(ProcessWareItemPage);
			//else
			//	act = new Action<ProcessQueueItem>(ProcessWareItemPage);
			//else act = null;
			return act;
		}

		private string lastUrl;

		public List<string> GetFieldNames()
		{
			try
			{
				if (string.IsNullOrEmpty(extSett.FilePath))
				{
					FieldNames.Clear();
					return FieldNames;
				}

				if (FieldNames.Count > 0 && extSett.FilePath == lastUrl)
					return FieldNames;
				using (var sr = new StreamReader(GetFile(extSett.FilePath)))
				{
					CsvReader csv = new CsvReader(sr, true, ',');
					var headers = csv.GetFieldHeaders();
					FieldNames.Clear();
					FieldNames.AddRange(headers);
					lastUrl = extSett.FilePath;
					return FieldNames;
				}
			}
			catch (Exception err)
			{
				MessagePrinter.PrintMessage(err.Message, ImportanceLevel.High);
				return FieldNames;
			}
		}

		public string GetFieldValue(string fieldName)
		{
			throw new NotImplementedException();
		}
	}
}