using Consumer;
using System;

namespace Bikewale.Sitemap.PriceInCity
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            PriceInCitySiteMap objMap = new PriceInCitySiteMap();
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
