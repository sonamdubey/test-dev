﻿using RabbitMqPublishing.Common;
using System;
using System.Text.RegularExpressions;

namespace Bikewale.Utility
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 1 Sept 2015
    /// </summary>
    public static class FormatDescription
    {
        /// <summary>
        /// Function will return the description with truncated content.
        /// Modified By :- Subodh Jain 23 jan 2017
        /// Summary :- Added white space reqex check
        /// </summary>
        /// <param name="_desc">description which needs to be truncated.</param>
        /// <param name="maxLength">Content lenth is optional. Default value is 170 chars</param>
        /// <returns></returns>
        public static string TruncateDescription(string _desc, int? maxLength = null)
        {
            try
            {
                int descLength = 170;
                if (!string.IsNullOrEmpty(_desc))
                {
                    _desc = Regex.Replace(_desc, @"<[^>]+>", string.Empty);

                    Regex regex = new Regex(@"\W+");
                    _desc = regex.Replace(_desc, " ");
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
                return _desc;
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "FormatDescription.TruncateDescription()");
                return "";
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
