using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

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
                    // LogLiveSps.LogSpInGrayLog(cmd);
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_emailid", DbType.String, 100, emailId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobileno", DbType.String, 50, mobileNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismobilever", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                    isVerified = Convert.ToBoolean(cmd.Parameters["par_ismobilever"].Value);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
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
                    // LogLiveSps.LogSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);
                    noOfOTPSend = Convert.ToSByte(cmd.Parameters["par_noofattempts"].Value);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
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
                    // LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    cvId = Convert.ToUInt64(cmd.Parameters["par_cvid"].Value);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
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
                    // LogLiveSps.LogSpInGrayLog(cmd);

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_mobileno", DbType.String, 50, mobileNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cwicode", DbType.String, 50, cwiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cuicode", DbType.String, 50, cuiCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isverified", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    verified = Convert.ToBoolean(cmd.Parameters["par_isverified"].Value);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            }

            return verified;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 14 Feb 2017
        /// Summary    : Get list of verified numbers from database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetBlockedPhoneNumbers()
        {
            ICollection<string> numberList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getblockedphonenumbers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {

                        if (dr != null)
                        {
                            numberList = new List<string>();
                            while (dr.Read())
                            {
                                numberList.Add(Convert.ToString(dr["MobileNumber"]));
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.MobileVerification.GetVerifiedPhoneNumbers");
            }
            return numberList;
        }
    }   // class
}   // namespace
