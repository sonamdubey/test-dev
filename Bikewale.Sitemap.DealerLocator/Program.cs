using Consumer;
using System;

namespace Bikewale.Sitemap.DealerLocator
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            DealerLocatorSitemap objMap = new DealerLocatorSitemap();

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
