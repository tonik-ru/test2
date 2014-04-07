using Databox.Libs.Common;
using Scraper.Lib.Main;
using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class AdditionalDataSourceManager
	{
		public List<AddDataFile> AddData { get; private set; }
		ISimpleScraper scr;
		List<FieldInfo> lstFieldsToExport;


		public AdditionalDataSourceManager(ISimpleScraper scr)
		{
			AddData = new List<AddDataFile>();
			//this.profile = profile;
			//lstFieldsToExport = fields;
			//fields = PropHelper.GetProperties(scr);
			this.scr = scr;
			//LoadDataFiles(scr);
		}

		public void UpdateDataFromFiles(WareInfo wi, string colHeader)
		{
			var extSett = AppSettings.Default.GetExtSettings(scr.Name);
			if (!extSett.ValidateAgainstAddData)
				return;

			//foreach (var wi in wares)
			{
				//foreach (var xlsData in addData)
				//{
				//	var fi = extSett.AddDataSourceFiles.Where(x => x.File == xlsData.Key).First();

				//	var brand = wi.Brand;
				//	var partNum = wi.PartNumber;

				//	var xlsDataRow = xlsData.Value.Where(x => x[fi.BrandColumnName] == brand && x[fi.PartNoColumnName] == partNum).FirstOrDefault();
				//	if (xlsDataRow == null)
				//		continue;

				//	var firstRow = xlsData.Value.First();
				//	foreach (var field in profile.Fields)
				//	{
				//		var xlsColName = firstRow.Keys.Where(x => x.ToLower() == field.Header.ToLower()).FirstOrDefault();
				//		if (xlsColName == null)
				//			continue;
				//		PropHelper.SetPropValue(wi, field.FieldName, xlsDataRow[xlsColName]);
				//	}
				//}
			}
		}

		public void LoadDataFiles(ISimpleScraper scr)
		{
			var extSett = AppSettings.Default.GetExtSettings(scr.Name);

			foreach (var fi in extSett.AddDataSourceFiles)
			{
				scr.MessagePrinter.PrintMessage("Loading file: " + fi.File);
				//if (addData.ContainsKey(fi.File))
				//{
				//	scr.MessagePrinter.PrintMessage("File " + fi.File + " already loaded", ImportanceLevel.Mid);
				//	continue;
				//}
				var xlsData = XlsReader.ReadXls(fi.File, fi.SheetName, "");
				if (!xlsData.Any())
					continue;
				if (!string.IsNullOrEmpty(fi.BrandColumnName))
				{
					if (!ValidateColumnExists(xlsData, fi.BrandColumnName))
					{
						scr.MessagePrinter.PrintMessage("Column [" + fi.BrandColumnName + "] is not found in " + fi.File, ImportanceLevel.High);
						continue;
					}
				}
				if (!ValidateColumnExists(xlsData, fi.PartNoColumnName))
				{
					scr.MessagePrinter.PrintMessage("Column [" + fi.PartNoColumnName + "] is not found in " + fi.File, ImportanceLevel.High);
					continue;
				}

				var xlsDataByKey = new Dictionary<string, Dictionary<string, string>>();
				foreach (var row in xlsData)
				{
					if (string.IsNullOrEmpty(row[fi.PartNoColumnName]) && string.IsNullOrEmpty(row[fi.BrandColumnName]))
						continue;
					string key = null;
					if (!string.IsNullOrEmpty(fi.BrandColumnName))
						key = row[fi.BrandColumnName];
					else
						key = "%%";
					key += "_" + row[fi.PartNoColumnName];
					xlsDataByKey[key] = row;
				}
				AddData.Add(new AddDataFile { FileName = fi.File, BrandColumn = fi.BrandColumnName, PartNoColumn = fi.PartNoColumnName, XlsDataByKey = xlsDataByKey });
			}
		}

		private bool ValidateColumnExists(List<Dictionary<string, string>> xlsData, string columName)
		{
			if (!xlsData.Any())
				return false;
			var firstRow = xlsData[0];
			return firstRow.Keys.Any(x => x.ToLower() == columName.ToLower());
		}
	}
}