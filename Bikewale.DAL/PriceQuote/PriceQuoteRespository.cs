//using Bikewale.CoreDAL;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Notifications.CoreDAL;
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

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, pqParams.CityId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Int32, (pqParams.AreaId > 0) ? pqParams.AreaId : Convert.DBNull));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, pqParams.VersionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, pqParams.SourceId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, Bikewale.CoreDAL.CommonOpn.GetClientIP()));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, pqParams.DealerId));

                            
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqsourceid", DbType.Byte, (pqParams.PQLeadId.HasValue) ? pqParams.PQLeadId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbType.String, 500, (!String.IsNullOrEmpty(pqParams.UTMA)) ? pqParams.UTMA : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, 500, (!String.IsNullOrEmpty(pqParams.UTMZ)) ? pqParams.UTMZ : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbType.String, 25, (!String.IsNullOrEmpty(pqParams.DeviceId)) ? pqParams.DeviceId : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_refpqid", DbType.Int64, (pqParams.RefPQId.HasValue) ? pqParams.RefPQId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int64, ParameterDirection.Output));
LogLiveSps.LogSpInGrayLog(cmd);
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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int64, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_exshowroomprice", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_rto", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_insurance", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_onroadprice", DbType.Int64, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makename", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelname", DbType.String, 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionname", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_city", DbType.String, 50, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_numofrows", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_campaignid", DbType.Int32, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_manufacturerid", DbType.Int32, ParameterDirection.Output));
                        LogLiveSps.LogSpInGrayLog(cmd);
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int64, pqId));
                    objVersionInfo = new List<OtherVersionInfoEntity>();



                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr !=null)
                        {
                            while (dr.Read())
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
                            dr.Close();
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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int64, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, pqParams.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_bikecolorid", DbType.Int32, (pqParams.ColorId > 0) ? pqParams.ColorId : Convert.DBNull));
                        LogLiveSps.LogSpInGrayLog(cmd);
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int32, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, Convert.ToInt32(state)));


                        LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd);
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int32, pqId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        objQuotation = new PriceQuoteParametersEntity();
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objQuotation.AreaId = !Convert.IsDBNull(dr["AreaId"]) ? Convert.ToUInt32(dr["AreaId"]) : default(UInt32);
                                objQuotation.CityId = !Convert.IsDBNull(dr["cityid"]) ? Convert.ToUInt32(dr["cityid"]) : default(UInt32);
                                objQuotation.VersionId = !Convert.IsDBNull(dr["BikeVersionId"]) ? Convert.ToUInt32(dr["BikeVersionId"]) : default(UInt32);
                                objQuotation.DealerId = !Convert.IsDBNull(dr["DealerId"]) ? Convert.ToUInt32(dr["DealerId"]) : default(UInt32);
                                objQuotation.CampaignId = !Convert.IsDBNull(dr["CampaignId"]) ? Convert.ToUInt32(dr["CampaignId"]) : default(UInt32);
                            }
                            dr.Close();
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

        /// <summary>
        /// Created By : Vivek Gupta
        /// Date : 20-05-2016
        /// Desc : Fetch BW Pricequote of top cities by modelId
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public IEnumerable<PriceQuoteOfTopCities> FetchPriceQuoteOfTopCities(uint modelId, uint topCount)
        {
            IList<PriceQuoteOfTopCities> objPrice = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getmodelpricefortopcities";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], topCount));
                        
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        objPrice = new List<PriceQuoteOfTopCities>();
                        while (dr.Read())
                        {
                            objPrice.Add(new PriceQuoteOfTopCities
                            {
                                CityName = !Convert.IsDBNull(dr["City"]) ? Convert.ToString(dr["City"]) : default(string),
                                CityMaskingName = !Convert.IsDBNull(dr["CityMaskingName"]) ? Convert.ToString(dr["CityMaskingName"]) : default(string),
                                OnRoadPrice = !Convert.IsDBNull(dr["OnRoadPrice"]) ? Convert.ToUInt32(dr["OnRoadPrice"]) : default(UInt32),
                                Make = Convert.IsDBNull(dr["Make"]) ? default(string) : Convert.ToString(dr["Make"]),
                                MakeMaskingName = Convert.IsDBNull(dr["MakeMaskingName"]) ? default(string) : Convert.ToString(dr["MakeMaskingName"]),
                                Model = Convert.IsDBNull(dr["Model"]) ? default(string) : Convert.ToString(dr["Model"]),
                                ModelMaskingName = Convert.IsDBNull(dr["ModelMaskingName"]) ? default(string) : Convert.ToString(dr["ModelMaskingName"])

                            });
                        }

                        if (dr != null)
                            dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " inputs: modelId : " + modelId + " : topCount :" + topCount);
                objErr.SendMail();
            }

            return objPrice;
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 23 May 2016
        /// Summary : Function to get the pricing of the nearest cities for the given model and city.
        /// Modified by :   Sumit Kate on 21 Jun 2016
        /// Description :   Added Null check for Data reader and Log the city id for exception emails
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="topCount"></param>
        /// <returns>Returns List of the pricing for the nearest cities for the given model and city</returns>
        public IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount)
        {
            IList<PriceQuoteOfTopCities> objPrice = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getmodelpricefornearestcities";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_toprecords", DbParamTypeMapper.GetInstance[SqlDbType.TinyInt], topCount));
                        
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        objPrice = new List<PriceQuoteOfTopCities>();

                        if (dr != null)
                        {
                                while (dr.Read())
                                {
                                    objPrice.Add(new PriceQuoteOfTopCities
                                    {
                                        CityName = Convert.IsDBNull(dr["City"]) ? default(string) : Convert.ToString(dr["City"]),
                                        CityMaskingName = Convert.IsDBNull(dr["CityMaskingName"]) ? default(string) : Convert.ToString(dr["CityMaskingName"]),
                                        OnRoadPrice = Convert.IsDBNull(dr["OnRoadPrice"]) ? default(UInt32) : Convert.ToUInt32(dr["OnRoadPrice"]),
                                        Make = Convert.IsDBNull(dr["Make"]) ? default(string) : Convert.ToString(dr["Make"]),
                                        MakeMaskingName = Convert.IsDBNull(dr["MakeMaskingName"]) ? default(string) : Convert.ToString(dr["MakeMaskingName"]),
                                        Model = Convert.IsDBNull(dr["Model"]) ? default(string) : Convert.ToString(dr["Model"]),
                                        ModelMaskingName = Convert.IsDBNull(dr["ModelMaskingName"]) ? default(string) : Convert.ToString(dr["ModelMaskingName"])
                                    });
                                }
                            dr.Close();
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + String.Format(" inputs: modelId : {0} : topCount : {1} : cityId : {2}", modelId, topCount, cityId));
                objErr.SendMail();
            }

            return objPrice;
        }
        /// <summary>
        /// Author: Sangram Nandkhile on 25 May 2016
        /// Summary: Get bike versions and prices by model Id
        /// Modified By : Sushil Kumar
        /// Modified On : 6th June 2016
        /// Description : Added makeId property to get makeId for dealers card widget 
        /// Modified By : Sushil Kumar on 8th June 2016
        /// Description : Added ismodelnew and isversion new data 
        /// </summary>
        /// <param name="modelId">Model Id</param>
        /// <param name="cityId">City Id </param>
        /// <param name="HasArea">If city has models in those areas</param>
        /// <returns></returns>
        public IEnumerable<BikeQuotationEntity> GetVersionPricesByModelId(uint modelId, uint cityId, out bool HasArea)
        {
            List<BikeQuotationEntity> bikePrices = null;
            HasArea = false;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "GetVersionPricesByModelId";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbParamTypeMapper.GetInstance[SqlDbType.Int], modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbParamTypeMapper.GetInstance[SqlDbType.Int], cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hasareasincity", DbParamTypeMapper.GetInstance[SqlDbType.Bit], ParameterDirection.Output));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        bikePrices = new List<BikeQuotationEntity>();
                        while (dr.Read())
                        {
                            bikePrices.Add(new BikeQuotationEntity
                            {
                                VersionId = Convert.ToUInt32(Convert.ToString(dr["BikeVersionId"])),
                                VersionName = Convert.ToString(dr["Version"]),
                                MakeName = Convert.ToString(dr["Make"]),
                                MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                ModelName = Convert.ToString(dr["Model"]),
                                ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                CityId = Convert.ToUInt32(dr["CityId"]),
                                CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                City = Convert.ToString(dr["City"]),
                                ExShowroomPrice = Convert.ToUInt64(dr["Price"]),
                                RTO = Convert.ToUInt32(dr["RTO"]),
                                Insurance = Convert.ToUInt32(dr["Insurance"]),
                                OnRoadPrice = Convert.ToUInt64(dr["OnRoadPrice"]),
                                OriginalImage = Convert.ToString(dr["OriginalImagePath"]),
                                HostUrl = Convert.ToString(dr["HostUrl"]),
                                MakeId = Convert.ToUInt32(Convert.ToString(dr["MakeId"])),
                                IsModelNew = Convert.ToBoolean(dr["IsModelNew"]),
                                IsVersionNew = Convert.ToBoolean(dr["IsVersionNew"])

                            });

                        }
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                HasArea = Convert.ToBoolean(dr["par_hasareasincity"].ToString());
                            }
                        }

                        if (dr != null) dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0} - {1}", HttpContext.Current.Request.ServerVariables["URL"], "GetVersionPricesByModelId"));
                objErr.SendMail();
            }

            return bikePrices;
        }
    }   // Class
}   // namespace