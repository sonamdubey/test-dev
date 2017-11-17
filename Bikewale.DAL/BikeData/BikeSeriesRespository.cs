using Bikewale.DAL.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using Bikewale.Entities.Images;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using Dapper;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

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
                        objBikeSeriesModels.NewBikes = reader.Read<BikeMakeBase, BikeModelEntityBase, ImageEntityBase, MinSpecsEntity, NewBikeEntityBase>(
                            (bikeMakeBase, bikeModelEntityBase, imageEntityBase, minSpecsEntity) =>
                            {
                                NewBikeEntityBase newBikeEntityBase = new NewBikeEntityBase()
                                {
                                    BikeMake = bikeMakeBase,
                                    BikeModel = bikeModelEntityBase,
                                    BikeImage = imageEntityBase,
                                    MinSpecs = minSpecsEntity
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetModelsListBySeriesId SeriesId = {0}", seriesId));
            }
            return objBikeSeriesModels;
        }   // end of GetModelsListBySeriesId 


        public IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId)
        {
            IList<BikeSeriesCompareBikes> objModelsList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikebyseriesidforcompare"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbType.Int32, seriesId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objModelsList = new List<BikeSeriesCompareBikes>();

                            while (dr.Read())
                            {
                                objModelsList.Add(new BikeSeriesCompareBikes()
                                {
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    HostUrl = Convert.ToString(dr["HostURL"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    Displacement = SqlReaderConvertor.ParseToDouble(dr["Displacement"]),
                                    FuelCapacity = SqlReaderConvertor.ParseToDouble(dr["FuelEfficiencyOverall"]),
                                    MaxPower = SqlReaderConvertor.ParseToDouble(dr["MaxPower"]),
                                    Weight = SqlReaderConvertor.ParseToDouble(dr["KerbWeight"]),
                                    Mileage = SqlReaderConvertor.ParseToDouble(dr["mileage"]),
                                    SeatHeight = SqlReaderConvertor.ParseToDouble(dr["seatheight"]),
                                    Gears = SqlReaderConvertor.ToUInt16(dr["NoOfGears"]),
                                    BrakeType = Convert.ToString(dr["BrakeType"]),
                                    MaxPowerRpm = SqlReaderConvertor.ParseToDouble(dr["MaxPowerRpm"])


                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetBikesToCompare SeriesId = {0}", seriesId));
            }
            return objModelsList;
        }



    }   // class
}   // namespace
