using DevExpress.XtraGrid.Columns;
using Scraper.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WheelsScraper.Export;

namespace WheelsScraper
{
	public class DataReplacer
	{
		public static void FindAndReplace(IEnumerable data, DataReplaceEvent e, ExportProfile curProfile, GridColumnCollection columns, EDFManager edfMgr)
		{
			var rgx = new Regex(e.SearchText);
			if (!e.MatchCase)
				rgx = new Regex(e.SearchText, RegexOptions.IgnoreCase);

			var rgxColP = new Regex("\\[(.*?)\\]");
			UndoData undo = null;

			foreach (DevExpress.XtraGrid.Columns.GridColumn col in columns)
			{
				if (e.Column != "All columns" && e.Column != col.Caption)
					continue;

				var curFieldInfo = curProfile.Fields.Where(x => x.Header == col.Caption).FirstOrDefault();
				foreach (var row in data)
				{
					var colValue = Scraper.Lib.Main.PropHelper.GetPropValue(row, curFieldInfo.FieldName);
					if (colValue == null)
						continue;
					var colStrVal = colValue.ToString();
					string strUpdated = colStrVal;

					string replaceText = e.ReplaceText;
					if (replaceText.IndexOf("[") > -1)
					{
						var sbReplString = new StringBuilder(replaceText);
						var ms = rgxColP.Matches(replaceText);

						foreach (Match m in ms)
						{
							var colName = m.Groups[1].Value;
							var curReplField = curProfile.Fields.Where(x => x.Header == colName).FirstOrDefault();
							if (curReplField == null)
								continue;

							var tmpStr = (Scraper.Lib.Main.PropHelper.GetPropValue(row, curReplField.FieldName) ?? "").ToString();
							sbReplString.Replace(m.Value, tmpStr);
						}
						replaceText = sbReplString.ToString();
					}

					if (e.IsRegex)
					{
						strUpdated = rgx.Replace(colStrVal, replaceText);
					}
					else
					{
						int pos;
						pos = colStrVal.IndexOf(e.SearchText, e.MatchCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
						if (pos != -1)
						{
							if (e.WholeCell && pos == 0 && colStrVal.Length == e.SearchText.Length)
								strUpdated = replaceText;
							else
							{
								strUpdated = colStrVal.Replace(e.SearchText, replaceText, (e.MatchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase));
							}
						}
					}

					if (colStrVal != strUpdated)
					{
						if (undo == null)
						{
							undo = edfMgr.UndoMgr.CreateUndo();
							undo.Name = "Replace";
						}
						undo.AddItem(row, curFieldInfo.FieldName, colStrVal, strUpdated);
						Scraper.Lib.Main.PropHelper.SetPropValue(row, curFieldInfo.FieldName, strUpdated);
					}
				}
			}
		}


		private static void ApplyRules(object row, FieldInfo fi, ExportProfile profile, ISimpleScraper scraper)
		{
			var curValue = Scraper.Lib.Main.PropHelper.GetPropValue(row, fi.FieldName);
			var rgxColP = new Regex("[(.*?)]");

			foreach (var rule in fi.ExportRules)
			{
				object testValue = curValue;
				//if (!string.IsNullOrEmpty(rule.FieldName))
				//	testValue = Scraper.Lib.Main.PropHelper.GetPropValue(row, rule.FieldName);

				string replaceText = rule.ReplaceString;
				var sbReplString = new StringBuilder(replaceText);
				var ms = rgxColP.Matches(replaceText);

				if (replaceText.IndexOf("[") > -1)
				{
					foreach (Match m in ms)
					{
						var colName = m.Groups[1].Value;

						var fieldVal = (Scraper.Lib.Main.PropHelper.GetPropValue(row, colName) ?? "").ToString();
						sbReplString.Replace(m.Value, fieldVal);
					}
					replaceText = sbReplString.ToString();
				}


			}
		}

		public static void ApplyAllRules(IEnumerable data, ExportProfile profile, ISimpleScraper scraper)
		{
			foreach (var field in profile.Fields)
			{
				if (field.ExportRules.Count == 0)
					continue;
				
				foreach (var row in data)
				{
					ApplyRules(row, field, profile, scraper);
				}
			}
			//	var rgx = new Regex(e.SearchText);
			//	if (!e.MatchCase)
			//		rgx = new Regex(e.SearchText, RegexOptions.IgnoreCase);

			//	var rgxColP = new Regex("[(.*?)]");

			//	foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView2.Columns)
			//	{
			//		if (e.Column != "All columns" && e.Column != col.Caption)
			//			continue;

			//		var curFieldInfo = curProfile.Fields.Where(x => x.Header == col.Caption).FirstOrDefault();
			//		foreach (var row in data)
			//		{
			//			var colValue = Scraper.Lib.Main.PropHelper.GetPropValue(row, curFieldInfo.FieldName);
			//			if (colValue == null)
			//				continue;
			//			var colStrVal = colValue.ToString();
			//			string strUpdated = null;

			//			string replaceText = e.ReplaceText;
			//			if (replaceText.IndexOf("[") > -1)
			//			{
			//				var sbReplString = new StringBuilder(replaceText);
			//				var ms = rgxColP.Matches(replaceText);

			//				foreach (Match m in ms)
			//				{
			//					var colName = m.Groups[1].Value;
			//					var curReplField = curProfile.Fields.Where(x => x.Header == colName).FirstOrDefault();
			//					if (curReplField == null)
			//						continue;

			//					var tmpStr = (Scraper.Lib.Main.PropHelper.GetPropValue(row, curReplField.FieldName) ?? "").ToString();
			//					sbReplString.Replace(m.Value, tmpStr);
			//				}
			//				replaceText = sbReplString.ToString();
			//			}

			//			if (e.IsRegex)
			//			{
			//				strUpdated = rgx.Replace(colStrVal, replaceText);
			//			}
			//			else
			//			{
			//				int pos;
			//				pos = colStrVal.IndexOf(e.SearchText, e.MatchCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
			//				if (pos != -1)
			//				{
			//					if (e.WholeCell && pos == 0 && colStrVal.Length == e.SearchText.Length)
			//						strUpdated = replaceText;
			//					else
			//					{
			//						strUpdated = colStrVal.Replace(e.SearchText, replaceText, (e.MatchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase));
			//					}
			//				}
			//			}

			//			if (colStrVal != strUpdated)
			//				Scraper.Lib.Main.PropHelper.SetPropValue(row, curFieldInfo.FieldName, colStrVal);
			//		}
			//}
		}
	}
}