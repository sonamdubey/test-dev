using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using Bikewale.Entities.Images;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
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
		/// <summary>
		/// Created by : Ashutosh Sharma on 17 Nov 2017
		/// Description : DAL method to get new models of a series with city price.
		/// </summary>
		/// <param name="seriesId"></param>
		/// <param name="cityId"></param>
		/// <returns>If cityId is 0 then models with Mumbai price, otherwise with city price.</returns>
        public IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId)
        {
            List<NewBikeEntityBase> objNewBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getnewmodelsbyseriesid"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesId", DbType.UInt32, seriesId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.UInt32, cityId));


                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objNewBikeList = new List<NewBikeEntityBase>();

                            while (dr.Read())
                            {
                                objNewBikeList.Add(new NewBikeEntityBase()
                                {
                                    BikeMake = new BikeMakeBase()
                                    {
                                        MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]),
                                        MakeName = Convert.ToString(dr["MakeName"]),
                                        MakeMaskingName = Convert.ToString(dr["MakeMaskingName"])
                                    },
                                    BikeModel = new BikeModelEntityBase()
                                    {
                                        ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]),
                                        ModelName = Convert.ToString(dr["ModelName"]),
                                        MaskingName = Convert.ToString(dr["MaskingName"])
                                    },
                                    Price = new PriceEntityBase()
                                    {
                                        AvgPrice = SqlReaderConvertor.ToUInt32(dr["AvgVersionPrice"]),
                                        ExShowroomPrice = SqlReaderConvertor.ToUInt32(dr["ExShowroomPrice"])
                                    },
                                    BikeImage = new ImageEntityBase()
                                    {
                                        HostUrl = Convert.ToString(dr["HostUrl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"])
                                    },
                                    MinSpecs = new MinSpecsEntity()
                                    {
                                        Displacement = SqlReaderConvertor.ToFloat(dr["Displacement"]),
                                        FuelEfficiencyOverall = SqlReaderConvertor.ToUInt16(dr["FuelEfficiencyOverall"]),
                                        MaxPower = SqlReaderConvertor.ToFloat(dr["MaxPower"]),
                                        KerbWeight = SqlReaderConvertor.ToUInt16(dr["KerbWeight"])
                                    },
                                    Rating = SqlReaderConvertor.ToFloat(dr["Rating"]),
                                    BodyStyle = SqlReaderConvertor.ToUInt16(dr["BodyStyleId"]),
                                    objVersion = new BikeVersionsListEntity()
                                    {

                                        VersionId = SqlReaderConvertor.ToUInt16(dr["VersionId"])
                                    }
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetNewModels_SeriesId_{0}_{1}", seriesId, cityId));
            }
            return objNewBikeList;
        }

		/// <summary>
		/// Created by : Ashutosh Sharma on 17 Nov 2017
		/// Description : DAL method to get upcoming models of a series.
		/// </summary>
		/// <param name="seriesId"></param>
		/// <returns></returns>
		public IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId)
        {
            List<UpcomingBikeEntityBase> objUpcomingBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getupcomingmodelsbyseriesid"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesId", DbType.UInt32, seriesId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUpcomingBikeList = new List<UpcomingBikeEntityBase>();

                            while (dr.Read())
                            {
                                objUpcomingBikeList.Add(new UpcomingBikeEntityBase()
                                {
                                    ExpectedLaunch = SqlReaderConvertor.ToDateTime(dr["ExpectedLaunch"]),
                                    BikeMake = new BikeMakeBase()
                                    {
                                        MakeId = SqlReaderConvertor.ToInt32(dr["MakeId"]),
                                        MakeName = Convert.ToString(dr["MakeName"]),
                                        MakeMaskingName = Convert.ToString(dr["MakeMaskingName"])
                                    },
                                    BikeModel = new BikeModelEntityBase()
                                    {
                                        ModelId = SqlReaderConvertor.ToInt32(dr["ModelId"]),
                                        ModelName = Convert.ToString(dr["ModelName"]),
                                        MaskingName = Convert.ToString(dr["MaskingName"])
                                    },
                                    BikeImage = new ImageEntityBase()
                                    {
                                        HostUrl = Convert.ToString(dr["HostUrl"]),
                                        OriginalImagePath = Convert.ToString(dr["OriginalImagePath"])
                                    },
                                    ExpectedPrice = new PriceEntityBase()
                                    {
                                        MinPrice = SqlReaderConvertor.ToUInt32(dr["EstimatedPriceMin"]),
                                        MaxPrice = SqlReaderConvertor.ToUInt32(dr["EstimatedPriceMax"])
                                    }
                                });

                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetUpcomingModels_SeriesId = {0}", seriesId));
            }
            return objUpcomingBikeList;
        }


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

		/// <summary>
		/// Created by : Ashutosh Sharma on 17 Nov 2017
		/// Description : DAL method to get synopsis of a series.
		/// </summary>
		/// <param name="seriesId"></param>
		/// <returns></returns>
		public BikeDescriptionEntity GetSynopsis(uint seriesId)
        {
            BikeDescriptionEntity synopsis = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getseriessynopsis"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesId", DbType.UInt32, seriesId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null && dr.Read())
                        {
                            synopsis = new BikeDescriptionEntity()
                            {
                                FullDescription = Convert.ToString(dr["BikeDescription"]),
                                Name = Convert.ToString(dr["seriesname"])
                            };
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetSynopsis_SeriesId_{0}", seriesId));
            }
            return synopsis;
        }

		/// <summary>
		/// Created by : Ashutosh Sharma on 17 Nov 2017
		/// Description : DAL method to get all series of a make.
		/// </summary>
		/// <param name="makeId"></param>
		/// <returns></returns>
		public IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId)
        {
            IList<BikeSeriesEntity> bikeSeriesEntityList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getseriesbymake_16112017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeId", DbType.Int32, makeId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            bikeSeriesEntityList = new List<BikeSeriesEntity>();
                            while (dr.Read())
                            {
                                bikeSeriesEntityList.Add(new BikeSeriesEntity()
                                {
                                    SeriesId = SqlReaderConvertor.ToUInt32(dr["SeriesId"]),
                                    SeriesName = Convert.ToString(dr["SeriesName"]),
                                    MaskingName = Convert.ToString(dr["SeriesMaskingName"]),
                                    ModelsCount = SqlReaderConvertor.ToUInt32(dr["ModelsCount"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"])
                                });

                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetOtherSeriesFromMake_makeId_{0}", makeId));
            }
            return bikeSeriesEntityList;
        }
    }   // class
}   // namespace
