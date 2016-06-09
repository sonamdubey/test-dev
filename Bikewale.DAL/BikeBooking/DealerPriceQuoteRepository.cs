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
using System.Data.Common;

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
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : To save utmz, utma, LeadSourceId, deviceId
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="pqId"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        public bool SaveCustomerDetail(DPQ_SaveEntity entity)
        {
            bool isSuccess = false;
           
            try
            {

                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "savebikedealerquotations_30122015";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32 , entity.DealerId));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int32 , entity.PQId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50 , entity.CustomerName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 50 , entity.CustomerEmail));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 50 , entity.CustomerMobile));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_colorid", DbType.Int32, (entity.ColorId.HasValue) ? entity.ColorId.Value : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbType.Byte, (entity.LeadSourceId.HasValue) ? entity.LeadSourceId.Value : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbType.String, 100, (!String.IsNullOrEmpty(entity.UTMA)) ? entity.UTMA : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, 100, (!String.IsNullOrEmpty(entity.UTMZ)) ? entity.UTMZ : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbType.String, 25, (!String.IsNullOrEmpty(entity.DeviceId)) ? entity.DeviceId : Convert.DBNull));

                        if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd)))
                            isSuccess = true;
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
        /// Modified By : Sadhana Upadhyay on 30 Nov 2015
        /// Summary : to add record in PQ_LeadNotifications table
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool UpdateIsMobileVerified(uint pqId)
        {
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updateismobileverified";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));

                    if (Convert.ToBoolean(MySqlDatabase.UpdateQuery(cmd)))
                        isSuccess = true;
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatemobilenumber";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 50, mobileNo));

                    if (Convert.ToBoolean(MySqlDatabase.UpdateQuery(cmd)))
                        isSuccess = true;
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

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "ispushedtoab";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_abinquiryid", DbType.Int64, abInquiryId));

                    if (Convert.ToBoolean(MySqlDatabase.UpdateQuery(cmd)))
                        isSuccess = true;
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

# if unused
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

           

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "BookAppointmentDate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@pqId", SqlDbType.BigInt).Value = pqId;
                    cmd.Parameters.Add("@appointmentDate", SqlDbType.DateTime).Value = date;

                    
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
#endif
        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Nov 2014
        /// Summary : To get customer details
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public PQCustomerDetail GetCustomerDetails(uint pqId)
        {
            PQCustomerDetail objCustomer = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getnewbikepricequotecustomerdetail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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
                                cityDetails = new Entities.Location.CityEntityBase() { CityName = dr["CityName"].ToString(), CityId = Convert.ToUInt32(dr["CityId"]) }
                            };
                            objCustomer.IsTransactionCompleted = Convert.ToBoolean(dr["TransactionCompleted"]);
                            objCustomer.AbInquiryId = Convert.ToString(dr["AbInquiryId"]);
                            objCustomer.SelectedVersionId = Convert.ToUInt32(dr["SelectedVersionId"]);
                            objCustomer.DealerId = Convert.ToUInt32(dr["DealerId"]);
                        }

                        if (dr.NextResult())
                        {
                            if (dr!=null && dr.Read())
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
           

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "isnewbikepqexists";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));

                    
                     using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr!=null &&  dr.Read())
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

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getdealerpriceversionslist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));

                    
                     using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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

            return objVersions;
        }


        public bool SaveRSAOfferClaim(RSAOfferClaimEntity objOffer, string bikeName)
        {
           
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "saversaofferclaim";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bookingnum", DbType.String, 20, objOffer.BookingNum));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 100, objOffer.CustomerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 50, objOffer.CustomerMobile));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 50, objOffer.CustomerEmail));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeregistrationno", DbType.String, 50, objOffer.BikeRegistrationNo));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customeraddress", DbType.String, 150, objOffer.CustomerAddress));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_deliverydate", DbType.DateTime, objOffer.DeliveryDate));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealername", DbType.String, 50, objOffer.DealerName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealeraddress", DbType.String, 150, objOffer.DealerAddress));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, objOffer.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_comments", DbType.String, 250, objOffer.Comments));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_helmetid", DbType.Byte, objOffer.HelmetId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_customerpincode", DbType.String, 6, objOffer.CustomerPincode));                     

                    isSuccess = MySqlDatabase.InsertQuery(cmd);
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

           
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatepqcolor";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_colorid", DbType.Int32, colorId)); 
                    
                    isSuccess = MySqlDatabase.UpdateQuery(cmd);
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatepqtransactionaldetail_22012016";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_transactionid", DbType.Int64, transId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_transactioncompleted", DbType.Boolean, isTransComplete));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bookingreferenceno", DbType.String, 20, bookingReferenceNo)); 

                    isSuccess = MySqlDatabase.UpdateQuery(cmd);
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
            try
            {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "isdealernotified";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.String, 50, customerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 50, customerMobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isdealernotified", DbType.Boolean, ParameterDirection.Output));

                        MySqlDatabase.ExecuteNonQuery(cmd);

                        isNotified = Convert.ToBoolean(cmd.Parameters["par_isdealernotified"].Value);
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
            try
            {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "isdealerpriceavailable";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.String, 50, versionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isdealerpriceavailable", DbType.Boolean, ParameterDirection.Output));

                         MySqlDatabase.ExecuteNonQuery(cmd);

                        isDealerAreaAvailable = Convert.ToBoolean(cmd.Parameters["par_isdealerpriceavailable"].Value);
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
           
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getpricequoteversion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    
                     using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getpricequotearea"))
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                    //if (modelId > 0)
                    //    cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int64, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int64, (modelId > 0) ? modelId : Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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
            List<DealerPriceCategoryItemEntity> DealerPriceCategoryItemEntities = null;
            List<BikeDealerPriceDetail> BikeDealerPriceDetails = null;
            List<string> disclaimers = null;
            List<DealerOfferEntity> offers = null;
            DealerDetails objDealerDetails = null;
            List<BikeVersionColorsAvailability> modelColorList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "bw_getvarientspricedetail_13012016";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (reader != null)
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
                                        Price = Convert.ToInt32(reader["Price"]),
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
                                            NoOfWaitingDays = Convert.ToInt16(reader["NumOfDays"]),
                                            OnRoadPrice = Convert.ToUInt32(reader["OnRoadPrice"]),
                                        }
                                        );
                                }
                            }
                            #endregion

                            #region Get Disclaimers
                            if (reader.NextResult())
                            {
                                disclaimers = new List<string>();
                                while (reader.Read())
                                {
                                    disclaimers.Add(Convert.ToString(reader["Disclaimer"]));
                                }
                            }
                            #endregion

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

                            #region Model Colors Versionwise
                            if (reader.NextResult())
                            {
                                if (reader !=null)
                                {
                                    modelColorList = new List<BikeVersionColorsAvailability>();
                                    while (reader.Read())
                                    {
                                        modelColorList.Add(new BikeVersionColorsAvailability()
                                        {
                                            ColorId = Convert.ToUInt32(reader["ColorId"]),
                                            NoOfDays = Convert.ToInt16(reader["NumOfDays"]),
                                            ColorName = Convert.ToString(reader["ColorName"]),
                                            HexCode = Convert.ToString(reader["HexCode"]),
                                            VersionId = Convert.ToUInt32(reader["BikeVersionId"])
                                        });
                                    }
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

                            if (entity.Varients != null && entity.Varients.Count > 0 && modelColorList != null)
                            {
                                foreach (var variant in entity.Varients)
                                {

                                    var ColorListForDealer = from color in modelColorList
                                                             where color.VersionId == variant.MinSpec.VersionId
                                                             group color by color.ColorId into newgroup
                                                             orderby newgroup.Key
                                                             select newgroup;


                                    var objColorAvail = new List<BikeVersionColorsWithAvailability>();
                                    foreach (var color in ColorListForDealer)
                                    {
                                        BikeVersionColorsWithAvailability objAvail = new BikeVersionColorsWithAvailability();

                                        objAvail.ColorId = color.Key;

                                        IList<string> HexCodeList = new List<string>();
                                        foreach (var colorList in color)
                                        {
                                            objAvail.ColorName = colorList.ColorName;
                                            objAvail.NoOfDays = colorList.NoOfDays;
                                            objAvail.VersionId = colorList.VersionId;
                                            HexCodeList.Add(colorList.HexCode);

                                        }
                                        objAvail.HexCode = HexCodeList;
                                        objColorAvail.Add(objAvail);
                                    }

                                    variant.BikeModelColors = objColorAvail;
                                }
                            }
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
           
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmodelcolor";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));                    

                     using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            colors = new List<BikeModelColor>();

                            while (dr.Read())
                            {
                                colors.Add(
                                    new BikeModelColor
                                    {
                                        Id = Convert.ToUInt32(dr["ID"]),
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

            return colors;
        }
        /// <summary>
        /// Get Version Id and respective color id for model id
        /// Author  :   Sangram Nandkhile
        /// Created On  :   04 Nov 2015
        /// </summary>
        private IEnumerable<BikeModelColor> GetVariantColorByModel(int modelId)
        {
            List<BikeModelColor> colors = null;
           
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getvariantcolorbymodel";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    
                     using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            colors = new List<BikeModelColor>();

                            while (dr.Read())
                            {
                                colors.Add(
                                    new BikeModelColor
                                    {
                                        Id = Convert.ToUInt32(dr["ID"]),
                                        ColorName = Convert.ToString(dr["Color"]),
                                        HexCode = Convert.ToString(dr["HexCode"]),
                                        ModelId = Convert.ToUInt32(dr["VersionId"]),
                                    }
                                );
                            }
                        }
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

            return colors;
        }

    }   //End of class
}   //End of namespace