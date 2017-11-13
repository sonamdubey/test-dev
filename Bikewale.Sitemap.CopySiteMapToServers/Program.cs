using Consumer;
using System;

namespace Bikewale.Sitemap.CopySiteMapToServers
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 31 Oct 2017
    /// Summary : Class to copy sitemaps to servers and submit sitemaps goggle
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Logs.WriteInfoLog("Started : CopySiteMapToServers");
            ProcessSitemaps.SitemapType sitemap;
            if (args != null && args.Length > 0 && Enum.TryParse(args[0], out sitemap))
            {
                ProcessSitemaps.CopyToServers(sitemap);
                //ProcessSitemaps.SubmitSitemapToGoogle();
            }
            Logs.WriteInfoLog("Finished : CopySiteMapToServers");
        }
    }
}
