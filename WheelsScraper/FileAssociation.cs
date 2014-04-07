using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text;

namespace WheelsScraper
{
	public class FileAssociation
	{
		public static void Associate(string extension,
			   string progID, string description, string icon, string application)
		{
			
			Registry.CurrentUser.OpenSubKey("Software\\Classes", true).CreateSubKey(extension).SetValue("", progID);
			if (progID != null && progID.Length > 0)
				using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Classes", true).CreateSubKey(progID))
				{
					if (description != null)
						key.SetValue("", description);
					if (icon != null)
						key.CreateSubKey("DefaultIcon").SetValue("", ToShortPathName(icon)+",0");
					if (application != null)
						key.CreateSubKey(@"Shell\Open\Command").SetValue("",
									ToShortPathName(application) + " \"%1\"");
				}
		}

		public static bool IsAssociated(string extension)
		{
			return (Registry.ClassesRoot.OpenSubKey(extension, false) != null);
		}

		[DllImport("Kernel32.dll")]
		private static extern uint GetShortPathName(string lpszLongPath,
			[Out] StringBuilder lpszShortPath, uint cchBuffer);

		private static string ToShortPathName(string longName)
		{
			return longName;
			StringBuilder s = new StringBuilder(1000);
			uint iSize = (uint)s.Capacity;
			uint iRet = GetShortPathName(longName, s, iSize);
			return s.ToString();
		}
	}
}