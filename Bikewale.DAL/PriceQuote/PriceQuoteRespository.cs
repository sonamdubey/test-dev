using Bikewale.CoreDAL;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.PriceQuote
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// Summary : class have functions related to price quote.
    /// Modified By :   Sumit Kate
    /// Date        :   16 Oct 2015
    /// Description :   Implemented newly added method of IPriceQuote interface
    /// </summary>
    public class PriceQuoteRepository : IPriceQuote
    {
        /// <summary>
        /// Summary : function to save the price quote.
        /// Modified By : Sadhana Upadhyay on 24th Oct 2014
        /// Summary : Added AreaId varible and removed customerid, customer name, customer email, customer mobile variable
        /// Modified By : Ashis G. Kamble on 22 Nov 2012.
        /// Added : Check whether version id is null or not. If null do not save pricequote.
        /// Modified By : Sadhana Upadhyay on 20 July 2015
        /// Summary : Added Dealer id as parameter to save in newbikepricequotes table
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : save utma. utmz, PQ Source id, device id 
        /// Modified by : Lucky Rathore on 20 April 2016
        /// Description : Added RefPQId .
        /// </summary>
        /// <param name="pqParams">All necessory parameters to save the price quote</param>
        /// <returns>Returns registered price quote id</returns>
        public ulong RegisterPriceQuote(PriceQuoteParametersEntity pqParams)
        {
            ulong quoteId = 0;
            try
            {

                if (pqParams.VersionId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "savepricequote_new_20042016";

                        //cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = pqParams.CityId;

                        //if (pqParams.AreaId > 0)
                        //{
                        //    cmd.Parameters.Add("@AreaId", SqlDbType.Int).Value = pqParams.AreaId;
                        //}

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], pqParams.CityId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (pqParams.AreaId > 0) ? pqParams.AreaId : Convert.DBNull));

                        //cmd.Parameters.Add("@BikeVersionId", SqlDbType.Int).Value = pqParams.VersionId;
                        //cmd.Parameters.Add("@SourceId", SqlDbType.TinyInt).Value = pqParams.SourceId;
                        //cmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 40).Value = String.IsNullOrEmpty(pqParams.ClientIP) ? Convert.DBNull : pqParams.ClientIP;
                        //cmd.Parameters.Add("@QuoteId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        //cmd.Parameters.Add("@DealerId", SqlDbType.Int).Value = pqParams.DealerId;

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], pqParams.VersionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], pqParams.SourceId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 40, String.IsNullOrEmpty(pqParams.ClientIP) ? Convert.DBNull : pqParams.ClientIP));
                       
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], pqParams.DealerId));

                        //if (pqParams.PQLeadId.HasValue)
                        //{
                        //    cmd.Parameters.Add("@PQSourceId", SqlDbType.TinyInt).Value = pqParams.PQLeadId.Value;
                        //}
                        //if (!String.IsNullOrEmpty(pqParams.UTMA))
                        //{
                        //    cmd.Parameters.Add("@utma", SqlDbType.VarChar, 100).Value = pqParams.UTMA;
                        //}
                        //if (!String.IsNullOrEmpty(pqParams.UTMZ))
                        //{
                        //    cmd.Parameters.Add("@utmz", SqlDbType.VarChar, 100).Value = pqParams.UTMZ;
                        //}
                        //if (!String.IsNullOrEmpty(pqParams.DeviceId))
                        //{
                        //    cmd.Parameters.Add("@deviceId", SqlDbType.VarChar, 25).Value = pqParams.DeviceId;
                        //}
                        //if(pqParams.RefPQId.HasValue)
                        //{
                        //    cmd.Parameters.Add("@refPQId", SqlDbType.Int).Value = pqParams.RefPQId.Value;
                        //}

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqsourceid", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], (pqParams.PQLeadId.HasValue) ? pqParams.PQLeadId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, (!String.IsNullOrEmpty(pqParams.UTMA)) ? pqParams.UTMA : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 100, (!String.IsNullOrEmpty(pqParams.UTMZ)) ? pqParams.UTMZ : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 25, (!String.IsNullOrEmpty(pqParams.DeviceId)) ? pqParams.DeviceId : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_refpqid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], (pqParams.RefPQId.HasValue) ? pqParams.RefPQId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));

                        MySqlDatabase.ExecuteNonQuery(cmd);
                            quoteId = Convert.ToUInt64(cmd.Parameters["par_quoteid"].Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SavePriceQuote sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SavePriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return quoteId;
        }

        /// <summary>
        /// Summary : Function to Get the price quote by price quote id.
        /// </summary>
        /// <param name="pqId">price quote id. Only positive numbers are allowed</param>
        /// <returns>Returns price quote object.</returns>
        public BikeQuotationEntity GetPriceQuoteById(ulong pqId)
        {
            BikeQuotationEntity objQuotation = null;
            try
            {
                objQuotation = new BikeQuotationEntity();
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getpricequote_new_01022016";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_exshowroomprice", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_rto", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_insurance", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_onroadprice", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_city", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_numofrows", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_manufacturerid", DbParamTypeMapper.GetInstance[SqlDbType.Int], ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd);

                        int numberOfRecords = Convert.ToInt32(cmd.Parameters["par_numofrows"].Value);
                        if (numberOfRecords > 0)
                        {
                            objQuotation.ExShowroomPrice = Convert.ToUInt64(cmd.Parameters["par_exshowroomprice"].Value);
                            objQuotation.RTO = Convert.ToUInt32(cmd.Parameters["par_rto"].Value);
                            objQuotation.Insurance = Convert.ToUInt32(cmd.Parameters["par_insurance"].Value);
                            objQuotation.OnRoadPrice = Convert.ToUInt64(cmd.Parameters["par_onroadprice"].Value);
                            objQuotation.MakeName = Convert.ToString(cmd.Parameters["par_makename"].Value);
                            objQuotation.ModelName = Convert.ToString(cmd.Parameters["par_modelname"].Value);
                            objQuotation.VersionName = Convert.ToString(cmd.Parameters["par_versionname"].Value);
                            objQuotation.City = Convert.ToString(cmd.Parameters["par_city"].Value);
                            objQuotation.VersionId = Convert.ToUInt32(cmd.Parameters["par_versionid"].Value);
                            objQuotation.CampaignId = Convert.ToUInt32(cmd.Parameters["par_campaignid"].Value);
                            objQuotation.ManufacturerId = Convert.ToUInt32(cmd.Parameters["par_manufacturerid"].Value);

                            objQuotation.PriceQuoteId = pqId;
                        }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetPriceQuote sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetPriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objQuotation;
        }

        /// <summary>
        /// Summary : function to get the price quote by providing all the necessory parameters to get the pq.
        /// </summary>
        /// <param name="pqParams">Price quote parameters.</param>
        /// <returns>Returns price qutoe object.</returns>
        public BikeQuotationEntity GetPriceQuote(PriceQuoteParametersEntity pqParams)
        {
            ulong pqId = RegisterPriceQuote(pqParams);

            BikeQuotationEntity objQuotation = GetPriceQuoteById(pqId);

            return objQuotation;
        }

        /// <summary>
        /// Summary : Function to get the other versions of the model along with on road prices.
        /// </summary>
        /// <param name="pqId">Price quote id. Only positive numbers are allowed.</param>
        /// <returns>Returns list containing all the versions with on road prices.</returns>
        public List<OtherVersionInfoEntity> GetOtherVersionsPrices(ulong pqId)
        {
            List<OtherVersionInfoEntity> objVersionInfo = null;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getpricequoteversions_new";

                    // cmd.Parameters.Add("@QuoteId", SqlDbType.BigInt).Value = pqId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], pqId));
                    objVersionInfo = new List<OtherVersionInfoEntity>();



                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        while (dr != null && dr.Read())
                        {
                            objVersionInfo.Add(new OtherVersionInfoEntity
                            {
                                VersionId = Convert.ToUInt32(dr["VersionId"]),
                                VersionName = Convert.ToString(dr["VersionName"]),
                                OnRoadPrice = Convert.ToUInt64(dr["OnRoadPrice"]),
                                Price = Convert.ToUInt32(dr["Price"]),
                                RTO = Convert.ToUInt32(dr["RTO"]),
                                Insurance = Convert.ToUInt32(dr["Insurance"])
                            });
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetOtherVersionsPrices sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetOtherVersionsPrices ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objVersionInfo;
        }


        /// <summary>
        /// Author      :   Sumit Kate
        /// Created On  :   16 Oct 2015
        /// Description :   Updates the price quote data
        /// Modified By : Sushil Kumar On 11th Nov 2015
        /// Summary : Update colorId in PQ_NewBikeDealerPriceQuotes
        /// </summary>
        /// <param name="pqParams"></param>
        /// <returns></returns>
        public bool UpdatePriceQuote(UInt32 pqId, PriceQuoteParametersEntity pqParams)
        {
            bool isUpdated = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "updatepricequotebikeversion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbParamTypeMapper.GetInstance[SqlDbType.BigInt], pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbParamTypeMapper.GetInstance[SqlDbType.Int], pqParams.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikecolorid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (pqParams.ColorId > 0) ? pqParams.ColorId : Convert.DBNull));

                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd)))
                        isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isUpdated;
        }

        /// <summary>
        /// Author          :   Sumit Kate
        /// Description     :   18 Nov 2015
        /// Created On      :   Saves the Booking Journey State
        /// </summary>
        /// <param name="pqId">PQ Id</param>
        /// <param name="state">PriceQuoteStates enum</param>
        /// <returns></returns>
        public bool SaveBookingState(uint pqId, PriceQuoteStates state)
        {
            bool isUpdated = false;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "savepqbookingstate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbParamTypeMapper.GetInstance[SqlDbType.Int], pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbParamTypeMapper.GetInstance[SqlDbType.Int], Convert.ToInt32(state)));


                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd)))
                        isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isUpdated;
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
                        cmd.CommandText = "GetPriceQuoteData_02052016";
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.Add("@QuoteId", SqlDbType.Int).Value = pqId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbParamTypeMapper.GetInstance[SqlDbType.Int], pqId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        objQuotation = new PriceQuoteParametersEntity();
                        while (dr!=null && dr.Read())
                        {
                            objQuotation.AreaId = !Convert.IsDBNull(dr["AreaId"]) ? Convert.ToUInt32(dr["AreaId"]) : default(UInt32);
                            objQuotation.CityId = !Convert.IsDBNull(dr["cityid"]) ? Convert.ToUInt32(dr["cityid"]) : default(UInt32);
                            objQuotation.VersionId = !Convert.IsDBNull(dr["BikeVersionId"]) ? Convert.ToUInt32(dr["BikeVersionId"]) : default(UInt32);
                            objQuotation.DealerId = !Convert.IsDBNull(dr["DealerId"]) ? Convert.ToUInt32(dr["DealerId"]) : default(UInt32);
                                objQuotation.CampaignId = !Convert.IsDBNull(dr["CampaignId"]) ? Convert.ToUInt32(dr["CampaignId"]) : default(UInt32);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objQuotation;
        }
    }   // Class
}   // namespace