
using Bikewale.Utility;
using Consumer;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
namespace Bikewale.RabbitMq.LeadProcessingConsumer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 24 Feb 2017
    /// Desription  :   LeadProcessingRepository contains required functions to perform Db operations
    /// </summary>
    internal class LeadProcessingRepository
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 3 Nov 2014
        /// Summary : To update ispushedtoab flag in pq_newbikedealerpricequote
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public bool PushedToAB(uint pqId, uint abInquiryId, UInt16 retryCount)
        {
            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "ispushedtoab_24022017";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_abinquiryid", DbType.Int64, abInquiryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_leadPushRetries", DbType.Int16, retryCount));
                    if (Convert.ToBoolean(MySqlDatabase.UpdateQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in PushedToAB({0},{1}) : Msg : {2}", pqId, abInquiryId, ex.Message));
                isSuccess = false;
            }

            return isSuccess;
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

                    isUpdateDealerCount = Convert.ToBoolean(!Convert.IsDBNull(cmd.Parameters["par_isupdatedealercount"].Value) ? cmd.Parameters["par_isupdatedealercount"].Value : false);

                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in UpdateDealerDailyLeadCount({0},{1}) : Msg : {2}", campaignId, abInquiryId, ex.Message));
            }
            return isUpdateDealerCount;
        }


        /// <summary>
        /// Author          :   Sumit Kate
        /// Created date    :   08 Jan 2016
        /// Description     :   Gets the Areaid, cityid, dealerid, bikeversionid by pqid
        ///                     This is required to form the PQ Query string
        /// Modified by     :   Sumit Kate on 02 May 2016
        /// Description     :   Return the Campaign Id
        /// </summary>
        /// <param name="pqId"></param>
        /// <returns></returns>
        public PriceQuoteParametersEntity FetchPriceQuoteDetailsById(ulong pqId)
        {
            PriceQuoteParametersEntity objQuotation = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getpricequoteleaddata";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int32, pqId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        objQuotation = new PriceQuoteParametersEntity();
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objQuotation.AreaId = SqlReaderConvertor.ToUInt32(dr["AreaId"]);
                                objQuotation.CityId = SqlReaderConvertor.ToUInt32(dr["cityid"]);
                                objQuotation.VersionId = SqlReaderConvertor.ToUInt32(dr["BikeVersionId"]);
                                objQuotation.DealerId = SqlReaderConvertor.ToUInt32(dr["DealerId"]);
                                objQuotation.CampaignId = SqlReaderConvertor.ToUInt32(dr["CampaignId"]);
                                objQuotation.CustomerMobile = Convert.ToString(dr["CustomerMobile"]);
                                objQuotation.CustomerEmail = Convert.ToString(dr["CustomerEmail"]);
                                objQuotation.CustomerName = Convert.ToString(dr["CustomerName"]);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in FetchPriceQuoteDetailsById({0}) : Msg : {1}", pqId, ex.Message));
            }

            return objQuotation;
        }

        public bool UpdateManufacturerLead(uint pqId, string custEmail, string mobile, string response)
        {
            bool status = false;
            try
            {
                if (pqId > 0 && !String.IsNullOrEmpty(custEmail) && !String.IsNullOrEmpty(mobile))
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "updatemanufacturerlead";

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 150, custEmail));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 10, mobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, pqId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_response", DbType.String, 250, response));
                        if (MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase) > 0)
                            status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in FetchPriceQuoteDetailsById({0}) : Msg : {1}", pqId, ex.Message));
            }

            return status;
        }

        /// <summary>
        /// Summary : Function to Get the price quote by price quote id.
        /// Modified by :   Sumit Kate on 18 Aug 2016
        /// Description :   Created new SP to return state name in result. Replaced in/out parameters with DataReader approach
        /// </summary>
        /// <param name="pqId">price quote id. Only positive numbers are allowed</param>
        /// <returns>Returns price quote object.</returns>
        public BikeQuotationEntity GetPriceQuoteById(uint pqId)
        {
            BikeQuotationEntity objQuotation = null;
            try
            {
                objQuotation = new BikeQuotationEntity();
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getpricequote_new_18082016";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.UInt32, pqId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objQuotation.ExShowroomPrice = SqlReaderConvertor.ToUInt64(dr["exshowroom"]);
                            objQuotation.RTO = SqlReaderConvertor.ToUInt32(dr["rto"]);
                            objQuotation.Insurance = SqlReaderConvertor.ToUInt32(dr["insurance"]);
                            objQuotation.OnRoadPrice = SqlReaderConvertor.ToUInt64(dr["onroad"]);
                            objQuotation.MakeName = Convert.ToString(dr["make"]);
                            objQuotation.ModelName = Convert.ToString(dr["model"]);
                            objQuotation.VersionName = Convert.ToString(dr["version"]);
                            objQuotation.City = Convert.ToString(dr["cityname"]);
                            objQuotation.VersionId = SqlReaderConvertor.ToUInt32(dr["versionid"]);
                            objQuotation.CampaignId = SqlReaderConvertor.ToUInt32(dr["campaignid"]);
                            objQuotation.ManufacturerId = SqlReaderConvertor.ToUInt32(dr["manufacturerid"]);
                            objQuotation.State = Convert.ToString(dr["statename"]);

                            objQuotation.PriceQuoteId = pqId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in GetPriceQuoteById({0}) : Msg : {1}", pqId, ex.Message));
            }

            return objQuotation;
        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 21th October 2015
        /// Summary : To capture maufacturer lead for bikewale pricequotes 
        /// </summary>
        /// <param name="objLead"></param>
        /// <returns>Lead submission status</returns>
        public bool SaveManufacturerLead(ManufacturerLeadEntity objLead)
        {
            bool status = false;
            try
            {
                if (objLead != null && objLead.PQId > 0 && objLead.DealerId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "savemanufacturerlead_03032017";

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, objLead.Name));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_email", DbType.String, 150, objLead.Email));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_mobile", DbType.String, 10, objLead.Mobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqid", DbType.Int64, objLead.PQId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_leadsourceid", DbType.Int16, objLead.LeadSourceId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pincode", DbType.String, objLead.PinCodeId));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int64, objLead.DealerId));

                        status = SqlReaderConvertor.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase));
                    }
                }
            }

            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in SaveManufacturerLead -  Message : {0}", ex.Message));
            }

            return status;

        }

        public BajajFinanceLeadEntity GetBajajFinanceBikeMappingInfo(uint versionid, uint pincodeId)
        {
            BajajFinanceLeadEntity objQuotation = null;
            try
            {
                objQuotation = new BajajFinanceLeadEntity();
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getbajajfinancebikemapping";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.UInt32, versionid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pincodeid", DbType.UInt32, pincodeId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objQuotation.ProductMake = Convert.ToString(dr["icas_makeid"]);
                            objQuotation.Model = Convert.ToString(dr["icas_model_id"]);
                            objQuotation.City = Convert.ToString(dr["F1_CityId"]);
                            objQuotation.State = Convert.ToString(dr["F1_State_code"]);
                            objQuotation.PinCode = Convert.ToString(dr["pincode"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(String.Format("Error in GetPriceQuoteById({0}) : Msg : {1}", 0, ex.Message));
            }

            return objQuotation;
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
                Logs.WriteErrorLog(String.Format("Error in IsDealerDailyLeadLimitExceeds({0}) : Msg : {1}", 0, ex.Message));
            }

            return islimitexceeds;

        }
    }
}
