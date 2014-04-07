
using HtmlAgilityPack;
public static class HtmlNodeHelper
{
	public static string InnerTextOrNull(this HtmlNode node)
	{
		if (node == null)
			return null;
		else
			return HtmlDecode(node.InnerText.Trim());
	}

	public static string TrimNull(this string str)
	{
		if (str == null)
			return null;
		else
			return str.Trim();
	}

	public static string AttributeOrNull(this HtmlNode node, string attributeName)
	{
		if (node == null)
			return null;
		if (node.Attributes[attributeName] == null)
			return null;
		return node.Attributes[attributeName].Value;
	}

	public static string HtmlDecode(this string str)
	{
		if (string.IsNullOrEmpty(str))
			return str;
		return System.Web.HttpUtility.HtmlDecode(str);
	}
}
