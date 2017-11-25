using System;
using System.Text.RegularExpressions;
using System.Web;

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
        /// Modified By :- Sangram Nandkhile 02 Aug 2017
        /// Summary :-  Removed \w regex operatior as it was removing commas and fullstop
        /// </summary>
        /// <param name="text">description which needs to be truncated.</param>
        /// <param name="maxLength">Content lenth is optional. Default value is 170 chars</param>
        /// <returns></returns>
        public static string TruncateDescription(string text, int? maxLength = null)
        {
            try
            {
                int descLength = 170;
                if (!string.IsNullOrEmpty(text))
                {
                    text = HttpUtility.HtmlDecode(text);
                    text = Regex.Replace(text, @"<[^>]+>", string.Empty);

                    //Regex regex = new Regex(@"\W+");
                    //_desc = regex.Replace(_desc, " ");
                    if (maxLength.HasValue) { descLength = maxLength.Value; }

                    if (text.Length < descLength)
                        return text;
                    else
                    {
                        text = text.Substring(0, (descLength - 5));
                        text = text.Substring(0, text.LastIndexOf(" "));
                        return text + "...";
                    }
                }
                return text;
            }
            catch (Exception ex)
            {
                return string.Empty;
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
