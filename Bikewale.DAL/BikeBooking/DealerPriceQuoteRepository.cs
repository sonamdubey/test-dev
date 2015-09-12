using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Interfaces.BikeBooking;
using System.Data.SqlClient;
using System.Web;
using Bikewale.Notifications;
using System.Data;
using Bikewale.CoreDAL;
using Bikewale.Entities.Customer;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Location;
using System.Configuration;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.DAL.BikeBooking
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 29 Oct 2014
    /// </summary>
    public class DealerPriceQuoteRepository : IDealerPriceQuote
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 29 Oct 2014
        /// Summary :  to save customer detail in newbikedealerpricequote table
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="pqId"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        public bool SaveCustomerDetail(uint dealerId, uint pqId, string customerName, string customerMobile, string customerEmail)
        {
            bool isSuccess = false;
            Database db = null;
            try
            {
                db = new Database();
                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SaveBikeDealerQuotations";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@dealerId", SqlDbType.Decimal);
                        cmd.Parameters["@dealerId"].Precision = 18;
                        cmd.Parameters["@dealerId"].Scale = 8;
                        cmd.Parameters["@dealerId"].Value = dealerId;

                        cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;
                        cmd.Parameters.Add("@customerName", SqlDbType.VarChar, 50).Value = customerName;
                        cmd.Parameters.Add("@customerEmail", SqlDbType.VarChar, 50).Value = customerEmail;
                        cmd.Parameters.Add("@customerMobile", SqlDbType.VarChar, 50).Value = customerMobile;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        isSuccess = true;
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("SaveCustomerDetail sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveCustomerDetail ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 29 Oct 2014
        /// Summary : To update isverified flag in newbikedealerpricequote table
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool UpdateIsMobileVerified(uint pqId)
        {
            bool isSuccess = false;

            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "UpdateIsMobileVerified";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;

                    db = new Database();
                    isSuccess = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("UpdateIsMobileVerified sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdateIsMobileVerified ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 29 Oct 2014
        /// Summary :  to update mobile no in newbikedealerpricequote table
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="mobileNo"></param>
        /// <returns></returns>
        public bool UpdateMobileNumber(uint pqId, string mobileNo)
        {
            bool isSuccess = false;

            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "UpdateMobileNumber";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;
                    cmd.Parameters.Add("@customerMobile", SqlDbType.VarChar, 50).Value = mobileNo;

                    db = new Database();
                    isSuccess = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("UpdateMobileNumber sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdateMobileNumber ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }

            return isSuccess;
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 3 Nov 2014
        /// Summary : To update ispushedtoab flag in pq_newbikedealerpricequote
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool PushedToAB(uint pqId, uint abInquiryId)
        {
            bool isSuccess = false;

            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "IsPushedToAB";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;
                    cmd.Parameters.Add("@ABInquiryId", SqlDbType.BigInt).Value = abInquiryId;

                    db = new Database();
                    isSuccess = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("PushedToAB sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("PushedToAB ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }

            return isSuccess;
        }
        /// <summary>
        /// Written By : Ashwini Todkar on 3 Oct 2014
        /// Method to shedule appointment to dealer
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool UpdateAppointmentDate(uint pqId, DateTime date)
        {
            bool isSuccess = false;

            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "BookAppointmentDate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;
                    cmd.Parameters.Add("@appointmentDate", SqlDbType.DateTime).Value = date;

                    db = new Database();
                    isSuccess = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("UpdateAppointmentDate sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                // isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdateAppointmentDate ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                // isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Nov 2014
        /// Summary : To get customer details
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public PQCustomerDetail GetCustomerDetails(uint pqId)
        {
            PQCustomerDetail objCustomer = null;

            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetNewBikePriceQuoteCustomerDetail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PqId", SqlDbType.BigInt).Value = pqId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        objCustomer = new PQCustomerDetail();
                        if (dr.Read())
                        {
                            objCustomer.objCustomerBase = new CustomerEntity()
                            {
                                CustomerId = Convert.ToUInt64(dr["CustomerId"]),
                                CustomerName = dr["CustomerName"].ToString(),
                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                CustomerMobile = dr["CustomerMobile"].ToString(),
                                AreaDetails = new Entities.Location.AreaEntityBase() { AreaName = dr["AreaName"].ToString() },
                                cityDetails = new Entities.Location.CityEntityBase() { CityName = dr["CityName"].ToString() }
                            };
                            objCustomer.IsTransactionCompleted = Convert.ToBoolean(dr["TransactionCompleted"]);
                        }
                        
                        if (dr.NextResult())
                        {
                            if (dr.Read())
                            {
                                objCustomer.objColor = new VersionColor()
                                {
                                    ColorName = dr["ColorName"].ToString(),
                                    ColorId = Convert.ToUInt32(dr["ColorID"])
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("GetCustomerDetails sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                // isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetCustomerDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                // isSuccess = false;
            }
            finally
            {
                db.CloseConnection();
            }
            return objCustomer;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 2nd Dec
        /// Summary : to check whether customer is verified for given pq id or not
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool IsNewBikePQExists(uint pqId)
        {
            bool isVerified = false;
            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "IsNewBikePQExists";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr.Read())
                            isVerified = Convert.ToBoolean(dr["IsMobileVerified"]);
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("IsNewBikePQExists sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                // isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("IsNewBikePQExists ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                // isSuccess = false;
            }
            finally
            {
                db.CloseConnection();
            }
            return isVerified;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 2 Dec 2014
        /// Summary : To get Version list
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<BikeVersionEntityBase> GetVersionList(uint versionId, uint dealerId, uint cityId)
        {
            List<BikeVersionEntityBase> objVersions = null;
            Database db = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetDealerPriceVersionsList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = versionId;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                    cmd.Parameters.Add("@DealerId", SqlDbType.Int).Value = dealerId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        objVersions = new List<BikeVersionEntityBase>();

                        while (dr.Read())
                            objVersions.Add(new BikeVersionEntityBase() { VersionId = Convert.ToInt32(dr["VersionId"]), VersionName = dr["VersionName"].ToString() });
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("GetVersionList sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetVersionList ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return objVersions;
        }


        public bool SaveRSAOfferClaim(RSAOfferClaimEntity objOffer, string bikeName)
        {
            Database db = null;
            bool isSuccess = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SaveRSAOfferClaim";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = objOffer.CustomerName;
                    cmd.Parameters.Add("@CustomerMobile", SqlDbType.VarChar, 50).Value = objOffer.CustomerMobile;
                    cmd.Parameters.Add("@CustomerEmail", SqlDbType.VarChar, 50).Value = objOffer.CustomerEmail;
                    cmd.Parameters.Add("@BikeRegistrationNo", SqlDbType.VarChar, 50).Value = objOffer.BikeRegistrationNo;
                    cmd.Parameters.Add("@CustomerAddress", SqlDbType.VarChar, 150).Value = objOffer.CustomerAddress;
                    cmd.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = objOffer.DeliveryDate;
                    cmd.Parameters.Add("@DealerName", SqlDbType.VarChar, 50).Value = objOffer.DealerName;
                    cmd.Parameters.Add("@DealerAddress", SqlDbType.VarChar, 150).Value = objOffer.DealerAddress;
                    cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = objOffer.VersionId;
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar, 250).Value = objOffer.Comments;
                    cmd.Parameters.Add("@HelmetId", SqlDbType.TinyInt).Value = objOffer.HelmetId;

                    db = new Database();

                    isSuccess = db.InsertQry(cmd);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SaveRSAOfferClaim sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveRSAOfferClaim ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 16 Dec 2014
        /// Summary : To update Color in dealer price quote table
        /// </summary>
        /// <param name="colorId"></param>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool UpdatePQBikeColor(uint colorId, uint pqId)
        {
            bool isSuccess = false;

            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "UpdatePQColor";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;
                    cmd.Parameters.Add("@ColorId", SqlDbType.Int).Value = colorId;

                    db = new Database();
                    isSuccess = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("UpdatePQBikeColor sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdatePQBikeColor ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Dec 2014
        /// Summary : To get customer selected bike color by pqid
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        //public VersionColor GetPQBikeColor(uint pqId)
        //{
        //    VersionColor objColor = null;
        //    Database db = null;

        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.CommandText = "GetPQBikeColor";
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add("@PqId", SqlDbType.Int).Value = pqId;

        //            db = new Database();
        //            using (SqlDataReader dr = db.SelectQry(cmd))
        //            {
        //                if (dr.Read())
        //                {
        //                    objColor = new VersionColor();

        //                    objColor.ColorId = Convert.ToUInt32(dr["ColorID"]);
        //                    objColor.ColorName = dr["ColorName"].ToString();
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException sqEx)
        //    {
        //        HttpContext.Current.Trace.Warn("GetPQBikeColor sqlex : " + sqEx.Message + sqEx.Source);
        //        ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Trace.Warn("GetPQBikeColor ex : " + ex.Message + ex.Source);
        //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
        //        objErr.SendMail();
        //    }
        //    finally
        //    {
        //        db.CloseConnection();
        //    }
        //    return objColor;
        //}

        /// <summary>
        /// Created By : Sadhana Upadhyay on 17 Dec 2014
        /// Summary : To update transactional details in dealer price quote table
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="transId"></param>
        /// <param name="isTransComplete"></param>
        /// <returns></returns>
        public bool UpdatePQTransactionalDetail(uint pqId, uint transId, bool isTransComplete, string bookingReferenceNo)
        {
            bool isSuccess = false;

            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "UpdatePQTransactionalDetail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PqId", SqlDbType.BigInt).Value = pqId;
                    cmd.Parameters.Add("@TransactionId", SqlDbType.BigInt).Value = transId;
                    cmd.Parameters.Add("@TransactionCompleted", SqlDbType.Bit).Value = isTransComplete;
                    cmd.Parameters.Add("@BookingReferenceNo", SqlDbType.VarChar, 20).Value = bookingReferenceNo;

                    db = new Database();
                    isSuccess = db.UpdateQry(cmd);
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("UpdatePQTransactionalDetail sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UpdatePQTransactionalDetail ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 31 Dec 2014
        /// Summary : to get status of dealer notification
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="date"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        public bool IsDealerNotified(uint dealerId, string customerMobile, ulong customerId)
        {
            bool isNotified = false;
            Database db = null;
            try
            {
                db = new Database();
                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "IsDealerNotified";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@DealerId", SqlDbType.Int).Value = dealerId;
                        cmd.Parameters.Add("@CustomerId", SqlDbType.VarChar, 50).Value = customerId;
                        cmd.Parameters.Add("@CustomerMobile", SqlDbType.VarChar, 50).Value = customerMobile;
                        cmd.Parameters.Add("@IsDealerNotified", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        con.Open();

                        cmd.ExecuteNonQuery();

                        isNotified = Convert.ToBoolean(cmd.Parameters["@IsDealerNotified"].Value);
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("IsDealerNotified sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isNotified = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("IsDealerNotified ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isNotified = false;
            }
            return isNotified;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 15 Jan 2015
        /// Summary : To get whether Dealer Prices Available or not
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public bool IsDealerPriceAvailable(uint versionId, uint cityId)
        {
            bool isDealerAreaAvailable = false;
            Database db = null;
            try
            {
                db = new Database();
                using (SqlConnection con = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "IsDealerPriceAvailable";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                        cmd.Parameters.Add("@VersionId", SqlDbType.VarChar, 50).Value = versionId;
                        cmd.Parameters.Add("@IsDealerPriceAvailable", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        con.Open();

                        cmd.ExecuteNonQuery();

                        isDealerAreaAvailable = Convert.ToBoolean(cmd.Parameters["@IsDealerPriceAvailable"].Value);
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("IsDealerPriceAvailable sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isDealerAreaAvailable = false;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("IsDealerPriceAvailable ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                isDealerAreaAvailable = false;
            }
            return isDealerAreaAvailable;
        }

        #region GetDefaultPriceQuoteVersion Method
        /// <summary>
        /// Created By : Sadhana Upadhyay on 20 July 2015
        /// Summary : to get default version id for price quote
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public uint GetDefaultPriceQuoteVersion(uint modelId, uint cityId)
        {
            uint versionId = 0;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetPriceQuoteVersion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                                versionId = Convert.ToUInt32(dr["VersionId"]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetDefaultPriceQuoteVersion sqlex : " + ex.Message);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetDefaultPriceQuoteVersion ex : " + ex.Message);
                objErr.SendMail();
            }
            return versionId;
        }   //End of GetDefaultPriceQuoteVersion
        #endregion

        #region GetAreaList Method
        /// <summary>
        /// Created By : Sadhana Upadhyay on 20 July 2015
        /// Summary : To get AreaList if Dealer Prices Available
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<Bikewale.Entities.Location.AreaEntityBase> GetAreaList(uint modelId, uint cityId)
        {
            List<Bikewale.Entities.Location.AreaEntityBase> objArea = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetPriceQuoteArea";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    if (modelId > 0)
                        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;

                    db = new Database();
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            objArea = new List<Bikewale.Entities.Location.AreaEntityBase>();
                            while (dr.Read())
                                objArea.Add(new Bikewale.Entities.Location.AreaEntityBase()
                                {
                                    AreaId = Convert.ToUInt32(dr["Value"]),
                                    AreaName = dr["Text"].ToString()
                                });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetAreaList sqlex : " + ex.Message);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetAreaList ex : " + ex.Message);
                objErr.SendMail();
            }
            return objArea;
        }   //End of GetAreaList
        #endregion


        public PQOutputEntity ProcessPQ(Entities.PriceQuote.PriceQuoteParametersEntity PQParams)
        {
            throw new NotImplementedException();
        }

        public BookingPageDetailsEntity FetchBookingPageDetails(uint cityId, uint versionId, uint dealerId)
        {
            BookingPageDetailsEntity entity = null;
            IEnumerable<BikeModelColor> bikeModelColors = null;
            Database db = null;
            SqlDataReader reader = null;
            List<DealerPriceCategoryItemEntity> DealerPriceCategoryItemEntities = null;
            List<BikeDealerPriceDetail> BikeDealerPriceDetails = null;
            List<string> disclaimers = null;
            List<DealerOfferEntity> offers = null;
            DealerDetails objDealerDetails = null;
            try
            {
                db = new Database(ConfigurationManager.AppSettings["connectionstring"]);

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "BW_GetVarientsPriceDetail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                    cmd.Parameters.Add("@VersionId", SqlDbType.Int).Value = versionId;
                    cmd.Parameters.Add("@DealerId", SqlDbType.Int).Value = dealerId;
                    using (reader = db.SelectQry(cmd))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            #region Price Breakup for all Versions
                            DealerPriceCategoryItemEntities = new List<DealerPriceCategoryItemEntity>();
                            while (reader.Read())
                            {
                                DealerPriceCategoryItemEntities.Add(
                                    new DealerPriceCategoryItemEntity()
                                    {
                                        DealerId = Convert.ToUInt32(reader["DealerId"]),
                                        ItemId = Convert.ToUInt32(reader["ItemId"]),
                                        ItemName = Convert.ToString(reader["ItemName"]),
                                        Price = Convert.ToUInt32(reader["Price"]),
                                        VersionId = Convert.ToUInt32(reader["VersionId"])
                                    }
                                );
                            }
                            #endregion
                            #region Version Price Details
                            if (reader.NextResult())
                            {
                                BikeDealerPriceDetails = new List<BikeDealerPriceDetail>();
                                while (reader.Read())
                                {
                                    BikeDealerPriceDetails.Add(
                                        new BikeDealerPriceDetail()
                                        {
                                            BookingAmount = Convert.ToUInt32(reader["BookingAmount"]),
                                            HostUrl = Convert.ToString(reader["HostURL"]),
                                            ImagePath = Convert.ToString(reader["OriginalImagePath"]),
                                            Make = new BikeMakeEntityBase()
                                            {
                                                MakeId = Convert.ToInt32(reader["MakeId"]),
                                                MakeName = Convert.ToString(reader["MakeName"]),
                                                MaskingName = Convert.ToString(reader["MakeMaskingName"])
                                            },
                                            MinSpec = new BikeVersionMinSpecs()
                                            {
                                                VersionId = Convert.ToInt32(reader["VersionId"]),
                                                VersionName = Convert.ToString(reader["VersionName"]),
                                                Price = Convert.ToUInt64(reader["OnRoadPrice"]),
                                                ModelName = Convert.ToString(reader["ModelName"]),
                                                ElectricStart = Convert.IsDBNull(reader["ElectricStart"]) ? false : Convert.ToBoolean(reader["ElectricStart"]),
                                                BrakeType = Convert.IsDBNull(reader["BreakType"]) ? String.Empty : Convert.ToString(reader["BreakType"]),
                                                AntilockBrakingSystem = Convert.IsDBNull(reader["ABS"]) ? false : Convert.ToBoolean(reader["ABS"]),
                                                AlloyWheels = Convert.IsDBNull(reader["AlloyWheels"]) ? false : Convert.ToBoolean(reader["AlloyWheels"])
                                            },
                                            Model = new BikeModelEntityBase()
                                            {
                                                MaskingName = Convert.ToString(reader["ModelMaskingName"]),
                                                ModelId = Convert.ToInt32(reader["ModelId"]),
                                                ModelName = Convert.ToString(reader["ModelName"])
                                            },
                                            NoOfWaitingDays = Convert.ToUInt32(reader["NumOfDays"]),
                                            OnRoadPrice = Convert.ToUInt32(reader["OnRoadPrice"]),
                                        }
                                        );
                                }
                            }
                            #endregion
                            if (reader.NextResult())
                            {
                                disclaimers = new List<string>();
                                while (reader.Read())
                                {
                                    disclaimers.Add(Convert.ToString(reader["Disclaimer"]));
                                }
                            }
                            #region Dealer Offer Entity
                            if (reader.NextResult())
                            {
                                offers = new List<DealerOfferEntity>();
                                while (reader.Read())
                                {
                                    offers.Add(
                                        new DealerOfferEntity()
                                        {
                                            Id = Convert.ToUInt32(reader["OfferCategoryId"]),
                                            Text = Convert.ToString(reader["OfferText"]),
                                            Type = Convert.ToString(reader["OfferType"]),
                                            Value = Convert.ToUInt32(reader["OfferValue"])
                                        }
                                        );
                                }
                            }
                            #endregion
                            #region Dealer Details
                            if (reader.NextResult())
                            {
                                if (reader.Read())
                                {
                                    objDealerDetails = new DealerDetails()
                                    {
                                        Address1 = Convert.ToString(reader["Address1"]),
                                        Address2 = Convert.ToString(reader["Address2"]),
                                        Area = Convert.ToString(reader["AreaName"]),
                                        City = Convert.ToString(reader["CityName"]),
                                        ContactHours = Convert.ToString(reader["ContactHours"]),
                                        EmailId = Convert.ToString(reader["EmailId"]),
                                        FaxNo = Convert.ToString(reader["FaxNo"]),
                                        FirstName = Convert.ToString(reader["FirstName"]),
                                        Id = Convert.ToUInt32(reader["Id"]),
                                        LastName = Convert.ToString(reader["LastName"]),
                                        Lattitude = Convert.ToString(reader["Lattitude"]),
                                        Longitude = Convert.ToString(reader["Longitude"]),
                                        MobileNo = Convert.ToString(reader["MobileNo"]),
                                        Organization = Convert.ToString(reader["Organization"]),
                                        PhoneNo = Convert.ToString(reader["PhoneNo"]),
                                        Pincode = Convert.ToString(reader["Pincode"]),
                                        State = Convert.ToString(reader["StateName"]),
                                        WebsiteUrl = Convert.ToString(reader["WebsiteUrl"])
                                    };
                                }
                            }
                            #endregion
                            entity = new BookingPageDetailsEntity();
                            entity.Dealer = objDealerDetails;
                            entity.Disclaimers = disclaimers;
                            entity.Offers = offers;
                            BikeDealerPriceDetails.ForEach(
                                version => version.PriceList =
                                    (from price in DealerPriceCategoryItemEntities
                                     where price.VersionId == version.MinSpec.VersionId
                                     select new DealerVersionPriceItemEntity()
                                     {
                                         DealerId = price.DealerId,
                                         ItemId = price.ItemId,
                                         ItemName = price.ItemName,
                                         Price = price.Price
                                     }).ToList());

                            entity.Varients = BikeDealerPriceDetails;
                            if (entity.Varients != null && entity.Varients.Count > 0)
                            {
                                bikeModelColors = GetModelColor(entity.Varients[0].Model.ModelId);
                            }
                            entity.BikeModelColors = bikeModelColors;
                        }
                    }
                }
            }
            catch (SqlException sqEx)
            {
                HttpContext.Current.Trace.Warn("FetchBookingPageDetails sqlex : " + sqEx.Message + sqEx.Source);
                ErrorClass objErr = new ErrorClass(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("FetchBookingPageDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return entity;
        }

        private IEnumerable<BikeModelColor> GetModelColor(int modelId)
        {
            List<BikeModelColor> colors = null;
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetModelColor";

                    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;

                    db = new Database();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        if (dr != null)
                        {
                            colors = new List<BikeModelColor>();

                            while (dr.Read())
                            {
                                colors.Add(
                                    new BikeModelColor
                                    {
                                        ColorName = Convert.ToString(dr["Color"]),
                                        HexCode = Convert.ToString(dr["HexCode"]),
                                        ModelId = Convert.ToUInt32(dr["BikeModelID"]),
                                    }
                                );
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetModelColor sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetModelColor ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return colors;
        }

    }   //End of class
}   //End of namespace
