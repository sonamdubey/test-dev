﻿using System.Text.RegularExpressions;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 1 Sept 2015
    /// </summary>
    public static class FormatDescription
    {
        /// Function will return the description with truncated content.
        /// </summary>
        /// <param name="_desc">description which needs to be truncated.</param>
        /// <param name="maxLength">Content lenth is optional. Default value is 170 chars</param>
        /// <returns></returns>
        public static string TruncateDescription(string _desc, int? maxLength = null)
        {
            int descLength = 170;
            _desc = Regex.Replace(_desc, @"<[^>]+>", string.Empty);

            if (maxLength.HasValue) { descLength = maxLength.Value; }

            if (_desc.Length < descLength)
                return _desc;
            else
            {
                _desc = _desc.Substring(0, (descLength - 5));
                _desc = _desc.Substring(0, _desc.LastIndexOf(" "));
                return _desc + "...";
            }
        }
        /// <summary>
        /// To remove all the HTML tags from text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SanitizeHtml(string descriptionText)
        {
            return Regex.Replace(descriptionText, @"<[^>]+>", string.Empty);
        }
    }   // class
} // namespace
