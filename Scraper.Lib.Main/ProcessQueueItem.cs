using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class ProcessQueueItem
	{
		public ProcessQueueItem()
		{
			DateAdd = DateTime.Now;
			URL = "";
		}
		public DateTime DateAdd { get; set; }
		public string Name { get; set; }
		public string URL { get; set; }
		public int ItemType { get; set; }
		public bool Processed { get; set; }
		public object Item { get; set; }
		public int NumberOfAttempts { get; set; }
		public bool DataFound { get; set; }
	}
}
