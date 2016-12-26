﻿using System;
using System.Globalization;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 1 Sept 2015
    /// Summary : Class have functions to format the date
    /// </summary>
    public static class FormatDate
    {
        /// <summary>
        /// Created By : Ashish G. Kamble on 1 Sept 2015
        /// Summary : function will return date in the DD/MM/YYYY format
        /// </summary>
        public static string GetDDMMYYYY(string _date)
        {
            if (String.IsNullOrEmpty(_date))
                return string.Empty;
            else
                return Convert.ToDateTime(_date).ToString("dd MMM yyyy");
        }


        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Created By : Ashish G. Kamble on 28 Sept 2015
        /// Summary : Function to format the date. Date will be formatted as years ago, months ago, days ago, hours ago, minutes ago, seconds ago.
        /// </summary>
        /// <param name="displayDate">Date in the string format.</param>
        /// <returns>Returns formatted date.</returns>
        public static string GetDaysAgo(string displayDate)
        {
            string retVal = "";

            TimeSpan timeSpan = DateTime.Now.Subtract(Convert.ToDateTime(displayDate));

            retVal = FormateDate(timeSpan);

            return retVal;
        }

        /// <summary>
        /// Created By : Ashish G. Kamble on 28 Sept 2015
        /// Summary : Function to format the date. Date will be formatted as years ago, months ago, days ago, hours ago, minutes ago, seconds ago.
        /// </summary>
        /// <param name="displayDate">Date in the datetime format.</param>
        /// <returns>Returns formatted date.</returns>
        public static string GetDaysAgo(DateTime displayDate)
        {
            string retVal = "";

            TimeSpan timeSpan = DateTime.Now.Subtract(displayDate);

            retVal = FormateDate(timeSpan);

            return retVal;
        }

        /// <summary>
        /// Created By : Ashish G. Kamble on 28 Sept 2015
        /// Summary : Function to formate the date. Date will be formatted as years ago, months ago, days ago, hours ago, minutes ago, seconds ago.
        /// </summary>
        /// <param name="timeSpan">Difference between todays date and date which needs formatting.</param>
        /// <returns></returns>
        private static string FormateDate(TimeSpan timeSpan)
        {
            string retVal = string.Empty;

            if (timeSpan.Days > 0)
            {
                retVal = timeSpan.Days.ToString();

                retVal += retVal == "1" ? " day ago" : " days ago";
            }
            else if (timeSpan.Hours > 0)
            {
                retVal = timeSpan.Hours.ToString();

                retVal += retVal == "1" ? " hour ago" : " hours ago";
            }
            else if (timeSpan.Minutes > 0)
            {
                retVal = timeSpan.Minutes.ToString();
                retVal += retVal == "1" ? " minute ago" : " minutes ago";
            }
            else if (timeSpan.Seconds > 0)
            {
                retVal = timeSpan.Seconds.ToString();

                retVal += retVal == "1" ? " second ago" : " seconds ago";

            }

            if (timeSpan.Days > 360)
            {
                retVal = Convert.ToString(timeSpan.Days / 360);

                retVal += retVal == "1" ? " year ago" : " years ago";

            }
            else if (timeSpan.Days > 30)
            {
                retVal = Convert.ToString(timeSpan.Days / 30);

                retVal += retVal == "1" ? " month ago" : " months ago";
            }

            return retVal;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 04 Nov. 2015.
        /// </summary>
        /// <param name="date">string of date in proper formate.</param>
        /// <param name="formate">Formate in which date needed to convert.</param>
        /// <returns>formeted date.</returns>
        public static string GetFormatDate(string date, string format)
        {
            DateTime ouput;
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }
            CultureInfo culture;
            DateTimeStyles styles = DateTimeStyles.None;
            // Parse a date and time with no styles.
            culture = CultureInfo.CreateSpecificCulture("en-US");
            return DateTime.TryParse(date, culture, styles, out ouput) ? ouput.ToString(format) : "";
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 26 Dec 2016
        /// Description :   Returns Current Month Name
        /// </summary>
        /// <returns></returns>
        public static string PreviousMonth()
        {
            return DateTime.Now.AddMonths(-1).ToString("MMMM");
        }

    }
}
