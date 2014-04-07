using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
	public class Executor
	{
		public static TResult ExecuteWithRetry<TResult>(Func<TResult> func, int retries)
		{
			for (int i = 1; i <= retries; i++)
			{
				try
				{
					var r = func();
					return r;
				}
				catch (Exception err)
				{
					if (i == retries)
						throw;
				}
			}

			return default(TResult);
		}

		public static void ExecuteWithRetry(Action act, int retries, int delayOnError)
		{
			for (int i = 1; i <= retries; i++)
			{
				try
				{
					act();
					return;
				}
				catch (Exception err)
				{
					if (i == retries)
						throw;
					if (delayOnError > 0)
						System.Threading.Thread.Sleep(delayOnError);
				}
			}
		}
	}
}