using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using WheelsScraper.Export;

namespace WheelsScraper
{
	public class ScraperSettings
	{
		[XmlIgnore]
		public List<Type> AdditionalTypes { get; set; }

        public object SpecialSettings { get; set; }

		[XmlIgnore]
		public bool IsRunning { get; set; }

		[XmlIgnore]
		public DateTime StartDate { get; set; }
		[XmlIgnore]
		public TimeSpan TimeLeft { get; set; }
		[XmlIgnore]
		public int ThreadsLimit{ get; set; }

		public string Name { get; set; }
		public int MaxThreads { get; set; }
		public int MaxDelay { get; set; }

		public List<LoginInfo> Logins { get; set; }
		public bool EnableSchedule { get; set; }
		public List<ScheduleItem> Schedule { get; set; }
		public List<BrandListItem> Brands { get; set; }
		public string BrandsStr { get; set; }
		public string FtpAddress { get; set; }
		public string FtpUsername { get; set; }
		public string FtpPassword { get; set; }

		public string FieldsToExport { get; set; }
		public string FieldHeaders { get; set; }
		public string LocalPath { get; set; }

		public string DBCLogin { get; set; }
		public string DBCPassword { get; set; }

		public bool OutputBadDataToError { get; set; }
		public bool ConvertPDFToJpg { get; set; }
		public bool SkipExistingImages { get; set; }
		public bool SkipUploadOfExistingImages { get; set; }
		public bool DontOverwriteImagesOnFtp { get; set; }

		//public string Brands { get; set; }
		public ScraperSettings()
		{
			Logins = new List<LoginInfo>();
			Fields = new List<FieldInfo>();
			Schedule = new List<ScheduleItem>();
			Brands = new List<BrandListItem>();
			AdditionalTypes = new List<Type>();
			ExportProfiles = new List<ExportProfile>();
			LocalExportSettings = new List<ExportSettings>();
			FtpExportSettings = new List<ExportSettings>();

			ExportRules = new List<ExportFieldRule>();
			BulletsMoveInfos = new List<BulletsMoveInfo>();
			CategoryInfos = new List<ExportFieldCategoryInfo>();

			SCEDemoSite = "scedesignsample.com";
			//AddDefaultFields();
		}

		public List<ExportProfile> ExportProfiles { get; set; }
		public List<ExportSettings> LocalExportSettings { get; set; }
		public List<ExportSettings> FtpExportSettings { get; set; }

		public List<ExportFieldRule> ExportRules { get; set; }
		public List<BulletsMoveInfo> BulletsMoveInfos { get; set; }

		public List<ExportFieldCategoryInfo> CategoryInfos { get; set; }

		public void AddDefaultFields()
		{
			Fields.Add(new FieldInfo { VisibleIndex = 0, FieldName = "PartNumber", Header = "Part #", Export = true });
			Fields.Add(new FieldInfo { VisibleIndex = 1, FieldName = "ManufacturerNumber", Header = "Man. #", Export = true });
			Fields.Add(new FieldInfo { VisibleIndex = 2, FieldName = "Name", Header = "Name", Export = true });
			Fields.Add(new FieldInfo { VisibleIndex = 3, FieldName = "Brand", Header = "Brand", Export = true });
			Fields.Add(new FieldInfo { VisibleIndex = 4, FieldName = "MSRP", Header = "MSRP", Export = true });
			Fields.Add(new FieldInfo { VisibleIndex = 5, FieldName = "Cost", Header = "Cost", Export = true });
			Fields.Add(new FieldInfo { VisibleIndex = 6, FieldName = "Quantity", Header = "Quantity", Export = true });
			Fields.Add(new FieldInfo { VisibleIndex = 7, FieldName = "ImageUrl", Header = "ImageUrl", Export = true });
			Fields.Add(new FieldInfo { VisibleIndex = 8, FieldName = "Description", Header = "Description", Export = true });
		}

