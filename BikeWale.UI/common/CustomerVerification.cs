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
			
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			CommonOpn op = new CommonOpn();
						
			string conStr = db.GetConString();
			
			con = new SqlConnection( conStr );
            HttpContext.Current.Trace.Warn("IsMobileVerified method" + "," + cvId);
			try
			{
                
				cmd = new SqlCommand("CV_VerifyMobile", con);
				cmd.CommandType = CommandType.StoredProcedure;
				
				prm = cmd.Parameters.Add("@EmailId", SqlDbType.VarChar, 100);
				prm.Value = eMail.ToLower();
				
				prm = cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50);
				prm.Value = mobile;
				
				prm = cmd.Parameters.Add("@CVID", SqlDbType.BigInt);
				prm.Value = cvId;
				
				prm = cmd.Parameters.Add("@CWICode", SqlDbType.VarChar, 50);
				prm.Value = cwiCode;
				
				prm = cmd.Parameters.Add("@CUICode", SqlDbType.VarChar, 50);
				prm.Value = cuiCode;
							
				prm = cmd.Parameters.Add("@EntryDateTime", SqlDbType.DateTime);
				prm.Value = DateTime.Now;
				
				prm = cmd.Parameters.Add("@IsMobileVer", SqlDbType.Bit);
				prm.Direction = ParameterDirection.Output;
								
				prm = cmd.Parameters.Add("@NewCVID", SqlDbType.BigInt);
				prm.Direction = ParameterDirection.Output;
                Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);					
				con.Open();
				//run the command
    			cmd.ExecuteNonQuery();
			
				isMobVer = Convert.ToBoolean(cmd.Parameters["@IsMobileVer"].Value);
				HttpContext.Current.Trace.Warn("customerverification isMobVer : " + isMobVer);
                HttpContext.Current.Trace.Warn("customerverification NewCVID : " + cmd.Parameters["@NewCVID"].Value.ToString());
				if(isMobVer == false)
				{
                    HttpContext.Current.Trace.Warn("isMobVer : ", isMobVer.ToString());
					//check whether a pending verification is already there for this customer
					if(cvId == "-1") //for the first time, hence add it into the database and also a fresh xml file
					{
						cvId = cmd.Parameters["@NewCVID"].Value.ToString();
						CustomerVerification.CVId = cvId;

                        HttpContext.Current.Trace.Warn("customerverification cvId" + cvId);
                        HttpContext.Current.Trace.Warn("customerverification isMobVer" + isMobVer);
						
						//send sms to the customer
						SMSTypes st = new SMSTypes();
						st.SMSMobileVerification(mobile, name, cwiCode, HttpContext.Current.Request.ServerVariables["URL"]);
					}
				}
			}
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn("CustomerVerification.IsMobileVerified sql : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "CustomerVerification.IsMobileVerified sql ");
                objErr.SendMail();
            } // catch Exception
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn("CustomerVerification.IsMobileVerified : " + err.Message);
				ErrorClass objErr = new ErrorClass(err,"CustomerVerification.IsMobileVerified");
				objErr.SendMail();
			} // catch Exception
			finally
			{
				//close the connection	
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
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
			
			SqlConnection con;
			SqlCommand cmd;
			SqlParameter prm;
			Database db = new Database();
			CommonOpn op = new CommonOpn();
						
			string conStr = db.GetConString();
			
			con = new SqlConnection( conStr );
									
			try
			{
				cmd = new SqlCommand("CV_CheckVerification", con);
				cmd.CommandType = CommandType.StoredProcedure;
			
				prm = cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50);
				prm.Value = mobile;
				
				prm = cmd.Parameters.Add("@CWICode", SqlDbType.VarChar, 50);
				prm.Value = cwiCode;
				
				prm = cmd.Parameters.Add("@CUICode", SqlDbType.VarChar, 50);
				prm.Value = cuiCode;
				
				prm = cmd.Parameters.Add("@IsVerified", SqlDbType.Bit);
				prm.Direction = ParameterDirection.Output;
                Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
				con.Open();
				//run the command
    			cmd.ExecuteNonQuery();
				
				verified = Convert.ToBoolean(cmd.Parameters["@IsVerified"].Value);
				
				HttpContext.Current.Trace.Warn("CustomerVerification.verified : " + verified.ToString());
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn("CustomerVerification.SavePendingListData : " + err.Message);
				ErrorClass objErr = new ErrorClass(err,"CustomerVerification.SavePendingListData");
				objErr.SendMail();
			} // catch Exception
			finally
			{
				//close the connection	
			    if(con.State == ConnectionState.Open)
				{
					con.Close();
				}
			}
			
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