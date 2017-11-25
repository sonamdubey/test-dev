using Bikewale.DAL.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Images;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Dapper;
using System;
using System.Data;

namespace Bikewale.DAL.BikeData
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 28th Sep 2017
    /// Summary : DAL for bike series
    /// </summary>
    public class BikeSeriesRepository : IBikeSeriesRepository
    {
        public BikeSeriesModels GetModelsListBySeriesId(uint seriesId)
        {
            BikeSeriesModels objBikeSeriesModels = null;
            try
            {
                using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("par_seriesId", seriesId);
                    var reader = connection.QueryMultiple("getmodelsbyseriesid", param: param, commandType: CommandType.StoredProcedure);
                    if (reader != null)
                    {
                        objBikeSeriesModels = new BikeSeriesModels();
                        objBikeSeriesModels.NewBikes = reader.Read<BikeMakeBase, BikeModelEntityBase, ImageEntityBase, MinSpecsEntity,  NewBikeEntityBase>(
                            (bikeMakeBase, bikeModelEntityBase, imageEntityBase, minSpecsEntity) => 
                            {
                                NewBikeEntityBase newBikeEntityBase = new NewBikeEntityBase()
                                {
                                    BikeMake = bikeMakeBase,
                                    BikeModel = bikeModelEntityBase,
                                    BikeImage = imageEntityBase,
                                    MinSpecs =  minSpecsEntity
                                };
                                return newBikeEntityBase;
                            }, splitOn: "ModelId, HostURL, Displacement"
                            );
                        objBikeSeriesModels.UpcomingBikes = reader.Read<UpcomingBikeEntityBase, BikeMakeBase, BikeModelEntityBase, ImageEntityBase, UpcomingBikeEntityBase>(
                                (upcomingBikeEntityBase, bikeMakeBase, bikeModelEntityBase, imageEntityBase) => 
                                {
                                    upcomingBikeEntityBase.BikeMake = bikeMakeBase;
                                    upcomingBikeEntityBase.BikeModel = bikeModelEntityBase;
                                    upcomingBikeEntityBase.BikeImage = imageEntityBase;
                                    return upcomingBikeEntityBase;
                                }, splitOn: "MakeId, ModelId, HostURL"
                            );
                    }
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetModelsListBySeriesId SeriesId = {0}", seriesId));
            }
            return objBikeSeriesModels;
        }   // end of GetModelsListBySeriesId

    }   // class
}   // namespace
