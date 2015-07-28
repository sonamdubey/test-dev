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

namespace Bikewale.Common
{	
	public class Validations
	{
		public static bool IsValidMobile(string mobile)
		{
			bool retVal = false;
			try
			{
				//check with the regular expression
				if(Regex.IsMatch(mobile, @"^[0-9]+$") == true)
				{
					//check its length
					if(mobile.Length == 10)
					{
						retVal = true;
					}
					else
					{
						retVal = false;
					}
				}
				else
				{
					retVal = false;
				}
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				retVal = false;
			}
			
			return retVal;
		}
		
		public static bool IsValidEmail(string email)
		{
			bool isValid = true;
			
			if( email == "" )
				isValid = false;
				
			if( !Regex.IsMatch(email, @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,6}$") )
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
	}//class
}//namespace
