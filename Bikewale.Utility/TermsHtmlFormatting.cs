﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            List<string> termsList = new List<string>(Regex.Split(terms, Environment.NewLine));
           
            string termsConditions;
            if (termsList != null)
            {
                termsFormatted.Append("<ol class='termsList'>");
                foreach (var term in termsList)
                {
                    termsFormatted.Append("<li>");
                    termsFormatted.Append(term);
                    termsFormatted.Append("</li>");

                }
                termsFormatted.Append("</ol>");
                termsConditions = termsFormatted.ToString();
            }
            else
                termsConditions = "";

            return termsConditions;
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
           else return ("");
       }



        }

   

    }
