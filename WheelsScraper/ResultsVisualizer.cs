using HtmlAgilityPack;
using Scraper.Lib.Main;
using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WheelsScraper.Export;

namespace WheelsScraper
{
	public class ResultsVisualizer
	{
		//ISimpleScraper Scrap { get; set; }
		//ScraperSettings Sett { get; set; }

		public static string EvaluateCategoryFormula(object wi, FieldInfo fi, ExportFieldCategoryInfo ci)
		{
			var sbRes = new StringBuilder();
			foreach (var c in ci.Categories)
			{
				var catName = EvaluateFormula(wi, c.Name);
				var sbVals = new StringBuilder();
				foreach (var v in c.Values)
				{
					var val = EvaluateFormula(wi, v.Value);
					if (!string.IsNullOrEmpty(val))
					{
						if (sbVals.Length > 0)
							sbVals.Append(ExportFieldSpcValueInfo.DescSepataror);
						sbVals.AppendFormat("{0}{1}{2}{3}{4}", v.Description, ExportFieldSpcValueInfo.DescValSeparator, val, ExportFieldSpcValueInfo.DescValSeparator, v.Visible ? "1" : "0");
					}
				}
				if (sbVals.Length > 0)
				{
					if (sbRes.Length > 0)
						sbRes.Append(ExportFieldCategoryInfo.SpcSeparator);
					sbRes.AppendFormat("{0}{1}{2}", c.Name, ExportFieldSpcCategoryInfo.NameDescSeparator, sbVals);
				}
			}
			return sbRes.ToString();
		}

		public static object GetPropValueWithRulesApplied(object wi, string extFieldName, ScraperSettings sett, FieldInfo fi)
		{
			if (!string.IsNullOrEmpty(fi.CustomValue))
				return fi.CustomValue;
			var fieldName = extFieldName;
			if (fieldName != null && fieldName.StartsWith("__"))
				fieldName = fieldName.Substring(2);
			object result = PropHelper.GetPropValue(wi, fieldName);

			foreach (var rule in sett.ExportRules)
			{
				if (rule.FieldName != fieldName || !rule.Enabled)
					continue;
				if (CheckIfMatchesRule(result, wi, rule))
				{
					result = ApplyFormula(result, wi, rule);
					//break;
				}
			}

			foreach (var bmi in sett.BulletsMoveInfos)
			{
				if (bmi.DstField != fieldName || !bmi.Enabled)
					continue;
				result = ApplyBullets(wi, bmi);
			}

			foreach (var ci in sett.CategoryInfos)
			{
				if (ci.OutputHeader == fi.Header)
				{
					result = EvaluateCategoryFormula(wi, fi, ci);
				}
			}

			var resStr = result == null ? "" : result.ToString();
			if (fi.StripHtml)
				resStr = StripHtml(resStr);

			resStr = ToProperCase(resStr, fi.Case);

			if (!string.IsNullOrEmpty(fi.UnitConvertion))
			{
				double dVal = 0;
				if (double.TryParse(resStr, out dVal))
					resStr = Databox.UnitConverters.UnitConverter.Convert(fi.UnitConvertion, dVal).ToString();
			}

			return resStr;
		}

		private static string ApplyFormula(object curValue, object wi, ExportFieldRule rule)
		{
			var rgxColP = new Regex("\\[(.*?)\\]");

			string curStrVal = curValue == null ? "" : curValue.ToString();
			string replaceText = rule.ReplaceString;
			replaceText = EvaluateFormula(wi, replaceText);
			if (rule.RuleType == OperationType.RegexReplace)
			{
				var searchStr = EvaluateFormula(wi, rule.SearchString);
				replaceText = Regex.Replace(curStrVal, searchStr, replaceText);
			}
			return replaceText;
		}

		public static string EvaluateFormula(object wi, string formula)
		{
			var rgxColP = new Regex("\\[(.*?)\\]");

			string replaceText = formula ?? "";
			var sbReplString = new StringBuilder(replaceText);
			var ms = rgxColP.Matches(replaceText);

			if (replaceText.IndexOf("[") > -1)
			{
				foreach (Match m in ms)
				{
					var colName = m.Groups[1].Value;

					var fieldVal = (Scraper.Lib.Main.PropHelper.GetPropValue(wi, colName) ?? "").ToString();
					sbReplString.Replace(m.Value, fieldVal);
				}
				replaceText = sbReplString.ToString();
			}
			return replaceText;
		}


		protected static string ToProperCase(string str, Scraper.Shared.Export.Case charCase)
		{
			if (string.IsNullOrEmpty(str))
				return str;
			if (charCase == Scraper.Shared.Export.Case.Ignore)
				return str;
			if (charCase == Scraper.Shared.Export.Case.Lower)
				return str.ToLower();
			if (charCase == Scraper.Shared.Export.Case.Upper)
				return str.ToUpper();
			if (charCase == Scraper.Shared.Export.Case.Title)
				return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
			return str;
		}

