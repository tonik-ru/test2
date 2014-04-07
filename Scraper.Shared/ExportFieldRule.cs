using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper.Export
{
	public class ExportFieldRule
	{
		public bool Enabled { get; set; }
		public string FieldHeader { get; set; }
		public string FieldName { get; set; }
		[System.Xml.Serialization.XmlElement("OpType", Namespace = "http://easydatafeed.com")]
		public OperationType RuleType { get; set; }
		public string SearchString { get; set; }
		public bool MatchCase { get; set; }
		public bool IsRegularExpression { get; set; }
		public string ReplaceString { get; set; }
		public int Order { get; set; }
	}

	public enum OperationType
	{
		Contains,
		DoesntContain,
		StartsWith,
		EndsWith,
		IsEmpty,
		IsNotEmpty,
		IsGreaterThan,
		EqualOrGreaterThan,
		IsLessThan,
		EqualOrLessThan,
		EqualsTo,
		NotEqualsTo,
		Always,
		Regex,
		RegexReplace
	}
}