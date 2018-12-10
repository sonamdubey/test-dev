/*THIS CLASS HOLDS ALL TH EFUNCTION FOR BINDING GRID, FILLING DROPDOWN LIST AND OTHER SORTS OF
COMMON OPERATIONS.
*/

using Carwale.DAL.CoreDAL;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using CarwaleAjax;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Carwale.UI.Common
{
    public class CommonOpn
    {

        #region constants

        private const string s_strImgSourcePath = "<img src='";
        private const string s_strOneDotPng = "0x0/images/ratings/1.png'>";
        private const string s_strZeroDotPng = "0x0/images/ratings/0.png'>";
        private const string s_strHalfDotPng = "0x0/images/ratings/half.png'>";
        private const string c_strNumberCheckPattern = @"^[0-9]+$";
        private const string c_strSingleDigitRegEx = @"^[0-9]$";

        #endregion

        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        private static readonly string s_strRootImagePath;
        private static readonly string s_strImgSourcePathAsRootImagePath;

        static CommonOpn()
        {
            s_strRootImagePath = System.Configuration.ConfigurationManager.AppSettings["CDNHostURL"] ?? "https://imgd.aeplcdn.com/";
            s_strImgSourcePathAsRootImagePath = s_strImgSourcePath + s_strRootImagePath;
        }

        public void BindRepeaterReaderDataSet(DataSet ds, Repeater rpt)
        {
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    rpt.DataSource = ds.Tables[0];
                    rpt.DataBind();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //this function binds the dropdownlist with the datareader
        //takes as input the sql string, dropdownlist name, the text field and the value field        
        public void FillDropDownMySql(DataTable dt, DropDownList drp, string text, string value, string firstText)
        {
            try
            {
                drp.DataSource = dt;
                drp.DataTextField = text;
                drp.DataValueField = value;
                drp.DataBind();
                drp.Items.Insert(0, new ListItem("--Select " + firstText + "--", "0"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void FillDropDownMysql(DbCommand cmd, DropDownList drp, string text, string value, string connection)
        {
            try
            {
                DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, connection);
                drp.DataSource = ds.Tables[0];
                drp.DataTextField = text;
                drp.DataValueField = value;
                drp.DataBind();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Common.FillDropDownMysql");
                objErr.SendMail();
            }
        }

        ///<summary>
        /// This Property will be used for Application Path.
        ///</summary>
        public static string AppPath
        {
            get
            {
                return (string)ConfigurationManager.AppSettings["AppPath"];
            }
        } // AppPath       

        ///<summary>
        /// This Method will be used for resolving relative paths 
        /// in reference to the Absolute Application Path.
        /// <param name="RelativePath">Relative Path of file.</param>
        ///</summary>
        public static string ResolvePhysicalPath(string RelativePath)
        {
            string fullPath = (string)ConfigurationManager.AppSettings["AppPath"] + RelativePath;
            // the next line is to make path compatible with Plesk 6.5.
            // Plesh uses paths after appending non_ssl.
            Page myPage = new Page();
            string makePleskCompatible = myPage.Server.MapPath(fullPath).Replace("\\default\\htdocs\\", "\\carwale.com\\httpdocs\\");
            return makePleskCompatible;

        } // ResolvePath

        // This Function will save image to the images.carwale.com.
        // If the application is running locally, it will save the 
        // file in the requested directory only.
        public static void SaveImage(HtmlInputFile fil, string RelativePath)
        {
            string fullPath = (string)ConfigurationManager.AppSettings["AppPath"] + RelativePath;
            fullPath = fullPath.Replace("images/", string.Empty);
            HttpContext.Current.Trace.Warn("Images Path : " + fullPath);
            // the next line is to make path compatible with Plesk 6.5.
            // Plesh uses paths after appending non_ssl.
            Page myPage = new Page();
            string makePleskCompatible = myPage.Server.MapPath(fullPath).Replace("\\default\\htdocs\\", "\\carwale.com\\subdomains\\images\\httpdocs\\");

            fil.PostedFile.SaveAs(makePleskCompatible);
        } // ResolvePath        

        // Returns the Images Path.
        public static string ImagePath
        {
            get
            {
                string imgPath = string.Empty;

                if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
                {
                    imgPath = "https://img.carwale.com/";

                    // remove the following line as soon as 
                    // images.carwale.com is activated.
                    //imgPath = CommonOpn.AppPath + "img/";
                }
                else
                {
                    imgPath = CommonOpn.AppPath + "images/";
                }

                //HttpContext.Current.Trace.Warn( "Image Path : " + imgPath );

                return imgPath;
            }
        }

        ///<summary>
        /// This Method is used to verify the id of as passed in the url
        /// This matches the string with the regular expression, and also
        /// check its length not to be greater than 9
        /// <param name="input">The input string to be verified.</param>
        ///</summary>
        public static bool CheckId(string input)
        {
            return CheckIfNumber(input, 9);
        } // CheckId

        public static bool IsNumeric(string input)
        {
            return CheckIfNumber(input, 9);
        } // IsNumeric

        private static bool CheckIfNumber(string input, int numLimit)
        {
            try
            {
                //check with the regular expression
                if (Regex.IsMatch(input, c_strNumberCheckPattern))
                {
                    //check its length
                    return (input.Length <= numLimit);
                }
                return false;
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
            }
            return false;
        }

        /********************************************************************************************
        SendMail()
        THIS FUNCTION sends the mail to the dealers with thte email id as passed, in the html format.
        Note that web.config file is case sensitive, 
        The mail id from which the mail is to be sent is get from the key, "localMail".
        ********************************************************************************************/
        public void SendMail(string email, string subject, string body, bool htmlType)
        {
            try
            {
                Email _mail = new Email();
                _mail.SendMail(email, subject, body);
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "SendMail in CommonOpn");
                objErr.SendMail();
            }

        }

        // Converts format of number provided to indian format.
        // e.g. 1000 -> 1,000 and 250000 -> 2,50,000.
        public static string FormatNumeric(string numberToFormat)
        {
            string formatted = string.Empty;
            int breakPoint = 3;

            for (int i = numberToFormat.Length - 1; i >= 0; i--)
            {
                formatted = numberToFormat[i].ToString() + formatted;
                if ((numberToFormat.Length - i) == breakPoint && numberToFormat.Length > breakPoint)
                {
                    HttpContext.Current.Trace.Warn(formatted);
                    formatted = "," + formatted;
                    breakPoint += 2;
                }
            }

            return formatted;
        }

        public static string GetRateImage(double value)
        {
			try
			{
				string oneImg = s_strImgSourcePathAsRootImagePath + s_strOneDotPng;
				string zeroImg = s_strImgSourcePathAsRootImagePath + s_strZeroDotPng;
				string halfImg = s_strImgSourcePathAsRootImagePath + s_strHalfDotPng;

				StringBuilder sb = new StringBuilder();
				int absVal = (int)Math.Floor(value);

				int i;
				for (i = 0; i < absVal; i++)
					sb.Append(oneImg);

				if (value > absVal)
					sb.Append(halfImg);
				else
					i--;

				for (int j = 5; j > i + 1; j--)
					sb.Append(zeroImg);

				return sb.ToString();
			}
			catch (Exception ex)
			{

				Logger.LogException(ex, "GetRateImage()");
				return string.Empty;
			}
            
        }

        public static bool CheckIsDealerFromProfileNo(string profileNo)
        {
            bool retVal = true;

            switch (profileNo.Substring(0, 1).ToUpper())
            {
                case "D":
                    retVal = true;
                    break;

                case "S":
                    retVal = false;
                    break;

                default:
                    retVal = true;
                    break;
            }

            return retVal;
        }

        public static string GetProfileNo(string profileNo)
        {
            if (!string.IsNullOrEmpty(profileNo))
            {
                switch (profileNo.Substring(0, 1).ToUpper())
                {
                    case "D":
                        return profileNo.Substring(1, profileNo.Length - 1);
                    case "S":
                        return profileNo.Substring(1, profileNo.Length - 1);
                    default:
                        return profileNo;
                }
            }
            return string.Empty;
        }

        //this function returns whether it is needed to ask the contact information from
        //the user who is registering
        public bool GetNeedContactInformation()
        {
            bool value = true;  //default false

            if (HttpContext.Current.Request.Cookies["NeedContactInformation"] != null &&
                HttpContext.Current.Request.Cookies["NeedContactInformation"].Value.ToString() != string.Empty)
            {
                value = Convert.ToBoolean(HttpContext.Current.Request.Cookies["NeedContactInformation"].Value);
            }
            else
            {
                value = true;
            }

            return value;
        }

        public static string ConvertToTitleCase(string input)
        {
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            return ti.ToTitleCase(input.ToLower());
        }

        public static string ParseMobileNumber(string input)
        {
            //get only the numeric data
            char[] chars = input.ToCharArray();
            string raw = string.Empty;

            for (int i = 0; i < chars.Length; i++)
            {
                if (Regex.IsMatch(chars[i].ToString(), c_strSingleDigitRegEx))
                {
                    raw += chars[i].ToString();
                }
            }

            //if the number is less than 10
            if (raw.Length < 10)
                return string.Empty;

            //get the last 10 characters if it is greater than 10
            if (raw.Length > 10)
                raw = raw.Substring(raw.Length - 10, 10);


            return raw;
        }

        public static string ParseLandlineNumber(string input)
        {
            //get only the numeric data
            char[] chars = input.ToCharArray();
            string raw = string.Empty;

            for (int i = 0; i < chars.Length; i++)
            {
                if (Regex.IsMatch(chars[i].ToString(), c_strSingleDigitRegEx) || chars[i].ToString() == "-")
                {
                    raw += chars[i].ToString();
                }
            }

            //if the number is less than 10
            if (raw.Length < 6)
                return string.Empty;

            //get the last 10 characters if it is greater than 10
            if (raw.Length > 11)
                raw = raw.Substring(raw.Length - 11, 11);

            return raw;
        }

        public static string GetFormatedValue(string value, int dataTypeId)
        {
            string greyNone = "<span class=\"cc-sprite grey-none\"></span>";
            string greyCross = "<span class=\"cc-sprite grey-cross\"></span>";
            string greyTick = "<span class=\"cc-sprite grey-tick\"></span>";

            if (string.IsNullOrEmpty(value) || value == "-")
            {
                value = greyNone;
            }
            else if (value == "Yes" || (dataTypeId == 2 && value == "1"))
            {
                value = greyTick;
            }
            else if (value == "No" || (dataTypeId == 2 && value == "0"))
            {
                value = greyCross;
            }
            else if (value == "Optional" || (dataTypeId == 2 && value == "2"))
            {
                value = "Optional";
            }
            value = Regex.Replace(value.Trim(), "~.*?~", string.Empty);
            return value;
        }

        public static string GetFormatedValueForCarData(string value, int dataTypeId)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "-";
            }
            else if (dataTypeId == 2 && value == "1")
            {
                value = "Yes";
            }
            else if (dataTypeId == 2 && value == "0")
            {
                value = "No";
            }
            else if (dataTypeId == 2 && value == "2")
            {
                value = "Optional";
            }
            value = Regex.Replace(value.Trim(), "~", string.Empty);
            return value;
        }

    }//class commonopn

    /******************************************************************************/
}//namespace