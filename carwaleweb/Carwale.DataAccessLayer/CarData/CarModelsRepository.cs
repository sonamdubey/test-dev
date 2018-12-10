using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace Carwale.DAL.CarData
{
    public class CarModelsRepository : RepositoryBase, ICarModelRepository
    {
        private static string _imgHostUrl = CWConfiguration._imgHostUrl;
        /// <summary>
        /// Gets the list of all models based on type passed
        /// Written By : Supriya on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CarMakeModelEntityBase> GetAllModels(string type, bool isCriticalRead = false)
        {
            List<CarMakeModelEntityBase> carModelsList;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelCond", type);
                param.Add("v_MakeId", null);
                param.Add("v_Year", null);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    carModelsList = con.Query<CarMakeModelEntityBase>("cwmasterdb.GetCarModels_v17_7_1", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return carModelsList ?? new List<CarMakeModelEntityBase>();
        }

        /// <summary>
        /// Get all models details for makeid
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<CarModelSummary> GetAllModelsSummary(int makeId, bool isCriticalRead = false)
        {
            List<CarModelSummary> carModelsList;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeId", makeId);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    carModelsList = con.Query<CarModelSummary>("GetAllModelDetailsByMake_v16_11_7", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return carModelsList ?? new List<CarModelSummary>();
        }

        /// <summary>
        /// Gets the list of all models based on type passed and makeId passed
        /// Written By : Supriya on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CarModelEntityBase> GetCarModelsByType(string type, int? makeId, int? year, int threeSixtyType, bool isCriticalRead = false)
        {
            List<CarModelEntityBase> carModelsList;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelCond", type);
                param.Add("v_MakeId", makeId > 0 ? makeId : null);
                param.Add("v_Year", year);
                param.Add("v_ThreeSixtyType", threeSixtyType);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    carModelsList = con.Query<CarModelEntityBase>("GetCarModels_v18_5_8", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }

            return carModelsList ?? new List<CarModelEntityBase>();
        }

        /// <summary>
        /// Gets the list of model details based on the modelId passed
        /// Written By : Shalini on 15/07/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public CarModelDetails GetModelDetailsById(int modelId, bool isCriticalRead = false)
        {
            var modelDetails = new CarModelDetails();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId > 0 ? modelId : 0);
                param.Add("v_MakeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("v_MakeName", dbType: DbType.String, direction: ParameterDirection.Output);
                param.Add("v_ModelName", dbType: DbType.String, direction: ParameterDirection.Output);
                param.Add("v_MaskingName", dbType: DbType.String, direction: ParameterDirection.Output);
                param.Add("v_OriginalImgPath", dbType: DbType.String, direction: ParameterDirection.Output);
                param.Add("v_HostURL", dbType: DbType.String, direction: ParameterDirection.Output);
                param.Add("v_Looks", dbType: DbType.Double, direction: ParameterDirection.Output);
                param.Add("v_Performance", dbType: DbType.Double, direction: ParameterDirection.Output);
                param.Add("v_Comfort", dbType: DbType.Double, direction: ParameterDirection.Output);
                param.Add("v_ValueForMoney", dbType: DbType.Double, direction: ParameterDirection.Output);
                param.Add("v_FuelEconomy", dbType: DbType.Double, direction: ParameterDirection.Output);
                param.Add("v_ReviewRate", dbType: DbType.Double, direction: ParameterDirection.Output);
                param.Add("v_ReviewCount", dbType: DbType.Decimal, direction: ParameterDirection.Output);
                param.Add("v_Futuristic", dbType: DbType.Int16, direction: ParameterDirection.Output, size: 1);
                param.Add("v_RootId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("v_RootName", dbType: DbType.String, direction: ParameterDirection.Output);
                param.Add("v_New", dbType: DbType.Int16, direction: ParameterDirection.Output, size: 1);
                param.Add("v_Used", dbType: DbType.Int16, direction: ParameterDirection.Output, size: 1);
                param.Add("v_MinPrice", dbType: DbType.Double, direction: ParameterDirection.Output);
                param.Add("v_MaxPrice", dbType: DbType.Double, direction: ParameterDirection.Output);
                param.Add("v_MinAvgPrice", dbType: DbType.Double, direction: ParameterDirection.Output);
                param.Add("v_VideoCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("v_BodyStyleId", dbType: DbType.Int16, direction: ParameterDirection.Output);
                param.Add("v_SubSegmentId", dbType: DbType.Int16, direction: ParameterDirection.Output);
                param.Add("v_PopularVersion", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("v_PhotoCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("v_Is360ExteriorAvailable", dbType: DbType.Byte, direction: ParameterDirection.Output);
                param.Add("v_Is360OpenAvailable", dbType: DbType.Byte, direction: ParameterDirection.Output);
                param.Add("v_IsDeleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                param.Add("v_Indian", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                param.Add("v_Imported", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                param.Add("v_DiscontinuationDate", dbType: DbType.DateTime, direction: ParameterDirection.Output);
                param.Add("v_ModelLaunchDate", dbType: DbType.DateTime, direction: ParameterDirection.Output);
                param.Add("v_IsSolidColor", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                param.Add("v_IsMetallicColor", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                param.Add("v_ModelPopularity", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("v_Is360InteriorAvailable", dbType: DbType.Byte, direction: ParameterDirection.Output);
                param.Add("v_ReplacedModelId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    con.Execute("GetModelDetails_v17_9_1", param, commandType: CommandType.StoredProcedure);
                }
                modelDetails.ModelId = modelId;
                modelDetails.MakeId = Convert.ToInt32(param.Get<int?>("v_MakeId"));
                modelDetails.ModelName = param.Get<string>("v_ModelName");
                modelDetails.MaskingName = param.Get<string>("v_MaskingName");
                modelDetails.MakeName = param.Get<string>("v_MakeName");
                string imagePath = param.Get<string>("v_OriginalImgPath");
                modelDetails.ModelImageLarge = _imgHostUrl + ImageSizes._160X89 + imagePath;
                modelDetails.ModelImageSmall = _imgHostUrl + ImageSizes._110X61 + imagePath;
                modelDetails.HostUrl = _imgHostUrl;
                modelDetails.OriginalImage = imagePath;
                modelDetails.Futuristic = Convert.ToBoolean(param.Get<Int16?>("v_Futuristic"));
                modelDetails.RootId = Convert.ToInt16(param.Get<int?>("v_RootId"));
                modelDetails.RootName = param.Get<string>("v_RootName");
                modelDetails.New = Convert.ToBoolean(param.Get<Int16?>("v_New"));
                modelDetails.Used = Convert.ToInt16(param.Get<Int16?>("v_Used"));
                modelDetails.MinPrice = Convert.ToDouble(param.Get<double?>("v_MinPrice"));
                modelDetails.MaxPrice = Convert.ToDouble(param.Get<double?>("v_MaxPrice"));
                modelDetails.MinAvgPrice = Convert.ToInt32(param.Get<double?>("v_MinAvgPrice"));
                modelDetails.ModelRating = CustomParser.parseDoubleObject(param.Get<double?>("v_ReviewRate"));
                modelDetails.FuelEconomy = CustomParser.parseDoubleObject(param.Get<double?>("v_FuelEconomy"));
                modelDetails.ValueForMoney = CustomParser.parseDoubleObject(param.Get<double?>("v_ValueForMoney"));
                modelDetails.Comfort = CustomParser.parseDoubleObject(param.Get<double?>("v_Comfort"));
                modelDetails.Performance = CustomParser.parseDoubleObject(param.Get<double?>("v_Performance"));
                modelDetails.Looks = CustomParser.parseDoubleObject(param.Get<double?>("v_Looks"));
                modelDetails.ReviewCount = Convert.ToInt16(param.Get<decimal?>("v_ReviewCount"));
                modelDetails.VideoCount = Convert.ToInt32(param.Get<int?>("v_VideoCount"));
                modelDetails.SubSegmentId = CustomParser.parseByteObject(param.Get<Int16?>("v_SubSegmentId"));
                modelDetails.BodyStyleId = CustomParser.parseIntObject(param.Get<Int16?>("v_BodyStyleId"));
                modelDetails.PopularVersion = CustomParser.parseIntObject(param.Get<Int32?>("v_PopularVersion"));
                modelDetails.PhotoCount = CustomParser.parseIntObject(param.Get<Int32?>("v_PhotoCount"));
                modelDetails.Is360ExteriorAvailable = CustomParser.parseBoolObject(param.Get<Byte?>("v_Is360ExteriorAvailable"));
                modelDetails.Is360OpenAvailable = CustomParser.parseBoolObject(param.Get<Byte?>("v_Is360OpenAvailable"));
                modelDetails.IsDeleted = CustomParser.parseBoolObject(param.Get<Byte?>("v_IsDeleted"));
                modelDetails.Indian = CustomParser.parseBoolObject(param.Get<Byte?>("v_Indian"));
                modelDetails.Imported = CustomParser.parseBoolObject(param.Get<Byte?>("v_Imported"));
                modelDetails.DiscontinuationDate = param.Get<DateTime?>("v_DiscontinuationDate");
                modelDetails.ModelLaunchDate = param.Get<DateTime?>("v_ModelLaunchDate");
                modelDetails.IsSolidColor = CustomParser.parseBoolObject(param.Get<Byte?>("v_IsSolidColor"));
                modelDetails.IsMetalicColor = CustomParser.parseBoolObject(param.Get<Byte?>("v_IsMetallicColor"));
                modelDetails.ModelPopularity = CustomParser.parseIntObject(param.Get<int?>("v_ModelPopularity"));
                modelDetails.Is360InteriorAvailable = CustomParser.parseBoolObject(param.Get<Byte?>("v_Is360InteriorAvailable"));
                modelDetails.ReplacedModelId = Convert.ToInt16(param.Get<int?>("v_ReplacedModelId"));
            }
            catch (SqlException ex)
            {
                Logger.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }

            return modelDetails;
        }

        /// <summary>
        /// Gets the list of all models based on makeId passed
        /// and models are ordered by popularity
        /// Written By : Shalini on 08/07/2014
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public List<CarModelSummary> GetModelsByMake(int makeId, bool isCriticalRead = false)
        {
            List<CarModelSummary> carModelsList;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeId", makeId > 0 ? makeId : 0);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    carModelsList = con.Query<CarModelSummary>("GetCarModels_BrowsePage_v17_9_1", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return carModelsList ?? new List<CarModelSummary>();
        }

        ///<summary>
        ///Gets the list of upcoming car models based on makeId passed
        ///Written By : Ajay Singh on 24 nov 2016
        ///</summary>
        public List<UpcomingCarModel> GetUpcomingCarModelsByMake(int makeId, int count, bool isCriticalRead = false)
        {
            List<UpcomingCarModel> upcomingModelsList;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeId", makeId > 0 ? makeId : 0);
                param.Add("v_Cnt", count > 0 ? count : 0);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    upcomingModelsList = con.Query<CarPrice, CarImageBase, UpcomingCarModel, UpcomingCarModel>("GetUpcomingCars_v16_11_7",
                        (price, image, upcomingcars) =>
                        {
                            upcomingcars.Price = new CarPrice
                            {
                                MinPrice = price.MinPrice > 0 ? (price.MinPrice * 100000) : 0,
                                MaxPrice = price.MaxPrice > 0 ? (price.MaxPrice * 100000) : 0,
                                AvgPrice = price.AvgPrice
                            };
                            upcomingcars.ExpectedLaunch = GetExpectedLaunchDate(upcomingcars.LaunchDate, upcomingcars.CWConfidence == (int)CwConfidence.High);
                            upcomingcars.Image = image;
                            return upcomingcars;
                        },
                          param, commandType: CommandType.StoredProcedure, splitOn: "HostUrl,MakeId").ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return upcomingModelsList ?? new List<UpcomingCarModel>();
        }

        /// <summary>
        /// Gets the list of model colors based on modelId passed 
        /// Written By : Shalini on 11/07/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<ModelColors> GetModelColorsByModel(int modelId, bool isCriticalRead = false)
        {
            List<ModelColors> modelColorsList;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    modelColorsList = con.Query<ModelColors>("GetModelColors_v16_11_7", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return (modelColorsList ?? new List<ModelColors>());
        }

        /// <summary>
        /// Created By : Kirtan Shetty
        /// Returns Car Model details for the given Year and Make
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="MakeId"></param>
        /// <returns></returns>
        public List<ValuationModel> GetValuationModels(int year, int makeId, bool isCriticalRead = false)
        {
            var carModelList = new List<ValuationModel>();
            string hostUrl = "https://" + ConfigurationManager.AppSettings["WebApi"] + "/webapi/carversionsdata/getvaluationversion/?year=" + year + "&model=";
            try
            {
                var param = new DynamicParameters();
                param.Add("v_caryear", year);
                param.Add("v_makeid", makeId);

                using (var con = isCriticalRead ? ClassifiedMySqlMasterConnection : ClassifiedMySqlReadConnection)
                {
                    carModelList = con.Query<ValuationModel>("GetValuationModels",
                        param, commandType: CommandType.StoredProcedure).Select<ValuationModel, ValuationModel>(vm =>
                        {
                            vm.Url = hostUrl + vm.Id;
                            return vm;
                        }).AsList();
                    LogLiveSps.LogSpInGrayLog("GetValuationModels");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return carModelList;
        }

        /// <summary>
        /// Written By : Shalini on 16/09/14
        /// Returns the list of Price in other cities based on modelId and cityId passed 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns>list of Price in other cities </returns>
        public List<ModelPriceInOtherCities> GetPricesInOtherCities(int modelId, int cityId, bool isCriticalRead = false)
        {
            List<ModelPriceInOtherCities> priceList;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);
                param.Add("v_cityid", cityId);
                using (var con = isCriticalRead ? NewCarMySqlMasterConnection : NewCarMySqlReadConnection)
                {
                    priceList = con.Query<ModelPriceInOtherCities>("getmodelcityprices_v18_6_1", param, commandType: CommandType.StoredProcedure).AsList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return priceList ?? new List<ModelPriceInOtherCities>();
        }

        /// <summary>
        /// Returns the details of Upcoming Car like Expected launchdate etc. 
        /// Written By : Shalini on 29/09/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public UpcomingCarModel GetUpcomingCarDetails(int modelId, bool isCriticalRead = false)
        {
            UpcomingCarModel upcomingCarDetail;

            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    upcomingCarDetail = con.Query<UpcomingCarModel, CarPrice, UpcomingCarModel>("UpcomingCarDetail_v16_11_7",
                        (upcomingcars, price) =>
                        {
                            upcomingcars.Price = price;
                            upcomingcars.ExpectedLaunch = GetExpectedLaunchDate(upcomingcars.LaunchDate, upcomingcars.CWConfidence == (int)CwConfidence.High);
                            return upcomingcars;
                        },
                        param, commandType: CommandType.StoredProcedure, splitOn: "MinPrice").FirstOrDefault();
                    
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return upcomingCarDetail ?? new UpcomingCarModel();
        }

        /// <summary>
        /// Returns the PageMetaTag Values 
        /// Written By : Shalini on 30/09/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public PageMetaTags GetModelPageMetaTags(int modelId, int pageId, bool isCriticalRead = false)
        {
            PageMetaTags pageMetaTags;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_PageId", pageId > 0 ? pageId : 0);
                param.Add("v_ModelId", modelId > 0 ? modelId : 0);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    pageMetaTags = con.Query<PageMetaTags>("PageMetaTagsByModel_v18_6_1", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return pageMetaTags ?? new PageMetaTags();
        }

        /// <summary>
        /// Returns the list of Car Models based on the Parameters passed 
        /// Written By : Shalini on 09/12/14
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public List<CarModelDetails> GetNewCarSearchResult(CarModelURI uri, bool isCriticalRead = false)
        {
            var modelDetails = new List<CarModelDetails>();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("NewCarSearchResult_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CarMakeIds", DbType.String, uri.CarMakeIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_FuelTypeIds", DbType.String, uri.FuelTypeIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_TransmissionTypeIds", DbType.String, uri.TransmissionTypeIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_BodyStyleIds", DbType.String, uri.BodyStyleIds));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_SortCriteria", DbType.String, uri.SortCriteria));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_SortOrder", DbType.String, uri.SortOrder));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MinPrice", DbType.Int32, uri.MinPrice));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MaxPrice", DbType.Int32, uri.MaxPrice));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex", DbType.Int32, uri.StartIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_LastIndex", DbType.Int32, uri.LastIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ExShowroomCityId", DbType.Int32, Convert.ToInt16(ConfigurationManager.AppSettings["DefaultCityId"])));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, isCriticalRead ? DbConnections.NewCarMySqlReadConnection : DbConnections.NewCarMySqlMasterConnection))
                    {
                        while (dr.Read())
                        {
                            modelDetails.Add(new CarModelDetails()
                            {
                                MakeId = Convert.ToInt16(dr["MakeId"]),
                                MakeName = dr["MakeName"].ToString(),
                                ModelId = Convert.ToInt16(dr["ModelId"]),
                                ModelName = dr["ModelName"].ToString(),
                                HostUrl = _imgHostUrl,
                                OriginalImage = dr["OriginalImgPath"].ToString(),
                                MinPrice = (dr["MinPrice"] != DBNull.Value ? Convert.ToDouble(dr["MinPrice"]) : 0),
                                MaxPrice = (dr["MaxPrice"] != DBNull.Value ? Convert.ToDouble(dr["MaxPrice"]) : 0),
                                ModelRating = Convert.ToDouble(dr["MoReviewRate"]),
                                CarCount = Convert.ToInt16(dr["CarCount"]),
                                ModelImageSmall = _imgHostUrl + ImageSizes._110X61 + dr["OriginalImgPath"],
                                ModelImageLarge = _imgHostUrl + ImageSizes._210X118 + dr["OriginalImgPath"],
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return modelDetails;
        }

        #region FRQ - Frequently Requested Queries
        /// <summary>
        /// Written By : Ashwini Todkar on 22 July 2015
        /// Method to get top selling models
        /// </summary>
        /// <param name="count">number of top records</param>
        /// <returns></returns>

        public List<TopSellingCarModel> GetTopSellingCarModels(int topCount, bool isCriticalRead = false)
        {
            List<TopSellingCarModel> _topSellingModels = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("GetTopSellingCars_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_DisplayCount", DbType.Int32, topCount));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, isCriticalRead ? DbConnections.CarDataMySqlMasterConnection : DbConnections.CarDataMySqlReadConnection))
                    {
                        _topSellingModels = new List<TopSellingCarModel>();

                        while (dr.Read())
                        {
                            int modelId;
                            int makeId;
                            int _minAvgPrice;
                            ushort reviewCount;
                            float _minPrice;
                            float _maxPrice;
                            float reviewrate;

                            float.TryParse((dr["MinPrice"] != DBNull.Value) ? dr["MinPrice"].ToString() : "0", out _minPrice);
                            float.TryParse((dr["MaxPrice"] != DBNull.Value) ? dr["MaxPrice"].ToString() : "0", out _maxPrice);
                            float.TryParse((dr["ReviewRate"] != DBNull.Value) ? dr["ReviewRate"].ToString() : "0", out reviewrate);
                            UInt16.TryParse(dr["ReviewCount"].ToString(), out reviewCount);
                            Int32.TryParse(dr["ModelId"] != DBNull.Value ? dr["ModelId"].ToString() : "0", out modelId);
                            Int32.TryParse(dr["MakeId"] != DBNull.Value ? dr["MakeId"].ToString() : "0", out makeId);
                            int.TryParse((dr["MinAvgPrice"] != DBNull.Value) ? dr["MinAvgPrice"].ToString() : "0", out _minAvgPrice);

                            _topSellingModels.Add(new TopSellingCarModel
                            {

                                Image = new CarImageBase { HostUrl = _imgHostUrl, ImagePath = dr["OriginalImgPath"].ToString() },
                                Make = new MakeEntity
                                {
                                    MakeName = dr["MakeName"].ToString(),
                                    MakeId = makeId
                                },
                                Model = new ModelEntity
                                {
                                    ModelName = dr["ModelName"].ToString(),
                                    ModelId = modelId,
                                    MaskingName = dr["MaskingName"].ToString()
                                },
                                Price = new CarPrice
                                {
                                    MinPrice = _minPrice,
                                    MaxPrice = _maxPrice,
                                    AvgPrice = _minAvgPrice
                                },
                                Review = new CarReviewBase
                                {
                                    ReviewCount = reviewCount,
                                    OverallRating = reviewrate
                                },
                                City = new Entity.Geolocation.City
                                {
                                    CityId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCityId"]),
                                    CityName = ConfigurationManager.AppSettings["DefaultCityName"]
                                }

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return _topSellingModels;



        }

        /// <summary>
        ///Gets the list of newly launched car models 
        //Written By : Ajay Singh on 24 nov 2016
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<LaunchedCarModel> GetLaunchedCarModels(int topCount, bool isCriticalRead = false)
        {
            var _launchedModels = new List<LaunchedCarModel>();
            try
            {
                ushort reviewCount;
                int modelId;
                int makeId;
                int basicId;
                int minAvgPrice;
                float minPrice;
                float maxPrice;
                using (DbCommand cmd = DbFactory.GetDBCommand("GetTopNewLaunches_v16_11_6"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_cnt", DbType.Int32, topCount));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, isCriticalRead ? DbConnections.CarDataMySqlMasterConnection : DbConnections.CarDataMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            float.TryParse((dr["MinPrice"] != DBNull.Value) ? dr["MinPrice"].ToString() : "0", out minPrice);
                            float.TryParse((dr["MaxPrice"] != DBNull.Value) ? dr["MaxPrice"].ToString() : "0", out maxPrice);
                            int.TryParse(dr["MakeId"] != DBNull.Value ? dr["MakeId"].ToString() : "0", out makeId);
                            int.TryParse(dr["ModelId"] != DBNull.Value ? dr["ModelId"].ToString() : "0", out modelId);
                            int.TryParse(dr["BasicId"] != DBNull.Value ? dr["BasicId"].ToString() : "0", out basicId);
                            UInt16.TryParse(dr["ReviewCount"] != DBNull.Value ? dr["ReviewCount"].ToString() : "0", out reviewCount);
                            int.TryParse((dr["MinAvgPrice"] != DBNull.Value) ? dr["MinAvgPrice"].ToString() : "0", out minAvgPrice);

                            _launchedModels.Add(new LaunchedCarModel
                            {
                                Model = new ModelEntity
                                {
                                    ModelId = modelId,
                                    ModelName = dr["Model"].ToString(),
                                    MaskingName = dr["MaskingName"].ToString()
                                },
                                Make = new MakeEntity
                                {
                                    MakeName = dr["Make"].ToString(),
                                    MakeId = makeId
                                },

                                Image = new CarImageBase { HostUrl = _imgHostUrl, ImagePath = dr["OriginalImgPath"].ToString() },

                                Price = new CarPrice
                                {
                                    MinPrice = minPrice,
                                    MaxPrice = maxPrice,
                                    AvgPrice = minAvgPrice
                                },
                                Review = new CarReviewBase { ReviewCount = reviewCount },
                                City = new Entity.Geolocation.City
                                {
                                    CityId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCityId"]),
                                    CityName = ConfigurationManager.AppSettings["DefaultCityName"]
                                },
                                BasicId = basicId
                            });
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return _launchedModels;
        }
        #endregion

        /// <summary>
        ///Gets the list of newly launched car models 
        ///Written By : Ajay Singh on 24 nov 2016
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<LaunchedCarModel> GetLaunchedCarModelsV1(bool isCriticalRead = false)
        {
            List<LaunchedCarModel> _launchedModels;
            try
            {
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    _launchedModels = con.Query<MakeEntity, ModelEntity, CarImageBase, CarReviewBase,
                        VersionEntity, LaunchedCarModel, LaunchedCarModel>("GetTopNewLaunches_v17_8_1",
                        (make, model, image, review, version, justlaunch) =>
                        {
                            justlaunch.Make = make;
                            justlaunch.Model = model;
                            justlaunch.Image = image;
                            justlaunch.Review = review;
                            justlaunch.Version = version;
                            return justlaunch;
                        },
                        commandType: CommandType.StoredProcedure,
                        splitOn: "ModelId,HostUrl,ReviewCount,VersionId,LaunchedDate").ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return _launchedModels ?? new List<LaunchedCarModel>();
        }


        /// <summary>
        /// For getting alternate Cars with active campaigns on submitting a lead
        /// Created: Vicky Lund, 03/12/2015
        /// </summary>
        /// <returns></returns>
        public List<CampaignInput> GetUserRecentLeadModels(string mobileNo, bool isCriticalRead = false)
        {
            List<CampaignInput> modelList = new List<CampaignInput>();
            try
            {
                using (var cmd = DbFactory.GetDBCommand("GetUserRecentLeadModels_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MobileNo", DbType.String, 100, mobileNo));

                    using (var dr = MySqlDatabase.SelectQuery(cmd, isCriticalRead ? DbConnections.NewCarMySqlMasterConnection : DbConnections.NewCarMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            CampaignInput campaignInputs = new CampaignInput();
                            campaignInputs.CampaignId = -1;
                            campaignInputs.ModelId = CustomParser.parseIntObject(dr["ModelId"]);
                            campaignInputs.CityId = CustomParser.parseIntObject(dr["CityId"]);
                            campaignInputs.ZoneId = CustomParser.parseIntObject(dr["ZoneId"]);
                            campaignInputs.PlatformId = CustomParser.parseShortObject(dr["PlatformId"]);
                            modelList.Add(campaignInputs);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Logger.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return modelList;
        }

        /// <summary>
        /// Written By : Chetan Thambad on <17/03/2016>
        /// Get all model details with or without bodystyle 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<CarModelDetails> GetModelsByBodyStyle(int bodyStyle, bool similarBodyStyle, bool isCriticalRead = false)
        {
            List<CarModelDetails> modelDetails;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_BodyStyle", bodyStyle);
                param.Add("v_similarBodyStyle", similarBodyStyle);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    modelDetails = con.Query<CarModelDetails>("GetModelsByBodyStyle_v16_11_7", param, commandType: CommandType.StoredProcedure).ToList();

                    if (modelDetails.Count <= 0)
                    {
                        return null;
                    }
                }
                modelDetails.ForEach(x =>
                {
                    x.ModelImageLarge = x.HostUrl + ImageSizes._160X89 + x.OriginalImgPath;
                    x.ModelImageSmall = x.HostUrl + ImageSizes._110X61 + x.OriginalImgPath;
                });
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                throw;
            }
            return modelDetails ?? new List<CarModelDetails>();
        }

        /// <summary>
        /// Written By : Supreksha on <23/12/2016>
        /// Get all model details with bodystyle and subsegment
        /// </summary>
        /// <returns></returns>
        public List<ModelDetails> GetModelSpecs(bool isCriticalRead = false)
        {
            try
            {
                using (var con = isCriticalRead ? EsMySqlMasterConnection : EsMySqlReadConnection)
                {
                    var modelDetailsList = con.Query<ModelDetails>("GetModelDetailsUserProfiling_v18_11_1", commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("GetModelDetailsUserProfiling_v18_11_1");
                    if (modelDetailsList != null && modelDetailsList.AsList().Count > 0)
                    {
                        return modelDetailsList.AsList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return null;
        }
        public DataSet GetUpcomingCarsForOldVersionApp(int id, int makeId, bool isCriticalRead = false)
        {
            DataSet ds;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("WA_UpComingCarDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Id", DbType.Int32, Convert.ToInt32(id)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MakeId", DbType.Int32, Convert.ToInt32(makeId)));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, isCriticalRead ? DbConnections.CarDataMySqlMasterConnection : DbConnections.CarDataMySqlReadConnection);
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
                throw;
            }
            return ds ?? new DataSet();
        }

        /// <summary>
        /// Written By : Supreksha on <16/2/2017>
        /// Get top models based on popularity
        /// </summary>
        /// <returns></returns>
        public List<CarMakeModelAdEntityBase> GetTrendingModels(int count, bool isCriticalRead = false)
        {
            try
            {
                using (var con = isCriticalRead ? EsMySqlMasterConnection : EsMySqlReadConnection)
                {
                    var trendingModelsList = con.Query<CarMakeModelAdEntityBase>("GetTrendingModels", new { v_Count = count }, commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("GetTrendingModels");

                    if (trendingModelsList != null && trendingModelsList.AsList().Count > 0)
                    {
                        return trendingModelsList.AsList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return null;
        }

        /// <!-- written by Meet Shah on 19 July 2017-->
        /// <summary>
        /// This method returns a dictionary that contains mapping of 
        /// modelid to its rank in its specific body type. 
        /// </summary>
        /// <param name="count">Maximum count in each
        /// body type is defined by the count parameter which defaults to 10</param>
        /// <param name="bodytypes">Comma separated string of bodytypes 
        /// for which ranks are required.</param>
        /// <returns>Dictionary<CarBodyStyle, Tuple<int[], string>></returns>
        public Dictionary<CarBodyStyle, Tuple<int[], string>> GetCarRanksByBodyType(string bodytypes, int count, bool isCriticalRead = false)
        {
            Dictionary<CarBodyStyle, Tuple<int[], string>> ranksByBodyStyle = null;
            List<dynamic> rankList;
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("v_Count", count, DbType.Int16, ParameterDirection.Input);
                param.Add("v_BodyTypes", bodytypes, DbType.String, ParameterDirection.Input);

                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    rankList = con.Query("GetCarRanksByBodyTypes_v17_8_1", param, commandType: CommandType.StoredProcedure).ToList();
                }

                if (rankList != null && rankList.Count > 0)
                {
                    ranksByBodyStyle = new Dictionary<CarBodyStyle, Tuple<int[], string>>();
                    foreach (int bodyStyleId in bodytypes.Split(',').Select(int.Parse))
                    {
                        IEnumerable<dynamic> bodyTypeRanks = rankList.Where(x => x.BodyType == bodyStyleId);
                        int[] ids = bodyTypeRanks.Select(x => (int)x.id).ToArray();
                        string[] models = bodyTypeRanks.Take(2).Select(x => (string)(x.MakeName + " " + x.ModelName)).ToArray(); //Only top 2 models needed
                        ranksByBodyStyle.Add((CarBodyStyle)bodyStyleId, new Tuple<int[], string>(ids, string.Join(", ", models)));
                    }
                }
                return ranksByBodyStyle;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarModelsRepository.GetCarRanksByBodyType()");
                throw;
            }
        }

        ///<summary>
        ///Gets the list of similar upcoming car models based on modelId passed
        ///Written By : Meet Shah and Ashutosh Udeniya on 6 September 2017
        ///</summary>
        public UpcomingCarModel GetSimilarUpcomingCarModel(int modelId, bool isCriticalRead = false)
        {
            var upcomingModel = new UpcomingCarModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);

                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    upcomingModel = con.Query<UpcomingCarModel, CarPrice, CarImageBase, UpcomingCarModel>("GetSimilarUpcomingCar",
                        (upcomingcars, price, image) =>
                        {
                            upcomingcars.Price = new CarPrice
                            {
                                MinPrice = price.MinPrice > 0 ? (price.MinPrice * 100000) : 0,
                                MaxPrice = price.MaxPrice > 0 ? (price.MaxPrice * 100000) : 0
                            };
                            upcomingcars.Image = image;
                            upcomingcars.ExpectedLaunch = GetExpectedLaunchDate(upcomingcars.LaunchDate, upcomingcars.CWConfidence == (int)CwConfidence.High);
                            return upcomingcars;
                        },
                        param, commandType: CommandType.StoredProcedure, splitOn: "MinPrice,HostUrl").SingleOrDefault();
                }
            }

            catch (SqlException ex)
            {
                Logger.LogException(ex, "CarModelsRepository.GetSimilarUpcomingCar()");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogException(ex);
                return upcomingModel; // This handles no similar upcoming available for given model
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarModelsRepository.GetSimilarUpcomingCar()");
                throw;
            }
            return upcomingModel;
        }

        ///<summary>
        ///Gets the upgraded upcoming car model based on modelId passed
        ///Written By : Meet Shah on 15/09/2017
        ///</summary>
        public UpcomingCarModel GetUpgradedModel(int modelId, bool isCriticalRead = false)
        {
            var upcomingModel = new UpcomingCarModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);

                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    upcomingModel = con.Query<UpcomingCarModel, CarPrice, CarImageBase, UpcomingCarModel>("GetUpgradedModel_v17_9_1",
                       (upcomingcars, price, image) =>
                       {
                           upcomingcars.Price = new CarPrice
                           {
                               MinPrice = price.MinPrice > 0 ? (price.MinPrice * 100000) : 0,
                               MaxPrice = price.MaxPrice > 0 ? (price.MaxPrice * 100000) : 0
                           };
                           upcomingcars.Image = image;
                           return upcomingcars;
                       },
                       param, commandType: CommandType.StoredProcedure, splitOn: "MinPrice,HostUrl").SingleOrDefault();
                }
            }

            catch (SqlException ex)
            {
                Logger.LogException(ex, "CarModelsRepository.GetSimilarUpcomingCar()");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogException(ex);
                return upcomingModel; // This handles no upgraded upcoming available for given model
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarModelsRepository.GetSimilarUpcomingCar()");
                throw;
            }
            return upcomingModel;
        }

        public CarModelMaskingResponse GetModelByMaskingName(string maskingName, bool isCriticalRead = false)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MaskingName", maskingName);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    CarModelMaskingResponse maskingObject = con.Query<CarModelMaskingResponse>("GetCarModelMaskingNames_v18_4_1", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return maskingObject ?? new CarModelMaskingResponse();
                }
            }
            catch (SqlException ex)
            {
                Logger.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public List<ModelSummary> GetActiveModelsByMake(int makeId, bool isCriticalRead = false)
        {
            List<ModelSummary> carModelsList;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MakeId", makeId > 0 ? makeId : 0);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    carModelsList = con.Query<ModelSummary>("GetActiveCarModels_v18_6_1", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return carModelsList ?? new List<ModelSummary>();
        }

        public string GetExpectedLaunchDate(DateTime launchDate, bool isHighConfidence)
        {
            if (launchDate == DateTime.MinValue) return string.Empty;

            string expectedLaunch = launchDate.ToString("MMM yyyy", CultureInfo.InvariantCulture);
            if (isHighConfidence && DateTimeUtility.IsDateWithIn31Days(launchDate, DateTime.Now))
            {
                return String.Format("{0}{1} {2}", launchDate.Day, Format.GetNumberSuffix(launchDate.Day), expectedLaunch);

            }
            return expectedLaunch;
        }
    }
}