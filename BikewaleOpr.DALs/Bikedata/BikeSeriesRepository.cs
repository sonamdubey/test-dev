using Bikewale.DAL.CoreDAL;
using Bikewale.Notifications;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using BikewaleOpr.Entities.BikeData;

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
                                        "bw_getbikeseries",
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
        /// <returns></returns>
        public uint AddSeries(BikeSeriesEntity bikeSeries, uint UpdatedBy)
        {
            uint seriesId = 0;
            try
            { 
                using(IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_name", bikeSeries.SeriesName);
                    param.Add("par_maskingname", bikeSeries.SeriesMaskingName);
                    param.Add("par_makeid", bikeSeries.BikeMake.MakeId);
                    param.Add("par_updatedby", UpdatedBy);
                    connection.Open();
                    connection.Execute("bw_addbikeseries", param: param, commandType: CommandType.StoredProcedure);
                    seriesId = param.Get<uint>("par_seriesid");
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.DAL.BikeSeriesRepository: AddSeries");
            }
            return seriesId;
        }
    }
}
