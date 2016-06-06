using Bikewale.CoreDAL;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
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
            Database db = null;
            try
            {

                if (pqParams.VersionId > 0)
                {
                    db = new Database();
                    using (SqlConnection conn = new SqlConnection(db.GetConString()))
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SavePriceQuote_New_20042016";
                            cmd.Connection = conn;

                            cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = pqParams.CityId;

                            if (pqParams.AreaId > 0)
                            {
                                cmd.Parameters.Add("@AreaId", SqlDbType.Int).Value = pqParams.AreaId;
                            }

                            cmd.Parameters.Add("@BikeVersionId", SqlDbType.Int).Value = pqParams.VersionId;
                            cmd.Parameters.Add("@SourceId", SqlDbType.TinyInt).Value = pqParams.SourceId;
                            cmd.Parameters.Add("@ClientIP", SqlDbType.VarChar, 40).Value = String.IsNullOrEmpty(pqParams.ClientIP) ? Convert.DBNull : pqParams.ClientIP;
                            cmd.Parameters.Add("@QuoteId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@DealerId", SqlDbType.Int).Value = pqParams.DealerId;

                            if (pqParams.PQLeadId.HasValue)
                            {
                                cmd.Parameters.Add("@PQSourceId", SqlDbType.TinyInt).Value = pqParams.PQLeadId.Value;
                            }
                            if (!String.IsNullOrEmpty(pqParams.UTMA))
                            {
                                cmd.Parameters.Add("@utma", SqlDbType.VarChar, 100).Value = pqParams.UTMA;
                            }
                            if (!String.IsNullOrEmpty(pqParams.UTMZ))
                            {
                                cmd.Parameters.Add("@utmz", SqlDbType.VarChar, 100).Value = pqParams.UTMZ;
                            }
                            if (!String.IsNullOrEmpty(pqParams.DeviceId))
                            {
                                cmd.Parameters.Add("@deviceId", SqlDbType.VarChar, 25).Value = pqParams.DeviceId;
                            }
                            if (pqParams.RefPQId.HasValue)
                            {
                                cmd.Parameters.Add("@refPQId", SqlDbType.Int).Value = pqParams.RefPQId.Value;
                            }

                            conn.Open();
                            cmd.ExecuteNonQuery();

                            quoteId = Convert.ToUInt64(cmd.Parameters["@QuoteId"].Value);
                        }
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
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                objQuotation = new BikeQuotationEntity();

                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetPriceQuote_New_01022016";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@QuoteId", SqlDbType.BigInt).Value = pqId;
                        cmd.Parameters.Add("@ExShowroomPrice", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RTO", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Insurance", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@OnRoadPrice", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MakeName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ModelName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@VersionName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@City", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@VersionId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@NumOfRows", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@CampaignId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ManufacturerId", SqlDbType.Int).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        int numberOfRecords = Convert.ToInt32(cmd.Parameters["@NumOfRows"].Value);
                        if (numberOfRecords > 0)
                        {
                            objQuotation.ExShowroomPrice = Convert.ToUInt64(cmd.Parameters["@ExShowroomPrice"].Value);
                            objQuotation.RTO = Convert.ToUInt32(cmd.Parameters["@RTO"].Value);
                            objQuotation.Insurance = Convert.ToUInt32(cmd.Parameters["@Insurance"].Value);
                            objQuotation.OnRoadPrice = Convert.ToUInt64(cmd.Parameters["@OnRoadPrice"].Value);
                            objQuotation.MakeName = Convert.ToString(cmd.Parameters["@MakeName"].Value);
                            objQuotation.ModelName = Convert.ToString(cmd.Parameters["@ModelName"].Value);
                            objQuotation.VersionName = Convert.ToString(cmd.Parameters["@VersionName"].Value);
                            objQuotation.City = Convert.ToString(cmd.Parameters["@City"].Value);
                            objQuotation.VersionId = Convert.ToUInt32(cmd.Parameters["@VersionId"].Value);
                            objQuotation.CampaignId = Convert.ToUInt32(cmd.Parameters["@CampaignId"].Value);
                            objQuotation.ManufacturerId = Convert.ToUInt32(cmd.Parameters["@ManufacturerId"].Value);

                            objQuotation.PriceQuoteId = pqId;
                        }
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
            finally
            {
                db.CloseConnection();
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
            Database db = null;

            try
            {
                db = new Database();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetPriceQuoteVersions_New";

                    cmd.Parameters.Add("@QuoteId", SqlDbType.BigInt).Value = pqId;

                    objVersionInfo = new List<OtherVersionInfoEntity>();



                    using (SqlDataReader dr = db.SelectQry(cmd))
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
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "UpdatePriceQuoteBikeVersion";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.Add("@QuoteId", SqlDbType.Int).Value = pqId;
                        cmd.Parameters.Add("@BikeVersionId", SqlDbType.Int).Value = pqParams.VersionId;

                        if (pqParams.ColorId > 0)
                        {
                            cmd.Parameters.Add("@BikeColorId", SqlDbType.Int).Value = pqParams.ColorId;
                        }

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            isUpdated = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SavePQBookingState";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.Add("@QuoteId", SqlDbType.Int).Value = pqId;
                        cmd.Parameters.Add("@stateId", SqlDbType.Int).Value = Convert.ToInt32(state);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            isUpdated = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                db = new Database();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetPriceQuoteData_02052016";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@QuoteId", SqlDbType.Int).Value = pqId;
                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        objQuotation = new PriceQuoteParametersEntity();
                        while (dr.Read())
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
            finally
            {
                db.CloseConnection();
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
            Database db = null;
            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetModelPriceForTopCities";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                        cmd.Parameters.Add("@TopCount", SqlDbType.TinyInt).Value = topCount;

                        using (SqlDataReader dr = db.SelectQry(cmd))
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
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " inputs: modelId : " + modelId + " : topCount :" + topCount);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objPrice;
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 23 May 2016
        /// Summary : Function to get the pricing of the nearest cities for the given model and city.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="topCount"></param>
        /// <returns>Returns List of the pricing for the nearest cities for the given model and city</returns>
        public IEnumerable<PriceQuoteOfTopCities> GetModelPriceInNearestCities(uint modelId, uint cityId, ushort topCount)
        {
            IList<PriceQuoteOfTopCities> objPrice = null;
            Database db = null;

            try
            {
                db = new Database();

                using (SqlConnection conn = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetModelPriceForNearestCities";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                        cmd.Parameters.Add("@TopRecords", SqlDbType.TinyInt).Value = topCount;

                        using (SqlDataReader dr = db.SelectQry(cmd))
                        {
                            objPrice = new List<PriceQuoteOfTopCities>();

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

                            if (dr != null)
                                dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " inputs: modelId : " + modelId + " : topCount :" + topCount);
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return objPrice;
        }
        /// <summary>
        /// Author: Sangram Nandkhile on 25 May 2016
        /// Summary: Get bike versions and prices by model Id
        /// </summary>
        /// <param name="modelId">Model Id</param>
        /// <param name="cityId">City Id </param>
        /// <param name="HasArea">If city has models in those areas</param>
        /// <returns></returns>
        public IEnumerable<BikeQuotationEntity> GetVersionPricesByModelId(uint modelId, uint cityId, out bool HasArea)
        {
            List<BikeQuotationEntity> bikePrices = null;
            HasArea = false;
            Database db = null;
            try
            {
                db = new Database();
                using (SqlConnection conn = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetVersionPricesByModelId";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@ModelId", SqlDbType.Int).Value = modelId;
                        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                        cmd.Parameters.Add("@HasAreasInCity", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        using (SqlDataReader dr = db.SelectQry(cmd))
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
                                    MakeId =  Convert.ToUInt32(Convert.ToString(dr["MakeId"])),
                                    IsModelNew   = Convert.ToBoolean(dr["IsModelNew"]),
                                    IsVersionId = Convert.ToBoolean(dr["IsVersionNew"])
                                });

                            }
                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    HasArea = Convert.ToBoolean(dr["HasAreasInCity"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0} - {1}", HttpContext.Current.Request.ServerVariables["URL"], "GetVersionPricesByModelId"));
                objErr.SendMail();
            }
            finally
            {
                db.CloseConnection();
            }

            return bikePrices;
        }
    }   // Class
}   // namespace