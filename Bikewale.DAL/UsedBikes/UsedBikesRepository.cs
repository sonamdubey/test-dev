using Bikewale.CoreDAL;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.DAL.UsedBikes
{
    public class UsedBikesRepository  : IUsedBikes
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
                using (DbCommand cmd = DbFactory.GetDBCommand("PopularUsedBikes"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@TopCount", SqlDbType.SmallInt).Value = totalCount;
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

    }
}
