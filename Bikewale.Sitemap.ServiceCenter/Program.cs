using Consumer;
using System;

namespace Bikewale.Sitemap.ServiceCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ServiceCenterSiteMap objMap = new ServiceCenterSiteMap();

            try
            {
                objMap.GenerateSiteMap();
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception " + ex.Message);
            }
        }
    }
}
