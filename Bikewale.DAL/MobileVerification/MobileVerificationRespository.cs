using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Interfaces.MobileVerification;

namespace Bikewale.DAL.MobileVerification
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 18 Apr 2014
    /// Summary : Class have functions related to entire mobile verification process.
    /// </summary>
    public class MobileVerificationRepository : IMobileVerificationRepository
    {
        /// <summary>
        /// Summary : Function will return true or false based on given mobile email pair exist in the database.
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="emailId"></param>
        /// <returns>Return if mobile is verified or not.</returns>
        public bool IsMobileVerified(string mobileNo, string emailId)
        {
            bool isVerified = false;
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "CV_IsVerifiedMobile";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@EmailId", SqlDbType.VarChar, 100).Value = emailId;
                        cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = mobileNo;
                        cmd.Parameters.Add("@IsMobileVer", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        con.Open();
                        cmd.ExecuteNonQuery();

                        isVerified = Convert.ToBoolean(cmd.Parameters["@IsMobileVer"].Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return isVerified;
        }

        /// <summary>
        /// To get number of attempts made for otp within specified time
        /// Internally it also checks whether mobile number is verfied or not
        /// If count is 0 then mobile number is not verfied
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public sbyte OTPAttemptsMade(string mobileNo, string emailId)
        {
            sbyte noOfOTPSend = 0;
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "CV_IsVerifiedMobAndNoOfOTPSend";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@EmailId", SqlDbType.VarChar, 100).Value = emailId;
                        cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = mobileNo;
                        cmd.Parameters.Add("@NoOfAttempts", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                        con.Open();
                        cmd.ExecuteNonQuery();

                        noOfOTPSend = Convert.ToSByte(cmd.Parameters["@NoOfAttempts"].Value);

                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return noOfOTPSend;
        }

        /// <summary>
        /// Summary : Function will add mobile number into the customer mobile verification pending list.
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="emailId"></param>
        /// <param name="cwiCode"></param>
        /// <param name="cuiCode"></param>
        /// <returns></returns>
        public ulong AddMobileNoToPendingList(string mobileNo, string emailId, string cwiCode, string cuiCode)
        {
            ulong cvId = 0;
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "CV_InsertPendingList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@CWICode", SqlDbType.VarChar, 50).Value = cwiCode;
                        cmd.Parameters.Add("@CUICode", SqlDbType.VarChar, 50).Value = cuiCode;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = emailId;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = mobileNo;
                        cmd.Parameters.Add("@EntryDateTime", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@CVID", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        cvId = Convert.ToUInt64(cmd.Parameters["@CVID"].Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return cvId;
        }

        /// <summary>
        /// Search from the database with the mobile and the code. if it matches then make an entry 
        /// in the email-mobile pair and return true, else return false. also delete the record for this mobile number from the database.
        /// In case this function is called from the user interface then pass cwiCode else if it is from sms
        /// received from the user, pass cuiCode
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="cwiCode"></param>
        /// <param name="cuiCode"></param>
        /// <returns></returns>
        public bool VerifyMobileVerificationCode(string mobileNo, string cwiCode, string cuiCode)
        {
            bool verified = false;
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "CV_CheckVerification";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = mobileNo;
                        cmd.Parameters.Add("@CWICode", SqlDbType.VarChar, 50).Value = cwiCode;
                        cmd.Parameters.Add("@CUICode", SqlDbType.VarChar, 50).Value = cuiCode;
                        cmd.Parameters.Add("@IsVerified", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        con.Open();
                        cmd.ExecuteNonQuery();

                        verified = Convert.ToBoolean(cmd.Parameters["@IsVerified"].Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return verified;
        }
    }   // class
}   // namespace
