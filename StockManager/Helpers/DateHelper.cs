using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManager.Helpers
{
    public static class DateHelper
    {
        public static DateTime ConvertDate(this DateTime time, string timeZoneId = "India Standard Time")
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTime(time, tzi);
            //return time.ToTimeZoneTime(tzi);
        }
    }
}