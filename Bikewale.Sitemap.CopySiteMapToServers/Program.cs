using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ProcessSitemaps.CopyToServers(ProcessSitemaps.SitemapType.General);
            ProcessSitemaps.SubmitSitemapToGoogle();

            Console.ReadKey();
        }
    }
}
