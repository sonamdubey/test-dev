﻿using Bikewale.Entities.HomePage;
using Bikewale.Interfaces.HomePage;
using Bikewale.Notifications;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;

namespace Bikewale.DAL.HomePage
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Dec 2016
    /// Description :   HomePageBanner Repository
    /// </summary>
    public class HomePageBannerRepository : IHomePageBannerRepository
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 29 Dec 2016 
        /// Description :   Calls sp gethomepagebanner
        /// Modified By:-Subodh Jain 26 july 2017
        /// Summary :- changed Sp and modified according to platform id
        /// </summary>
        /// <returns></returns>
        public HomePageBannerEntity GetHomePageBanner(uint platformId)
        {
            HomePageBannerEntity banner = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "gethomepagebannerbyplatformid";
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
                            }
                         
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("HomePageBannerRepository.GetHomePageBanner platformid:{0}",platformId));
            }
            return banner;
        }
    }
}
