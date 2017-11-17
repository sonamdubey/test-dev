using System;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System.Data;
using Bikewale.DAL.CoreDAL;
using Dapper;
using Bikewale.Entities.Images;
using System.Collections.Generic;
using System.Data.Common;
using MySql.CoreDAL;
using Bikewale.Utility;

namespace Bikewale.DAL.BikeData
{
	/// <summary>
	/// Created by : Ashutosh Sharma on 28th Sep 2017
	/// Summary : DAL for bike series
	/// </summary>
	public class BikeSeriesRepository : IBikeSeriesRepository
	{
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
										MakeId = Convert.ToInt32(dr["MakeId"]),
										MakeName = Convert.ToString(dr["MakeName"]),
										MakeMaskingName = Convert.ToString(dr["MakeMaskingName"])
									},
									BikeModel = new BikeModelEntityBase()
									{
										ModelId = Convert.ToInt32(dr["ModelId"]),
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
									BodyStyle = SqlReaderConvertor.ToUInt16(dr["BodyStyleId"])
								});
							}
							dr.Close();
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetNewModels_SeriesId = {0}", seriesId));
			}
			return objNewBikeList;
		}

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
									ExpectedLaunch = Convert.ToDateTime(dr["ExpectedLaunch"]),
									BikeMake = new BikeMakeBase()
									{
										MakeId = Convert.ToInt32(dr["MakeId"]),
										MakeName = Convert.ToString(dr["MakeName"]),
										MakeMaskingName = Convert.ToString(dr["MakeMaskingName"])
									},
									BikeModel = new BikeModelEntityBase()
									{
										ModelId = Convert.ToInt32(dr["ModelId"]),
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
										MinPrice = Convert.ToUInt32(dr["EstimatedPriceMin"]),
										MaxPrice = Convert.ToUInt32(dr["EstimatedPriceMax"])
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

		//public BikeSeriesModels GetModelsListBySeriesId(uint seriesId)
		//      {
		//          BikeSeriesModels objBikeSeriesModels = null;
		//          try
		//          {
		//              using (IDbConnection connection = DatabaseHelper.GetReadonlyConnection())
		//              {
		//                  DynamicParameters param = new DynamicParameters();
		//                  param.Add("par_seriesId", seriesId);
		//                  var reader = connection.QueryMultiple("getmodelsbyseriesid", param: param, commandType: CommandType.StoredProcedure);
		//                  if (reader != null)
		//                  {
		//                      objBikeSeriesModels = new BikeSeriesModels();
		//                      objBikeSeriesModels.NewBikes = reader.Read<BikeMakeBase, BikeModelEntityBase, ImageEntityBase, MinSpecsEntity,  NewBikeEntityBase>(
		//                          (bikeMakeBase, bikeModelEntityBase, imageEntityBase, minSpecsEntity) => 
		//                          {
		//                              NewBikeEntityBase newBikeEntityBase = new NewBikeEntityBase()
		//                              {
		//                                  BikeMake = bikeMakeBase,
		//                                  BikeModel = bikeModelEntityBase,
		//                                  BikeImage = imageEntityBase,
		//                                  MinSpecs =  minSpecsEntity
		//                              };
		//                              return newBikeEntityBase;
		//                          }, splitOn: "ModelId, HostURL, Displacement"
		//                          );
		//                      objBikeSeriesModels.UpcomingBikes = reader.Read<UpcomingBikeEntityBase, BikeMakeBase, BikeModelEntityBase, ImageEntityBase, UpcomingBikeEntityBase>(
		//                              (upcomingBikeEntityBase, bikeMakeBase, bikeModelEntityBase, imageEntityBase) => 
		//                              {
		//                                  upcomingBikeEntityBase.BikeMake = bikeMakeBase;
		//                                  upcomingBikeEntityBase.BikeModel = bikeModelEntityBase;
		//                                  upcomingBikeEntityBase.BikeImage = imageEntityBase;
		//                                  return upcomingBikeEntityBase;
		//                              }, splitOn: "MakeId, ModelId, HostURL"
		//                          );
		//                  }
		//                  if (connection.State == ConnectionState.Open)
		//                  {
		//                      connection.Close();
		//                  }
		//              }
		//          }
		//          catch (Exception ex)
		//          {
		//              ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetModelsListBySeriesId SeriesId = {0}", seriesId));
		//          }
		//          return objBikeSeriesModels;
		//      }   // end of GetModelsListBySeriesId

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
				ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetSynopsis_SeriesId = {0}", seriesId));
			}
			return synopsis;
		}

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
									ModelsCount = SqlReaderConvertor.ToUInt32(dr["ModelsCount"])
								});

							}
							dr.Close();
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetOtherSeriesFromMake_makeId = {0}", makeId));
			}
			return bikeSeriesEntityList;
		}
	}   // class
}   // namespace
