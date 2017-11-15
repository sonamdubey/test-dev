using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace BikewaleOpr.DALs.Bikedata
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 11th Sep 2017
    /// Summary: DAL for bike series
    /// </summary>
    public class BikeSeriesRepository: IBikeSeriesRepository
    {

        public IEnumerable<BikeSeriesEntity> GetSeries()
        {
            IEnumerable<BikeSeriesEntity> objBikeSeries = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    connection.Open();
                    objBikeSeries = connection.Query<BikeSeriesEntity, BikeMakeEntityBase, BikeSeriesEntity>
                                    (
                                        "getbikeseries",
                                        (bikeseries, bikemakebase) =>
                                        {
                                            bikeseries.BikeMake = bikemakebase;
                                            return bikeseries;
                                        }, splitOn: "MakeId", commandType: CommandType.StoredProcedure
                                    );
                    if(connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BikeSeriesRepository: GetSeries");
            }
            return objBikeSeries;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 12th Sep 2017
        /// Summary : Add new bike series
        /// </summary>
        /// <param name="bikeSeries"></param>
        /// <param name="UpdatedBy"></param>
        /// <returns></returns>
        public void AddSeries(BikeSeriesEntity bikeSeries, uint updatedBy)
        {
            try
            { 
                using(IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_name", bikeSeries.SeriesName);
                    param.Add("par_maskingname", bikeSeries.SeriesMaskingName);
                    param.Add("par_makeid", bikeSeries.BikeMake.MakeId);
                    param.Add("par_isseriespageurl", bikeSeries.IsSeriesPageUrl);
                    param.Add("par_userid", updatedBy);
                    param.Add("par_updatedby", dbType: DbType.String, direction: ParameterDirection.Output);
                    param.Add("par_seriesid", dbType: DbType.UInt32, direction: ParameterDirection.Output);
                    param.Add("par_createdon", dbType: DbType.Date, direction: ParameterDirection.Output);
                    connection.Open();
                    connection.Execute("addbikeseries", param: param, commandType: CommandType.StoredProcedure);
                    bikeSeries.SeriesId = param.Get<uint>("par_seriesid");
                    if(bikeSeries.SeriesId != 0)
                    {
                        bikeSeries.UpdatedBy = param.Get<string>("par_updatedby");
                        bikeSeries.CreatedOn = param.Get<DateTime>("par_createdon");
                        bikeSeries.UpdatedOn = param.Get<DateTime>("par_createdon");
                    }
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: AddSeries_{0}_{1}", bikeSeries, updatedBy));
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
                using(IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_makeid", makeId);
                    connection.Open();
                    objBikeSeriesList = connection.Query<BikeSeriesEntityBase>("getseriesbymake", param: param, commandType: CommandType.StoredProcedure);
                    if(connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: GetSeriesByMake_{0}", makeId));
            }
            return objBikeSeriesList;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : DAL Method to edit bike series
        /// Modified by : Ashutosh Sharma on 23 Oct 2017
        /// Description : Replaced sp from 'editbikeseries' to 'editbikeseries_23102017'.
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

                    connection.Open();

                    rowsAffected = connection.Execute("editbikeseries_23102017", param: param, commandType: CommandType.StoredProcedure);


                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: EditSeries_{0}_{1}", bikeSeries, updatedBy));
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

                    connection.Open();

                    connection.Execute("deletebikeseries_23102017", param: param, commandType: CommandType.StoredProcedure);
                    rowsAffected = param.Get<int>("par_rowsAffected");

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: DeleteSeries_{0}_{1}", bikeSeriesId, deletedBy));
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

                    connection.Open();

                    rowsAffected = connection.Execute("deletemappingofmodelseries_23102017", param: param, commandType: CommandType.StoredProcedure);

                    if (rowsAffected > 0)
                        seriesId = param.Get<int>("par_seriesId");

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.DAL.BikeSeriesRepository: DeleteMappingOfModelSeries{0}", modelId));
            }
            return seriesId;
        }
    }
}
