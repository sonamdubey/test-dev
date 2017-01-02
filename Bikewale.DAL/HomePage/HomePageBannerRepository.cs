using Bikewale.Entities.HomePage;
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
        /// </summary>
        /// <returns></returns>
        public HomePageBannerEntity GetHomePageBanner()
        {
            HomePageBannerEntity banner = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "gethomepagebanner";
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (dr != null)
                        {
                            if (dr.Read())
                            {
                                banner = new HomePageBannerEntity()
                                {
                                    DesktopCss = Convert.ToString(dr["desktopcss"]),
                                    DesktopHtml = Convert.ToString(dr["desktophtml"]),
                                    DesktopJS = Convert.ToString(dr["desktopjs"]),
                                    MobileCss = Convert.ToString(dr["mobilecss"]),
                                    MobileHtml = Convert.ToString(dr["mobilehtml"]),
                                    MobileJS = Convert.ToString(dr["mobilejs"])
                                };
                            }
                            dr.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "HomePageBannerRepository.GetHomePageBanner");
            }
            return banner;
        }
    }
}