		public DateTime LastRun { get; set; }
		public string FileName { get; set; }
		public bool OverwriteFile { get; set; }
		public string FtpFileName { get; set; }
		public bool OverwriteFtpFile { get; set; }
		public string ProxyFilePath { get; set; }
		public bool UseProxy { get; set; }
		public bool UseFtp { get; set; }
		public List<FieldInfo> Fields { get; set; }
		public bool SaveLocally { get; set; }
		public bool Googlebot { get; set; }
		public string Url { get; set; }
        public int MaxRecords { get; set; }
		public bool LocalSaveByBrand { get; set; }
		public bool FtpSaveByBrand { get; set; }
		public bool LimitRecords { get; set; }
		public bool UseLogin { get; set; }
		public string ProxyListUrl { get; set; }

		public string ProcessAssembly { get; set; }

		public bool ConvertImages { get; set; }
		public bool DownloadImages { get; set; }
		public bool UploadImagesToFtp { get; set; }
		public string ImagesPublicPath { get; set; }
		public string ImagesFolder { get; set; }


		public string SCEAPIKey { get; set; }
		public string SCEAPISecret { get; set; }
		public string SCEAccessKey { get; set; }
		public string SCEStore{ get; set; }
		public string SCEServerAddress { get; set; }
		public bool SCEBatchProcess { get; set; }

		public string SCEDemoSite { get; set; }

		[XmlIgnore]
		public bool SCEEnabled { get; set; }

		public void SaveConfig(string configFile)
		{
			using (var fs = System.IO.File.Create(configFile))
			{
				XmlSerializer xs = new XmlSerializer(typeof(ScraperSettings), AdditionalTypes.ToArray());
				xs.Serialize(fs, this);
			}
		}

		static void UpdateProfiles(ScraperSettings sett)
		{
			if (sett.ExportProfiles.Count == 0)
			{
				var prof = new ExportProfile { Name = "Default", Fields = sett.Fields };
				sett.ExportProfiles.Add(prof);
				ExportSettings ex = new ExportSettings();
				sett.LocalExportSettings.Add(ex);
				ex.FileName = sett.FileName;
				ex.Profile = prof;
				ex.ProfileName = prof.Name;

				ex = new ExportSettings();
				sett.FtpExportSettings.Add(ex);
				ex.FileName = sett.FtpFileName;
				ex.Profile = prof;
				ex.ProfileName = prof.Name;
			}
			/*
			foreach(var item in  sett.LocalExportSettings)
				item.Profile = sett.ExportProfiles.Where(x => x.Name == item.ProfileName).FirstOrDefault();
			foreach (var item in sett.FtpExportSettings)
				item.Profile = sett.ExportProfiles.Where(x => x.Name == item.ProfileName).FirstOrDefault();
			*/
		}

		public static ScraperSettings ReadConfig(string configFile, Type[] additionalTypes = null)
		{
			if (additionalTypes == null)
				additionalTypes = new Type[0];

			if (File.Exists(configFile))
			{
				var xs = new XmlSerializer(typeof(ScraperSettings), additionalTypes);
				using (var sr = System.IO.File.OpenText(configFile))
				{
					var xtr = new System.Xml.XmlTextReader(sr);
					xs.UnknownAttribute += xs_UnknownAttribute;
					xs.UnknownElement += xs_UnknownElement;
					xs.UnknownNode += xs_UnknownNode;
					xs.UnreferencedObject += xs_UnreferencedObject;
					var conf = (ScraperSettings)xs.Deserialize(xtr);
					conf.AdditionalTypes.AddRange(additionalTypes);
					UpdateProfiles(conf);
					return conf;
				}
			}
			return new ScraperSettings();
		}

		static void xs_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
		{
			throw new NotImplementedException();
		}

		static void xs_UnknownNode(object sender, XmlNodeEventArgs e)
		{
			//throw new NotImplementedException();
		}

		static void xs_UnknownElement(object sender, XmlElementEventArgs e)
		{
			//throw new NotImplementedException();
		}

		static void xs_UnknownAttribute(object sender, XmlAttributeEventArgs e)
		{
			//throw new NotImplementedException();
		}
	}
}
