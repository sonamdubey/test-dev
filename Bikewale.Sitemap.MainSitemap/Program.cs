using Consumer;
using System;

namespace Bikewale.Sitemap.MainSitemap
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                Logs.WriteInfoLog("Program Started.");
                Sitemap s = new Sitemap();
                s.Generate();
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Error Occured", ex);
            }
            finally
            {
                Logs.WriteInfoLog("Program Ended.");
            }
        }
    }
}
