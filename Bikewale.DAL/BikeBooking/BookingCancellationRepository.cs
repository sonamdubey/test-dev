using Bikewale.CoreDAL;
using Bikewale.Entities.Customer;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Notifications;
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
                    cmd.Parameters.Add("@MobileNumber", SqlDbType.VarChar, 10).Value = Mobile;
                    cmd.Parameters.Add("@OTP", SqlDbType.VarChar, 5).Value = OTP;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        cancelBikeDetail = new CancelledBikeCustomer
                        {
                            CustomerId = 0,//?Convert.ToUInt64(dr["CustomerId"])
                            CustomerEmail = dr["CustomerEmail"].ToString(),
                            CustomerMobile = dr["CustomerMobile"].ToString(),
                            CustomerName = dr["CustomerName"].ToString(),
                            BikeName = dr["BikeName"].ToString(),
                            BookingDate = Convert.ToDateTime(dr["BookingDate"])
                        };

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
    }
}
