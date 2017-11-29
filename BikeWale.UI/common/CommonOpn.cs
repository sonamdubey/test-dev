/*THIS CLASS HOLDS ALL TH EFUNCTION FOR BINDING GRID, FILLING DROPDOWN LIST AND OTHER SORTS OF
COMMON OPERATIONS.
*/

using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Bikewale.Common
{
    /// <summary>
    /// Modified By : Sushil Kumar
    /// DEscription : Removed unused methods
    /// </summary>
    public class CommonOpn
    {

        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
        private char _delimiter = '|';

        /// <summary>
        /// Added By : Sadhana Upadhyay on 15/11/2013
        /// Summary : To display Formatted Ex-showroom price.
        /// </summary>
        /// <param name="minPrice">Minimum Ex-showroom price. eg.-"25000"</param>
        /// <param name="maxPrice">Maximum Ex-showroom price. eg.-"35000"</param>
        /// <returns>Formatted price. eg.-"25,000-35,000" </returns>
        public static string FormatPrice(string minPrice, string maxPrice)
        {
            if ((string.IsNullOrEmpty(minPrice) && string.IsNullOrEmpty(maxPrice)) || (minPrice == "0" && maxPrice == "0"))
            {
                return "N/A";
            }
            else if (minPrice == maxPrice)
            {
                return FormatNumeric(minPrice);
            }
            else
                return FormatNumeric(minPrice) + "-" + FormatNumeric(maxPrice);
        }

        /// <summary>
        /// Created By : Ashish G. Kamble on 31/7/2012
        /// Modified By : Sadhana Upadhyay on 7th may 
        /// Summary : to display 0 price as N/A
        /// </summary>
        /// <param name="price">Ex-showroom price</param>
        /// <returns>formatted ex-showroom price</returns>
        public static string FormatPrice(string price)
        {
            if (price == "" || price == "0")
                return "N/A";
            else
                return FormatNumeric(price);
        }

       

        //this function binds the repeater with the datareader
        //takes as input the sql string and the datagridname
        public void BindRepeaterReader(string sql, Repeater rpt, DbParameter[] param)
        {
            try
            {
                using (IDataReader datareader = MySqlDatabase.SelectQuery(sql, param, ConnectionType.ReadOnly))
                {
                    rpt.DataSource = datareader;
                    rpt.DataBind();
                    datareader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        //this function binds the dropdownlist with the datareader
        //takes as input the sql string, dropdownlist name, the text Budget and the value Budget
        public void FillDropDown(string sql, DropDownList drp, string text, string value)
        {

            try
            {
                using (IDataReader dataReader = MySqlDatabase.SelectQuery(sql, ConnectionType.ReadOnly))
                {
                    if (dataReader != null)
                    {
                        drp.DataSource = dataReader;
                        drp.DataTextField = text;
                        drp.DataValueField = value;
                        drp.DataBind();
                        dataReader.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //this function binds the dropdownlist with the datareader
        //takes as input the sql string, dropdownlist name, the text Budget and the value Budget
        public void FillDropDown(string sql, DropDownList drp, string text, string value, DbParameter[] param)
        {
            try
            {
                using (IDataReader dataReader = MySqlDatabase.SelectQuery(sql, param, ConnectionType.ReadOnly))
                {
                    drp.DataSource = dataReader;
                    drp.DataTextField = text;
                    drp.DataValueField = value;
                    drp.DataBind();
                    dataReader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        /// <summary>
        /// Using in User tracking for SaveActivity 
        /// </summary>
        /// <returns></returns>
        public bool IsSearchEngine()
        {
            bool ret = false;
            try
            {
                if (HttpContext.Current.Request.Browser.Crawler)
                    ret = true;
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "IsSearchEngine");
                
            }
            return ret;
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

        public static string ConvertToTitleCase(string input)
        {
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            return ti.ToTitleCase(input.ToLower());
        }

        //this function returns whether it is needed to ask the contact information from
        //the user who is registering
        public bool GetNeedContactInformation()
        {
            bool value = true;	//default false

            if (HttpContext.Current.Request.Cookies["NeedContactInformation"] != null &&
                HttpContext.Current.Request.Cookies["NeedContactInformation"].Value.ToString() != "")
            {
                value = Convert.ToBoolean(HttpContext.Current.Request.Cookies["NeedContactInformation"].Value);
            }
            else
            {
                value = true;
            }

            return value;
        }

        /********************************************************************************************
        Does exactly what the conventional SendMail function does except the sender's address is newsletter.
        ********************************************************************************************/
        public void SendNewsletterMail(string email, string subject, string body, bool htmlType)
        {
            try
            {
                // make sure we use the local SMTP server
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

                //get the from mail address
                string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

                MailAddress from = new MailAddress(localMail, "BikeWale.com");

                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(email);

                // create mail message object
                MailMessage msg = new MailMessage(from, to);

                // Add bw in the message header.
                msg.Headers.Add("bw", "contact@bikewale.com");

                // set some properties
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                //prepare the subject
                msg.Subject = subject;

                //body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
                msg.Body = body;

                // Mail Server Configuration. Needed for Rediff Hosting.
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

                // Send the e-mail
                client.Send(msg);

                objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
                //ErrorClass.LogError(err,"SendMail in CommonOpn");
                //
            }
        }
        ///<summary>
        /// This Property will be used for Ad Path.
        ///</summary>
        public static string AdPath
        {
            get
            {
                string adPath = "";

                if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("bikewale.com") >= 0)
                    adPath = "https://www.bikewale.com/";
                else
                    adPath = "/";

                return adPath;
            }
        } // AdPath


        ///<summary>
        /// This PopulateWhere will be used for resolving relative paths 
        /// in reference to the Absolute Application Path.
        /// <param name="RelativePath">Relative Path of file.</param>
        ///</summary>
        public static string ResolvePath(string RelativePath)
        {
            return (string)ConfigurationManager.AppSettings["AppPath"] + RelativePath;

        } // ResolvePath

        ///<summary>
        /// This PopulateWhere will be used for resolving relative paths 
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
            fullPath = fullPath.Replace("images/", "");
            HttpContext.Current.Trace.Warn("Images Path : " + fullPath);
            // the next line is to make path compatible with Plesk 6.5.
            // Plesh uses paths after appending non_ssl.
            Page myPage = new Page();
            string makePleskCompatible = myPage.Server.MapPath(fullPath).Replace("\\default\\htdocs\\", "\\carwale.com\\subdomains\\images\\httpdocs\\");

            fil.PostedFile.SaveAs(makePleskCompatible);
        } // ResolvePath

        // This Function returns the path of the image for the images.carwale.com.
        // If the application is running locally, it will return the path of the  
        // file in the requested directory only.
        //NOTE:-- if some changes is done in the below function, then corresponding changes
        //is to be done in the function SaveImage, just above this function
        //for the path of the image.
        public static string ImagePathForSavingImages(string RelativePath)
        {
            string fullPath = (string)ConfigurationManager.AppSettings["AppPath"] + RelativePath;

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
                fullPath = fullPath.Replace("images/", "");

            HttpContext.Current.Trace.Warn("Images Path : " + fullPath);
            // the next line is to make path compatible with Plesk 6.5.
            // Plesh uses paths after appending non_ssl.
            Page myPage = new Page();
            string makePleskCompatible = myPage.Server.MapPath(fullPath).Replace("\\default\\htdocs\\", "\\carwale.com\\subdomains\\images\\httpdocs\\");
            HttpContext.Current.Trace.Warn("makePleskCompatible : " + makePleskCompatible);

            return makePleskCompatible;
        } // ResolvePath

        // Returns the Images Path.
        public static string ImagePath
        {
            get
            {
                string imgPath = "";

                if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
                {
                    imgPath = "https://img.aeplcdn.com/";

                    // remove the following line as soon as 
                    // images.carwale.com is activated.
                    //imgPath = CommonOpn.AppPath + "img/";
                }
                else
                {
                    imgPath = AppPath + "images/";
                }

                //HttpContext.Current.Trace.Warn( "Image Path : " + imgPath );

                return imgPath;
            }
        }

        public static string ResolveImagePath(string imgPath)
        {

            Page myPage = new Page();
            //HttpContext.Current.Trace.Warn( "Original Image Path : " + imgPath);
            //string absPath = myPage.Server.MapPath( "/" );
            string absPath = "";

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("carwale.com") >= 0)
            {
                absPath = myPage.Server.MapPath(imgPath.Replace("https://img.aeplcdn.com/", "/")).ToLower().Replace("\\carwale\\", "\\carwaleimg\\");

                // remove the following line as soon as 
                // images.carwale.com is activated.
                //imgPath = CommonOpn.AppPath + "img/";
            }
            else
            {
                absPath = myPage.Server.MapPath(imgPath);
            }

            //return makePleskCompatible; 

            //HttpContext.Current.Trace.Warn( "Index Of : " + imgPath.IndexOf( "images.carwale.com" ) );

            //absPath = absPath.Replace( "\\default\\htdocs\\","\\carwale.com\\subdomains\\images\\httpdocs\\" )
            //+ imgPath.Replace( "https://images.carwale.com", "" ).Replace( "/", "\\" );

            HttpContext.Current.Trace.Warn("Resolved Image Path : " + absPath);
            return absPath;

        }


        ///<summary>
        /// This PopulateWhere is used to verify the id of as passed in the url
        /// This matches the string with the regular expression, and also
        /// check its length not to be greater than 9
        /// <param name="input">The input string to be verified.</param>
        ///</summary>
        public static bool CheckId(string input)
        {
            bool retVal = false;
            try
            {
                //check with the regular expression
                if (Regex.IsMatch(input, @"^[0-9]+$") == true)
                {
                    //check its length
                    if (input.Length <= 9)
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
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                retVal = false;
            }

            return retVal;
        } // CheckId


        public static bool IsNumeric(string input)
        {
            bool retVal = false;
            try
            {
                //check with the regular expression
                if (Regex.IsMatch(input, @"^[0-9]+$") == true)
                {
                    //check its length
                    if (input.Length <= 9)
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
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                retVal = false;
            }

            return retVal;
        } // IsNumeric

        ///<summary>
        /// This PopulateWhere is used to verify the id of as passed in the url
        /// This matches the string with the regular expression, and also
        /// check its length not to be greater than 15
        /// <param name="input">The input string to be verified.</param>
        ///</summary>
        public static bool CheckLongId(string input)
        {
            bool retVal = false;
            try
            {
                //check with the regular expression
                if (Regex.IsMatch(input, @"^[0-9]+$") == true)
                {
                    //check its length
                    if (input.Length <= 15)
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
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                retVal = false;
            }

            return retVal;
        } // CheckLongId



        /********************************************************************************************
        6()
        THIS FUNCTION sends the mail to the dealers with thte email id as passed in the text format.
        Note that web.config file is case sensitive, 
        The mail id from which the mail is to be sent is get from the key, "localMail".
        ********************************************************************************************/
        public void SendMail(string email, string subject, string body)
        {
            try
            {
                // make sure we use the local SMTP server
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

                //get the from mail address
                string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

                MailAddress from = new MailAddress(localMail, "BikeWale.com");

                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(email);

                // create mail message object
                MailMessage msg = new MailMessage(from, to);

                // Add Reply-to in the message header.
                msg.Headers.Add("Reply-to", "contact@bikewale.com");

                // set some properties
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                //prepare the subject
                msg.Subject = subject;

                //body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
                msg.Body = body;

                // Mail Server Configuration. Needed for Rediff Hosting.
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

                // Send the e-mail
                client.Send(msg);

                objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
                ErrorClass.LogError(err, "SendMail in CommonOpn");
                
            }

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
                // make sure we use the local SMTP server
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

                //get the from mail address
                string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

                MailAddress from = new MailAddress(localMail, "BikeWale.com");

                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(email);

                // create mail message object
                MailMessage msg = new MailMessage(from, to);

                // Add Reply-to in the message header.
                msg.Headers.Add("Reply-to", "contact@bikewale.com");

                // set some properties
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                //prepare the subject
                msg.Subject = subject;

                //body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
                msg.Body = body;

                // Mail Server Configuration. Needed for Rediff Hosting.
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

                // Send the e-mail
                client.Send(msg);

                objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
                ErrorClass.LogError(err, "SendMail in CommonOpn");
                
            }

        }

        /********************************************************************************************
        Does exactly what the conventional SendMail function does except it 
        needs replyTo parameter as well.
        ********************************************************************************************/
        public void SendMail(string email, string subject, string body, bool htmlType, string replyTo)
        {
            try
            {
                // make sure we use the local SMTP server
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

                //get the from mail address
                string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

                MailAddress from = new MailAddress(localMail, "BikeWale.com");

                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(email);

                // create mail message object
                MailMessage msg = new MailMessage(from, to);

                // Add Reply-to in the message header.
                msg.Headers.Add("Reply-to", "contact@bikewale.com");

                // set some properties
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                //prepare the subject
                msg.Subject = subject;

                //body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
                msg.Body = body;

                // Mail Server Configuration. Needed for Rediff Hosting.
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

                // Send the e-mail
                client.Send(msg);

                objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
                ErrorClass.LogError(err, "SendMail in CommonOpn");
                
            }

        }


        //Send mail to multiple clients
        public void SendMail(string email, string subject, string body, int mutipleMail)
        {
            try
            {
                if (email != "")
                {
                    // make sure we use the local SMTP server
                    //SmtpClient client = new SmtpClient("124.153.73.180");//"127.0.0.1";
                    SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);

                    //get the from mail address
                    string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

                    MailAddress from = new MailAddress(localMail, "BikeWale.com");

                    // Set destinations for the e-mail message.
                    MailAddress to = new MailAddress(email);

                    // create mail message object
                    MailMessage msg = new MailMessage(from, to);

                    string[] emailList = email.Split(',');
                    if (emailList.Length > 0)
                    {
                        for (int i = 0; i < emailList.Length; i++)
                        {
                            msg.To.Add(new MailAddress(emailList[i].ToString()));
                        }
                    }

                    // Add Reply-to in the message header.
                    msg.Headers.Add("Reply-to", "contact@bikewale.com");

                    // set some properties
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.High;

                    //prepare the subject
                    msg.Subject = subject;

                    //body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
                    msg.Body = body;

                    objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);

                    // Send the e-mail
                    client.Send(msg);

                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "SendMail in CommonOpn");
                
            }
        }

        //following is the function which returns a portion of a query which excludes the members whose status is in suspended mode
        public string GetExcludedMembers()
        {
            string sql = "";
            sql = " NOT IN ( SELECT Id FROM Dealers With(NoLock) WHERE Status = 1 )";

            return sql;
        }


        /********************************************************************************************
        Does exactly what the conventional SendMail function does except it 
        needs replyTo parameter as well.
        ********************************************************************************************/
        public void SendMail(string email, string subject, string body, bool htmlType, string replyTo, string fromEmail, string addCC, string adBCC)
        {
            try
            {
                // make sure we use the local SMTP server
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

                //get the from mail address
                string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

                MailAddress from = new MailAddress(fromEmail, "");

                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(email);

                // create mail message object
                MailMessage msg = new MailMessage(from, to);

                // Add Reply-to in the message header.
                msg.Headers.Add("Reply-to", replyTo);

                // Check if cc is there or not
                if (addCC != "")
                {
                    MailAddress cc = new MailAddress(addCC);
                    msg.CC.Add(cc);
                }

                // Check if BCC is there or not
                if (adBCC != "")
                {
                    MailAddress bcc = new MailAddress(adBCC);//BCC
                    msg.Bcc.Add(bcc);
                }

                // set some properties
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                //prepare the subject
                msg.Subject = subject;

                //body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            
                msg.Body = body;

                // Mail Server Configuration. Needed for Rediff Hosting.
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
                //msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

                // Send the e-mail
                client.Send(msg);

                objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
                ErrorClass.LogError(err, "SendMail in CommonOpn");
                
            }

        }

        

        // Converts format of number provided to indian format.
        // e.g. 1000 -> 1,000 and 250000 -> 2,50,000.
        public static string FormatNumeric(string numberToFormat)
        {
            string formatted = "";
            int breakPoint = 3;

            for (int i = numberToFormat.Length - 1; i >= 0; i--)
            {
                formatted = numberToFormat[i].ToString() + formatted;
                if ((numberToFormat.Length - i) == breakPoint && numberToFormat.Length > breakPoint)
                {
                    //HttpContext.Current.Trace.Warn(formatted);
                    formatted = "," + formatted;
                    breakPoint += 2;
                }
            }

            return formatted;
        }

        /*  Summary     : Sending SqlException to Trace and Mail
            Author      : Dilip V 25-Jan-2012
            Modifier    :*/
        public static void SqlError(SqlException err)
        {
            HttpContext.Current.Trace.Warn("sql: " + err.Message + err.Source);
            ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            
        }

        /*  Summary     : Sending Exception to Trace and Mail
            Author      : Dilip V 25-Jan-2012
            Modifier    :*/
        public static void ExceptionError(Exception err)
        {
            HttpContext.Current.Trace.Warn(err.Message + err.Source);
            ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            
        }

        // Calculates EMI for any price provided,
        // this calculation is for 84 months and 
        // 80% of total price provided.
        public static string GetEMI(int bikePrice)
        {
            // EMI will be availed for 80% of total bike amount.
            string emi = "";
            try
            {
                double loanAmount = bikePrice * 0.8;
                double rate = 10; // Finance will be made avialabel for 16% rate of interest.
                double months = 84; // 84 Months.

                double interest = rate / (12 * 100);
                //HttpContext.Current.Trace.Warn("interest : " + loanAmount.ToString());
                double finalEmi = (loanAmount * interest * Math.Pow(1 + interest, months)) / (Math.Pow(1 + interest, months) - 1);

                emi = Math.Round(finalEmi, 0).ToString();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return emi;
        }

        // Calculates EMI for any price ,months and %.
        public static string GetEMI(int bikePrice, double months, float percent)
        {
            // EMI will be availed for 80% of total bike amount.
            string emi = "";
            try
            {
                double loanAmount = bikePrice * percent;
                double rate = 10; // Finance will be made avialabel for 10% rate of interest.

                double interest = rate / (12 * 100);

                double finalEmi = (loanAmount * interest * Math.Pow(1 + interest, months)) / (Math.Pow(1 + interest, months) - 1);

                emi = Math.Round(finalEmi, 0).ToString();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return emi;
        }

        // Calculates EMI for any price ,months and %.
        public static string GetEMI(int bikePrice, int months, double percent, double rate)
        {
            // EMI will be availed for 80% of total bike amount.
            string emi = "";
            try
            {
                if (percent > 0) percent = percent / 100;

                double loanAmount = bikePrice * percent;

                double interest = rate / (12 * 100);

                double finalEmi = (loanAmount * interest * Math.Pow(1 + interest, months)) / (Math.Pow(1 + interest, months) - 1);

                emi = Math.Round(finalEmi, 0).ToString();

            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return emi;
        }

        public static string GetRateImage(double value)
        {
            string oneImg = "<img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/1.gif\">";
            string zeroImg = "<img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/0.gif\">";
            string halfImg = "<img src=\"https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/half.gif\">";

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
        //this function expires the cookie for the needing of the contact information
        public void ExpireNeedContactInformation()
        {
            HttpContext.Current.Response.Cookies["NeedContactInformation"].Expires = DateTime.Now.AddYears(-1);
        }

        //this function returns the sellinquiryId
        public static string CustomerLoginEmail
        {
            get
            {
                string value = "";	//default false

                if (HttpContext.Current.Request.Cookies["CustomerLoginEmail"] != null &&
                    HttpContext.Current.Request.Cookies["CustomerLoginEmail"].Value.ToString() != "")
                {
                    value = HttpContext.Current.Request.Cookies["CustomerLoginEmail"].Value.ToString();
                }
                else
                {
                    value = "";
                }

                return value;
            }
            set
            {
                //set the cookie
                HttpCookie objCookie;
                objCookie = new HttpCookie("CustomerLoginEmail");
                objCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(objCookie);
            }
        }


        public static string ParseMobileNumber(string input)
        {
            //get only the numeric data
            char[] chars = input.ToCharArray();
            string raw = "";

            for (int i = 0; i < chars.Length; i++)
            {
                if (Regex.IsMatch(chars[i].ToString(), @"^[0-9]$") == true)
                {
                    raw += chars[i].ToString();
                }
            }

            //if the number is less than 10
            if (raw.Length < 10)
                return "";

            //get the last 10 characters if it is greater than 10
            if (raw.Length > 10)
                raw = raw.Substring(raw.Length - 10, 10);


            return raw;
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
            string retVal = "";

            switch (profileNo.Substring(0, 1).ToUpper())
            {
                case "D":
                    retVal = profileNo.Substring(1, profileNo.Length - 1);
                    break;

                case "S":
                    retVal = profileNo.Substring(1, profileNo.Length - 1);
                    break;

                default:
                    retVal = profileNo;
                    break;
            }

            return retVal;
        }

        //Check Whether this profileno bike belongs to mumbai or not 
        //Used in SMSCommon and Mails Pages
        //Purpose : Append LOAN message for mumbai bikes
        //public static bool CheckForMumbai(string bikeProfileId)
        //{
        //    bool isFromMumbai = false;

        //    if (CommonOpn.CheckIsDealerFromProfileNo(bikeProfileId))	// if dealer
        //    {
        //        if (GetBikeCity(CommonOpn.GetProfileNo(bikeProfileId), true))
        //        {
        //            isFromMumbai = true;
        //        }
        //    }
        //    else // if individual
        //    {
        //        if (GetBikeCity(CommonOpn.GetProfileNo(bikeProfileId), false))
        //        {
        //            isFromMumbai = true;
        //        }
        //    }
        //    return isFromMumbai;
        //}

        //Supportive function of above function
        //        static bool GetBikeCity(string bikeProfileNo, bool isDealer)
        //        {
        //            string sql = "";
        //            bool isCity = false;

        //            //dealer
        //            if (isDealer)
        //            {
        //                sql = @"select si.id from sellinquiries as si, dealers as d  
        //                    where d.dealerid = si.dealerid and d.cityid in(1,6,8,13,40)
        //                    and si.id = @bikeprofileno";
        //            }
        //            //Individual
        //            else
        //            {
        //                sql = "select id from classifiedindividualsellinquiries  where cityid in(1,6,8,13,40) and id = @bikeprofileno";
        //            }
        //            try
        //            {

        //                using (DbCommand cmd = DbFactory.GetDBCommand(sql))
        //                {
        //                    cmd.Parameters.Add(DbFactory.GetDbParam("@bikeprofileno", DbType.Int32, bikeProfileNo));

        //                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
        //                    {
        //                        if (dr != null && dr.Read())
        //                        {
        //                            isCity = true;
        //                            dr.Close();
        //                        }
        //                    }
        //                }

        //            }
        //            catch (Exception err)
        //            {
        //                HttpContext.Current.Trace.Warn("Common.SMSCommon : " + err.Message);
        //                ErrorClass.LogError(err, "Common.GetBikeCity");
        //                
        //            }

        //            return isCity;
        //        }

        //this function returns the city id as selected by the user and is as set in
        //the cookie
        public string GetCityId()
        {
            string cityId = "1";	//default 1 for mumbai

            if (HttpContext.Current.Request.Cookies["SelectedCity"] != null &&
                HttpContext.Current.Request.Cookies["SelectedCity"].Value.ToString() != "")
            {
                cityId = HttpContext.Current.Request.Cookies["SelectedCity"].Value.ToString();
            }
            else
            {
                cityId = "1";	//1 for mumbai
            }

            return cityId;
        }

        //this function returns the city id as selected by the user and is as set in
        //the cookie
        public static void SetCityId(string cityId)
        {
            //set the cookie
            HttpCookie objCookie;
            objCookie = new HttpCookie("SelectedCity");
            objCookie.Value = cityId;
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        //this function returns the city id as selected by the user and is as set in
        //the cookie
        public string GetCityDistance()
        {
            string cityDist = "0";	//default 1 for mumbai

            if (HttpContext.Current.Request.Cookies["SelectedCityDistance"] != null &&
                HttpContext.Current.Request.Cookies["SelectedCityDistance"].Value.ToString() != "")
            {
                cityDist = HttpContext.Current.Request.Cookies["SelectedCityDistance"].Value.ToString();
            }
            else
            {
                cityDist = "50";	//default 50 km area
            }

            return cityDist;
        }

        //this function returns the city id as selected by the user and is as set in
        //the cookie
        public static void SetCityDistance(string cityDist)
        {
            //set the cookie
            HttpCookie objCookie;
            objCookie = new HttpCookie("SelectedCityDistance");
            objCookie.Value = cityDist;
            HttpContext.Current.Response.Cookies.Add(objCookie);
        }

        private static double lattSecPerKm = 32.57940665;
        //private static double lattKmPerSec = 0.030694236;

        private static double longSecPerKm = 34.63696611;

        //  private static double longKmPerSec = 0.028870889;

        //private static double longKmPerSec = 0.028870889;


        public static double GetLattitude(int diffKm)
        {
            return lattSecPerKm * diffKm;
        }

        public static double GetLongitude(int diffKm)
        {
            return longSecPerKm * diffKm;
        }

        // Added By : Ashish G. Kamble on 12/10/2012
        //the content is in the form of name value pairs separated with |
        //split the content and then add them to the dropdownlist, and 
        //make the selected item true
        public void UpdateContents(DropDownList drp, string content, string selectedValue)
        {
            UpdateContents(drp, content, selectedValue, "Any");
        }

        // Added By : Ashish G. Kamble on 12/10/2012
        //selectName is the value which is to be at the top of the dropdown
        public void UpdateContents(DropDownList drp, string content, string selectedValue, string selectName)
        {
            drp.Items.Clear();
            drp.Enabled = true;

            //add Any at the top
            drp.Items.Add(new ListItem(selectName, "0"));

            if (content != "")
            {
                string[] listItems = content.Split(_delimiter);

                for (int i = 0; i < listItems.Length - 1; i++)
                {
                    drp.Items.Add(new ListItem(listItems[i], listItems[i + 1]));
                    i++;
                }

                ListItem selectedListItem = drp.Items.FindByValue(selectedValue);
                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        /// <summary>
        ///     Written By : Ashish G. Kamble on 6 Nov 2012
        ///     Summary : Function will encode the string into base 64 string
        /// </summary>
        /// <param name="toEncode">string to encode</param>
        /// <returns>returns 64 character string</returns>
        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        /// <summary>
        ///     Written By : Ashish G.kamble on 6 Nov 2012
        ///     Summary : Function will decode the encoded string into the original string
        /// </summary>
        /// <param name="encodedData">String which is decoded using EncodeTo64 method</param>
        /// <returns>Returns the decoded string</returns>
        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 1st April 2014
        /// Summary : Function to get Client IP Address
        /// Modified by :   Sumit Kate on 11 May 2016
        /// Summary :   handle all possible server variables to get the client ip
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            // Commented by sadhana Upadhyay
            // Capturing CarWale server Address


            //string ip, trueIP = string.Empty;

            //ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //HttpContext.Current.Trace.Warn("ClientIp", ip);

            //if (!string.IsNullOrEmpty(ip))
            //{
            //    string[] ipRange = ip.Split(',');
            //    trueIP = ipRange[ipRange.Length - 1];
            //}
            //else
            //{
            //    trueIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //}

            //IPAddress result;
            //if (!IPAddress.TryParse(trueIP, out result))
            //    result = IPAddress.None;

            //return result.ToString();

            string[] serVars = { "HTTP_CLIENT_IP", "HTTP_X_FORWARDED_FOR", "HTTP_X_FORWARDED", "HTTP_X_CLUSTER_CLIENT_IP", "HTTP_FORWARDED_FOR", "HTTP_FORWARDED", "REMOTE_ADDR" };
            string clientIp = string.Empty;
            foreach (string serverVariable in serVars)
            {
                clientIp = HttpContext.Current.Request.ServerVariables[serverVariable] == null ? DBNull.Value.ToString() : HttpContext.Current.Request.ServerVariables[serverVariable];
                if (!String.IsNullOrEmpty(clientIp))
                {
                    if (serverVariable == "HTTP_X_FORWARDED_FOR")
                    {
                        if (!string.IsNullOrEmpty(clientIp))
                        {
                            string[] ipRange = clientIp.Split(',');
                            if (ipRange != null)
                            {
                                clientIp = ipRange[ipRange.Length - 1];
                            }
                        }
                    }
                    break;
                }
            }
            //string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] == null ? DBNull.Value.ToString() : HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];

            return clientIp;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 20 May 2014
        /// Summary    : PopulateWhere to get formated string from date e.g. 1 day ago
        /// </summary>
        /// <param name="_displayDate"></param>
        /// <returns></returns>
        public static string GetDisplayDate(string _displayDate)
        {
            string retVal = "";
            TimeSpan tsDiff = DateTime.Now.Subtract(Convert.ToDateTime(_displayDate));

            if (tsDiff.Days > 0)
                retVal = tsDiff.Days.ToString() + " days ago";
            else if (tsDiff.Hours > 0)
                retVal = tsDiff.Hours.ToString() + " hours ago";
            else if (tsDiff.Minutes > 0)
                retVal = tsDiff.Minutes.ToString() + " minutes ago";
            else if (tsDiff.Seconds > 0)
                retVal = tsDiff.Seconds.ToString() + " seconds ago";

            if (tsDiff.Days > 360)
                retVal = Convert.ToString(tsDiff.Days / 360) + " years ago";
            else if (tsDiff.Days > 30)
                retVal = Convert.ToString(tsDiff.Days / 30) + " months ago";

            return retVal;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 12 Aug 2014
        /// Summary    : PopulateWhere to permanent redirect(301) url to new path  
        /// </summary>
        /// <param name="newPath"></param>
        public static void RedirectPermanent(string newPath)
        {
            HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Status = "301 Moved Permanently";
            HttpContext.Current.Response.StatusCode = 301;
            HttpContext.Current.Response.AddHeader("Location", newPath);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : PopulateWhere to get catagory list in string format
        /// </summary>
        /// <returns></returns>
        public static string GetContentTypesString<T>(List<T> contentList)
        {

            string _contentTypes = string.Empty;
            ushort _contentType = 0;

            foreach (var item in contentList)
            {
                _contentType = Convert.ToUInt16(item);
                _contentTypes += _contentType.ToString() + ',';
            }

            _contentTypes = _contentTypes.Remove(_contentTypes.LastIndexOf(','));

            return _contentTypes;

        } //End of GetContentTypes


        /// <summary>
        /// Created By : Sadhana Upadhyay on 5 Nov 2014
        /// Summary : To get upcoming date 
        /// </summary>
        /// <param name="noOfDays"></param>
        /// <returns></returns>
        public static DateTime GetValidDate(int noOfDays)
        {
            DateTime dt = DateTime.Now.AddDays(noOfDays);
            return dt;
        }

    }//End Class 
}//namespace