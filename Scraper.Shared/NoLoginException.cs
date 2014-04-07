using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Scraper.Shared
{
	public class NoLoginException : Exception
	{
		public NoLoginException()
		{
			
		}
		public NoLoginException(string message)
			: base(message)
		{
			
		}
	}
}
