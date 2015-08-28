using Bikewale.CoreDAL;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
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
            Database db = null;
            List<PopularUsedBikesEntity> objUsedBikesList = null;

            using (SqlCommand cmd = new SqlCommand("PopularUsedBikes"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TotalCount", totalCount);

                if(cityId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CityId",cityId);
                }

                try
                {
                    db = new Database();
                    objUsedBikesList = new List<PopularUsedBikesEntity>();

                    using (SqlDataReader dr = db.SelectQry(cmd))
                    {
                        while (dr.Read())
                        {
                            objUsedBikesList.Add(new PopularUsedBikesEntity
                            {                                   
                                MakeName= Convert.ToString(dr["MakeName"]),
                                TotalBikes = Convert.ToUInt32(dr["MakewiseCount"]),
                                AvgPrice = Convert.ToDouble(dr["AvgPrice"]),
                                HostURL = Convert.ToString(dr["HostURL"]),                     
                                OriginalImagePath = Convert.ToString(dr["OriginalImagePath"]) 
                            });
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
            }
            return objUsedBikesList;
        }   // End of GetPopularUsedBikes method

    }
}
