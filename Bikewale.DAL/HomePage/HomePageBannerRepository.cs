using Bikewale.Entities.HomePage;
using Bikewale.Interfaces.HomePage;
using Bikewale.Notifications;
using Bikewale.Utility;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.HomePage
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   HomePageBanner Repository
    /// Modified by :   Rajan Chauhan on 26 Apr 2018
    /// Description :   Returns caching time along with homebanner entity 
    ///                 changed sp gethomepagebannerbyplatformid to gethomepagebannerbyplatformid_25042018
    /// </summary>
    public class HomePageBannerRepository : IHomePageBannerRepository
    {
        public Tuple<HomePageBannerEntity, TimeSpan> GetHomePageBannerWithCacheTime(uint platformId)
        {
            HomePageBannerEntity banner = null;
            TimeSpan duration = new TimeSpan(0, 30, 0); //Default cache time to be 30 minutes when no data returned 
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "gethomepagebannerbyplatformid_25042018";
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_platformid", DbType.Int32, platformId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {

                            if (dr.Read())
                            {

                                banner = new HomePageBannerEntity()
                                {
                                    Html = Convert.ToString(dr["html"]),
                                    Css = Convert.ToString(dr["css"]),
                                    JS = Convert.ToString(dr["js"]),
                                };
                                if (dr["endtime"] != null)
                                {
                                    duration = Convert.ToDateTime(dr["endtime"]) - DateTime.Now;
                                }
                            }

                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("HomePageBannerRepository.GetHomePageBanner platformid:{0}", platformId));
            }
            return Tuple.Create(banner, duration);
        }
    }
}
