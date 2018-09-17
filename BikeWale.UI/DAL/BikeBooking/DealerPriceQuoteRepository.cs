using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
        /// Modified By : Sushil Kumar on 29th Nov 2016
        /// Description : Removed unused function UpdateAppointmentDate
        /// Modified by : Snehal Dange on 14th May 2018
        /// Descripton : Changed sp name from 'savebikedealerquotations_07062017' to 'savebikedealerquotations_14052018' .Added par_spamscore,par_isaccepted,par_overallspamscore to store overall score
        /// Modified by : Pratibha Verma on 26 June 2018
        /// Description : changed sp 'savebikedealerquotations_14052018' to 'savecustomerdetailsbypqid' .Returned leadid from db
        /// Modified by : Pratibha Verma on 2 August 2018
        /// Description : changed sp from 'savecustomerdetailsbypqid' to 'savecustomerdetailsbypqid_02082018'. Added parameters SourceId and ClientIP
        /// Description :  
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="pqId"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <returns></returns>
        public uint SaveCustomerDetailByPQId(DPQ_SaveEntity entity)
        {
            uint leadId = 0;

            try
            {
                if (entity != null)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "savecustomerdetailsbypqid_02082018";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, entity.DealerId));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int32, entity.PQId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, entity.CustomerName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 50, entity.CustomerEmail));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 50, entity.CustomerMobile));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_colorid", DbType.Int32, (entity.ColorId.HasValue) ? entity.ColorId.Value : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbType.Byte, (entity.LeadSourceId.HasValue) ? entity.LeadSourceId.Value : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbType.String, 500, (!String.IsNullOrEmpty(entity.UTMA)) ? entity.UTMA : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, 500, (!String.IsNullOrEmpty(entity.UTMZ)) ? entity.UTMZ : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbType.String, 25, (!String.IsNullOrEmpty(entity.DeviceId)) ? entity.DeviceId : Convert.DBNull));


                        cmd.Parameters.Add(DbFactory.GetDbParam("par_spamscore", DbType.Double, entity.SpamScore));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isaccepted", DbType.Boolean, entity.IsAccepted));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_overallspamscore", DbType.Byte, entity.OverallSpamScore));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_leadid", DbType.Int32, ParameterDirection.Output));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, entity.PlatformId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, !String.IsNullOrEmpty(entity.ClientIP) ? entity.ClientIP : null));

                        // LogLiveSps.LogSpInGrayLog(cmd);
                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                        leadId = SqlReaderConvertor.ToUInt32(cmd.Parameters["par_leadid"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SaveCustomerDetailByPQId ex : " + ex.Message);
            }

            return leadId;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 26 june 2018
        /// Description : passed leadId in param and return leadId
        /// Modified by : Pratibha Verma on 2 August 2018
        /// Description : changed sp from "savecustomerdetailsbyleadid" to "savecustomerdetailsbyleadid_02082018" in order to store sourceId and clientIP
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public uint SaveCustomerDetailByLeadId(Bikewale.Entities.BikeBooking.v2.DPQ_SaveEntity entity)
        {
            uint leadId = 0;

            try
            {
                if (entity != null)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandText = "savecustomerdetailsbyleadid_02082018";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, entity.DealerId));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqguid", DbType.String, 40, entity.PQId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, entity.CustomerName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 50, entity.CustomerEmail));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 50, entity.CustomerMobile));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_colorid", DbType.Int32, (entity.ColorId.HasValue) ? entity.ColorId.Value : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbType.Byte, (entity.LeadSourceId.HasValue) ? entity.LeadSourceId.Value : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbType.String, 500, (!String.IsNullOrEmpty(entity.UTMA)) ? entity.UTMA : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, 500, (!String.IsNullOrEmpty(entity.UTMZ)) ? entity.UTMZ : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbType.String, 25, (!String.IsNullOrEmpty(entity.DeviceId)) ? entity.DeviceId : Convert.DBNull));


                        cmd.Parameters.Add(DbFactory.GetDbParam("par_spamscore", DbType.Double, entity.SpamScore));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_isaccepted", DbType.Boolean, entity.IsAccepted));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_overallspamscore", DbType.Byte, entity.OverallSpamScore));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, entity.VersionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, entity.CityId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Int32, entity.AreaId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_leadid", DbType.Int32, ParameterDirection.InputOutput, entity.LeadId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, entity.PlatformId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, !String.IsNullOrEmpty(entity.ClientIP) ? entity.ClientIP : null));

                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                        leadId = SqlReaderConvertor.ToUInt32(cmd.Parameters["par_leadid"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SaveCustomerDetailByLeadId ex : " + ex.Message);
            }

            return leadId;
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

                    if (Convert.ToBoolean(MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UpdateIsMobileVerified ex : " + ex.Message);

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

                    if (Convert.ToBoolean(MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UpdateMobileNumber ex : " + ex.Message);

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

                    if (Convert.ToBoolean(MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "PushedToAB ex : " + ex.Message);

                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Nov 2014
        /// Summary : To get customer details
        /// Modified by : Pratibha Verma on 26 June 2018
        /// Description : replaced sp 'getnewbikepricequotecustomerdetail'  with 'getcustomerdetailsbypqid'
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public PQCustomerDetail GetCustomerDetailsByPQId(uint pqId)
        {
            PQCustomerDetail objCustomer = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcustomerdetailsbypqid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
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
                            objCustomer.LeadId = SqlReaderConvertor.ToUInt32(dr["leadid"]);
                        }

                        if (dr.NextResult())
                        {
                            if (dr != null && dr.Read())
                            {
                                objCustomer.objColor = new VersionColor()
                                {
                                    ColorName = dr["ColorName"].ToString(),
                                    ColorId = Convert.ToUInt32(dr["ColorID"])
                                };
                            }
                        }
                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetCustomerDetailsByPQId ex : " + ex.Message);

                // isSuccess = false;
            }
            return objCustomer;
        }


        /// <summary>
        /// Created by  : Pratibha Verma on 26 June 2018
        /// Description : removed PQId dependency and retrieved customer details based on LeadId
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public PQCustomerDetail GetCustomerDetailsByLeadId(uint leadId)
        {
            PQCustomerDetail objCustomer = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getcustomerdetailsbyleadid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadId", DbType.Int32, leadId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objCustomer = new PQCustomerDetail();
                        if (dr.Read())
                        {
                            objCustomer.objCustomerBase = new CustomerEntity()
                            {
                                CustomerId = SqlReaderConvertor.ToUInt64(dr["CustomerId"]),
                                CustomerName = dr["CustomerName"].ToString(),
                                CustomerEmail = dr["CustomerEmail"].ToString(),
                                CustomerMobile = dr["CustomerMobile"].ToString(),
                                AreaDetails = new Entities.Location.AreaEntityBase() { AreaName = dr["AreaName"].ToString() },
                                cityDetails = new Entities.Location.CityEntityBase() { CityName = dr["CityName"].ToString(), CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]) }
                            };
                            objCustomer.IsTransactionCompleted = SqlReaderConvertor.ToBoolean(dr["TransactionCompleted"]);
                            objCustomer.AbInquiryId = Convert.ToString(dr["AbInquiryId"]);
                            objCustomer.SelectedVersionId = SqlReaderConvertor.ToUInt32(dr["SelectedVersionId"]);
                            objCustomer.DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]);
                            objCustomer.LeadId = SqlReaderConvertor.ToUInt32(dr["LeadId"]);
                        }

                        if (dr.NextResult() && dr.Read())
                        {
                            objCustomer.objColor = new VersionColor()
                            {
                                ColorName = dr["ColorName"].ToString(),
                                ColorId = SqlReaderConvertor.ToUInt32(dr["ColorID"])
                            };
                        }
                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "DealerPriceQuoteRepository.GetCustomerDetailsByLeadId ex : " + ex.Message);
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


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                            isVerified = Convert.ToBoolean(dr["IsMobileVerified"]);
                        if (dr != null) dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "IsNewBikePQExists ex : " + ex.Message);

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


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objVersions = new List<BikeVersionEntityBase>();

                        while (dr.Read())
                            objVersions.Add(new BikeVersionEntityBase() { VersionId = Convert.ToInt32(dr["VersionId"]), VersionName = dr["VersionName"].ToString() });

                        if (dr != null) dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetVersionList ex : " + ex.Message);

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

                    isSuccess = MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "SaveRSAOfferClaim ex : " + ex.Message);

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

                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UpdatePQBikeColor ex : " + ex.Message);

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

                    isSuccess = MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UpdatePQTransactionalDetail ex : " + ex.Message);

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
                    // LogLiveSps.LogSpInGrayLog(cmd);

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isNotified = SqlReaderConvertor.ToBoolean(cmd.Parameters["par_isdealernotified"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "IsDealerNotified ex : " + ex.Message);

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

                    // LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.ReadOnly);

                    isDealerAreaAvailable = Convert.ToBoolean(cmd.Parameters["par_isdealerpriceavailable"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "IsDealerPriceAvailable ex : " + ex.Message);

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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                                versionId = Convert.ToUInt32(dr["VersionId"]);
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetDefaultPriceQuoteVersion ex : " + ex.Message);

            }

            return versionId;
        }   //End of GetDefaultPriceQuoteVersion

        /// <summary>
        /// Created by  :   Sumit Kate on 16 Dec 2016
        /// Description :   Call getpricequoteversion_15122016 SP
        /// Modified by :   Sumit Kate on 15 May 2017
        /// Description :   Change getpricequoteversion_15052017
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public uint GetDefaultPriceQuoteVersion(uint modelId, uint cityId, uint areaId)
        {
            uint versionId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getpricequoteversion_15052017";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Int32, areaId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                                versionId = Convert.ToUInt32(dr["VersionId"]);
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("GetDefaultPriceQuoteVersion({0},{1},{2}) : Exception : {3}", modelId, cityId, areaId, ex.Message));

            }
            return versionId;
        }
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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
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
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetAreaList ex : " + ex.Message);

            }
            return objArea;
        }   //End of GetAreaList
        #endregion

        /// <summary>
        /// Modified By  : Rajan Chauhan on 23 Mar 2018
        /// Description  : Removed MinSpec binding
        /// Modified by : Ashutosh Sharma on 07 Apr 2018.
        /// Description : Changed sp from 'bw_getvarientspricedetail_13012016' to 'bw_getvarientspricedetail_07042018' to remove min specs.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
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
                    cmd.CommandText = "bw_getvarientspricedetail_07042018";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));

                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
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
                                                ModelName = Convert.ToString(reader["ModelName"])
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
                                if (reader != null)
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

                            reader.Close();

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
                ErrorClass.LogError(sqEx, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("FetchBookingPageDetails ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
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
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
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
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return colors;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 02 June 2016
        /// Description : Refer BAL.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns>DealerInfo entity</returns>
        public DealerInfo IsDealerExists(uint versionId, uint areaId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Created By : Sushil Kumar on 29th Nov 2016
        /// Description : To update dealer daily limit count  
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="abInquiryId"></param>
        public bool UpdateDealerDailyLeadCount(uint campaignId, uint abInquiryId)
        {
            bool isUpdateDealerCount = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatedealerdailyleadcount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_abinquiryid", DbType.Int32, abInquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isupdatedealercount", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    isUpdateDealerCount = SqlReaderConvertor.ToBoolean(cmd.Parameters["par_isupdatedealercount"].Value);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + " DealerPriceQuoteRepository.UpdateDealerDailyLeadCount");

            }
            return isUpdateDealerCount;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 29th Nov 2016
        /// Description : To check dealer daily limit count exceeds or not for campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        public bool IsDealerDailyLeadLimitExceeds(uint campaignId)
        {
            bool islimitexceeds = false;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "isdealerdailyleadlimitexceeds";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, campaignId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_islimitexceeds", DbType.Boolean, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);

                    islimitexceeds = SqlReaderConvertor.ToBoolean(cmd.Parameters["par_islimitexceeds"].Value);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + " DealerPriceQuoteRepository.IsDealerDailyLeadLimitExceeds");

            }

            return islimitexceeds;

        }

        public PQOutputEntity ProcessPQ(PriceQuoteParametersEntity PQParams, bool isManufacturerCampaignRequired = false)
        {
            throw new NotImplementedException();
        }


        public Entities.BikeBooking.v2.PQOutputEntity ProcessPQV2(Bikewale.Entities.PriceQuote.v2.PriceQuoteParametersEntity PQParams, bool isDealerSubscriptionRequired = true)
        {
            throw new NotImplementedException();
        }

        public Entities.BikeBooking.v2.PQOutputEntity ProcessPQV3(Entities.PriceQuote.v2.PriceQuoteParametersEntity PQParams)
        {
            throw new NotImplementedException();
        }
        
        public BikeWale.Entities.AutoBiz.DealerInfo GetDefaultVersionAndSubscriptionDealer(uint modelId, uint cityId, uint areaId, uint versionId, bool isDealerSubscriptionRequired, out uint defaultVersionId)
        {
            throw new NotImplementedException();
        }

        public uint GetManufacturerCampaignDealer(uint modelId, uint cityId, ManufacturerCampaignServingPages page, out ManufacturerCampaignEntity campaigns, out bool isManufacturerDealer)
        {
            throw new NotImplementedException();
        }
    }   //End of class
}   //End of namespace