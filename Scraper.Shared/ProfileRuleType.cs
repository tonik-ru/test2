using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WheelsScraper
{
	public enum ProfileRuleType
	{
		None = 0,
		[Description("Increase by $")]
		IncreaseByValue = 1,
		[Description("Increase by %")]
		IncreaseByPercent = 2,
		[Description("Decrease by $")]
		DecreaseByValue = 3,
		[Description("Decrease by %")]
		DecreaseByPercent = 4,
		[Description("Make net profit %")]
		NetProfit = 5

	}
}