using System;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 26 May 2017
    /// Summary : Class have functions to format the string objects
    /// </summary>
    public static class FormatString
    {
        #region ConvertToCSV extension method
        /// <summary>
        /// Written By : Ashish G. Kamble on 26 may 2017
        /// Summary : Extension method to convert the given array of string into comma separated values (csv)
        /// </summary>
        /// <param name="input">Input string array</param>
        /// <returns>Returns comma separated values</returns>
        public static string ToCSV(this string[] input)
        {
            string csv = string.Empty;

            try
            {
                if (input != null)
                {
                    csv = string.Join(",", input); 
                }
            }
            catch (Exception)
            {
                throw;
            }

            return csv;
        } // convert to csv 
        #endregion

    }   // class
}   // namespace
