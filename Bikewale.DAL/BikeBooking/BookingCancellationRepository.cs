using Bikewale.CoreDAL;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.DAL.BikeBooking
{
    public class BookingCancellationRepository : IBookingCancellation
    {
        /// <summary>
        /// Created By : Lucky Rathore
        /// Dated : 22 Jan 2016
        /// Description : Return the Deatil of the customer who wants to cancel booking.
        /// </summary>
        /// <param name="BwId">Unique BwId</param>
        /// <param name="Mobile">Mobile Number</param>
        /// <param name="OTP">OTP</param>
        /// <returns>Deatil of the customer who wants to cancel booking.</returns>
        public CancelledBikeCustomer VerifyCancellationOTP(string BwId, String Mobile, String OTP)
        {
            CancelledBikeCustomer cancelBikeDetail = null;
            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "VerifyOTPCancelRequest";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@BwId", SqlDbType.VarChar, 15).Value = BwId;
                    cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 10).Value = Mobile;
                    cmd.Parameters.Add("@OTP", SqlDbType.VarChar, 5).Value = OTP;
                    cmd.Parameters.Add("@isCancellable", SqlDbType.TinyInt).Direction = ParameterDirection.Output;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null && dr.HasRows )
                        {
                            if (dr.FieldCount > 1)
                            {
                                if (dr.Read())
                                {
                                    cancelBikeDetail = new CancelledBikeCustomer
                                    {
                                        //CustomerId = 0,//?Convert.ToUInt64(dr["CustomerId"])
                                        CustomerEmail = dr["CustomerEmail"].ToString(),
                                        CustomerMobile = dr["CustomerMobile"].ToString(),
                                        CustomerName = dr["CustomerName"].ToString(),
                                        BikeName = dr["BikeName"].ToString(),
                                        BookingDate = FormatDate.GetDDMMYYYY(Convert.ToString(dr["BookingDate"]))
                                    };
                                }
                                if (dr.HasRows && dr.NextResult() && cancelBikeDetail != null)
                                {
                                    if (dr.Read())
                                    {
                                        if (dr["isCancellable"] != null)
                                        {
                                            cancelBikeDetail.isCancellable = Convert.ToUInt16(dr["isCancellable"]);
                                        }
                                    }
                                }
                            }
                            else if(dr.FieldCount == 1)
                            {
                                cancelBikeDetail = new CancelledBikeCustomer();
                                if (dr.Read())
                                {
                                    if (dr["isCancellable"] != null)
                                    {
                                        cancelBikeDetail.isCancellable = Convert.ToUInt16(dr["isCancellable"]);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("PreCancellationDetail sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("PreCancellationDetail ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return cancelBikeDetail;
        }

        /// <summary>
        /// Created By : Sangram Nandkhile on 21st Jan 2016
        /// Summary :    To check if booking cancellation request is valid or not
        /// </summary>
        /// <returns></returns>
        public ValidBikeCancellationResponseEntity IsValidCancellation(string bwId, string mobile)
        {
            int responseFlag = 0;
            ValidBikeCancellationResponseEntity response = default(ValidBikeCancellationResponseEntity);
            Database db = null;
            try
            {
                db = new Database();
                response = new ValidBikeCancellationResponseEntity();
                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "VerifyCancelRequest";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@bwid", SqlDbType.VarChar).Value = bwId;
                        cmd.Parameters.Add("@mobilenumber", SqlDbType.VarChar, 10).Value = mobile;
                        cmd.Parameters.Add("@clientip", SqlDbType.VarChar, 40).Value = mobile;
                        cmd.Parameters.Add("@ResponseFlag", SqlDbType.TinyInt).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        object flag = cmd.Parameters["@ResponseFlag"].Value;
                        if (flag != null)
                            responseFlag = Convert.ToInt16(flag);
                        response.ResponseFlag = responseFlag;
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("IsValidCancellation: VerifyCancelRequest sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("IsValidCancellation: VerifyCancelRequest ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return response;
        }


        /// <summary>
        /// Created By :    Sangram Nandkhile on 21st Jan 2016
        /// Summary :       To Push BWid, mobile and OTP with entry Date
        /// </summary>
        /// <param name="bwId"></param>
        /// <param name="mobile"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        public bool SaveCancellationOTP(string bwId, string mobile, string otp)
        {
            bool isSuccess = true;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SaveBookingCancelOTP";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@bwid", SqlDbType.VarChar).Value = bwId;
                    cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 10).Value = mobile;
                    cmd.Parameters.Add("@otp", SqlDbType.VarChar, 5).Value = otp;

                    db = new Database();
                    isSuccess = db.InsertQry(cmd);
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("SaveCancellationOTP sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveCancellationOTP ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }
    }
}
