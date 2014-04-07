using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public interface ISimpleScraper
	{
		bool RequireSCEAPIKey { get; }
		string Name { get; }
		string Url { get; set; }
		int MaxThreads { get; set; }
		List<LoginInfo> LoginInfos { get; set; }
		List<string> BrandsToLoad { get; }
		List<string> ImageFields { get; set; }

		void Login(string login, string password);

		event EventHandler ItemLoaded;
		event EventHandler BrandLoaded;

		List<WareInfo> Wares { get; }

		List<ProxyInfo> Proxies { get; set; }

		void Cancel();
		void Pause();
		void Resume();
		bool IsPaused { get; }


		void ProcessAllData();

		log4net.ILog Log { get; set; }
		IMessagePrinter MessagePrinter { get; set; }

		int MaxDelay { get; set; }

		bool IsRunning { get; }
		bool CompleteSuccessfully { get; }
		bool HasActiveLogin { get; }
		bool Googlebot { get; set; }
		int MaxRecords { get; set; }
		WareInfo WareInfoType { get; }
		object WareInfoList { get; }
		bool UseLogin { get; set; }

		bool ConvertImages { get; set; }
		bool DownloadImages { get; set; }
		bool UploadImagesToFtp { get; set; }
		string ImagesPublicPath { get; set; }
		string LocalPath { get; set; }
		ScraperSettings Settings { get; set; }

		System.Windows.Forms.Control SettingsTab { get; }

		object SpecialSettings { get; set; }
		Type[] GetTypesForXmlSerialization();

		void OnPostProcess();
		void OnFileUploaded(string fileName);
		void OnResultsLoaded();

		event Action<object, string, object> SomeEvent;
	}
}
