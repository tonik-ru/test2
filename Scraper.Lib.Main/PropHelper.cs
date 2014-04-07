using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WheelsScraper;
using WheelsScraper.Export;

namespace Scraper.Lib.Main
{
	public class PropHelper
	{
		//private static Dictionary<string, PropertyInfo> propsInType;
		private static Dictionary<Type, Dictionary<string, PropertyInfo>> dicPropTypes = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

		public static List<string> GetProperties(ISimpleScraper scr)
		{
			var fields = new List<string>();
			if (scr is IFieldInfoProvider)
				fields = (scr as IFieldInfoProvider).GetFieldNames();
			else
				fields = scr.WareInfoType.GetType().GetProperties().Where(x => x.PropertyType == typeof(string)).Select(x => x.Name).Distinct().ToList();
			return fields;
		}

		public static object GetPropValue(object src, string propName)
		{
			var propsInType = GetPropertiesInType(src);

			if (string.IsNullOrEmpty(propName))
				return null;

			var realPropName = propName;
			if (propName.StartsWith("__"))
				realPropName = propName.Substring(2);

			if (src is IExtWareInfoProvider)
			{
				return (src as IExtWareInfoProvider).GetFieldValue(realPropName);
			}
			try
			{
				if (!propsInType.ContainsKey(realPropName))
					return null;
				else
					return propsInType[realPropName].GetValue(src, null);
			}
			catch (Exception err)
			{
				return "";
			}
		}

		public static object ApplyRule(object val, ProfileRule rule)
		{
			object res = val;
			try
			{
				var dVal = Convert.ToDouble(val);
				if (rule.RuleType == ProfileRuleType.IncreaseByValue)
					res = dVal + rule.Value;
				else if (rule.RuleType == ProfileRuleType.IncreaseByPercent)
					res = dVal + dVal / 100 * rule.Value;
				else if (rule.RuleType == ProfileRuleType.IncreaseByPercent)
					res = dVal + dVal / 100 * rule.Value;
				else if (rule.RuleType == ProfileRuleType.IncreaseByValue)
					res = dVal - rule.Value;
				else if (rule.RuleType == ProfileRuleType.NetProfit)
				{
					if (rule.Value < 100)
						res = dVal / (1 - rule.Value / 100);
				}
			}
			catch (Exception)
			{
			}
			return res;
		}

		public static object GetExportFieldValue(object obj, ScraperSettings sett, ExportProfile profile, string fieldHeader, string fieldName)
		{
			object result = obj;
			var curFieldInfo = profile.Fields.Where(x => x.Header == fieldHeader).FirstOrDefault();
			var rule = sett.ExportRules.Where(x => x.FieldHeader == fieldHeader).FirstOrDefault();
			if (rule == null)
				result = PropHelper.GetPropValue(obj, fieldName);
			if (curFieldInfo != null)
				result = ApplyRule(result, curFieldInfo.ProfileRule);
			return result;
		}


		private static Dictionary<string, PropertyInfo> GetPropertiesInType(object src)
		{
			lock (dicPropTypes)
			{
				Dictionary<string, PropertyInfo> propsInType;

				//if (dicPropTypes == null)
				//	dicPropTypes = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
				var type = src.GetType();

				dicPropTypes.TryGetValue(type, out propsInType);
				if (propsInType == null)
				{
					propsInType = type.GetProperties().ToDictionary(x => x.Name);
					dicPropTypes.Add(type, propsInType);
				}
				return propsInType;
			}
		}

		public static void SetPropValue(object src, string propName, object value)
		{
			var propsInType = GetPropertiesInType(src);

			if (string.IsNullOrEmpty(propName))
				return;
			if (src is IExtWareInfoProvider)
			{
				(src as IExtWareInfoProvider).SetFieldValue(propName, value);
				return;
			}

			try
			{
				if (!propsInType.ContainsKey(propName))
					return;
				else
				{
					var pr = propsInType[propName];
					object r4 = Convert.ChangeType(value, pr.PropertyType);
					propsInType[propName].SetValue(src, r4, null);
				}
			}
			catch (Exception err)
			{

			}
		}
	}
}
