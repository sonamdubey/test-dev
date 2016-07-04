using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AutoBiz;
using Bikewale.Notifications;
using Bikewale.Notifications.CoreDAL;
using BikeWale.Entities.AutoBiz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Bikewale.DAL.AutoBiz
{
    public class DealerPriceQuoteRepository : IDealerPriceQuote
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 27th Oct 2014
        /// Summary : function to get dealer price quote
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public PQ_QuotationEntity GetDealerPriceQuote(PQParameterEntity objParams)
        {
            PQ_QuotationEntity objPriceQuote = null;
            List<PQ_BikeVarient> varients = null;
            IList<PQ_VersionPrice> priceSplits = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealerPriceQuote_08012016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_CityId", DbType.Int32, objParams.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_VersionId", DbType.Int64, objParams.VersionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_DealerId", DbType.Int32, objParams.DealerId > 0 ? objParams.DealerId : Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        objPriceQuote = new PQ_QuotationEntity();
                        // Get version details
                        if (dr.Read())
                        {
                            objPriceQuote.objMake = new BikeMakeEntityBase() { MakeName = dr["MakeName"].ToString(), MaskingName = dr["MakeMaskingName"].ToString() };
                            objPriceQuote.objModel = new BikeModelEntityBase() { ModelName = dr["ModelName"].ToString(), MaskingName = dr["ModelMaskingName"].ToString() };
                            objPriceQuote.objVersion = new BikeVersionEntityBase() { VersionName = dr["VersionName"].ToString() };
                            objPriceQuote.HostUrl = dr["HostURL"].ToString();
                            objPriceQuote.LargePicUrl = dr["largePic"].ToString();
                            objPriceQuote.SmallPicUrl = dr["smallPic"].ToString();
                            objPriceQuote.OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]);
                        }
                        dr.NextResult();
                        // Get pricing
                        objPriceQuote.PriceList = new List<PQ_Price>();
                        while (dr.Read())
                        {
                            objPriceQuote.PriceList.Add(new PQ_Price() { CategoryName = dr["ItemName"].ToString(), Price = Convert.ToUInt32(dr["Price"]), DealerId = Convert.ToUInt32(dr["DealerId"]) });
                        }

                        dr.NextResult();
                        // Get dealer disclaimer
                        objPriceQuote.Disclaimer = new List<string>();
                        while (dr.Read())
                            objPriceQuote.Disclaimer.Add(dr["Disclaimer"].ToString());

                        //Get Offer details
                        dr.NextResult();
                        objPriceQuote.objOffers = new List<OfferEntity>();

                        while (dr.Read())
                        {
                            objPriceQuote.objOffers.Add(new OfferEntity() { OfferText = dr["OfferText"].ToString(), OfferType = dr["OfferType"].ToString(), OfferValue = Convert.ToUInt32(dr["OfferValue"]), OfferCategoryId = Convert.ToUInt32(dr["OfferCategoryId"]), OfferId = Convert.ToUInt32(dr["OfferId"]), IsOfferTerms = Convert.ToBoolean(dr["IsOfferTerms"]), IsPriceImpact = Convert.ToBoolean(dr["IsPriceImpact"]) });
                        }
                        //On road price for the varients
                        if (dr.NextResult())
                        {
                            varients = new List<PQ_BikeVarient>();
                            while (dr.Read())
                            {
                                varients.Add(new PQ_BikeVarient()
                                {
                                    objMake = new BikeMakeEntityBase()
                                    {
                                        MakeId = Convert.ToInt32(dr["MakeId"]),
                                        MakeName = Convert.ToString(dr["MakeName"]),
                                        MaskingName = Convert.ToString(dr["MakeMaskingName"])
                                    },
                                    objModel = new BikeModelEntityBase()
                                    {
                                        ModelId = Convert.ToInt32(dr["ModelId"]),
                                        ModelName = Convert.ToString(dr["ModelName"]),
                                        MaskingName = Convert.ToString(dr["ModelMaskingName"])
                                    },
                                    objVersion = new BikeVersionEntityBase()
                                    {
                                        VersionId = Convert.ToInt32(dr["VersionId"]),
                                        VersionName = Convert.ToString(dr["VersionName"])
                                    },
                                    HostUrl = Convert.ToString(dr["HostURL"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    OnRoadPrice = Convert.ToUInt32(dr["OnRoadPrice"]),
                                    BookingAmount = Convert.ToUInt32(dr["BookingAmount"]),
                                    PriceList = new List<PQ_Price>()
                                });
                            }
                            if (dr.NextResult())
                            {
                                priceSplits = new List<PQ_VersionPrice>();
                                while (dr.Read())
                                {
                                    priceSplits.Add(
                                        new PQ_VersionPrice()
                                        {
                                            CategoryId = Convert.ToUInt32(dr["ItemId"]),
                                            CategoryName = Convert.ToString(dr["ItemName"]),
                                            DealerId = Convert.ToUInt32(dr["DealerId"]),
                                            Price = Convert.ToUInt32(dr["Price"]),
                                            VersionId = Convert.ToUInt32(dr["VersionId"])
                                        }
                                    );
                                }
                            }

                            if ((varients != null && varients.Count > 0) && (priceSplits != null && priceSplits.Count > 0))
                            {
                                varients.ForEach(
                                    varient => varient.PriceList =
                                        (
                                            from price in priceSplits
                                            where price.VersionId == varient.objVersion.VersionId
                                            select new PQ_Price()
                                            {
                                                CategoryId = price.CategoryId,
                                                CategoryName = price.CategoryName,
                                                DealerId = price.DealerId,
                                                Price = price.Price
                                            }
                                        ).ToList()
                                    );
                            }
                            objPriceQuote.Varients = varients;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerPriceQuote ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objPriceQuote;
        }


        /// <summary>
        /// Created By : Created By Sanjay On 29/10/2014
        /// Summary : function to get Bike Category Items Name
        /// Modified By : Ashwini Todkar on 7 Nove 2014 addition category list parameter added
        /// </summary>
        /// <returns> list of prices(Rto , ex-showroom ,insurance etc)</returns>
        /// <param name="categoryList"></param>
        public List<PQ_Price> GetBikeCategoryItems(string categoryList)
        {
            List<PQ_Price> objCategoryList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetCategoryItemsName"))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CategoryItemList", DbType.String, !String.IsNullOrEmpty(categoryList) ? categoryList : Convert.DBNull));

                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            objCategoryList = new List<PQ_Price>();
                            while (dr.Read())
                            {
                                objCategoryList.Add(new PQ_Price() { CategoryName = dr["ItemName"].ToString(), CategoryId = Convert.ToUInt32(dr["ItemCategoryId"]) });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeCategoryItems ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objCategoryList;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 7 Nov 2014
        /// Method to get all versions and dealer prices in a city
        /// 
        /// Modified By : Suresh Prajapati on 29th Jan, 2015
        /// Description : To Get dealer prices on make specific.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="modelId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public DataSet GetDealerPrices(uint cityId, uint makeId, uint dealerId)
        {
            DataSet ds = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealerPrices"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId", DbType.Int64, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerId", DbType.Int32, dealerId));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeCategoryItems ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }


        /// <summary>
        /// Written By : Ashwini Todkar on 7 Nov 2014
        /// Method to save or update dealer bike price
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <param name="itemId"></param>
        /// <param name="itemValue"></param>
        /// <returns></returns>
        public bool SaveDealerPrice(uint dealerId, uint versionId, uint cityId, ushort itemId, uint itemValue)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_SaveDealerPrices"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_BikeVersionId", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PItemId", DbType.Int16, itemId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Itemvalue", DbType.Int32, itemValue));

                    //run the command
                    isSuccess = MySqlDatabase.UpdateQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveDealerPrice ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return isSuccess;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 7 Nov 2014
        /// Method to get all versions and dealer prices in a city
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public bool SaveDealerPrice(DataTable dt)
        {
            bool isPriceSaved = false;

            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("BW_SaveDealerPrices"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.UpdatedRowSource = UpdateRowSource.None;

                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("v_DealerId", DbType.Int32, 8, dt.Columns[0].ColumnName));
                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("v_BikeVersionId", DbType.Int32, 4, dt.Columns[1].ColumnName));
                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("v_CityId", DbType.Int32, 4, dt.Columns[2].ColumnName));
                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("v_ItemId", DbType.Int16, 4, dt.Columns[3].ColumnName));
                    cmd.Parameters.Add(DbFactory.GetDbParamWithColumnName("v_Itemvalue", DbType.Int32, 8, dt.Columns[4].ColumnName));

                    //run the command
                    int recordsInserted = MySqlDatabase.InsertQueryViaAdaptor(cmd, dt);
                    if (recordsInserted > 0)
                        isPriceSaved = true;
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("SaveDealerPrice ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return isPriceSaved;
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 8 Nov 2014
        /// Method to remove versions dealer prices in a city
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIdList">success status</param>
        /// <returns></returns>
        public bool DeleteVersionPrices(uint dealerId, uint cityId, string versionIdList)
        {


            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_RemoveDealerPrices"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_BikeVersionId", DbType.String, versionIdList));

                    //run the command

                    return MySqlDatabase.ExecuteNonQuery(cmd) > 0;

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("DeleteVersionPrices ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return false;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 3 Nov 2014
        /// Summary : To check Dealer exists or not w.r.t. versionId and areaId 
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public uint IsDealerExists(uint versionId, uint areaId)
        {
            uint dealerId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AreaId", DbType.Int32, areaId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, versionId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr.Read())
                        {
                            dealerId = Convert.ToUInt32(dr["DealerId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("IsDealerExists ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return dealerId;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 6 Nov 2014
        /// Summary : To get all dealer area mapping detail by city id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<DealerAreaDetails> GetDealerAreaDetails(uint cityId)
        {

            List<DealerAreaDetails> objMapping = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetDealerAreaList"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, cityId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        objMapping = new List<DealerAreaDetails>();
                        while (dr.Read())
                        {
                            objMapping.Add(new DealerAreaDetails()
                            {
                                AreaId = Convert.ToUInt32(dr["AreaId"]),
                                AreaName = dr["AreaName"].ToString(),
                                CityId = Convert.ToUInt32(dr["CityId"]),
                                PinCode = dr["PinCode"].ToString(),
                                DealerId = Convert.ToUInt32(dr["DealerId"]),
                                DealerOrganization = dr["Organization"].ToString(),
                                DealerCount = Convert.ToUInt32(dr["DealerCount"]),
                                DealerRank = Convert.ToUInt32(dr["DealerRank"]),
                                MakeName = dr["MakeName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealerAreaDetails ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objMapping;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 7 Nov 2014
        /// Summary : Map Dealer with area list
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areaIdList"></param>
        /// <returns></returns>
        public bool MapDealerWithArea(uint dealerId, string areaIdList)
        {

            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_MapDealers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AreaIdList", DbType.String, -1, areaIdList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_result", DbType.Byte, ParameterDirection.Output));
                    MySqlDatabase.InsertQuery(cmd);

                    isSuccess = Convert.ToBoolean(cmd.Parameters["v_result"].Value);

                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("MapDealerWithArea ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSuccess;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 7 Nov 2014
        /// Summary : unmap Dealer with area list
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="areaIdList"></param>
        /// <returns></returns>
        public bool UnmapDealer(uint dealerId, string areaIdList)
        {

            bool isSuccess = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_UnmapDealers"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerId", DbType.Int32, dealerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AreaIdList", DbType.String, -1, areaIdList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_result", DbType.Byte, ParameterDirection.Output));
                    MySqlDatabase.InsertQuery(cmd);
                    isSuccess = Convert.ToBoolean(cmd.Parameters["v_result"].Value);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("UnmapDealer ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSuccess;
        }


        /// <summary>
        /// Written BY : Ashish G. Kamble on 12 Jan 2014
        /// Summary : function to get the dealers for the given version with lattitude and longitude.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public List<DealerLatLong> GetDealersLatLong(uint versionId, uint areaId)
        {

            List<DealerLatLong> objDealersList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealersLatLong"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AreaId", DbType.Int32, areaId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        objDealersList = new List<DealerLatLong>();

                        while (dr.Read())
                        {
                            objDealersList.Add(new DealerLatLong
                            {
                                DealerId = Convert.ToUInt32(dr["DealerId"]),
                                Lattitude = Convert.ToDouble(dr["Lattitude"]),
                                Longitude = Convert.ToDouble(dr["Longitude"]),
                                ServingDistance = Convert.ToUInt16(dr["LeadServingDistance"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetDealersLatLong ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objDealersList;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 12 Jan 2015
        /// Summary : Function to get the lattitude and longitude of the given city.
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        public void GetAreaLatLong(uint areaId, out double lattitude, out double longitude)
        {

            lattitude = 0;
            longitude = 0;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetAreaLatLong"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AreaId", DbType.Int32, areaId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                lattitude = Convert.ToDouble(dr["Lattitude"]);
                                longitude = Convert.ToDouble(dr["Longitude"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetCityLatLong ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 10 May 2015
        /// Summary : Function to get the list of cities where bike booking is available.
        /// </summary>
        /// <returns>Returns list of the cities.</returns>
        public List<CityEntityBase> GetBikeBookingCities(uint? modelId)
        {

            List<CityEntityBase> cities = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetBikeBookingCities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_BikeModelId", DbType.Int32, (modelId.HasValue) ? modelId : Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            cities = new List<CityEntityBase>();

                            while (dr.Read())
                            {
                                cities.Add(new CityEntityBase() { CityId = Convert.ToUInt32(dr["CityId"]), CityName = Convert.ToString(dr["City"]) });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeBookingCities ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return cities;
        }   // End of GetBikeBookingCities

        /// <summary>
        /// Written By : Ashish G. Kamble on 10 May 2015
        /// Summary : Function to get the list of bike makes in the particular city where booking option is available.
        /// </summary>
        /// <param name="cityId">Should be greater than 0.</param>
        /// <returns>Returns list of bike makes</returns>
        public List<BikeMakeEntityBase> GetBikeMakesInCity(uint cityId)
        {

            List<BikeMakeEntityBase> makes = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetBikeMakesInCity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            makes = new List<BikeMakeEntityBase>();

                            while (dr.Read())
                            {
                                makes.Add(new BikeMakeEntityBase() { MakeId = Convert.ToInt32(dr["MakeId"]), MakeName = Convert.ToString(dr["Make"]) });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetBikeMakesInCity ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return makes;
        }   // end of GetBikeMakesInCity
        /// <summary>
        /// Written By : Sangram Nandkhile on 05 October 2015
        /// Summary : Function to Retrun HTML for the masking name provided in url
        /// </summary>
        /// <param name="offerMaskingName"></param>
        /// <param name="isExpired">To check if offer is expired or not</param>
        /// <returns></returns>
        public OfferHtmlEntity GetOfferTerms(string offerMaskingName, int? offerId)
        {
            string termsHtml = string.Empty;
            OfferHtmlEntity offerTerms = null;
            DbCommand cmd = null;
            try
            {
                using (cmd = DbFactory.GetDBCommand("BW_GetOfferTerms"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    #region params

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_maskingName", DbType.String, offerMaskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_offerId", DbType.Int32, offerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_isExpired", DbType.Boolean, ParameterDirection.InputOutput, false));
                    #endregion


                    termsHtml = Convert.ToString(MySqlDatabase.ExecuteScalar(cmd));
                    if (!string.IsNullOrEmpty(termsHtml))
                    {
                        offerTerms = new OfferHtmlEntity();
                        offerTerms.Html = termsHtml;
                        offerTerms.IsExpired = Convert.ToBoolean(cmd.Parameters["v_isExpired"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return offerTerms;
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 23 Oct 2015
        /// Summary : To get priceQuote for all dealer who can serve for perticular area
        /// Modified By : Sushil Kumar on 10th January 2016
        /// Description : Added provision to retrieve bike availability by color
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <param name="dealerIds"></param>
        /// <returns></returns>
        public DealerPriceQuoteEntity GetPriceQuoteForAllDealer(uint versionId, uint cityId, string dealerIds)
        {

            DealerPriceQuoteEntity objList = null;
            IList<PQ_Price> PriceList = null;
            IList<BikeColorAvailability> ColorwiseAvailabilty = null;
            IList<OfferEntityBase> OfferList = null;
            IList<DealerQuotation> DealerDetails = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_Opr_GetPriceQuoteDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerIds", DbType.String, -1, dealerIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_BikeVersionId", DbType.Int32, versionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, cityId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            objList = new DealerPriceQuoteEntity();

                            DealerDetails = new List<DealerQuotation>();
                            while (dr.Read())
                            {
                                DealerDetails.Add(new DealerQuotation()
                                {
                                    BookingAmount = Convert.ToUInt32(dr["BookingAmount"]),
                                    Availability = Convert.ToUInt32(dr["NumOfDays"]),
                                    DealerId = Convert.ToUInt32(dr["DealerId"]),
                                    Dealer = new NewBikeDealers()
                                    {
                                        DealerId = Convert.ToUInt32(dr["DealerId"]),
                                        Name = dr["Name"].ToString(),
                                        Organization = dr["Organization"].ToString(),
                                        Address = dr["Address"].ToString(),
                                        MobileNo = dr["MobileNo"].ToString(),
                                        WorkingTime = dr["ContactHours"].ToString()
                                    }
                                });
                            }
                            if (dr.NextResult())
                            {
                                OfferList = new List<OfferEntityBase>();
                                while (dr.Read())
                                {
                                    OfferList.Add(new OfferEntityBase()
                                    {
                                        OfferId = Convert.ToUInt32(dr["OfferId"]),
                                        OfferCategoryId = Convert.ToUInt32(dr["OfferCateGoryId"]),
                                        OfferType = dr["OfferType"].ToString(),
                                        OfferText = dr["OfferText"].ToString(),
                                        OfferValue = Convert.ToUInt32(dr["OfferValue"]),
                                        OffervalidTill = Convert.ToDateTime(dr["OfferValidTill"]),
                                        DealerId = Convert.ToUInt32(dr["DealerId"])
                                    });
                                }
                            }

                            if (dr.NextResult())
                            {
                                PriceList = new List<PQ_Price>();
                                while (dr.Read())
                                {
                                    PriceList.Add(new PQ_Price()
                                    {
                                        CategoryId = Convert.ToUInt32(dr["ItemId"]),
                                        CategoryName = dr["ItemName"].ToString(),
                                        Price = Convert.ToUInt32(dr["Price"]),
                                        DealerId = Convert.ToUInt32(dr["DealerId"])
                                    });
                                }
                            }

                            if (dr.NextResult())
                            {
                                ColorwiseAvailabilty = new List<BikeColorAvailability>();
                                while (dr.Read())
                                {
                                    ColorwiseAvailabilty.Add(new BikeColorAvailability()
                                    {
                                        ColorId = Convert.ToUInt32(dr["ColorId"]),
                                        NoOfDays = Convert.ToInt16(dr["NumOfDays"]),
                                        DealerId = Convert.ToUInt32(dr["DealerId"]),
                                        ColorName = Convert.ToString(dr["ColorName"]),
                                        HexCode = Convert.ToString(dr["HexCode"]),
                                        VersionId = Convert.ToUInt32(dr["BikeVersionId"])
                                    });
                                }
                            }

                            objList.DealerDetails = DealerDetails;
                            objList.OfferList = OfferList;
                            objList.PriceList = PriceList;
                            objList.BikeAvailabilityByColor = ColorwiseAvailabilty;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DealerPriceQuoteRepository.GetPriceQuoteForAllDealer");
                objErr.SendMail();
            }
            return objList;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 14 Mar 2016
        /// Description :   Get Dealer's Price Quotes
        /// Modified by :   Sumit Kate on 22 Mar 2016
        /// Description :   Removed redundant DataReader checks and added DbNull checks
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public DetailedDealerQuotationEntity GetDealerPriceQuoteByPackage(PQParameterEntity objParams)
        {
            DetailedDealerQuotationEntity objDetailedDealerQuotationEntity = null;
            DealerQuotationEntity dealerQuotation = null;
            NewBikeDealers primaryDealer = null;
            IList<PQ_Price> PriceList = null;
            IList<BikeColorAvailability> ColorwiseAvailabilty = null;
            EMI objEMI = null;
            IList<OfferEntityBase> OfferList = null;
            IList<DealerBenefitEntity> benefits = null;
            IList<NewBikeDealerBase> secondaryDealers = null;
            IList<string> disclaimer = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("BW_GetDealerDetails_14032016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DealerId", DbType.Int32, objParams.DealerId > 0 ? Convert.ToInt64(objParams.DealerId) : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, Convert.ToInt64(objParams.VersionId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int32, Convert.ToInt64(objParams.CityId)));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        if (dr != null)
                        {
                            objDetailedDealerQuotationEntity = new DetailedDealerQuotationEntity();
                            dealerQuotation = new DealerQuotationEntity();
                            #region Bike Details
                            if (dr.Read())
                            {
                                BikeMakeEntityBase objMake = new BikeMakeEntityBase();
                                objMake.MakeId = !Convert.IsDBNull(dr["MakeId"]) ? Convert.ToInt32(dr["MakeId"]) : default(int);
                                objMake.MakeName = !Convert.IsDBNull(dr["MakeName"]) ? Convert.ToString(dr["MakeName"]) : default(string);
                                objMake.MaskingName = !Convert.IsDBNull(dr["MakeMaskingName"]) ? Convert.ToString(dr["MakeMaskingName"]) : default(string);
                                objDetailedDealerQuotationEntity.objMake = objMake;
                                BikeModelEntityBase objModel = new BikeModelEntityBase();
                                objModel.ModelId = !Convert.IsDBNull(dr["ModelId"]) ? Convert.ToInt32(dr["ModelId"]) : default(int);
                                objModel.ModelName = !Convert.IsDBNull(dr["ModelName"]) ? Convert.ToString(dr["ModelName"]) : default(string);
                                objModel.MaskingName = !Convert.IsDBNull(dr["ModelMaskingName"]) ? Convert.ToString(dr["ModelMaskingName"]) : default(string);
                                objDetailedDealerQuotationEntity.objModel = objModel;
                                BikeVersionEntityBase objVersion = new BikeVersionEntityBase();
                                objVersion.VersionId = !Convert.IsDBNull(dr["VersionId"]) ? Convert.ToInt32(dr["VersionId"]) : default(int);
                                objVersion.VersionName = !Convert.IsDBNull(dr["VersionName"]) ? Convert.ToString(dr["VersionName"]) : default(string);
                                objDetailedDealerQuotationEntity.objVersion = objVersion;

                                objDetailedDealerQuotationEntity.HostUrl = !Convert.IsDBNull(dr["HostURL"]) ? Convert.ToString(dr["HostURL"]) : default(string);
                                objDetailedDealerQuotationEntity.OriginalImagePath = !Convert.IsDBNull(dr["OriginalImagePath"]) ? Convert.ToString(dr["OriginalImagePath"]) : default(string);
                            }
                            #endregion

                            #region Price Break-Up
                            if (dr.NextResult())
                            {
                                PriceList = new List<PQ_Price>();
                                while (dr.Read())
                                {
                                    PriceList.Add(new PQ_Price()
                                    {
                                        CategoryId = !Convert.IsDBNull(dr["ItemId"]) ? Convert.ToUInt32(dr["ItemId"]) : default(UInt32),
                                        CategoryName = !Convert.IsDBNull(dr["ItemName"]) ? Convert.ToString(dr["ItemName"]) : default(string),
                                        Price = !Convert.IsDBNull(dr["Price"]) ? Convert.ToUInt32(dr["Price"]) : default(UInt32),
                                        DealerId = !Convert.IsDBNull(dr["DealerId"]) ? Convert.ToUInt32(dr["DealerId"]) : default(UInt32)
                                    });
                                }
                                dealerQuotation.PriceList = PriceList;
                            }
                            #endregion
                            //Read Dealer Details
                            #region Dealer Details
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    primaryDealer = new NewBikeDealers();
                                    primaryDealer.DealerId = !Convert.IsDBNull(dr["ID"]) ? Convert.ToUInt32(dr["ID"]) : default(UInt32);
                                    primaryDealer.Organization = !Convert.IsDBNull(dr["Organization"]) ? Convert.ToString(dr["Organization"]) : default(string);
                                    primaryDealer.Name = String.Format("{0} {1}", (!Convert.IsDBNull(dr["FirstName"]) ? Convert.ToString(dr["FirstName"]) : default(string)), (!Convert.IsDBNull(dr["LastName"]) ? Convert.ToString(dr["LastName"]) : default(string)));
                                    primaryDealer.Address = !Convert.IsDBNull(dr["Address"]) ? Convert.ToString(dr["Address"]) : default(string);
                                    primaryDealer.PhoneNo = !Convert.IsDBNull(dr["PhoneNo"]) ? Convert.ToString(dr["PhoneNo"]) : default(string);
                                    primaryDealer.MobileNo = !Convert.IsDBNull(dr["MobileNo"]) ? Convert.ToString(dr["MobileNo"]) : default(string);
                                    primaryDealer.MaskingNumber = !Convert.IsDBNull(dr["PhoneNo"]) ? Convert.ToString(dr["PhoneNo"]) : default(string);
                                    primaryDealer.WorkingTime = !Convert.IsDBNull(dr["ContactHours"]) ? Convert.ToString(dr["ContactHours"]) : default(string);
                                    primaryDealer.objArea = new Bikewale.Entities.BikeBooking.AreaEntityBase()
                                    {
                                        AreaId = !Convert.IsDBNull(dr["AreaId"]) ? Convert.ToUInt32(dr["AreaId"]) : default(UInt32),
                                        AreaName = !Convert.IsDBNull(dr["AreaName"]) ? Convert.ToString(dr["AreaName"]) : default(string),
                                        Latitude = !Convert.IsDBNull(dr["Lattitude"]) ? Convert.ToDouble(dr["Lattitude"]) : default(double),
                                        Longitude = !Convert.IsDBNull(dr["Longitude"]) ? Convert.ToDouble(dr["Longitude"]) : default(double),
                                        PinCode = !Convert.IsDBNull(dr["Pincode"]) ? Convert.ToString(dr["Pincode"]) : default(string)
                                    };
                                    primaryDealer.objCity = new CityEntityBase() { CityName = !Convert.IsDBNull(dr["CityName"]) ? Convert.ToString(dr["CityName"]) : default(string) };
                                    primaryDealer.objState = new StateEntityBase() { StateName = !Convert.IsDBNull(dr["StateName"]) ? Convert.ToString(dr["StateName"]) : default(string) };
                                    primaryDealer.Website = !Convert.IsDBNull(dr["WebsiteUrl"]) ? Convert.ToString(dr["WebsiteUrl"]) : default(string);
                                    primaryDealer.EmailId = !Convert.IsDBNull(dr["EmailId"]) ? Convert.ToString(dr["EmailId"]) : default(string);
                                    DealerPackageTypes s;
                                    if (Enum.TryParse((!Convert.IsDBNull(dr["DealerPackageType"]) ? Convert.ToString(dr["DealerPackageType"]) : default(string)), out s))
                                        primaryDealer.DealerPackageType = s;
                                }
                                dealerQuotation.DealerDetails = primaryDealer;
                            }
                            #endregion
                            //Booking Amount
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    dealerQuotation.BookingAmount = !Convert.IsDBNull(dr["Amount"]) ? Convert.ToUInt32(dr["Amount"]) : default(UInt32);
                                }
                            }

                            //Booking Num Of Days
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    dealerQuotation.BikeAvailability = !Convert.IsDBNull(dr["NumOfDays"]) ? Convert.ToUInt32(dr["NumOfDays"]) : default(UInt32);
                                }
                            }

                            #region Color and Color level availability
                            if (dr.NextResult())
                            {
                                ColorwiseAvailabilty = new List<BikeColorAvailability>();
                                while (dr.Read())
                                {
                                    ColorwiseAvailabilty.Add(new BikeColorAvailability()
                                    {
                                        ColorId = !Convert.IsDBNull(dr["ColorId"]) ? Convert.ToUInt32(dr["ColorId"]) : default(UInt32),
                                        NoOfDays = !Convert.IsDBNull(dr["NumOfDays"]) ? Convert.ToInt16(dr["NumOfDays"]) : default(Int16),
                                        DealerId = !Convert.IsDBNull(dr["DealerId"]) ? Convert.ToUInt32(dr["DealerId"]) : default(UInt32),
                                        ColorName = !Convert.IsDBNull(dr["ColorName"]) ? Convert.ToString(dr["ColorName"]) : default(string),
                                        HexCode = !Convert.IsDBNull(dr["HexCode"]) ? Convert.ToString(dr["HexCode"]) : default(string),
                                        VersionId = !Convert.IsDBNull(dr["BikeVersionId"]) ? Convert.ToUInt32(dr["BikeVersionId"]) : default(UInt32)
                                    });
                                }
                                dealerQuotation.AvailabilityByColor = ColorwiseAvailabilty;
                            }
                            #endregion
                            #region EMI Values
                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    objEMI = new EMI();
                                    objEMI.Id = !Convert.IsDBNull(dr["Id"]) ? Convert.ToUInt32(dr["Id"]) : default(UInt32);
                                    objEMI.LoanProvider = !Convert.IsDBNull(dr["LoanProvider"]) ? Convert.ToString(dr["LoanProvider"]) : default(string);
                                    objEMI.LoanToValue = !Convert.IsDBNull(dr["LTV"]) ? Convert.ToUInt16(dr["LTV"]) : default(UInt16);
                                    objEMI.MaxDownPayment = !Convert.IsDBNull(dr["MaxDownPayment"]) ? Convert.ToUInt32(dr["MaxDownPayment"]) : default(float);
                                    objEMI.MaxRateOfInterest = !Convert.IsDBNull(dr["MaxRateOfInterest"]) ? Convert.ToUInt32(dr["MaxRateOfInterest"]) : default(float);
                                    objEMI.MaxTenure = !Convert.IsDBNull(dr["MaxTenure"]) ? Convert.ToUInt16(dr["MaxTenure"]) : default(UInt16);
                                    objEMI.MinDownPayment = !Convert.IsDBNull(dr["MinDownPayment"]) ? Convert.ToUInt32(dr["MinDownPayment"]) : default(float);
                                    objEMI.MinRateOfInterest = !Convert.IsDBNull(dr["MinRateOfInterest"]) ? Convert.ToUInt32(dr["MinRateOfInterest"]) : default(float);
                                    objEMI.MinTenure = !Convert.IsDBNull(dr["MinTenure"]) ? Convert.ToUInt16(dr["MinTenure"]) : default(UInt16);
                                    objEMI.ProcessingFee = !Convert.IsDBNull(dr["ProcessingFee"]) ? Convert.ToUInt32(dr["ProcessingFee"]) : default(float);
                                    objEMI.RateOfInterest = !Convert.IsDBNull(dr["RateOfInterest"]) ? Convert.ToSingle(dr["RateOfInterest"]) : default(float);
                                    objEMI.Tenure = !Convert.IsDBNull(dr["Tenure"]) ? Convert.ToUInt16(dr["Tenure"]) : default(UInt16);
                                }
                                dealerQuotation.EMIDetails = objEMI;
                            }
                            #endregion
                            #region Dealer Offers
                            if (dr.NextResult())
                            {
                                OfferList = new List<OfferEntityBase>();
                                while (dr.Read())
                                {
                                    OfferList.Add(new OfferEntityBase()
                                    {
                                        OfferId = !Convert.IsDBNull(dr["OfferId"]) ? Convert.ToUInt32(dr["OfferId"]) : default(UInt32),
                                        OfferCategoryId = !Convert.IsDBNull(dr["OfferCategoryId"]) ? Convert.ToUInt32(dr["OfferCategoryId"]) : default(UInt32),
                                        OfferType = !Convert.IsDBNull(dr["OfferType"]) ? Convert.ToString(dr["OfferType"]) : default(string),
                                        OfferText = !Convert.IsDBNull(dr["OfferText"]) ? Convert.ToString(dr["OfferText"]) : default(string),
                                        OfferValue = !Convert.IsDBNull(dr["OfferValue"]) ? Convert.ToUInt32(dr["OfferValue"]) : default(UInt32),
                                        OffervalidTill = !Convert.IsDBNull(dr["OfferValidTill"]) ? Convert.ToDateTime(dr["OfferValidTill"]) : default(DateTime),
                                        DealerId = !Convert.IsDBNull(dr["DealerId"]) ? Convert.ToUInt32(dr["DealerId"]) : default(UInt32)
                                    });
                                }
                                dealerQuotation.OfferList = OfferList;
                            }
                            #endregion
                            #region Dealer Benefits
                            if (dr.NextResult())
                            {
                                benefits = new List<DealerBenefitEntity>();
                                while (dr.Read())
                                {
                                    DealerBenefitEntity objDealerBenefitEntity = new DealerBenefitEntity();
                                    objDealerBenefitEntity.BenefitId = !Convert.IsDBNull(dr["BenefitId"]) ? Convert.ToInt32(dr["BenefitId"]) : default(int);
                                    objDealerBenefitEntity.DealerId = !Convert.IsDBNull(dr["DealerId"]) ? Convert.ToInt32(dr["DealerId"]) : default(int);
                                    objDealerBenefitEntity.CatId = !Convert.IsDBNull(dr["CatId"]) ? Convert.ToInt32(dr["CatId"]) : default(int);
                                    objDealerBenefitEntity.CategoryText = !Convert.IsDBNull(dr["CategoryText"]) ? Convert.ToString(dr["CategoryText"]) : default(string);
                                    objDealerBenefitEntity.BenefitText = !Convert.IsDBNull(dr["BenefitText"]) ? Convert.ToString(dr["BenefitText"]) : default(string);
                                    objDealerBenefitEntity.City = !Convert.IsDBNull(dr["City"]) ? Convert.ToString(dr["City"]) : default(string);
                                    benefits.Add(objDealerBenefitEntity);
                                }
                                dealerQuotation.Benefits = benefits;
                            }
                            #endregion
                            #region Disclaimer
                            if (dr.NextResult())
                            {
                                disclaimer = new List<string>();
                                while (dr.Read())
                                {
                                    disclaimer.Add(!Convert.IsDBNull(dr["Disclaimer"]) ? Convert.ToString(dr["Disclaimer"]) : default(string));
                                }
                                dealerQuotation.Disclaimer = disclaimer;
                            }
                            #endregion

                            if (dr.NextResult())
                            {
                                secondaryDealers = new List<NewBikeDealerBase>();
                                DealerPackageTypes s;
                                while (dr.Read())
                                {
                                    secondaryDealers.Add(
                                        new NewBikeDealerBase()
                                        {
                                            Area = !Convert.IsDBNull(dr["AreaName"]) ? Convert.ToString(dr["AreaName"]) : default(string),
                                            DealerId = !Convert.IsDBNull(dr["ID"]) ? Convert.ToUInt32(dr["ID"]) : default(UInt32),
                                            Name = !Convert.IsDBNull(dr["Organization"]) ? Convert.ToString(dr["Organization"]) : default(string),
                                            MaskingNumber = !Convert.IsDBNull(dr["MaskingNumber"]) ? Convert.ToString(dr["MaskingNumber"]) : default(string),
                                            DealerPackageType = (Enum.TryParse((!Convert.IsDBNull(dr["DealerPackageType"]) ? Convert.ToString(dr["DealerPackageType"]) : default(string)), out s)) ? s : DealerPackageTypes.Invalid
                                        }
                                        );
                                }
                            }
                        }
                        objDetailedDealerQuotationEntity.PrimaryDealer = dealerQuotation;
                        objDetailedDealerQuotationEntity.SecondaryDealers = secondaryDealers;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.DealerPriceQuoteRepository.GetDealerPriceQuoteByPackage");
                objErr.SendMail();
            }
            return objDetailedDealerQuotationEntity;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 21 Mar 2016
        /// Description :   To get the Dealers Lattitude and Longitude based on subscription model
        /// SP Called : BW_GetDealersLatLong_21032016
        /// Modified By : Vivek Gupta on 12-04-2016, new sp BW_GetDealersLatLong_12042016
        /// Description : changed return type from Inumerable<DealerLatLong> to DealerLatLong coz we are fetching nearest dealer from database itself. 
        ///               we have commute distance availbele in database.
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public DealerLatLong GetCampaignDealersLatLong(uint versionId, uint areaId)
        {

            DealerLatLong objDealersList = null;

            try
            {
                if (versionId > 0 && areaId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerslatlong_12042016"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, versionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_AreaId", DbType.Int32, areaId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                        {
                            objDealersList = new DealerLatLong();

                            while (dr.Read())
                            {
                                objDealersList.DealerId = !Convert.IsDBNull(dr["DealerId"]) ? Convert.ToUInt32(dr["DealerId"]) : default(UInt32);
                                objDealersList.Lattitude = !Convert.IsDBNull(dr["Lattitude"]) ? Convert.ToDouble(dr["Lattitude"]) : default(UInt32);
                                objDealersList.Longitude = !Convert.IsDBNull(dr["Longitude"]) ? Convert.ToDouble(dr["Longitude"]) : default(UInt32);
                                objDealersList.ServingDistance = !Convert.IsDBNull(dr["LeadServingDistance"]) ? Convert.ToUInt16(dr["LeadServingDistance"]) : default(UInt16);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetCampaignDealersLatLong");
                objErr.SendMail();
            }
            return objDealersList;
        }
        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 28-04-2016
        /// Desc : to check dealer exists for areaId and version id and isSecondaryDealers availble
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public DealerInfo GetCampaignDealersLatLongV3(uint versionId, uint areaId)
        {
            DealerInfo objDealersList = null;

            try
            {
                if (versionId > 0 && areaId > 0)
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("bw_getdealerslatlong_28042016"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, versionId));
                        cmd.Parameters.Add(DbFactory.GetDbParam("v_AreaId", DbType.Int32, areaId));

                        using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                        {
                            objDealersList = new DealerInfo();

                            while (dr.Read())
                            {
                                objDealersList.DealerId = !Convert.IsDBNull(dr["DealerId"]) ? Convert.ToUInt32(dr["DealerId"]) : default(UInt32);
                                objDealersList.IsDealerAvailable = !Convert.IsDBNull(dr["IsDealerAvailable"]) ? Convert.ToBoolean(dr["IsDealerAvailable"]) : default(bool);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetCampaignDealersLatLongV3");
                objErr.SendMail();
            }
            return objDealersList;
        }

    } // class
} // namespace
