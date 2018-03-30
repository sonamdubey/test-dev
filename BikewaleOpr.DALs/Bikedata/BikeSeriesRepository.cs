using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace BikewaleOpr.DALs.Bikedata
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 11th Sep 2017
    /// Summary: DAL for bike series
    /// Modified by : Rajan Chauhan on 12th Dec 2017
    /// Description : Replaced sp from 'getbikeseries' to 'getbikeseries_12122017'
    /// </summary>
    public class BikeSeriesRepository : IBikeSeriesRepository
    {

        public IEnumerable<BikeSeriesEntity> GetSeries()
        {
            IEnumerable<BikeSeriesEntity> objBikeSeries = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    objBikeSeries = connection.Query<BikeSeriesEntity, BikeMakeEntityBase,BikeBodyStyleEntity, BikeSeriesEntity>
                                    (
                                        "getbikeseries_12122017",
                                        (bikeseries, bikemakebase,bikebodystyle) =>
                                        {
                                            bikeseries.BikeMake = bikemakebase;
                                            bikeseries.BodyStyle = bikebodystyle;
                                            return bikeseries;
                                        }, splitOn: "MakeId, BodyStyleId", commandType: CommandType.StoredProcedure
                                    );
                 
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.BikeSeriesRepository: GetSeries");
            }
            return objBikeSeries;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 12th Sep 2017
        /// Summary : Add new bike series
        /// Modified by : Rajan Chauhan on 12th Dec 2017
        /// Description : Added bodystyle field and replaced sp from 'addbikeseries' to 'addbikeseries_12122017'
        /// </summary>
        /// <param name="bikeSeries"></param>
        /// <param name="UpdatedBy"></param>
        /// <returns></returns>
        public void AddSeries(BikeSeriesEntity bikeSeries, uint updatedBy)
        {
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_name", bikeSeries.SeriesName);
                    param.Add("par_maskingname", bikeSeries.SeriesMaskingName);
                    param.Add("par_makeid", bikeSeries.BikeMake.MakeId);
                    param.Add("par_isseriespageurl", bikeSeries.IsSeriesPageUrl);
                    param.Add("par_userid", updatedBy);
                    param.Add("par_bodystyleid",bikeSeries.BodyStyle.BodyStyleId);
                    param.Add("par_updatedby", dbType: DbType.String, direction: ParameterDirection.Output);
                    param.Add("par_seriesid", dbType: DbType.UInt32, direction: ParameterDirection.Output);
                    param.Add("par_createdon", dbType: DbType.Date, direction: ParameterDirection.Output);

                    connection.Execute("addbikeseries_12122017", param: param, commandType: CommandType.StoredProcedure);
                    bikeSeries.SeriesId = param.Get<uint>("par_seriesid");
                    if (bikeSeries.SeriesId != 0)
                    {
                        bikeSeries.UpdatedBy = param.Get<string>("par_updatedby");
                        bikeSeries.CreatedOn = param.Get<DateTime>("par_createdon");
                        bikeSeries.UpdatedOn = param.Get<DateTime>("par_createdon");
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: AddSeries_{0}_{1}", bikeSeries, updatedBy));
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 13th Sep 2017
        /// Summary : Get Series by make
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<BikeSeriesEntityBase> GetSeriesByMake(int makeId)
        {
            IEnumerable<BikeSeriesEntityBase> objBikeSeriesList = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_makeid", makeId);
                    objBikeSeriesList = connection.Query<BikeSeriesEntityBase>("getseriesbymake", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: GetSeriesByMake_{0}", makeId));
            }
            return objBikeSeriesList;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : DAL Method to edit bike series
        /// Modified by : Ashutosh Sharma on 23 Oct 2017
        /// Description : Replaced sp from 'editbikeseries' to 'editbikeseries_23102017'.
        /// Modified by : Rajan Chauhan on 12th Dec 2017
        /// Description : Added bodystyle field and replaced sp from 'editbikeseries_23102017' to 'editbikeseries_12122017'
        /// </summary>
        /// <param name="bikeSeries"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public bool EditSeries(BikeSeriesEntity bikeSeries, int updatedBy)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_name", bikeSeries.SeriesName);
                    param.Add("par_maskingname", bikeSeries.SeriesMaskingName);
                    param.Add("par_isseriespageurl", bikeSeries.IsSeriesPageUrl);
                    param.Add("par_updatedby", updatedBy);
                    param.Add("par_seriesid", bikeSeries.SeriesId);
                    param.Add("par_bodystyleid", bikeSeries.BodyStyle.BodyStyleId);

                    rowsAffected = connection.Execute("editbikeseries_12122017", param: param, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: EditSeries_{0}_{1}", bikeSeries, updatedBy));
            }
            return rowsAffected > 0;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : DAL Method to delete bike series
        /// Modified by : Ashutosh Sharma on 23 Oct 2017
        /// Description : Replaced sp from 'deletebikeseries' to 'deletebikeseries_23102017'.
        /// </summary>
        /// <param name="bikeSeriesId"></param>
        /// <returns></returns>
        public bool DeleteSeries(uint bikeSeriesId, uint deletedBy)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_seriesid", bikeSeriesId);
                    param.Add("par_updatedby", deletedBy);
                    param.Add("par_rowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("deletebikeseries_23102017", param: param, commandType: CommandType.StoredProcedure);
                    rowsAffected = param.Get<int>("par_rowsAffected");

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: DeleteSeries_{0}_{1}", bikeSeriesId, deletedBy));
            }
            return rowsAffected > 0;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : DAL Method to delete bike series mapping with model
        /// Modified by : Ashutosh Sharma on 23 Oct 2017
        /// Description : Replaced sp from 'deletemappingofmodelseries' to 'deletemappingofmodelseries_23102017'.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public int DeleteMappingOfModelSeries(uint modelId)
        {
            int rowsAffected = 0;
            int seriesId = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_modelid", modelId);
                    param.Add("par_seriesId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    rowsAffected = connection.Execute("deletemappingofmodelseries_23102017", param: param, commandType: CommandType.StoredProcedure);

                    if (rowsAffected > 0)
                        seriesId = param.Get<int>("par_seriesId");

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: DeleteMappingOfModelSeries{0}", modelId));
            }
            return seriesId;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 7th Nov 2017
        /// Description : Get series synopsis
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public SynopsisData Getsynopsis(int seriesId)
        {
            SynopsisData objSynopsis = null;

            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {

                    var param = new DynamicParameters();

                    param.Add("par_seriesid", seriesId);

                    objSynopsis = connection.Query<SynopsisData>("getseriessynopsis", param: param, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikeSeriesRepotory.Getsynopsis");
            }

            return objSynopsis;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 7th Nov 2017
        /// Description : Update Synopsis
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="updatedBy"></param>
        /// <param name="objSynopsis"></param>
        public bool UpdateSynopsis(int seriesId, int updatedBy, SynopsisData objSynopsis)
        {
            int rowsAffected = 0;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {                    
                    var param = new DynamicParameters();

                    param.Add("par_seriesid", seriesId);
                    param.Add("par_userid", updatedBy);
                    param.Add("par_discription", objSynopsis.BikeDescription);

                    rowsAffected = connection.Execute("manageseriessynopsis", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.DALs.BikeSeriesRepository.UpdateSynopsis");
            }

            return rowsAffected > 0;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 21 Nov 2017
        /// Description :   Check if masking name exists in series table
        /// </summary>
        /// <param name="seriesMaskingName"></param>
        /// <returns></returns>
        public bool IsSeriesMaskingNameExists(uint makeId, string seriesMaskingName)
        {
            bool isExists = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "isseriesmaskingnameexists";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbType.String, seriesMaskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.String, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_ismaskingexist", DbType.Int16, ParameterDirection.Output));

                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                    isExists = Bikewale.Utility.SqlReaderConvertor.ToBoolean(cmd.Parameters["par_ismaskingexist"].Value);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BikewaleOpr.DALs.IsSeriesMaskingNameExists({0})", seriesMaskingName));
            }
            return isExists;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 30 Nov 2017
        /// Summary : Get model ids as commar separated string for given series id
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public string GetModelIdsBySeries(uint seriesId)
        {
            string modelIds = string.Empty;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getmodelidsbyseries"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbType.UInt32, seriesId));
                    using (IDataReader reader = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (reader != null && reader.Read())
                        {
                            modelIds = Convert.ToString(reader["modelids"]);
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.DAL.BikeData.BikeSeriesRepository.GetMaskingNames_seriesId {0}", seriesId));
            }

            return modelIds;
        }
    }
}
