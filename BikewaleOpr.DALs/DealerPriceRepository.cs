using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BikewaleOpr.DALs
{
    /// <summary>
    /// Created by  :   Vishnu Teja Yalakuntla on 28-Jul-2017
    /// Description :   Performs all DAL operations for Manage Dealer Pricing page.
    /// </summary>
    public class DealerPriceRepository : IDealerPriceRepository
    {
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
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_MakeId", makeId);
                    param.Add("par_CityId", cityId);
                    param.Add("par_DealerId", dealerId);

                    priceSheetBase = new DealerPriceBaseEntity();

                    using (var results = connection.QueryMultiple("bw_getdealerprices_28072017", param: param, commandType: CommandType.StoredProcedure))
                    {
                        priceSheetBase.DealerVersions = results.Read<DealerVersionEntity>();
                        priceSheetBase.VersionPrices = results.Read<VersionPriceEntity>();
                    }

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            catch (Exception ex)
            {
                string exString = string.Format("GetDealerPrices cityId={0} makeId={1} dealerId={2}", cityId, makeId, dealerId);
                ErrorClass objErr = new ErrorClass(ex, exString);
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
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_DealerId", dealerId);
                    param.Add("par_CityId", cityId);
                    param.Add("par_BikeVersionId", versionIdList);

                    connection.Query<dynamic>("bw_removedealerprices", param: param, commandType: CommandType.StoredProcedure);
                    isSuccess = true;

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                string exString = string.Format("GetDealerPrices dealerId={0} cityId={1} versionIdList={2}", dealerId, cityId, versionIdList);
                ErrorClass objErr = new ErrorClass(ex, exString);
            }

            return isSuccess;
        }
        /// <summary>
        /// Created by  :   Vishnu Teja Yalakuntla on 31-Jul-2017
        /// Description :   Updates or inserts dealer pricing.
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="cityId"></param>
        /// <param name="versionIdList"></param>
        /// <param name="itemIdList"></param>
        /// <param name="itemValueList"></param>
        /// <param name="enteredBy"></param>
        /// <returns></returns>
        public bool SaveDealerPrice(uint dealerId, uint cityId, string versionIdList,
            string itemIdList, string itemValueList, uint enteredBy)
        {
            bool isPriceSaved = false;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();

                    var param = new DynamicParameters();
                    param.Add("par_dealerid", dealerId);
                    param.Add("par_cityid", cityId);
                    param.Add("par_bikeversionid", versionIdList);
                    param.Add("par_itemid", itemIdList);
                    param.Add("par_itemvalue", itemValueList);
                    param.Add("par_updatedby", enteredBy);

                    connection.Query<dynamic>("bw_savedealerprices_28072017", param: param, commandType: CommandType.StoredProcedure);
                    isPriceSaved = true;

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                string exString = string.Format(
                    "GetDealerPrices dealerId={0} cityId={1} versionIdList={2} itemIdList={3} itemValueList={4} enteredBy={5}",
                    dealerId, cityId, versionIdList, itemIdList, itemValueList, enteredBy);
                ErrorClass objErr = new ErrorClass(ex, exString);
            }

            return isPriceSaved;
        }
    }
}
