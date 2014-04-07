using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Databox.UnitConverters
{
	public class UnitConverter
	{
		static List<Tuple<UnitTypes, UnitTypes, Func<double, double>>> ConvertFunctions;

		protected static void AddConverter(UnitTypes from, UnitTypes to, Func<double, double> func)
		{
			ConvertFunctions.Add(new Tuple<UnitTypes, UnitTypes, Func<double, double>>(from, to, func));
		}

		public static List<string> GetConvertions()
		{
			var lstConv = ConvertFunctions.Select(x => x.Item1.ToString() + "->" + x.Item2.ToString()).ToList();
			return lstConv;
		}

		static UnitConverter()
		{
			ConvertFunctions = new List<Tuple<UnitTypes, UnitTypes, Func<double, double>>>();
			AddConverter(UnitTypes.In, UnitTypes.Cm, FromInToCm);
			AddConverter(UnitTypes.Cm, UnitTypes.In, FromCmToIn);
			AddConverter(UnitTypes.Kg, UnitTypes.Lbs, FromKgToLBS);
			AddConverter(UnitTypes.Lbs, UnitTypes.Kg, FromLBSToKg);
		}

		protected static Func<double, double> GetConverter(string convertion)
		{
			var func = ConvertFunctions.Where(x => (x.Item1.ToString() + "->" + x.Item2.ToString()) == convertion).Select(x => x.Item3).FirstOrDefault();
			return func;
		}

		public static double FromInToCm(double val)
		{
			return Math.Round(1 / 0.3937 * val, 2);
		}

		public static double FromCmToIn(double val)
		{
			return Math.Round(0.3937 * val, 2);
		}

		public static double FromKgToLBS(double val)
		{
			return Math.Round(453.5923 * val, 2);
		}

		public static double FromLBSToKg(double val)
		{
			return Math.Round(val / 453.5923, 2);
		}

		public static double Convert(string convertion, double val)
		{
			var func = GetConverter(convertion);
			if (func == null)
				return val;
			return func(val);
		}
	}
}