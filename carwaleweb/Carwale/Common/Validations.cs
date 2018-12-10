/*
	This class will use to bind controls like filling makes, states
	Written by: Satish Sharma On Jan 21, 2008 12:28 PM
*/

using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Carwale.UI.Common
{
    public class Validations
    {
        public static bool IsValidEmail(string email)
        {
            bool isValid = true;

            if (email == "")
                isValid = false;

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$"))
                isValid = false;

            return isValid;
        }

        /// <summary>
        /// To validate profile id
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns>boolean</returns>
        public static bool IsValidProfileId(string profileId)
        {
            bool isValid = true;

            if (profileId == "")
                isValid = false;

            if (!Regex.IsMatch(profileId, @"^((d|D)|(s|S))[0-9]*$"))
                isValid = false;

            return isValid;
        }
		public static bool IsValidProsCons(string pros)
		{
			if (string.IsNullOrEmpty(pros))
			{
				return false;
			}
			else
			{
				pros = pros.ToLower().Trim();
				return !(pros.Equals("na") || pros.Equals("n/a") || pros.Equals("n\a"));
			}
		}
	}//class
}//namespace
