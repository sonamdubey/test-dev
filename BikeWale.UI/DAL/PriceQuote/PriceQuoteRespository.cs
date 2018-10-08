using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, CurrentUser.GetClientIP()));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, pqParams.DealerId));

                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqsourceid", DbType.Byte, (pqParams.PQLeadId.HasValue) ? pqParams.PQLeadId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbType.String, 500, (!String.IsNullOrEmpty(pqParams.UTMA)) ? pqParams.UTMA : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_refpqid", DbType.Int64, (pqParams.RefPQId.HasValue) ? pqParams.RefPQId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, 500, (!String.IsNullOrEmpty(pqParams.UTMZ)) ? pqParams.UTMZ : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbType.String, 25, (!String.IsNullOrEmpty(pqParams.DeviceId)) ? pqParams.DeviceId : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int64, ParameterDirection.Output));
                        // LogLiveSps.LogSpInGrayLog(cmd);
                        MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                        quoteId = Convert.ToUInt64(cmd.Parameters["par_quoteid"].Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("SavePriceQuote sql ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SavePriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return quoteId;
        }

        /// <summary>
        /// Summary : Function to Get the price quote by price quote id.
        /// Modified by :   Sumit Kate on 18 Aug 2016
        /// Description :   Created new SP to return state name in result. Replaced in/out parameters with DataReader approach
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
                    cmd.CommandText = "getpricequote_new_18082016";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int64, pqId));
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
                HttpContext.Current.Trace.Warn("GetPriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return objQuotation;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 19 June 2018
        /// Description : removed PQId dependency
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity GetPriceQuote(uint cityId, uint versionId)
        {
            Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity objQuotation = null;
            try
            {
                objQuotation = new Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity();
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getpricequote_new_19062018";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.PriceQuote.PriceQuoteRepository.GetPriceQuote");
            }

            return objQuotation;
        }

        /// <summary>
        /// Summary : Function to Get the price quote by price quote id.
        /// Modified by :   Vivek Gupta on 29th Aug 2016
        /// Description :   Created new SP to return manufacturere Ad value and created overload of the function
        /// Modifide By :- Subodh jain on 02 March 2017
        /// Summary:- added manufacturer campaign leadpopup changes
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Changed SP from 'getpricequote_new_28062017' to 'getpricequote_new_30082017', removed IsGstPrice flag
        /// <param name="pqId">price quote id. Only positive numbers are allowed</param>
        /// <returns>Returns price quote object.</returns>
        public BikeQuotationEntity GetPriceQuoteById(ulong pqId, LeadSourceEnum page)
        {
            BikeQuotationEntity objQuotation = null;
            try
            {
                objQuotation = new BikeQuotationEntity();
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getpricequote_new_30082017";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_quoteid", DbType.Int64, pqId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pageid", DbType.Int32, Convert.ToInt32(page)));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objQuotation.CampaignId = SqlReaderConvertor.ToUInt32(dr["campaignId"]);
                            objQuotation.ManufacturerName = Convert.ToString(dr["organization"]);
                            objQuotation.MaskingNumber = Convert.ToString(dr["MaskingNumber"]);
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
                            objQuotation.ManufacturerAd = Convert.ToString(dr["manufacturerAd"]);
                            objQuotation.PriceQuoteId = pqId;
                            objQuotation.LeadCapturePopupDescription = Convert.ToString(dr["LeadCapturePopupDescription"]);
                            objQuotation.LeadCapturePopupHeading = Convert.ToString(dr["LeadCapturePopupHeading"]);
                            objQuotation.LeadCapturePopupMessage = Convert.ToString(dr["LeadCapturePopupMessage"]);
                            objQuotation.PinCodeRequired = SqlReaderConvertor.ToBoolean(dr["PinCodeRequired"]);
                            objQuotation.EmailRequired = SqlReaderConvertor.ToBoolean(dr["EmailIDRequired"]);
                            objQuotation.DealersRequired = SqlReaderConvertor.ToBoolean(dr["DealersRequired"]);
                            objQuotation.CityMaskingName = Convert.ToString(dr["citymaskingname"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetPriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            return objQuotation;
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 20 June 2018
        /// Description : removed PQId dependency
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="versionId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity GetPriceQuote(uint cityId, uint versionId, LeadSourceEnum page)
        {
            Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity objQuotation = null;
            try
            {
                objQuotation = new Bikewale.Entities.PriceQuote.v2.BikeQuotationEntity();
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getpricequote_new_20062018";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pageid", DbType.Int32, Convert.ToInt32(page)));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null && dr.Read())
                        {
                            objQuotation.CampaignId = SqlReaderConvertor.ToUInt32(dr["campaignId"]);
                            objQuotation.ManufacturerName = Convert.ToString(dr["organization"]);
                            objQuotation.MaskingNumber = Convert.ToString(dr["MaskingNumber"]);
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
                            objQuotation.ManufacturerAd = Convert.ToString(dr["manufacturerAd"]);
                            objQuotation.LeadCapturePopupDescription = Convert.ToString(dr["LeadCapturePopupDescription"]);
                            objQuotation.LeadCapturePopupHeading = Convert.ToString(dr["LeadCapturePopupHeading"]);
                            objQuotation.LeadCapturePopupMessage = Convert.ToString(dr["LeadCapturePopupMessage"]);
                            objQuotation.PinCodeRequired = SqlReaderConvertor.ToBoolean(dr["PinCodeRequired"]);
                            objQuotation.EmailRequired = SqlReaderConvertor.ToBoolean(dr["EmailIDRequired"]);
                            objQuotation.DealersRequired = SqlReaderConvertor.ToBoolean(dr["DealersRequired"]);
                            objQuotation.CityMaskingName = Convert.ToString(dr["citymaskingname"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.PriceQuote.PriceQuoteRepository.GetPriceQuote({0},{1},{2})", cityId, versionId, page));
            }

            return objQuotation;
        }


        /// <summary>
        /// Summary : function to get the price quote by providing all the necessory parameters to get the pq.
        /// </summary>
        /// <param name="pqParams">Price quote parameters.</param>
        /// <returns>Returns price qutoe object.</returns>
        [Obsolete("Unused")]
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



                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objVersionInfo.Add(new OtherVersionInfoEntity
                                {
                                    VersionId = SqlReaderConvertor.ToUInt32(dr["VersionId"]),
                                    VersionName = Convert.ToString(dr["VersionName"]),
                                    OnRoadPrice = SqlReaderConvertor.ToUInt64(dr["OnRoadPrice"]),
                                    Price = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                    RTO = SqlReaderConvertor.ToUInt32(dr["RTO"]),
                                    Insurance = SqlReaderConvertor.ToUInt32(dr["Insurance"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetOtherVersionsPrices ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
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
                    // LogLiveSps.LogSpInGrayLog(cmd);
                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase)))
                        isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return isUpdated;
        }

		/// <summary>
		/// Created By  : Rajan Chauhan on 27 June 2018
		/// Description : Method to update version, color in PQ details based on leadId
		/// </summary>
		/// <param name="leadId"></param>
		/// <param name="pqParams"></param>
		/// <returns></returns>
		public bool UpdatePriceQuoteDetailsByLeadId(UInt32 leadId, PriceQuoteParametersEntity pqParams)
		{
			bool isUpdated = false;

			try
			{
				using (DbCommand cmd = DbFactory.GetDBCommand())
				{
					cmd.CommandText = "updatepricequotedetailsbyleadid";
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.Add(DbFactory.GetDbParam("par_leadid", DbType.Int32, leadId));
					cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, pqParams.VersionId));
					cmd.Parameters.Add(DbFactory.GetDbParam("par_bikecolorid", DbType.Int32, (pqParams.ColorId > 0) ? pqParams.ColorId : Convert.DBNull));
					// LogLiveSps.LogSpInGrayLog(cmd);
					if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase)))
						isUpdated = true;
				}
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

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


                    // LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
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

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.SByte, topCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objPrice = new List<PriceQuoteOfTopCities>();
                        while (dr.Read())
                        {
                            objPrice.Add(new PriceQuoteOfTopCities
                            {
                                CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                CityName = Convert.ToString(dr["City"]),
                                CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                OnRoadPrice = SqlReaderConvertor.ToUInt32(dr["OnRoadPrice"]),
                                Make = Convert.ToString(dr["Make"]),
                                MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                Model = Convert.ToString(dr["Model"]),
                                ModelMaskingName = Convert.ToString(dr["ModelMaskingName"])

                            });
                        }

                        if (dr != null)
                            dr.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + " inputs: modelId : " + modelId + " : topCount :" + topCount);
                
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
                    cmd.CommandText = "getmodelpricefornearestcities_15092017";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_toprecords", DbType.Int16, topCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        objPrice = new List<PriceQuoteOfTopCities>();

                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objPrice.Add(new PriceQuoteOfTopCities
                                {
                                    CityName = Convert.ToString(dr["City"]),
                                    CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                    OnRoadPrice = SqlReaderConvertor.ToUInt32(dr["OnRoadPrice"]),
                                    Make = Convert.ToString(dr["Make"]),
                                    MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                    Model = Convert.ToString(dr["Model"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"])
                                });
                            }
                            dr.Close();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + String.Format(" inputs: modelId : {0} : topCount : {1} : cityId : {2}", modelId, topCount, cityId));
                
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
        /// Modified By :   Sumit Kate on 15 May 2017
        /// Description :   New sp version getversionpricesbymodelid
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Changed SP from 'getversionpricesbymodelid_28062017' to 'getversionpricesbymodelid_30082017', removed IsGstPrice flag
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
                    cmd.CommandText = "getversionpricesbymodelid_02112017";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_hasareasincity", DbType.Boolean, ParameterDirection.Output));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        bikePrices = new List<BikeQuotationEntity>();
                        while (dr.Read())
                        {
                            bikePrices.Add(new BikeQuotationEntity
                            {
                                VersionId = SqlReaderConvertor.ToUInt32(dr["BikeVersionId"]),
                                VersionName = Convert.ToString(dr["Version"]),
                                MakeName = Convert.ToString(dr["Make"]),
                                MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                ModelName = Convert.ToString(dr["Model"]),
                                ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                CityMaskingName = Convert.ToString(dr["CityMaskingName"]),
                                City = Convert.ToString(dr["City"]),
                                ExShowroomPrice = SqlReaderConvertor.ToUInt64(dr["Price"]),
                                RTO = SqlReaderConvertor.ToUInt32(dr["RTO"]),
                                Insurance = SqlReaderConvertor.ToUInt32(dr["Insurance"]),
                                OnRoadPrice = SqlReaderConvertor.ToUInt64(dr["OnRoadPrice"]),
                                OriginalImage = Convert.ToString(dr["OriginalImagePath"]),
                                HostUrl = Convert.ToString(dr["HostUrl"]),
                                MakeId = SqlReaderConvertor.ToUInt32(Convert.ToString(dr["MakeId"])),
                                IsModelNew = SqlReaderConvertor.ToBoolean(dr["IsModelNew"]),
                                IsVersionNew = SqlReaderConvertor.ToBoolean(dr["IsVersionNew"]),
                                IsScooterOnly = SqlReaderConvertor.ToBoolean(dr["isScooterOnly"])
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
                ErrorClass.LogError(ex, string.Format("{0} - {1}", HttpContext.Current.Request.ServerVariables["URL"], "GetVersionPricesByModelId"));
                
            }

            return bikePrices;
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 28 September 2018
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeQuotationEntity> GetVersionPricesByModelId(uint modelId, uint cityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 07 Apr 2017
        /// Description :   GetOtherVersionsPrices
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<OtherVersionInfoEntity> GetOtherVersionsPrices(uint modelId, uint cityId)
        {
            ICollection<OtherVersionInfoEntity> objVersionInfo = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getpricequoteversionsbymodelcity_22052018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId));
                    objVersionInfo = new List<OtherVersionInfoEntity>();



                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objVersionInfo.Add(new OtherVersionInfoEntity
                                {
                                    VersionId = SqlReaderConvertor.ToUInt32(dr["VersionId"]),
                                    VersionName = Convert.ToString(dr["VersionName"]),
                                    OnRoadPrice = SqlReaderConvertor.ToUInt64(dr["OnRoadPrice"]),
                                    Price = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                    RTO = SqlReaderConvertor.ToUInt32(dr["RTO"]),
                                    Insurance = SqlReaderConvertor.ToUInt32(dr["Insurance"]),
                                    HostUrl = Convert.ToString(dr["hosturl"]),
                                    OriginalImagePath = Convert.ToString(dr["originalimagepath"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetOtherVersionsPrices({0},{1})", modelId, cityId));
            }
            return objVersionInfo;
        }
        /// <summary>
        /// Summary; To Fetch manufacturer Dealers
        /// </summary>
        /// <returns></returns>
        public IDictionary<uint, List<ManufacturerDealer>> GetManufacturerDealers(uint dealerId)
        {
            IDictionary<uint, List<ManufacturerDealer>> dealer = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getmanufacturerdealers_12072018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, dealerId));
                    dealer = new Dictionary<uint, List<ManufacturerDealer>>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                uint key=SqlReaderConvertor.ToUInt32(dr["cityid"]);
                                if(dealer.ContainsKey(key))
                                {
                                    dealer[key].Add(new ManufacturerDealer
                                    {
                                        Id = Convert.ToString(dr["id"]),
                                        DealerName = Convert.ToString(dr["bwdealername"]),
                                        City = Convert.ToString(dr["city"]),
                                        CityId = key,
                                        DealerArea = Convert.ToString(dr["dealerarea"]),
                                        DealerId = SqlReaderConvertor.ToUInt32(Convert.ToString(dr["dealerid"]))
                                    });    
                                }
                                else
                                {
                                    dealer.Add(key, new List<ManufacturerDealer> 
                                    { 
                                        new ManufacturerDealer
                                        {
                                            Id = Convert.ToString(dr["id"]),
                                            DealerName = Convert.ToString(dr["bwdealername"]),
                                            City = Convert.ToString(dr["city"]),
                                            CityId = key,
                                            DealerArea = Convert.ToString(dr["dealerarea"]),
                                            DealerId = SqlReaderConvertor.ToUInt32(Convert.ToString(dr["dealerid"]))
                                        }
                                    });
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, String.Format("PriceQuoteRepository.GetManufacturerDealers()"));
            }
            return dealer;
        }

        public void GetDealerVersionsPriceByModelCity(IEnumerable<BikeVersionMinSpecs> versionList, uint cityId, uint modelId, uint dealerId = 0)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Author  : kartik on 20 jun 2018 price qoute changes
        /// </summary>
        /// <param name="pqParams"></param>
        /// <returns></returns>
        public string RegisterPriceQuoteV2(Entities.PriceQuote.v2.PriceQuoteParametersEntity pqParams)
        {
            try
            {

                if (pqParams.VersionId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "savepricequote_19062018";
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_guid", DbType.String, 40, pqParams.GUID));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_bikeversionid", DbType.Int32, pqParams.VersionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, pqParams.CityId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Int32, (pqParams.AreaId > 0) ? pqParams.AreaId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customerid", DbType.Int32, pqParams.CustomerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customername", DbType.String, 50, pqParams.CustomerName));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customeremail", DbType.String, 100, pqParams.CustomerEmail));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_customermobile", DbType.String, 20, pqParams.CustomerMobile));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_clientip", DbType.String, 40, !String.IsNullOrEmpty(pqParams.ClientIP) ? pqParams.ClientIP : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_sourceid", DbType.Byte, pqParams.SourceId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_dealerid", DbType.Int32, pqParams.DealerId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_deviceid", DbType.String, 25, (!String.IsNullOrEmpty(pqParams.DeviceId)) ? pqParams.DeviceId : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utma", DbType.String, 500, (!String.IsNullOrEmpty(pqParams.UTMA)) ? pqParams.UTMA : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_utmz", DbType.String, 500, (!String.IsNullOrEmpty(pqParams.UTMZ)) ? pqParams.UTMZ : null));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_pqsourceid", DbType.Byte, (pqParams.PQLeadId.HasValue) ? pqParams.PQLeadId : Convert.DBNull));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_refguid", DbType.String, 40, !String.IsNullOrEmpty(pqParams.RefGUID) ? pqParams.RefGUID : Convert.DBNull)); // RefGUID string 40
                        MySqlDatabase.InsertQuery(cmd, ConnectionType.MasterDatabase);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex,String.Format("PQConsumer.PriceQuoteRepository.RegisterPriceQuote : SqlException {0}", JsonConvert.SerializeObject(pqParams)));
            }
            return "";
        }

		public bool GetMLAStatus(int makeId, uint cityId)
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// Created by  : Pratibha Verma on 29 August 2018
        /// Description : returns version price by cityid
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public VersionPrice GetVersionPriceByCityId(uint versionId, uint cityId)
        {
            VersionPrice versionPrice = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getversionpricebycityid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionid", DbType.UInt32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            versionPrice = new VersionPrice();
                            if (dr.Read())
                            {
                                versionPrice.VersionId = SqlReaderConvertor.ToUInt32(dr["versionid"]);
                                versionPrice.Exshowroom = SqlReaderConvertor.ToUInt32(dr["exshowroom"]);
                                versionPrice.RTO = SqlReaderConvertor.ToUInt32(dr["rto"]);
                                versionPrice.Insurance = SqlReaderConvertor.ToUInt32(dr["insurance"]);
                                versionPrice.VersionPrice = SqlReaderConvertor.ToUInt32(dr["onroad"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.PriceQuote.PriceQuoteRepository.GetVersionPriceByCityId(versionId = {0}, cityId = {1})", versionId, cityId));
            }
            return versionPrice;
        }

        public IList<PriceCategory> GetVersionPriceListByCityId(uint versionId, uint cityId)
        {
            throw new NotImplementedException();
        }
    }   // Class
}   // namespace