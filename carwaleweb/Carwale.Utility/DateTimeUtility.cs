using System;
using System.Globalization;

namespace Carwale.Utility
{
    public static class DateTimeUtility
    {
        private const short _launchedDateInterval = 7;
        public static int GetDateDiffInMonths(DateTime startDateTime)
        {
            DateTime dateTimeNow = DateTime.Now;
            return ((dateTimeNow.Year - startDateTime.Year) * 12) + dateTimeNow.Month - startDateTime.Month;
        }
        public static bool ShowJustLaunchedLabel(DateTime launchDate, DateTime presentDate)
        {
            var dateDiff = (presentDate - launchDate).TotalDays;
            if (launchDate.Equals(default(DateTime)) || dateDiff < 0)
            {
                return false;
            }
            return dateDiff <= _launchedDateInterval;
        }

        public static bool ShowReplaceModel(DateTime? launchDate, bool isUpcoming, short replaceModelDateInterval)
        {
            if (launchDate == null || launchDate.Equals(default(DateTime)))
            {
                return false;
            }
            var dateDiff = isUpcoming ? ((DateTime)launchDate - DateTime.Now).TotalDays : (DateTime.Now - (DateTime)launchDate).TotalDays;
            return dateDiff <= replaceModelDateInterval && dateDiff >= 0;
        }

        public static bool CheckTollFreeCallTime(DateTime dt)
        {
            return (dt.DayOfWeek.ToString().ToLower() != "sunday" && dt.DayOfWeek.ToString().ToLower() != "saturday" && dt.Hour >= 9 && dt.Hour < 20);
        }
        public static bool ShowLaunchLabel(DateTime launchDate, DateTime presentDate)
        {
            var dateDiff = Math.Abs((presentDate - launchDate).TotalDays);
            if (launchDate.Equals(default(DateTime)) || dateDiff < 0)
            {
                return false;
            }
            return dateDiff <= _launchedDateInterval;
        }
        public static bool IsDateWithIn31Days(DateTime date, DateTime presentDate)
        {
            int dateDiff = (int)((date - presentDate).TotalDays);
            return dateDiff >= 0 && dateDiff < 31;
        }

        public static DateTime LastDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }
        
        public static TimeSpan GetTimeSpan(int hourOfDay)
        {
            int currHour = DateTime.Now.Hour;
            int diffHour;
            int diffMinute = 60 - DateTime.Now.Minute;
            diffHour = (currHour < hourOfDay) ? (hourOfDay - currHour - 1) : (23 + hourOfDay - currHour);
            return new TimeSpan(diffHour, diffMinute, 0);
        }
    }
}
