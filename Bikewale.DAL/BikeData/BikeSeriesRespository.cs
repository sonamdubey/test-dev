using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using Bikewale.Entities.Images;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// Modified by : Ashutosh Sharma on 05 Apr 2018.
        /// Description : Changed sp from 'getnewmodelsbyseriesid' to 'getnewmodelsbyseriesid_05042018' to remove min specs.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="cityId"></param>
        /// <returns>If cityId is 0 then models with Mumbai price, otherwise with city price.</returns>
        public IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId)
        {
            ICollection<NewBikeEntityBase> objNewBikeList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getnewmodelsbyseriesid_05042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesId", DbType.UInt32, seriesId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityId", DbType.UInt32, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objNewBikeList = new Collection<NewBikeEntityBase>();
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
                ErrorClass.LogError(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetNewModels_SeriesId_{0}_{1}", seriesId, cityId));
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
                ErrorClass.LogError(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetUpcomingModels_SeriesId = {0}", seriesId));
            }
            return objUpcomingBikeList;
        }

        /// <summary>
        /// Modified by : Pratibha Verma on 3 April 2018
        /// Description : Replaced sp 'getbikebyseriesidforcompare' with 'getbikebyseriesidforcompare_03042018' and Removed specs mapping
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId)
        {
            ICollection<BikeSeriesCompareBikes> objModelsList = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getbikebyseriesidforcompare_03042018"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbType.Int32, seriesId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objModelsList = new Collection<BikeSeriesCompareBikes>();

                            while (dr.Read())
                            {
                                objModelsList.Add(new BikeSeriesCompareBikes()
                                {
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    HostUrl = Convert.ToString(dr["HostURL"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    VersionId = SqlReaderConvertor.ToInt32(dr["TopVersionId"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetBikesToCompare SeriesId = {0}", seriesId));
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
                ErrorClass.LogError(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetSynopsis_SeriesId_{0}", seriesId));
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
                ErrorClass.LogError(ex, string.Format("DAL.BikeData.BikeSeriesRepository.GetModelsListBySeriesId SeriesId = {0}", makeId));
            }
            return bikeSeriesEntityList;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 20 Nov 2017
        /// Summary : Function to get the series and model masking names in the hashtable.
        /// </summary>
        /// <returns>Returns series and model masking names in hashtable. Key is MaskingName(model/series). Value is details associated with masking name.</returns>
        public Hashtable GetMaskingNames()
        {
            Hashtable ht = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getseriesmodelmaskingmapping_12122017"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            ht = new Hashtable();
                            string makeMaskingName = "", modelMaskingName = "", htKey = "";
                            Bikewale.Entities.GenericBikes.EnumBikeBodyStyles bodyStyle;
                            while (dr.Read())
                            {
                                modelMaskingName = Convert.ToString(dr["MaskingName"]);
                                makeMaskingName = Convert.ToString(dr["MakeMaskingName"]);
                                htKey = String.Format("{0}_{1}", makeMaskingName, modelMaskingName);
                                SeriesMaskingResponse objMaskingNames = new SeriesMaskingResponse()
                                {
                                    ModelId = SqlReaderConvertor.ParseToUInt32(dr["ModelId"]),
                                    SeriesId = SqlReaderConvertor.ParseToUInt32(dr["SeriesId"]),
                                    MaskingName = modelMaskingName,
                                    NewMaskingName = Convert.ToString(dr["NewMaskingName"]),
                                    IsSeriesPageCreated = SqlReaderConvertor.ToBoolean(dr["IsSeriesPageUrl"]),
                                    StatusCode = SqlReaderConvertor.ToUInt16(dr["Status"]),
                                    Name = Convert.ToString(dr["Name"]),
                                    MakeMaskingName = makeMaskingName,
                                    BodyStyle = Enum.TryParse(Convert.ToString(dr["BodyStyleId"]), out bodyStyle) ? bodyStyle : Entities.GenericBikes.EnumBikeBodyStyles.AllBikes
								};

                                if (!ht.ContainsKey(htKey))
                                {
                                    ht.Add(htKey, objMaskingNames);
                                }
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.DAL.BikeData.BikeSeriesRepository.GetMaskingNames");
            }
            return ht;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 24 Nov 2017
        /// Summary : Get model ids as comma separated string for given series id
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
                ErrorClass.LogError(ex, string.Format("Bikewale.DAL.BikeData.BikeSeriesRepository.GetMaskingNames seriesId {0}", seriesId));
            }

            return modelIds;
        }
    }   // class
}   // namespace
