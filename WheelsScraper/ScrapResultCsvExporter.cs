using Scraper.Lib.Main;
using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WheelsScraper
{
	public class ScrapResultCsvExporter
	{
		private Dictionary<string, PropertyInfo> propsInType;

		public ScraperSettings Sett { get; set; }

		protected static Stream GetOutStream(string path, bool zip)
		{
			var fs = new FileStream(path, FileMode.Create);
			return fs;
			/*
			Stream fs2 = fs;
			if (zip)
			{
				var fs3 = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(fs);
				ZipEntry newEntry = new ZipEntry(Path.GetFileName(path));
				newEntry.DateTime = DateTime.Now;

				fs3.PutNextEntry(newEntry);
				fs2 = fs3;
			}
				//fs2 = new GZipStream(fs, CompressionMode.Compress);
			return fs2;
			*/
		}

		AdditionalDataSourceManager addDataMgr;
		public ScrapResultCsvExporter(ISimpleScraper scr, bool useAddData, ScraperSettings sett)
		{
			addDataMgr = new AdditionalDataSourceManager(scr);
			if (useAddData)
				addDataMgr.LoadDataFiles(scr);
			Sett = sett;
		}

		private void ExportSingleItem(List<WareInfo> badWares, bool outputToError, bool onlyBad, string separator, List<FieldInfo> fieldsToShow, WareInfo item, StringBuilder sb, List<Dictionary<string, string>> lstXlsDataForKey, List<FieldInfo> addStaticFields)
		{
			int i = 0;
			foreach (var field in fieldsToShow)
			{
				object val = field.CustomValue;

				var foundF = addStaticFields.Where(x => string.Compare(x.Header, field.Header, true) == 0).FirstOrDefault();
				if (foundF != null)
					val = foundF.CustomValue;
				else
					val = ResultsVisualizer.GetPropValueWithRulesApplied(item, field.FieldName, Sett, field);

				if (val == null)
					val = "";
				if (field.ProfileRule != null)
					val = PropHelper.ApplyRule(val, field.ProfileRule);
				var strVal = val.ToString();

				if (!string.IsNullOrEmpty(field.ValidValues))
				{
					var spls = field.ValidValues.Split('\n');
					if (!spls.Any(x => x.ToLower().Trim() == strVal.ToLower()))
						strVal = "";
				}

				if (field.UseDataFromAdditionalSources)
				{
					foreach (var xlsDataForKey in lstXlsDataForKey)
					{
						string replVal = null;
						if (xlsDataForKey.TryGetValue(field.Header, out replVal))
							strVal = replVal;
					}
				}

				bool addQuote = false;
				//strVal = strVal.Replace("\n", ". ");
				if (strVal.IndexOf("\"") != -1)
					strVal = strVal.Replace("\"", "\"\"");

				addQuote = strVal.IndexOf(separator) != -1 || strVal.IndexOf('\n') != -1 || strVal.IndexOf('"') != -1;

				if (!onlyBad && field.MaxLength > 0 && strVal.Length > field.MaxLength)
				{
					if (outputToError)
					{
						badWares.Add(item);
						sb.Length = 0;
						break;
					}
					else
						strVal = strVal.Substring(0, field.MaxLength);
				}

				if (addQuote)
					strVal = "\"" + strVal + "\"";

				if (i > 0)
					sb.Append(separator);

				i++;
				sb.Append(strVal);
			}
		}
		public void Export(List<WareInfo> wares, List<FieldInfo> fields, string path, bool zip, out List<WareInfo> badWares, bool outputToError, bool onlyBad, ExportProfile profile)
		{
			badWares = new List<WareInfo>();
			string separator = ",";
			if (wares.Count == 0)
				return;

			using (var fs = GetOutStream(path, zip))
			{
				using (StreamWriter sw = new StreamWriter(fs))
				{
					if (propsInType == null)
						propsInType = wares[0].GetType().GetProperties().ToDictionary(x => x.Name);
					var fieldsToShow = fields.Where(x => x.Export).OrderBy(x => x.VisibleIndex).ToList();
					var fieldsToShow2 = profile.AdditionalStaticFields.Where(x => x.Export).OrderBy(x => x.VisibleIndex).ToList();
					if (!profile.UseStaticFields)
						fieldsToShow2.Clear();
					else
					{
						var fieldsToShow3 = fieldsToShow2.Where(x => !fieldsToShow.Any(y => string.Compare(x.Header, y.Header, true) == 0)).OrderBy(x => x.VisibleIndex).ToList();
						fieldsToShow.AddRange(fieldsToShow3);
					}

					var brandColumn = fieldsToShow.Where(x => x.Header != null && x.Header.ToLower() == "brand").FirstOrDefault();

					var headerStr = string.Join(separator, fieldsToShow.Select(x => x.Header).ToArray());
					sw.WriteLine(headerStr);
					foreach (var item in wares)
					{
						var sb = new StringBuilder();

						var lstXlsDataForKey = new List<Dictionary<string, string>>();
						//var dicKeysForFiles = new Dictionary<string, string>();
						if (!string.IsNullOrEmpty(item.PartNumber))
						{
							string brand = null;
							if (brandColumn != null)
								brand = (PropHelper.GetPropValue(item, brandColumn.FieldName) ?? "").ToString();

							foreach (var extDataRow in addDataMgr.AddData)
							{
								string key = null;
								if (!string.IsNullOrEmpty(extDataRow.BrandColumn))
									key = item.Brand;
								else
									key = "%%";
								key += "_" + item.PartNumber;
								//dicKeysForFiles[extDataRow.FileName] = key;

								Dictionary<string, string> xlsData;
								if (extDataRow.XlsDataByKey.TryGetValue(key, out xlsData))
								{
									lstXlsDataForKey.Add(xlsData);
								}
							}
						}

						ExportSingleItem(badWares, outputToError, onlyBad, separator, fieldsToShow, item, sb, lstXlsDataForKey, fieldsToShow2);

						if (sb.Length > 0)
							sw.WriteLine(sb.ToString());
					}
				}
			}
		}


		public object GetPropValue(object src, string propName)
		{
			if (string.IsNullOrEmpty(propName))
				return null;
			if (src is IExtWareInfoProvider)
			{
				return (src as IExtWareInfoProvider).GetFieldValue(propName);
			}
			try
			{
				if (!propsInType.ContainsKey(propName))
					return null;
				else
					return propsInType[propName].GetValue(src, null);
			}
			catch (Exception err)
			{
				return "";
			}
		}
	}
}