		private static string ApplyBullets(object wi, BulletsMoveInfo bmi)
		{
			var bulletsObj = PropHelper.GetPropValue(wi, bmi.SourceField);
			var bulletsHtml = bulletsObj == null ? "" : bulletsObj.ToString();
			if (string.IsNullOrEmpty(bulletsHtml))
				return null;

			var doc = new HtmlDocument();
			doc.LoadHtml(bulletsHtml);

			var lis = doc.DocumentNode.SelectNodes("//li");
			if (lis == null)
				return null;

			var res = new StringBuilder();
			foreach (var li in lis)
			{
				if (res.Length > 0)
					res.Append("<br>" + Environment.NewLine);
				res.Append(li.OuterHtml);
			}
			return res.ToString();
		}

		protected static string ReplaceTagsWithText(string html, string xPath, string text)
		{
			if (string.IsNullOrEmpty(html))
				return null;

			var doc = new HtmlDocument();
			doc.LoadHtml(html);
			var brs = doc.DocumentNode.SelectNodes(xPath);
			if (brs != null)
			{
				foreach (var br in brs)
				{
					var tag = doc.CreateElement("text");
					tag.InnerHtml = text;
					br.ParentNode.ReplaceChild(tag, br);
				}
			}
			return doc.DocumentNode.OuterHtml;
		}

		static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
		static Regex rgxRemoveDuplicates = new Regex(@"[ ]{2,}");

		public static string StripHtml(string html)
		{
			if (string.IsNullOrEmpty(html))
				return null;

			var res = ReplaceTagsWithText(html, "//br | //p", "%LINE_BREAK%");
			res = _htmlRegex.Replace(html, " ");
			var sbRes = new StringBuilder(rgxRemoveDuplicates.Replace(res, " ").Trim());
			sbRes.Replace("%LINE_BREAK%", "<br>" + Environment.NewLine);
			sbRes.Replace(Environment.NewLine + " ", Environment.NewLine);
			return sbRes.ToString();
		}

		private static bool CheckIfMatchesRule(object objValue, object wi, ExportFieldRule rule)
		{
			bool validRuleFound = false;
			//if (string.IsNullOrEmpty(rule.SearchString))
			//	return false;

			//var field=profile.Fields.Where(x=>x.Header==)
			string value = null;
			if (objValue == null)
				value = "";
			else
				value = objValue.ToString();

			var ruleValue = rule.SearchString ?? "";
			ruleValue = ResultsVisualizer.EvaluateFormula(wi, ruleValue);

			if (!rule.MatchCase)
			{
				ruleValue = ruleValue.ToLower();
				value = value.ToLower();
			}

			if ((rule.RuleType == OperationType.Regex || rule.RuleType == OperationType.RegexReplace) && !string.IsNullOrEmpty(ruleValue))
			{
				var rgs = new Regex(ruleValue);

				if (rgs.IsMatch(value))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.Contains)
			{
				if (value.Contains(ruleValue))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.DoesntContain)
			{
				if (!value.Contains(ruleValue))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.StartsWith)
			{
				if (value.StartsWith(ruleValue))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.EqualsTo)
			{
				if (value == ruleValue)
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.NotEqualsTo)
			{
				if (value != ruleValue)
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.EqualOrGreaterThan)
			{
				if (CompareAsInt(rule.RuleType, value, ruleValue))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.EqualOrLessThan)
			{
				if (CompareAsInt(rule.RuleType, value, ruleValue))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.IsEmpty)
			{
				if (string.IsNullOrEmpty(value))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.IsNotEmpty)
			{
				if (!string.IsNullOrEmpty(value))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.IsGreaterThan)
			{
				if (CompareAsInt(rule.RuleType, value, ruleValue))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.IsLessThan)
			{
				if (CompareAsInt(rule.RuleType, value, ruleValue))
					validRuleFound = true;
			}
			else if (rule.RuleType == OperationType.Always)
				validRuleFound = true;

			return validRuleFound;
		}

		public static bool CompareAsInt(OperationType ruleType, string str1, string str2)
		{
			try
			{
				var i1 = Convert.ToInt32(str1);
				var i2 = Convert.ToInt32(str2);

				if (ruleType == OperationType.EqualOrGreaterThan)
				{
					if (i1 >= i2)
						return true;
				}
				else if (ruleType == OperationType.EqualOrLessThan)
				{
					if (i1 <= i2)
						return true;
				}
				else if (ruleType == OperationType.IsGreaterThan)
				{
					if (i1 > i2)
						return true;
				}
				else if (ruleType == OperationType.IsLessThan)
				{
					if (i1 < i2)
						return true;
				}
			}
			catch
			{

			}

			return false;
		}
	}
}