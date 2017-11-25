using Consumer;
using System;

namespace BikeWale.Sitemap
{
    class Program
    {
        static void Main(string[] args)
        {
            UsedBikeSiteMap siteMap = new UsedBikeSiteMap();
            try
            {
                siteMap.GenerateSiteMap();
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception " + ex.Message);
            }
        }
    }
}
