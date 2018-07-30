using Bikewale.Entities.BikeData;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.UsedBikes
{
    public class UsedBikesRepository : IUsedBikesRepository
    {

        /// <summary>
        /// Written By : Sushil Kumar 
        /// To get List of Popular Used Bikes
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<PopularUsedBikesEntity> GetPopularUsedBikes(uint totalCount, int? cityId = null)
        {
            List<PopularUsedBikesEntity> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("popularusedbikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, (cityId.HasValue && cityId.Value > 0) ? cityId.Value : Convert.DBNull));

                    objUsedBikesList = new List<PopularUsedBikesEntity>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new PopularUsedBikesEntity
                                {
                                    MakeName = Convert.ToString(dr["MakeName"]),
                                    TotalBikes = Convert.ToUInt32(dr["MakewiseCount"]),
                                    AvgPrice = Convert.ToDouble(dr["AvgPrice"]),
                                    HostURL = Convert.ToString(dr["HostURL"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    MakeMaskingName = Convert.ToString(dr["MakeMaskingName"]),
                                    CityMaskingName = (Convert.ToString(dr["CityMaskingName"])).Trim()
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception in getPopularBikes parametres totalCount : {0}, cityId : {1}", totalCount, cityId));
                
            }
            return objUsedBikesList;
        }   // End of GetPopularUsedBikes method

        /// <summary>
        /// Author : subodh jain on 21 june 2016
        /// Desc :  Fetch most recent used bikes by make only
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="totalCount"></param>

        /// <returns></returns>

        public IEnumerable<MostRecentBikes> GetUsedBikesbyMake(uint makeid, uint totalCount)
        {
            IList<MostRecentBikes> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikesbymake"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int16, makeid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUsedBikesList = new List<MostRecentBikes>();
                            while (dr.Read())
                            {

                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MakeMaskingName = Convert.ToString(dr["makemaskingname"]),
                                    CityName = Convert.ToString(dr["city"]),
                                    AvailableBikes = SqlReaderConvertor.ParseToUInt32(dr["availablebikes"]),
                                    CityMaskingName = Convert.ToString(dr["citymaskingname"]),
                                    CityId = SqlReaderConvertor.ParseToUInt32(dr["cityid"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception in UsedBikesRepository.GetUsedBikesbyMake Parametres makeId : {0}, totalCount : {1}", makeid, totalCount));
                
            }
            return objUsedBikesList;
        }//end of GetUsedBikesbyMake

        /// <summary>
        /// Author : subodh jain on 21 june 2016
        /// Desc :  Fetch most recent used bikes by model only
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<MostRecentBikes> GetUsedBikesbyModel(uint modelId, uint totalCount)
        {
            IList<MostRecentBikes> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikesbymodel"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int16, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalCount));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUsedBikesList = new List<MostRecentBikes>();
                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new MostRecentBikes
                                {

                                    ModelName = Convert.ToString(dr["Name"]),
                                    ModelMaskingName = Convert.ToString(dr["modelmaskingname"]),
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MakeMaskingName = Convert.ToString(dr["makemaskingname"]),
                                    CityName = Convert.ToString(dr["city"]),
                                    AvailableBikes = SqlReaderConvertor.ParseToUInt32(dr["availablebikes"]),
                                    CityMaskingName = Convert.ToString(dr["citymaskingname"]),
                                    CityId = SqlReaderConvertor.ParseToUInt32(dr["cityid"])

                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("Exception in UsedBikesRepository.GetUsedBikesbyModel Parametres modelId : {0}, totalCount : {1}", modelId, totalCount));
                
            }
            return objUsedBikesList;
        }//end of GetUsedBikesbyModel

        /// <summary>
        /// Created:- by Subodh Jain on 14 sep 2016
        /// Description:- Fetch Most recent used bikes for particular model and city
        /// Modified by :   Sangram Nandkhile on 09 Nov 2016
        /// Description :   Added lower() for profile id
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<MostRecentBikes> GetUsedBikesbyModelCity(uint modelId, uint cityId, uint totalCount)
        {
            IList<MostRecentBikes> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikesbymodelcity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int16, modelId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUsedBikesList = new List<MostRecentBikes>();

                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MakeMaskingName = Convert.ToString(dr["makemaskingname"]),
                                    CityName = Convert.ToString(dr["city"]),
                                    ModelMaskingName = Convert.ToString(dr["modelmaskingname"]),
                                    CityMaskingName = Convert.ToString(dr["citymaskingname"]),
                                    MakeYear = SqlReaderConvertor.ParseToUInt32(dr["bikeyear"]),
                                    ModelName = Convert.ToString(dr["modelname"]),
                                    VersionName = Convert.ToString(dr["versionname"]),
                                    BikePrice = SqlReaderConvertor.ParseToUInt32(dr["bikeprice"]),
                                    ProfileId = Convert.ToString(dr["ProfileId"]).ToLower(),
                                    Kilometer = SqlReaderConvertor.ParseToUInt32(dr["Kilometers"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    owner = SqlReaderConvertor.ParseToUInt32(dr["owner"]),
                                    HostUrl = Convert.ToString(dr["HostURL"])

                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception in UsedBikesRepository.GetUsedBikesbyModelCity Parametres modelId : {0}, totalCount : {1}, cityId {2}", modelId, totalCount, cityId));
                
            }
            return objUsedBikesList;
        }//end of GetUsedBikesbyModelCity




        /// <summary>
        /// Created:- by Subodh Jain on 14 sep 2016
        /// Description:- Fetch Most recent used bikes for particular model and city
        /// Modified by :   Sangram Nandkhile on 09 Nov 2016
        /// Description :   Added lower() for profile id
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<MostRecentBikes> GetUsedBikesSeries(uint seriesid, uint cityId)
        {
            IList<MostRecentBikes> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikesbymodelcitylist"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_seriesid", DbType.Int16, seriesid));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUsedBikesList = new List<MostRecentBikes>();

                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    ModelName = Convert.ToString(dr["ModelName"]),
                                    ModelMaskingName = Convert.ToString(dr["ModelMaskingName"]),
                                    AvailableBikes = SqlReaderConvertor.ParseToUInt32(dr["AvailableBikes"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    HostUrl = Convert.ToString(dr["HostUrl"]),
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MakeMaskingName = Convert.ToString(dr["makemaskingname"]),
                                    MinimumPrice = Convert.ToString(dr["price"]),
                                    ModelId = SqlReaderConvertor.ToUInt32(dr["modelid"]),
                                    UsedHostUrl = Convert.ToString(dr["usedHostUrl"]),
                                    UsedOriginalImagePath = Convert.ToString(dr["usedOriginalImagePath"]),
                                    BikePrice = SqlReaderConvertor.ToUInt32(dr["price"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, String.Format("Exception in Bikewale.Cache.UsedBikes.GetUsedBikesSeries parametres  modelId : {0}, cityId : {1}", seriesid, cityId));

            }
            return objUsedBikesList;
        }//end of GetUsedBikesbyModelCity


        /// <summary>
        /// / Created:- by Subodh Jain on 14 sep 2016
        /// Description:- Fetch Most recent used bikes for particular make and city
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<MostRecentBikes> GetUsedBikesbyMakeCity(uint makeId, uint cityId, uint totalCount)
        {
            IList<MostRecentBikes> objUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikesbymakecity"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int16, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.Int16, totalCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityId));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            objUsedBikesList = new List<MostRecentBikes>();
                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MakeMaskingName = Convert.ToString(dr["makemaskingname"]),
                                    CityName = Convert.ToString(dr["city"]),
                                    ModelMaskingName = Convert.ToString(dr["modelmaskingname"]),
                                    CityMaskingName = Convert.ToString(dr["citymaskingname"]),
                                    MakeYear = SqlReaderConvertor.ParseToUInt32(dr["bikeyear"]),
                                    ModelName = Convert.ToString(dr["modelname"]),
                                    VersionName = Convert.ToString(dr["versionname"]),
                                    BikePrice = SqlReaderConvertor.ParseToUInt32(dr["bikeprice"]),
                                    ProfileId = Convert.ToString(dr["ProfileId"]).ToLower(),
                                    Kilometer = SqlReaderConvertor.ParseToUInt32(dr["Kilometers"]),
                                    OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]),
                                    owner = SqlReaderConvertor.ParseToUInt32(dr["owner"]),
                                    HostUrl = Convert.ToString(dr["HostURL"])

                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception in UsedBikesRepository.GetUsedBikesbyMakeCity Parametres makeId : {0}, totalCount : {1}, cityId {2}", makeId, totalCount, cityId));
                
            }
            return objUsedBikesList;
        }// end of GetUsedBikesbyMakeCity

        /// <summary>
        /// Created by: Sangram Nandkhile on 10 oct 2016
        /// Summary: Fetch make id, name and list of used bike counts for makepage
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsedBikeMakeEntity> GetUsedBikeMakesWithCount()
        {
            IList<UsedBikeMakeEntity> usedMakeList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getusedbikemakeswithcount_06102016"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            usedMakeList = new List<UsedBikeMakeEntity>();
                            while (dr.Read())
                            {
                                usedMakeList.Add(new UsedBikeMakeEntity
                                {
                                    MakeId = Convert.ToInt16(dr["makeid"]),
                                    MakeName = Convert.ToString(dr["makename"]),
                                    MaskingName = Convert.ToString(dr["makemaskingname"])
                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception in UsedRepository.GetUsedBikeMakesWithCount");
                
            }
            return usedMakeList;

        }
    }
}
