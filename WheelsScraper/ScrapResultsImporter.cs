using LumenWorks.Framework.IO.Csv;
using Scraper.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WheelsScraper
{
	public class ScrapResultsImporter
	{
		char delimiter = ',';
		private Dictionary<string, PropertyInfo> propsInType;

		public void ImportResults(ISimpleScraper scrap, string fileName, Scraper.Shared.ExportProfile profile)
		{
			var wareInfoType = scrap.WareInfoType.GetType();
			propsInType = wareInfoType.GetProperties().ToDictionary(x => x.Name);
			var fields = profile.Fields.Where(x => x.Export).OrderBy(x => x.VisibleIndex).ToList();

			using (var sr = File.OpenText(fileName))
			{
				var csv = new CsvReader(sr, true, delimiter);

				int fieldCount = csv.FieldCount;
				string[] headers = csv.GetFieldHeaders();
				while (csv.ReadNextRecord())
				{
					object wareInfo = Activator.CreateInstance(wareInfoType);
					for (int i = 0; i < fieldCount; i++)
					{
						var wi2 = wareInfo as IExtWareInfoProvider;
						if (wi2 != null)
						{
							wi2.SetFieldValue(headers[i], csv[i]);
							continue;
						}
						var fieldInfo = profile.Fields[i];
						if (string.IsNullOrEmpty(fieldInfo.FieldName))
							continue;
						Scraper.Lib.Main.PropHelper.SetPropValue(wareInfo, fieldInfo.FieldName, csv[i]);
						//SetPropValue(wareInfo, fieldInfo.FieldName, csv[i]);
					}
					scrap.Wares.Add((WareInfo)wareInfo);
					((IList)scrap.WareInfoList).Add(wareInfo);
				}
			}
		}

		protected void SetPropValue(object obj, string propName, object value)
		{
			if (string.IsNullOrEmpty(propName))
				return;
			try
			{
				if (obj is IExtWareInfoProvider)
				{
					(obj as IExtWareInfoProvider).SetFieldValue(propName, value);
					return;
				}
				if (!propsInType.ContainsKey(propName))
					return;
				else
				{
					var prop = propsInType[propName];
					object convertedVal = value;
					if (prop.PropertyType != typeof(string))
					{
						if (prop.PropertyType == typeof(int))
							convertedVal = Convert.ToInt32(value);
						else if (prop.PropertyType == typeof(double))
							convertedVal = Convert.ToDouble(value);
						else if (prop.PropertyType == typeof(float))
							convertedVal = (float)Convert.ToDouble(value);
					}
					propsInType[propName].SetValue(obj, convertedVal, null);
				}
			}
			catch (Exception err)
			{
			}
		}
	}
}