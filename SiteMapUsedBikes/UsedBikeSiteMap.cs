using Consumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SiteMapUsedBikes
{
    /// <summary>
    /// Created By: Aditi Srivastava on 22 Sep 2016
    /// DEscription: To generate xml files for used bikes urls
    /// </summary>
    public class UsedBikeSiteMap
    {
        public void GenerateSiteMap()
        {
            string domain = "http://www.bikewale.com/used/";
            List<String> bikes1 = new List<String>();
            List<String> bikes2 = new List<String>();
            UsedBikeUrlsRepository urlObj = new UsedBikeUrlsRepository();
            try
            {
                urlObj.GetUsedBikeUrls(bikes1, bikes2);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                System.IO.Directory.CreateDirectory("used");
                using (XmlWriter writer = XmlWriter.Create("used/bikes1.xml ", settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    if (bikes1 != null && bikes1.Count > 0)
                    {
                        foreach (var url in bikes1)
                        {
                            writer.WriteStartElement("url");
                            writer.WriteElementString("loc",String.Format("{0}{1}",domain,url));
                            writer.WriteEndElement();
                        }
                     }
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                }

                using (XmlWriter writer = XmlWriter.Create("used/bikes2.xml ", settings))
                {
                   
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    if (bikes2 != null && bikes2.Count > 0)
                    {
                        foreach (var url in bikes2)
                        {
                            writer.WriteStartElement("url");
                            writer.WriteElementString("loc", String.Format("{0}{1}",domain, url));
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                }
                   }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("GenerateSiteMap: Exception " + ex.Message);
            }
        }
    }
}