using Bikewale.CoreDAL;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
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

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
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
        /// Author : Vivek gupta
        /// Date : 21 june 2016
        /// Desc :  Fetch most recent used bikes
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="totalCount"></param>
        /// <param name="cityId"> cityId can be null in case when user does not select city</param>
        /// <returns></returns>
        public IEnumerable<MostRecentBikes> GetMostRecentUsedBikes(uint makeId, uint totalCount, int? cityId = null)
        {

            List<MostRecentBikes> objMostRecentUsedBikesList = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    if (cityId.HasValue && cityId > 0)
                        cmd.CommandText = "getusedbikesbymakecity";
                    else cmd.CommandText = "getusedbikesbymake";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_makeid", DbType.Int32, makeId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_topcount", DbType.SByte, totalCount));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_cityid", DbType.Int32, cityId ?? Convert.DBNull));

                    objMostRecentUsedBikesList = new List<MostRecentBikes>();

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd))
                    {
                        while (dr.Read())
                        {
                            if (cityId.HasValue && cityId > 0)
                            {
                                objMostRecentUsedBikesList.Add(new MostRecentBikes
                                {
                                    MakeYear = !Convert.IsDBNull(dr["BikeYear"]) ? Convert.ToUInt32(dr["BikeYear"]) : default(UInt32),
                                    MakeName = !Convert.IsDBNull(dr["MakeName"]) ? Convert.ToString(dr["MakeName"]) : default(string),
                                    ModelName = !Convert.IsDBNull(dr["ModelName"]) ? Convert.ToString(dr["ModelName"]) : default(string),
                                    MakeMaskingName = !Convert.IsDBNull(dr["MakeMaskingName"]) ? Convert.ToString(dr["MakeMaskingName"]) : default(string),
                                    ModelMaskingName = !Convert.IsDBNull(dr["ModelMaskingName"]) ? Convert.ToString(dr["ModelMaskingName"]) : default(string),
                                    VersionName = !Convert.IsDBNull(dr["VersionName"]) ? Convert.ToString(dr["VersionName"]) : default(string),
                                    BikePrice = !Convert.IsDBNull(dr["BikePrice"]) ? Convert.ToUInt32(dr["BikePrice"]) : default(UInt32),
                                    CityName = !Convert.IsDBNull(dr["City"]) ? Convert.ToString(dr["City"]) : default(string),
                                    CityMaskingName = !Convert.IsDBNull(dr["CityMaskingName"]) ? Convert.ToString(dr["CityMaskingName"]) : default(string),
                                    ProfileId = !Convert.IsDBNull(dr["ProfileId"]) ? Convert.ToString(dr["ProfileId"]) : default(string),
                                });
                            }

                            else
                            {
                                objMostRecentUsedBikesList.Add(new MostRecentBikes
                                {
                                    MakeName = !Convert.IsDBNull(dr["MakeName"]) ? Convert.ToString(dr["MakeName"]) : default(string),
                                    MakeMaskingName = !Convert.IsDBNull(dr["MakeMaskingName"]) ? Convert.ToString(dr["MakeMaskingName"]) : default(string),
                                    CityName = !Convert.IsDBNull(dr["City"]) ? Convert.ToString(dr["City"]) : default(string),
                                    AvailableBikes = !Convert.IsDBNull(dr["AvailableBikes"]) ? Convert.ToUInt32(dr["AvailableBikes"]) : default(UInt32),
                                    CityMaskingName = !Convert.IsDBNull(dr["CityMaskingName"]) ? Convert.ToString(dr["CityMaskingName"]) : default(string),
                                    CityId = !Convert.IsDBNull(dr["CityId"]) ? Convert.ToUInt32(dr["CityId"]) : default(UInt32)
                                });
                            }
                        }

                        dr.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("{0} - {1} - {2}", HttpContext.Current.Request.ServerVariables["URL"], MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodInfo.GetCurrentMethod().Name));
                objErr.SendMail();
            }
            return objMostRecentUsedBikesList;
        }

    }
}
