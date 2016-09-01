using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Bikewale.Utility.Terms
{
    /// <summary>
    /// Created By: Aditi Srivastava on 8th Aug, 2016
    /// Description: To add and remove html tags from terms and conditions
    /// </summary>

    public class TermsHtmlFormatting
    {
        /// <summary>
        /// Created By: Aditi Srivastava on 8th Aug, 2016
        /// Description: To create html list for terms and conditions entered by the user
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        public string MakeHtmlList(string terms)
        {
            StringBuilder termsFormatted = new StringBuilder();
            string[] termsList = terms.Trim().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            if (termsList != null)
            {
                termsFormatted.Append("<ol>");
                foreach (var term in termsList)
                {
                    termsFormatted.Append("<li>");
                    termsFormatted.Append(term);
                    termsFormatted.Append("</li>");

                }
                termsFormatted.Append("</ol>");
                return termsFormatted.ToString();
            }
            else
                return string.Empty;
        }
        /// <summary>
        ///  Created By: Aditi Srivastava on 8th Aug, 2016
        /// Description: To remove html tags from terms and conditions fetched from db
        /// </summary>
        /// <param name="terms"></param>
        /// <returns></returns>
        public string RemoveHtmlListTag(string terms)
        {
            if (terms != null)
            {
                terms = terms.Replace("</li>", "\n</li>");
                return Regex.Replace(terms, @"<(.|\n)*?>", string.Empty);
            }
            else return string.Empty;
        }
    }
}
