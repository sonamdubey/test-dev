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
                                        PQId = Convert.ToUInt32(dr["pqid"]),
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
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                db = new Database();
                response = new ValidBikeCancellationResponseEntity();
                using (conn = new SqlConnection(db.GetConString()))
                {
                    using (cmd = new SqlCommand())
                    {
                        cmd.CommandText = "VerifyCancelRequest";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@bwid", SqlDbType.VarChar).Value = bwId;
                        cmd.Parameters.Add("@mobilenumber", SqlDbType.VarChar, 10).Value = mobile;
                        cmd.Parameters.Add("@clientip", SqlDbType.VarChar, 40).Value = CommonOpn.GetClientIP();
                        cmd.Parameters.Add("@ResponseFlag", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                        LogLiveSps.LogSpInGrayLog(cmd);
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
            finally
            {
                db.CloseConnection();
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
        public uint SaveCancellationOTP(string bwId, string mobile, string otp)
        {
            uint attempts = 0;
            Database db = null;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                db = new Database();

                using (conn = new SqlConnection(db.GetConString()))
                {
                    using (cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SaveBookingCancelOTP";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.Add("@bwid", SqlDbType.VarChar, 15).Value = bwId;
                        cmd.Parameters.Add("@mobile", SqlDbType.VarChar, 10).Value = mobile;
                        cmd.Parameters.Add("@otp", SqlDbType.VarChar, 5).Value = otp;
                        cmd.Parameters.Add("@AttemptsMade", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                        LogLiveSps.LogSpInGrayLog(cmd);
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        attempts = Convert.ToUInt16(cmd.Parameters["@AttemptsMade"].Value);
                      

                    }
                }
            }

            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("SaveCancellationOTP sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveCancellationOTP ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return attempts;
        }

        /// <summary>
        /// Added By : Sadhana Upadhyay on 27 Jan 2016
        /// Summary : To get cancellation details
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public CancelledBikeCustomer GetCancellationDetails(uint pqId)
        {
            CancelledBikeCustomer objCancellation = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetBookingCancellationDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PQId", SqlDbType.Int).Value = pqId;

                    db = new Database();

                    using (SqlDataReader reader = db.SelectQry(cmd))
                    {
                        if (reader != null)
                        {
                            objCancellation = new CancelledBikeCustomer();
                            if (reader.Read())
                            {
                                objCancellation.BikeName = Convert.ToString(reader["Bike"]);
                                objCancellation.BookingDate = FormatDate.GetDDMMYYYY(Convert.ToString(reader["BookingDate"]));
                                objCancellation.BWId = Convert.ToString(reader["BookingReferenceNo"]);
                                objCancellation.CustomerEmail = Convert.ToString(reader["CustomerEmail"]);
                                objCancellation.CustomerMobile = Convert.ToString(reader["CustomerMobile"]);
                                objCancellation.CustomerName = Convert.ToString(reader["CustomerName"]);
                                objCancellation.DealerName = Convert.ToString(reader["Organization"]);
                                objCancellation.TransactionId = Convert.ToUInt32(reader["TransactionId"]);
                                objCancellation.CityName = Convert.ToString(reader["CityName"]);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("CancelBooking sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, "Bikewale.DAL.BikeBooking.BookingCancellationRepository SQLEx");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("CancelBooking ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DAL.BikeBooking.BookingCancellationRepository");
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return objCancellation;
        }

        public bool ConfirmCancellation(uint pqId)
        {
            throw new NotImplementedException();
        }
    }   //End of Class
}   //End of Namespace
