// This is for the customer verification
using System;
using System.Web;
using System.Configuration;
using System.Web.Mail;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Security.Cryptography;
using System.Xml;
using Bikewale.Common;
using System.Data.Common;
using Bikewale.Notifications.CoreDAL;

namespace Bikewale.CV
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 23/8/2012
    ///     Class contains methods for customer verification process
    /// </summary>
	public class CustomerVerification
	{
		public string CUICode = "";
		
		public bool IsMobileVerified(string name, string eMail, string mobile)
		{
			bool isMobVer = false;
			string cvId = CVId;
			
			mobile = CommonOpn.ParseMobileNumber(mobile);
			Random rnd = new Random();
			Random rnd1 = new Random(rnd.Next());
			
			//get a random 5 digit number for Bikewale initiated code and the customer initiated code
			string cwiCode = GetRandomCode(rnd, 5);
			string cuiCode = GetRandomCode(rnd1, 5);
			
			CUICode = cuiCode;

			CommonOpn op = new CommonOpn();

            HttpContext.Current.Trace.Warn("IsMobileVerified method" + "," + cvId);
			try
			{ 
                using (DbCommand cmd = DbFactory.GetDBCommand("cv_verifymobile"))
                {                     
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailid", DbType.String, 100, eMail.ToLower()));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobileno", DbType.String, 50, mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cwicode", DbType.String, 50, cwiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cuicode", DbType.String, 50, cuiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cvid", DbType.Int64, cvId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_newcvid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismobilever", DbType.Boolean, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_entrydatetime", DbType.DateTime, DateTime.Now));  

                Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);					
                    //run the command
                    MySqlDatabase.ExecuteNonQuery(cmd);

                    isMobVer = Convert.ToBoolean(cmd.Parameters["par_ismobilever"].Value);

                    if (isMobVer == false)
                    {
                        HttpContext.Current.Trace.Warn("isMobVer : ", isMobVer.ToString());
                        //check whether a pending verification is already there for this customer
                        if (cvId == "-1") //for the first time, hence add it into the database and also a fresh xml file
                        {
                            cvId = cmd.Parameters["par_newcvid"].Value.ToString();
                            CustomerVerification.CVId = cvId;

                            //send sms to the customer
                            SMSTypes st = new SMSTypes();
                            st.SMSMobileVerification(mobile, name, cwiCode, HttpContext.Current.Request.ServerVariables["URL"]);
                        }
                    } 
                }
			}
            catch (SqlException err)
            { 
                ErrorClass objErr = new ErrorClass(err, "CustomerVerification.IsMobileVerified sql ");
                objErr.SendMail();
            } // catch Exception
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,"CustomerVerification.IsMobileVerified");
				objErr.SendMail();
			} // catch Exception

            return isMobVer;
		}
		
		//for the verification
		public bool CheckVerification(string mobile, string cwiCode, string cuiCode)
		{
			bool verified = false;
			//search from the database with the mobile and the code. if it matches then make an entry 
			//in the emailmobile pair and return true, else return false. also delete the record 
			//for this mobile number from the database
			//in case this function is called from the user interface then pass cwiCode else if it is from sms
			//received from the user, pass cuiCode
									
			try
			{
                using (DbCommand cmd = DbFactory.GetDBCommand("cv_checkverification"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobileno", DbType.String, 50, mobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cwicode", DbType.String, 50, cwiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cuicode", DbType.String, 50, cuiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                    verified = Convert.ToBoolean(cmd.Parameters["par_isverified"].Value); 
                Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);

                }
				

			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,"CustomerVerification.SavePendingListData");
				objErr.SendMail();
			} // catch Exception

			return verified;
		}
		
		//this function generates a random 5 digit code where all the characters are numeric
		string GetRandomCode(Random rnd, int length)
		{
			string charPool = "1234567890098765432112345678900987654321";
			StringBuilder rs = new StringBuilder();
						
			while (length-- > 0)
				rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);
			
			return rs.ToString();
		}
		
		public static string CVId
		{
			get
			{
				string val = "";	
			
				if(HttpContext.Current.Request.Cookies["CVId"] != null &&
					HttpContext.Current.Request.Cookies["CVId"].Value.ToString() != "")
				{
					val = HttpContext.Current.Request.Cookies["CVId"].Value.ToString();
				}	
				else
					val = "-1";	
				
				return val;
			}
			set
			{
				HttpCookie objCookie;
				objCookie = new HttpCookie("CVId");
				objCookie.Value = value;
				objCookie.Expires = DateTime.Now.AddMinutes(30);	//expire the cookie in 30 minutes
				HttpContext.Current.Response.Cookies.Add(objCookie);
			}
		}
		
		public static void ExpireCookie()
		{
			HttpContext.Current.Response.Cookies["CVId"].Expires 	= DateTime.Now.AddYears( -1 );
		}

	}//class	
}//namespace