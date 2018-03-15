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
                ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikePricing.BikeShowroomPrices.GetBikePrices");
                
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

        /// <summary>
        /// Created By : Deepak Israni on 21 Feb 2018
        /// Description: DAL method to generate an bikewalepricingindex (ES Index) document.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public List<ModelPriceDocument> GetModelPriceDocument(string modelIds, string cityIds)
        {
            String spName = "getmodelpriceindexbycity";

            List<ModelPriceDocument> objList = null;
            ModelPriceDocument docObj = null;
            VersionEntity verObj = null;

            uint _lastModelId = 0;
            uint _lastCityId = 0;
            uint _currentModelId = 0;
            uint _currentCityId = 0;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand(spName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelids", DbType.String, modelIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityids", DbType.String, cityIds));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.MasterDatabase))
                    {
                        if (dr != null)
                        {
                            objList = new List<ModelPriceDocument>();
                            List<VersionEntity> versions = null;

                            while (dr.Read())
                            {
                                _currentModelId = SqlReaderConvertor.ToUInt32(dr["BikeModelId"]);
                                _currentCityId = SqlReaderConvertor.ToUInt32(dr["CityId"]);

                                if (_currentModelId != _lastModelId || _currentCityId != _lastCityId)
                                {
                                    if (docObj != null)
                                    {
                                        docObj.VersionPrice = versions;
                                        objList.Add(docObj);
                                    }

                                    docObj = new ModelPriceDocument()
                                    {
                                        Id = SqlReaderConvertor.ToUInt32(dr["BikeModelId"]) + "_" + SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                        BikeModel = new ModelEntity()
                                        {
                                            ModelId = SqlReaderConvertor.ToUInt32(dr["BikeModelId"]),
                                            ModelName = Convert.ToString(dr["ModelName"]),
                                            ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                            ModelStatus = GetStatus(Convert.ToBoolean(dr["IsNewModel"]), Convert.ToBoolean(dr["IsFuturisticModel"]))
                                        },
                                        BikeMake = new MakeEntity()
                                        {
                                            MakeId = SqlReaderConvertor.ToUInt32(dr["BikeMakeId"]),
                                            MakeName = Convert.ToString(dr["MakeName"]),
                                            MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                            MakeStatus = GetStatus(Convert.ToBoolean(dr["IsNewMake"]), Convert.ToBoolean(dr["IsFuturisticMake"]))
                                        },
                                        City = new CityEntity()
                                        {
                                            CityId = SqlReaderConvertor.ToUInt32(dr["CityId"]),
                                            CityName = Convert.ToString(dr["CityName"]),
                                            CityMaskingName = Convert.ToString(dr["CityMaskingName"])
                                        }
                                    };

                                    versions = new List<VersionEntity>();

                                    _lastModelId = _currentModelId;
                                    _lastCityId = _currentCityId;
                                }

                                verObj = new VersionEntity()
                                {
                                    VersionId = SqlReaderConvertor.ToUInt32(dr["VersionId"]),
                                    VersionName = Convert.ToString(dr["VersionName"]),
                                    Exshowroom = SqlReaderConvertor.ToUInt32(dr["Price"]),
                                    VersionStatus = GetStatus(Convert.ToBoolean(dr["IsNewVersion"]), Convert.ToBoolean(dr["IsFuturisticVersion"]))
                                };

                                IList<PriceEntity> prices = new List<PriceEntity>();

                                prices.Add(new PriceEntity()
                                {
                                    PriceType = "Exshowroom",
                                    PriceValue = SqlReaderConvertor.ToUInt32(dr["Price"])
                                });

                                prices.Add(new PriceEntity()
                                {
                                    PriceType = "RTO",
                                    PriceValue = SqlReaderConvertor.ToUInt32(dr["RTO"])
                                });
                                prices.Add(new PriceEntity()
                                {
                                    PriceType = "Insurance",
                                    PriceValue = SqlReaderConvertor.ToUInt32(dr["Insurance"])
                                });

                                verObj.PriceList = prices;
                                verObj.Onroad = (uint)verObj.PriceList.Sum(prc => prc.PriceValue);

                                versions.Add(verObj);
                            }

                            if (docObj != null)
                            {
                                docObj.VersionPrice = versions;
                                objList.Add(docObj);
                            }

                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.GetModelPriceDocument: Makeid- {0}, Cityid- {1}", modelIds, cityIds));
            }

            return objList;
        }

        /// <summary>
        /// Created By : Deepak Israni on 22 Feb 2018
        /// Description: To get the status of make/model/version depending on they are new or futuristic.
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="isFuturistic"></param>
        /// <returns></returns>
        private static BikeStatus GetStatus(bool isNew, bool isFuturistic)
        {
            return !isNew ? (!isFuturistic ? BikeStatus.Discontinued : BikeStatus.Upcoming) : (!isFuturistic ? BikeStatus.New : BikeStatus.Invalid);
        }

    }
}
