using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Entity.Dealers;
using BikewaleOpr.Interface.Dealers;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace BikewaleOpr.DALs.BikePricing
{
    /// <summary>
    /// Created by : Aditi Srivastava on 18 Jan 2017
    /// Summary    : Repository for bike price categories
    /// </summary>
    public class DealerPriceRepository : IDealerPriceRepository
    {
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Jan 2017
        /// Summary    : To get a list of all price categories
        /// </summary>
        /// <returns></returns>
        public ICollection<PriceCategoryEntity> GetAllPriceCategories()
        {
            ICollection<PriceCategoryEntity> priceCatList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikepricecategories"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            priceCatList = new Collection<PriceCategoryEntity>();
                            while (dr.Read())
                            {
                                PriceCategoryEntity objPriceCat = new PriceCategoryEntity();
                                objPriceCat.PriceCategoryId = SqlReaderConvertor.ToInt32(dr["ItemCategoryId"]);
                                objPriceCat.PriceCategoryName = Convert.ToString(dr["ItemName"]);
                                priceCatList.Add(objPriceCat);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.DealerPriceRepository.GetAllPriceCategories");
            }
            return priceCatList;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Jan 2017
        /// Summary    : To add a new bike price category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public bool SaveBikeCategory(string categoryName)
        {
            bool isSuccess = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("addpricecategories"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_pricecategory", DbType.String, categoryName));
                    if (Convert.ToBoolean(MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase)))
                        isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BikewaleOpr.Dals.BikePricing.DealerPriceRepository.SaveBikeCategory : Category:{0}", categoryName));
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
        /// Description :   Fetches bike price quotes for given parameters.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public DealerPriceBaseEntity GetDealerPrices(uint cityId, uint makeId, uint dealerId)
        {
            DealerPriceBaseEntity priceSheetBase = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();
                    param.Add("par_MakeId", makeId);
                    param.Add("par_CityId", cityId);
                    param.Add("par_DealerId", dealerId);

                    using (var results = connection.QueryMultiple("bw_getdealerprices_28072017", param: param, commandType: CommandType.StoredProcedure))
                    {
                        priceSheetBase = new DealerPriceBaseEntity();
                        priceSheetBase.DealerVersions = results.Read<DealerVersionEntity>();
                        priceSheetBase.VersionPrices = results.Read<VersionPriceEntity>();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "GetDealerPrices cityId={0} makeId={1} dealerId={2}", cityId, makeId, dealerId));
            }

            return priceSheetBase;
        }
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
        /// Description :   Deletes bike price quotes for given parameters. Accepts a list of versionsIds.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIdList"></param>
        /// <returns></returns>
        public bool DeleteVersionPrices(uint dealerId, uint cityId, string versionIdList)
        {
            bool isSuccess = false;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();
                    param.Add("par_DealerId", dealerId);
                    param.Add("par_CityId", cityId);
                    param.Add("par_BikeVersionId", versionIdList);

                    connection.Execute("bw_removedealerprices", param: param, commandType: CommandType.StoredProcedure);
                    isSuccess = true;

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "DeleteVersionPrices dealerId={0} cityId={1} versionIdList={2}", dealerId, cityId, versionIdList));
            }

            return isSuccess;
        }
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Updates or inserts dealer pricing.
        /// Modified by : Ashutosh Sharma on 30 Aug 2017
        /// Description : Changed SP from 'bw_savedealerprices_28072017' to 'bw_savedealerprices_30082017'
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIdList"></param>
        /// <param name="itemIdList"></param>
        /// <param name="itemValueList"></param>
        /// <param name="enteredBy"></param>
        /// <returns></returns>
        public bool SaveDealerPrices(string dealerIdList, string cityIdList, string versionIdList,
            string itemIdList, string itemValueList, uint enteredBy)
        {
            bool isPriceSaved = false;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_dealerid", dealerIdList);
                    param.Add("par_cityid", cityIdList);
                    param.Add("par_bikeversionid", versionIdList);
                    param.Add("par_itemid", itemIdList);
                    param.Add("par_itemvalue", itemValueList);
                    param.Add("par_updatedby", enteredBy);

                    connection.Execute("bw_savedealerprices_30082017", param: param, commandType: CommandType.StoredProcedure);
                    isPriceSaved = true;

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format(
                    "SaveDealerPrices dealerId={0} cityId={1} versionIdList={2} itemIdList={3} itemValueList={4} enteredBy={5}",
                    dealerIdList, cityIdList, versionIdList, itemIdList, itemValueList, enteredBy));
            }

            return isPriceSaved;
        }
    }
}
