using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public class WareInfo
	{
		public string PartNumber { get; set; }
		public string ManufacturerNumber { get; set; }
		public string Name { get; set; }
		public string Brand { get; set; }
		public double MSRP { get; set; }
		public double Cost { get; set; }
		public string Jobber { get; set; }
		public int Quantity { get; set; }
		public string URL { get; set; }
		public bool Processed { get; set; }
		public string ImageUrl { get; set; }
		public string Description { get; set; }

		public string LocalImageFile { get; set; }

		public int QuantityLocal { get; set; }
		public int QuantityGlobal { get; set; }

		public bool ImageProcessed { get; set; }
	}
}
