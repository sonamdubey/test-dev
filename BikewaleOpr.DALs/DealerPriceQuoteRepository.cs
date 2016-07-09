using Bikewale.Notifications;
using Bikewale.Notifications.CoreDAL;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web;

namespace BikewaleOpr.DALs
{
    public class DealerPriceQuoteRepository : IDealerPriceQuote
    {
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


        public DealerInfo GetCampaignDealersLatLongV3(uint versionId, uint areaId)
        {
            throw new NotImplementedException();
        }

        public void GetAreaLatLong(uint areaId, out double lattitude, out double longitude)
        {
            lattitude = 0;
            longitude = 0;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("bw_getarealatlong"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_areaid", DbType.Int32, areaId));

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
                HttpContext.Current.Trace.Warn("GetAreaLatLong ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

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


        public DealerPriceQuoteEntity GetPriceQuoteForAllDealer(uint versionId, uint cityId, string dealerIds)
        {
            throw new NotImplementedException();
        }
    }
}
