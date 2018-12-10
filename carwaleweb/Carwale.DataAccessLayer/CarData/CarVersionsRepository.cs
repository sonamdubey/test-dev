using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Interfaces;
using Carwale.Entity.CarData;
using System.Data;
using Carwale.Entity.Common;
using System.Configuration;
using Carwale.Entity.Enum;
using Carwale.Utility;
using Carwale.Notifications.Logs;
using Dapper;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using System.Data.SqlClient;
using Carwale.Entity;
using Carwale.Entity.CompareCars;
using MySql.Data.MySqlClient;

namespace Carwale.DAL.CarData
{
    public class CarVersionsRepository : RepositoryBase, ICarVersionRepository
    {
        public List<CarVersionEntity> GetCarVersionsByType(string type, int modelId, UInt16? year = null, bool isCriticalRead = false)
        {
            List<CarVersionEntity> carVersionsList = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_VersionCond", type);
                param.Add("v_ModelId", modelId);
                param.Add("v_year", year);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    carVersionsList = con.Query<CarVersionEntity>("GetCarVersions_v18_3_8", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return carVersionsList ?? new List<CarVersionEntity>();
        }

        /// <summary>
        /// Gets the list of CarVersions based on the modelId passed 
        /// Written By : Shalini on 14/07/14
        /// Modified By : Shalini on 20/11/14 
        /// Modified for retrieving TransmissionId,BodyStyleId,CarFuelType,FuelTypeId
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public List<CarVersions> GetVersionSummaryByModel(int modelId, Status status, bool isCriticalRead = false)
        {
            List<CarVersions> availableVersionsList = null;

            try
            {
                var param = new DynamicParameters();
                param.Add("v_ModelId", modelId);
                param.Add("v_Type", status.ToString());
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    availableVersionsList = con.Query<CarVersions>("GetVersions_BrowsePage_v18_3_1", param, commandType: CommandType.StoredProcedure).ToList();
                }
                return availableVersionsList;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public List<ValuationVersions> GetValuationVersion(int year, int modelId, bool isCriticalRead = false)
        {
            List<ValuationVersions> carVersionList = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_caryear", year);
                param.Add("v_modelid", modelId);

                using (var con = isCriticalRead ? ClassifiedMySqlMasterConnection : ClassifiedMySqlReadConnection)
                {
                    carVersionList = con.Query<ValuationVersions>("GetValuationVersions_V15_8_1",
                        param, commandType: CommandType.StoredProcedure).Select<ValuationVersions, ValuationVersions>(vv =>
                        {
                            if (vv.OriginalImgPath == null)
                            {
                                vv.OriginalImgPath = string.Empty;
                                vv.SmallImage = vv.HostUrl + ImageSizes._310X174 + vv.OriginalImgPath;
                                vv.LargeImage = vv.HostUrl + ImageSizes._762X429 + vv.OriginalImgPath;
                            }
                            return vv;
                        }).AsList();
                    LogLiveSps.LogSpInGrayLog("GetValuationVersions_V15_8_1");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return carVersionList ?? new List<ValuationVersions>();
        }

        /// <summary>
        /// Gets the Car details based on version id 
        /// Written By : Ahish Verma on 29/09/14
        /// Modified to correct the MinPrice value from default zero and added CityId as a parameter
        /// Modified By: Shalini Nair on 14/11/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public CarVersionDetails GetVersionDetailsById(int versionId, bool isCriticalRead = false)
        {
            CarVersionDetails versionDetailsList = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_VersionId", versionId);
                param.Add("v_CityId", ConfigurationManager.AppSettings["DefaultCityId"]);
                using (var con = isCriticalRead ? NewCarMySqlMasterConnection : NewCarMySqlReadConnection)
                {
                    versionDetailsList = con.Query<CarVersionDetails>("GetPQVersionDetails_v18_8_6", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
                if (versionDetailsList != null)
                {
                    versionDetailsList.ShareUrl = "https://" + ConfigurationManager.AppSettings["WebApi"] + "/" + Format.FormatSpecial(versionDetailsList.MakeName)
                    + "-cars/" + versionDetailsList.MaskingName + "/" + versionDetailsList.VersionMasking + "-" + versionDetailsList.VersionId + "/";
                    versionDetailsList.ModelImageLarge = CWConfiguration._imgHostUrl + ImageSizes._160X89 + versionDetailsList.OriginalImgPath;
                    versionDetailsList.ModelImageSmall = CWConfiguration._imgHostUrl + ImageSizes._110X61 + versionDetailsList.OriginalImgPath;
                    versionDetailsList.ModelImageXtraLarge = CWConfiguration._imgHostUrl + ImageSizeHash.GetImageSize("XLarge") + versionDetailsList.OriginalImgPath;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }

            return versionDetailsList ?? new CarVersionDetails();
        }

        /// <summary>
        /// Returns the List of Carversions of a Model apart from the VersionId passed 
        /// Written By : Shalini on 19/12/14
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="versionId"></param>
        /// <returns></returns> 
        public List<CarVersions> GetOtherCarVersionsOfModel(int versionId, bool isCriticalRead = false)
        {
            var otherVersions = new List<CarVersions>();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetOtherVersions_17_11_1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_versionId", DbType.Int16, versionId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, isCriticalRead ? DbConnections.CarDataMySqlMasterConnection : DbConnections.CarDataMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            otherVersions.Add(new CarVersions
                            {
                                Id = Convert.ToInt16(dr["VersionId"]),
                                Version = dr["VersionName"].ToString(),
                                New = Convert.ToBoolean(dr["New"]),
                                SpecsSummary = dr["SpecsSummary"].ToString(),
                                VersionMaskingName = dr["VersionMaskingName"].ToString()
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

            return otherVersions ?? new List<CarVersions>();
        }

        /// <summary>
        /// For Getting Default Version Id on basis of Minimum value of PriceQuote
        /// Written By : Sanjay Soni
        /// </summary>
        /// <param name="PQId">cityid,modelId</param>
        /// <returns>Default Version Id</returns>
        public int GetDefaultVersionId(int cityId, int modelId, bool isCriticalRead = false)
        {
            int defaultVersionId = 0;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetDefaultVersionId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CityId", DbType.Int16, cityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ModelId", DbType.Int16, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_VersionId", DbType.Int32, ParameterDirection.Output));

                    LogLiveSps.LogMySqlSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, isCriticalRead ? DbConnections.NewCarMySqlMasterConnection : DbConnections.NewCarMySqlReadConnection);

                    defaultVersionId = CustomParser.parseIntObject(cmd.Parameters["v_VersionId"].Value);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "modelId:" + modelId + " cityId:" + cityId);
                throw;
            }
            return defaultVersionId;
        }

        /// <summary>
        /// Written By : Supreksha on <23/12/2016>
        /// Get all version details with subsegment
        /// </summary>
        /// <returns></returns>
        public List<VersionSubSegment> GetVersionSpecs(bool isCriticalRead = false)
        {
            try
            {
                using (var con = isCriticalRead ? NewCarMySqlMasterConnection : NewCarMySqlReadConnection)
                {
                    var versionDetailsList = con.Query<VersionSubSegment>("GetVersionDetailsUserProfiling_v18_11_1", commandType: CommandType.StoredProcedure);
                    LogLiveSps.LogSpInGrayLog("GetModelDetailsUserProfiling_v18_11_1");
                    if (versionDetailsList != null && versionDetailsList.AsList().Count > 0)
                    {
                        return versionDetailsList.AsList();
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                Logger.LogException(ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
            return null;
        }
        public VersionMaskingResponse GetVersionInfoFromMaskingName(string maskingName, int modelId, int versionId, bool isCriticalRead = false)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_MaskingName", maskingName);
                param.Add("v_CarModelId", modelId);
                param.Add("v_VersionId", versionId);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    VersionMaskingResponse maskingObject = con.Query<VersionMaskingResponse>("GetVersionInfoFromMaskingName_v17_10_1", param,
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    return maskingObject ?? new VersionMaskingResponse();

                }
            }
            catch (SqlException ex)
            {
                Logger.LogException(ex);
                throw;
            }
            catch (InvalidExpressionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public List<Color> GetVersionColors(int versionId, bool isCriticalRead = false)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("v_VersionId", versionId);
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    List<Color> versionColors = con.Query<Color>("GetVersionColorList", param,
                        commandType: CommandType.StoredProcedure).AsList();
                    return versionColors;
                }
            }
            catch (SqlException ex)
            {
                Logger.LogException(ex);
                throw;
            }
            catch (InvalidExpressionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }
        public int GetVersionCountByModel(int modelId, bool isCriticalRead)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ModelId", modelId);
                string cmd = "select count(1) from cwmasterdb.carversions where carmodelid = @ModelId and new = 1 and IsDeleted = 0;";
                using (var con = isCriticalRead ? CarDataMySqlMasterConnection : CarDataMySqlReadConnection)
                {
                    int versionCount = con.Query<int>(cmd, param,
                        commandType: CommandType.Text).FirstOrDefault();
                    return versionCount;
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex);
                return 0;
            }
        }
    }
}
