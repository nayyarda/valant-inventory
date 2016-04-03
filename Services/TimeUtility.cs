using System;

namespace ValantData
{
	//Allows unit tests to manipulate DateTime.Now 
	public static class TimeUtility
	{
		private static bool overrideDateTimeNow = false;
		private static DateTime modifiedNow;

		public static DateTime Now
		{
			get{
				if (overrideDateTimeNow) {
					return modifiedNow;
				} else {
					return System.DateTime.Now;
				}
			}
		}

		public static void SetDateTimeNow(DateTime dateTimeNow)
		{
			overrideDateTimeNow = true;
			modifiedNow = dateTimeNow;
		}

		public static void ResetDateTimeNow()
		{
			overrideDateTimeNow = false;
			modifiedNow = DateTime.Now;
		}
	}
}

