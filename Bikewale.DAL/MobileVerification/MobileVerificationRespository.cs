using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Bikewale.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Interfaces.MobileVerification;
using System.Data.Common;

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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "cv_isverifiedmobile";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailid", DbType.String, 100, emailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobileno", DbType.String, 50, mobileNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismobilever", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);
                    isVerified = Convert.ToBoolean(cmd.Parameters["par_ismobilever"].Value);
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "cv_isverifiedmobandnoofotpsend";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailid", DbType.String, 100, emailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobileno", DbType.String, 50, mobileNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_noofattempts", DbType.Int16, ParameterDirection.Output));

                     MySqlDatabase.ExecuteNonQuery(cmd);
                        noOfOTPSend = Convert.ToSByte(cmd.Parameters["par_noofattempts"].Value);

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

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "cv_insertpendinglist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cwicode", DbType.String, 50, cwiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cuicode", DbType.String, 50, cuiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 100, emailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 50, mobileNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_entrydatetime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cvid", DbType.Int64, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);
                        cvId = Convert.ToUInt64(cmd.Parameters["par_cvid"].Value);
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "cv_checkverification";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobileno", DbType.String, 50, mobileNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cwicode", DbType.String, 50, cwiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cuicode", DbType.String, 50, cuiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);
                        verified = Convert.ToBoolean(cmd.Parameters["par_isverified"].Value);
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

            return verified;
        }
    }   // class
}   // namespace
