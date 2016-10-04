using Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
