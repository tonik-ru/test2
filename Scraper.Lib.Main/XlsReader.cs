using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Databox.Libs.Common
{
	public static class XlsReader
	{
		private static int lastReadField;
		private static string lastReadValue;

		private static object ReadField(IDataReader reader, int column)
		{
			try
			{
				lastReadField = column;
				var r = reader[column];
				if (r is DBNull)
					lastReadValue = null;
				else
					lastReadValue = r.ToString();
				return r;
			}
			catch (Exception err)
			{
				return null;
			}
		}

		private static string ReadString(this IDataReader reader, int column)
		{
			var r = ReadField(reader, column);
			//lastReadField = column;
			//var r = reader[column];
			if (r is DBNull)
				return null;
			else if (r == null)
				return null;
			else
				return r.ToString().Trim();
		}

		private static double ParseDouble(string str)
		{
			//LogManager.PrintMessage("Parsing " + str);
			CultureInfo myCI = new CultureInfo(Thread.CurrentThread.CurrentCulture.LCID);
			myCI.NumberFormat.NumberDecimalSeparator = ".";
			myCI.NumberFormat.NumberGroupSeparator = ",";

			double val = 0;
			double.TryParse(str, NumberStyles.Number, myCI.NumberFormat, out val);
			//LogManager.PrintMessage("Result = " + val);
			return val;
		}

		private static double ReadDouble(this IDataReader reader, int column)
		{
			double val = 0;
			var r = ReadField(reader, column);
			//var r = reader[column];
			if (r is DBNull)
				return 0;
			else
				if (r is double)
					return (double)r;
				else
				{
					var r2 = r.ToString().Replace("$", "");
					return ParseDouble(r2);
				}
		}

		private static int ReadInt(this IDataReader reader, int column)
		{
			var r = ReadField(reader, column);
			if (r is DBNull)
				return 0;
			else
				if (r is int)
					return (int)r;
				else
					return int.Parse(r.ToString());
		}

		private static IDataReader OpenCsv(string filePath)
		{
			var sr = new StreamReader(filePath);
			var csv = new CsvReader(sr, true, ',');
			return csv;
		}

		private static IDataReader OpenExcel(string filePath, string sheetName)
		{
			string conStr = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0 Xml; HDR=YES\";";
			//using (
			var con = new OleDbConnection(conStr);
			{
				con.Open();

				var cmd = con.CreateCommand();

				cmd.CommandText = "Select * from [" + sheetName + "]";
				var reader = cmd.ExecuteReader();
				return reader;
			}
		}

		private static IDataReader OpenFile(string filePath, string sheetName)
		{
			if (filePath.ToLower().EndsWith(".csv") || filePath.ToLower().EndsWith(".txt"))
				return OpenCsv(filePath);
			else
				return OpenExcel(filePath, sheetName);
		}

		//public static List<Dictionary<string, string>> ReadXls(string filePath, string sheetName, string fieldPrefix)
		//{
		//	return ReadXls(filePath, sheetName, fieldPrefix, ',');
		//}

		//public static List<Dictionary<string, string>> ReadXls(string filePath, string sheetName, string fieldPrefix, char delimiter)		
		public static List<Dictionary<string, string>> ReadXls(string filePath, string sheetName, string fieldPrefix)
		{
			if (!File.Exists(filePath))
				return new List<Dictionary<string, string>>();
			//string conStr = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0 Xml; HDR=YES\";";
			//int wareCount = 0;
			//string articul1 = null;
			if (string.IsNullOrEmpty(sheetName))
			{
				var sheets = GetSheetNames(filePath);
				if (sheets.Count == 0)
					throw new Exception("No sheets found");
				sheetName = sheets[0];
			}
			using (var reader = OpenFile(filePath, sheetName))
			{
				var vals = new List<Dictionary<string, string>>();

				while (reader.Read())
				{
					var dicValues = new Dictionary<string, string>();
					vals.Add(dicValues);
					for (int i = 0; i < reader.FieldCount; i++)
					{
						var keyName = string.Format("{0}{1}", fieldPrefix, reader.GetName(i));
						dicValues.Add(keyName, reader.ReadString(i));
					}
				}
				return vals;
			}
		}

		public static List<Dictionary<string, string>> ReadXls_old(string filePath, string sheetName, string fieldPrefix)
		{
			string conStr = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0 Xml; HDR=YES\";";
			int wareCount = 0;
			string articul1 = null;
			using (OleDbConnection con = new OleDbConnection(conStr))
			{
				con.Open();

				OleDbCommand cmd = con.CreateCommand();

				cmd.CommandText = "Select * from [" + sheetName + "]";
				OleDbDataReader reader = cmd.ExecuteReader();

				var vals = new List<Dictionary<string, string>>();

				while (reader.Read())
				{
					var dicValues = new Dictionary<string, string>();
					vals.Add(dicValues);
					for (int i = 0; i < reader.FieldCount; i++)
					{
						var keyName = string.Format("{0}{1}", fieldPrefix, reader.GetName(i));
						dicValues.Add(keyName, reader.ReadString(i));
					}
				}
				return vals;
			}
		}

		public static List<string> GetSheetNames(string filePath)
		{
			var lstNames = new List<string>();
			if (!System.IO.File.Exists(filePath))
				return lstNames;

			if (filePath.ToLower().EndsWith(".csv"))
			{
				lstNames.Add("");
				return lstNames;
			}

			string conStr = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0 Xml; HDR=YES\";";
			using (OleDbConnection con = new OleDbConnection(conStr))
			{
				con.Open();

				var tables = con.GetSchema("Tables");
				foreach (DataRow tblRow in tables.Rows)
				{
					var tblName = (string)tblRow["TABLE_NAME"];
					lstNames.Add(tblName);
				}
			}

			return lstNames;
		}

		public static List<string> GetSheetColumnNames(string filePath, string sheetName)
		{
			var lstNames = new List<string>();
			if (!System.IO.File.Exists(filePath))
				return lstNames;

			if (filePath.ToLower().EndsWith(".csv"))
			{
				using (var sr = new StreamReader(filePath))
				{
					var csv = new CsvReader(sr, true, ',');
					lstNames.AddRange(csv.GetFieldHeaders());
					return lstNames;
				}
			}

			string conStr = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=\"" + filePath + "\";Extended Properties=\"Excel 12.0 Xml; HDR=YES\";";
			using (OleDbConnection con = new OleDbConnection(conStr))
			{
				con.Open();

				var cols = con.GetSchema("Columns");
				lstNames = cols.Rows.Cast<DataRow>().Where(x => (string)x["TABLE_NAME"] == sheetName).Select(x => (string)x["COLUMN_NAME"]).ToList();
			}

			return lstNames;
		}

		public static int lastRow;
	}
}