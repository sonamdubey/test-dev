using Bikewale.DAL.CoreDAL;
using Bikewale.ElasticSearch.Entities;
using Bikewale.Notifications;
using Bikewale.Utility;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entities.BikePricing;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.Dealers;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace BikewaleOpr.DALs.BikePricing
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 23 Sept 2016
    /// Summary : Class have functions to process pricing in the bikewale opr
    /// Modified By : Ashutosh Sharma on 29-07-2017
    /// Discription : Added GetModelsByMake and GetPriceMonitoringDetails
    /// </summary>
    public class BikeShowroomPrices : IShowroomPricesRepository
    {
		/// <summary>
		/// Writteny By : Ashish G. Kamble on 23 Sept 2016
		/// Summary : Function to get the existing pricing for the given make and city
		/// Modified by : Ashutosh Sharma on 27 Nov 2017
		/// Description : Changed SP from 'GetVersionPricesByMakeCity_07042017' to 'GetVersionPricesByMakeCity_28112017' to get seriesId
		/// </summary>
		/// <param name="makeId"></param>
		/// <param name="cityId"></param>
		/// <returns></returns>
		public IEnumerable<BikePrice> GetBikePrices(uint makeId, uint cityId)
        {
            IList<BikePrice> objPrices = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetVersionPricesByMakeCity_28112017"))
                {
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.UInt32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.UInt32, cityId));

                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objPrices = new List<BikePrice>();

                            while (dr.Read())
                            {
                                BikePrice objPrice = new BikePrice();
                                objPrice.MakeName = Convert.ToString(dr["MakeName"]);
                                objPrice.ModelName = Convert.ToString(dr["ModelName"]);
                                objPrice.VersionName = Convert.ToString(dr["VersionName"]);
                                objPrice.VersionId = Convert.ToUInt32(dr["VersionId"]);
								objPrice.BikeSeriesId = Convert.ToUInt32(dr["BikeSeriesId"]);
                                objPrice.Price = Convert.ToString(dr["Price"]);
                                objPrice.Insurance = Convert.ToString(dr["Insurance"]);
                                objPrice.RTO = Convert.ToString(dr["RTO"]);

                                objPrice.LastUpdatedDate = Convert.ToString(dr["LastUpdatedDate"]);
                                objPrice.LastUpdatedBy = Convert.ToString(dr["LastUpdatedBy"]);
                                objPrice.BikeModelId = Convert.ToUInt32(dr["bikemodelid"]);
                                objPrices.Add(objPrice);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.BikeShowroomPrices.GetBikePrices");
                
            }
            return objPrices;
        }


        /// <summary>
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Changed SP from 'saveversionprices_28062017' to 'saveversionprices_30082017'
        /// </summary>
        /// <param name="versionPriceList"></param>
        /// <param name="citiesList"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public bool SaveBikePrices(string versionPriceList, string citiesList, int updatedBy)
        {
            bool isUpdated = false;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("saveversionprices_30082017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_versionPriceList", DbType.String, versionPriceList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_citiesList", DbType.String, citiesList));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, updatedBy));

                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.BikeShowroomPrices.SaveBikePrices");
                
            }

            return isUpdated;
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 29-07-2017
        /// Discription : DAL method to get list of bike models of a bike make.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>List of Bike models.</returns>
        public IEnumerable<BikeModelEntityBase> GetModelsByMake(uint makeId)
        {
            IEnumerable<BikeModelEntityBase> modelList = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("par_makeid", makeId);
                param.Add("par_requesttype", "NEW");
                
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    modelList = connection.Query<BikeModelEntityBase>("getbikemodels_new", param: param, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.BikePricing.BikeShowroomPrices.GetModelsByMake_makeId:{0}",makeId));
            }

            return modelList;
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 29-07-2017
        /// Discription : DAL Method to get price last updated details of bike versions in cities.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns>List of cities, bike versions and price last updated.</returns>
        public PriceMonitoringEntity GetPriceMonitoringDetails(uint makeId, uint modelId, uint stateId)
        {
            PriceMonitoringEntity priceMonitoring = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("par_makeId", makeId);
                param.Add("par_modelId", modelId);
                param.Add("par_stateId", stateId);

                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    priceMonitoring = new PriceMonitoringEntity();
                    var reader = connection.QueryMultiple("getpricemonitoring", param: param, commandType: CommandType.StoredProcedure);
                    priceMonitoring.CityList = reader.Read<Entities.MfgCityEntity>();
                    priceMonitoring.BikeVersionList = reader.Read<BikeVersionEntityBase>();
                    priceMonitoring.PriceLastUpdatedList = reader.Read<PriceLastUpdateEntity>();
                    priceMonitoring.BikeModelList = reader.Read<BikeModelEntityBase>();
                }

            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.BikePricing.BikeShowroomPrices.GetPriceMonitoringDetails makeid:{0} modelid:{1}", makeId, modelId));
            }

            return priceMonitoring;
        }

    }
}
