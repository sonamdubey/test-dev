using Carwale.DAL.CoreDAL.MySql;
using Carwale.Entity.CarData;
using Carwale.Interfaces.CarData.RecentLaunchedCar;
using Carwale.Notifications;
using Carwale.Utility;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Carwale.DAL.CarData
{

    public class RecentLaunchedCarRepository : IRecentLaunchedCarRepository
    {
        private static string _imgHostUrl = Carwale.Utility.CWConfiguration._imgHostUrl;
        /// <summary>
        /// recentlaunchedcar repository for getting list of cars
        /// written by Natesh Kumar on 1/10/2014
        /// </summary>
        /// <returns></returns>
        public List<RecentLaunchedCarEntity> GetRecentLaunchedCars()
        {
            var result = new List<RecentLaunchedCarEntity>();

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("RecentlyLaunchedCars_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_startindex", DbType.Int32, 0));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_endindex", DbType.Int32, Convert.DBNull));

                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            result.Add(new RecentLaunchedCarEntity() {
                                BasicId = DBNull.Value==dr["BasicId"] ? 0 : Convert.ToUInt32(dr["BasicId"]),
                                HostUrl = _imgHostUrl,
                                LaunchDate = Convert.ToDateTime(dr["LaunchDate"]),
                                LaunchId = Convert.ToUInt32(dr["launchid"]),
                                MakeName = dr["Make"].ToString(),
                                MaskingName = dr["MaskingName"].ToString(),
                                ModelId = Convert.ToInt32(dr["ModelId"]),
                                ModelName = dr["Model"].ToString(),
                                MaxPrice = string.IsNullOrEmpty(dr["MaxPrice"].ToString()) ? 0 : Convert.ToDouble(dr["MaxPrice"]),
                                MinPrice = string.IsNullOrEmpty(dr["MinPrice"].ToString()) ? 0 : Convert.ToDouble(dr["MinPrice"]),
                                ModelImage = ImageSizes._210X118 + dr["OriginalImgPath"].ToString(),
                                ReviewCount = Convert.ToInt32(dr["ReviewCount"] == DBNull.Value ?"0" : dr["ReviewCount"].ToString()),
                                ReviewRate = Convert.ToSingle(dr["ReviewRate"] == DBNull.Value ? "0" : dr["ReviewRate"].ToString()),
                                CarName = dr["CarName"].ToString(),
                                SmallImage = dr["ModelSmallImage"].ToString(),
                                OriginalImgPath = dr["OriginalImgPath"].ToString()
                            });
                        }
                    }
                }
            }
            catch (MySqlException err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "RecentLaunchedCar SQL Exception in GetRecentLaunchedCars() ");
                objErr.LogException();
                throw;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.DAL.CarData.GetRecentLaunchedCars()");
                objErr.LogException();
                throw;
            }

            return result;
        }
    }
}
