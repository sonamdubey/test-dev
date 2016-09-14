using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace Bikewale.DAL.UsedBikes
{
    public class UsedBikesRepository : IUsedBikes
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
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objUsedBikesList;
        }   // End of GetPopularUsedBikes method

        /// <summary>
        /// Author : subodh jain on 21 june 2016
        ///
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


                    objUsedBikesList = new List<MostRecentBikes>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    MakeName = GetString(dr["makename"]),
                                    MakeMaskingName = GetString(dr["makemaskingname"]),
                                    CityName = GetString(dr["city"]),
                                    AvailableBikes = GetUint32(dr["availablebikes"]),
                                    CityMaskingName = GetString(dr["citymaskingname"]),
                                    CityId = GetUint32(dr["cityid"])


                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objUsedBikesList;
        }//end of GetUsedBikesbyMake
        /// <summary>
        /// Author : subodh jain on 21 june 2016
        ///
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


                    objUsedBikesList = new List<MostRecentBikes>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    MakeName = GetString(dr["makename"]),
                                    MakeMaskingName = GetString(dr["makemaskingname"]),
                                    CityName = GetString(dr["city"]),
                                    AvailableBikes = GetUint32(dr["availablebikes"]),
                                    CityMaskingName = GetString(dr["citymaskingname"]),
                                    CityId = GetUint32(dr["cityid"])

                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objUsedBikesList;
        }//end of GetUsedBikesbyModel
        /// <summary>
        /// Created:- by Subodh Jain on 14 sep 2016
        /// Description:- Fetch Most recent used bikes for particular model and city
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_totalCount", DbType.Int16, totalCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityId));


                    objUsedBikesList = new List<MostRecentBikes>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    MakeName = GetString(dr["makename"]),
                                    MakeMaskingName = GetString(dr["makemaskingname"]),
                                    CityName = GetString(dr["city"]),
                                    ModelMaskingName = GetString(dr["modelmaskingname"]),
                                    CityMaskingName = GetString(dr["citymaskingname"]),

                                    MakeYear = GetUint32(dr["bikeyear"]),

                                    ModelName = GetString(dr["modelname"]),

                                    VersionName = GetString(dr["versionname"]),
                                    BikePrice = GetUint32(dr["bikeprice"]),

                                    ProfileId = GetString(dr["ProfileId"]),
                                    Kilometer = GetUint32(dr["Kilometers"]),
                                    OriginalImagePath = GetString(dr["OriginalImagePath"]),
                                    owner = GetUint32(dr["owner"]),
                                    HostUrl = GetString(dr["HostURL"]),

                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int16, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_totalCount", DbType.Int16, totalCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int16, cityId));


                    objUsedBikesList = new List<MostRecentBikes>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                objUsedBikesList.Add(new MostRecentBikes
                                {
                                    MakeName = GetString(dr["makename"]),
                                    MakeMaskingName = GetString(dr["makemaskingname"]),
                                    CityName = GetString(dr["city"]),
                                    ModelMaskingName = GetString(dr["modelmaskingname"]),
                                    CityMaskingName = GetString(dr["citymaskingname"]),

                                    MakeYear = GetUint32(dr["bikeyear"]),

                                    ModelName = GetString(dr["modelname"]),

                                    VersionName = GetString(dr["versionname"]),
                                    BikePrice = GetUint32(dr["bikeprice"]),

                                    ProfileId = GetString(dr["ProfileId"]),
                                    Kilometer = GetUint32(dr["Kilometers"]),
                                    OriginalImagePath = GetString(dr["OriginalImagePath"]),
                                    owner = GetUint32(dr["owner"]),
                                    HostUrl = GetString(dr["HostURL"]),

                                });
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return objUsedBikesList;
        }// end of GetUsedBikesbyMakeCity
        private string GetString(object o)
        {
            return (DBNull.Value == o) ? string.Empty : o.ToString();
        }
        private uint GetUint32(object o)
        {
            return (DBNull.Value == o) ? 0 : Convert.ToUInt32(o);
        }

    }
}
