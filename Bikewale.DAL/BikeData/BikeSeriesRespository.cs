using System;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System.Data;
using Bikewale.DAL.CoreDAL;
using Dapper;

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : DAL for bike series
    /// </summary>
    public class BikeSeriesRepository : IBikeSeriesRepository
    {
        public BikeSeriesModels GetModelsListBySeriesId(uint seriesId)
        {
            BikeSeriesModels objBikeSeriesModels = null;
            try
            {
                using(IDbConnection connection = DatabaseHelper.GetMasterConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("par_seriesId", seriesId);
                    connection.Open();
                    if(connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetModelsListBySeriesId SeriesId = {0}", seriesId));
            }
            return objBikeSeriesModels;
        }   // end of GetModelsListBySeriesId

    }   // class
}   // namespace
