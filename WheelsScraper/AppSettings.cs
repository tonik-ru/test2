using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WheelsScraper
{
	public class AppSettings
	{
		[XmlIgnore]
		public List<ScraperSettings> ScrapSettings { get; set; }
		//public List<ProfileRule> ProfileRules { get; set; }

		public AppSettings()
		{
			ExtSettings = new List<ExtendedScraperSettings>();
			ScrapSettings = new List<ScraperSettings>();
			SCEFiles = new List<SCEFile>();
			MRUFiles = new List<MRUItem>();
			MRUPaths = new List<MRUItem>();
			//ProfileRules = new List<ProfileRule>();
			MaxProcessors = 1;
		}

		public string SkinName { get; set; }

		[XmlIgnore]
		public int MaxThreads { get; set; }

		[XmlIgnore]
		public TimeSpan TimeLeft { get; set; }

		[XmlIgnore]
		public int MaxSeconds { get; set; }

		[XmlIgnore]
		public DateTime StartDate{ get; set; }

		public string EdfListUrl { get; set; }
		[XmlIgnore]
		public bool EnableSchedule { get; set; }
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public int IntervalMinutes { get; set; }
		public int MaxProcessors { get; set; }
		public bool InventoryOnly { get; set; }
		public bool PricesOnly { get; set; }

		public int ReleaseID { get; set; }
		public string UpdatesServer { get; set; }

		public List<SCEFile> SCEFiles { get; set; }

		public List<ExtendedScraperSettings> ExtSettings { get; set; }

		public List<MRUItem> MRUFiles { get; set; }
		public List<MRUItem> MRUPaths { get; set; }

		private static AppSettings _default;
		public static AppSettings Default
		{
			get
			{
				if (_default == null)
				{
					_default = ReadConfig();
					//_default = new AppSettings();
				}
				return _default;
			}
		}

		public void SaveConfig()
		{
			try
			{
				//var settingsDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				//var settingsDir = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "EDF");
				//var configFile = Path.Combine(settingsDir, "appSettings.config");
				//Directory.CreateDirectory(settingsDir);
				var configFile = GetConfigFileName();
				using (var fs = System.IO.File.Create(configFile))
				{
					XmlSerializer xs = new XmlSerializer(typeof(AppSettings));
					xs.Serialize(fs, _default);
				}
			}
			catch (Exception err)
			{
				Program.Log.Error(err);
			}
		}

		public static string GetConfigFileName()
		{
			var settingsDir = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "EDF");
			var configFile = Path.Combine(settingsDir, "appSettings120.config");
			Directory.CreateDirectory(settingsDir);
			return configFile;
		}

		protected static AppSettings ReadConfig()
		{
			try
			{
				//var settingsDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
				var settingsDir = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "EDF");
				//var configFile = Path.Combine(settingsDir, "appSettings.config");
				var configFile = GetConfigFileName();
				if (!File.Exists(configFile))
				{
					settingsDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
					configFile = Path.Combine(settingsDir, "appSettings.config");
				}

				if (File.Exists(configFile))
				{
					XmlSerializer xs = new XmlSerializer(typeof(AppSettings));
					using (var sr = System.IO.File.OpenText(configFile))
					{
						var conf = (AppSettings)xs.Deserialize(sr);
						foreach (var scr in conf.ScrapSettings)
						{
							if (scr.Fields.Count == 0)
								scr.AddDefaultFields();
							/*
							var fields = scr.ExportProfiles.SelectMany(x => x.Fields);
							foreach (var field in fields)
							{
								if (field.ProfileRuleName != null)
									field.ProfileRule = conf.ProfileRules.Where(x => x.Name == field.ProfileRuleName).FirstOrDefault();
							}
							*/
						}
						return conf;
					}
				}
			}
			catch (Exception err)
			{
				Program.Log.Error(err);
			}
			return new AppSettings();
		}


		public ExtendedScraperSettings GetExtSettings(string moduleName)
		{
			var r = ExtSettings.Where(x => x.ModuleName == moduleName).FirstOrDefault();
			if (r == null)
			{
				r = new ExtendedScraperSettings { ModuleName = moduleName };
				ExtSettings.Add(r);
			}
			return r;
		}
	}
}
