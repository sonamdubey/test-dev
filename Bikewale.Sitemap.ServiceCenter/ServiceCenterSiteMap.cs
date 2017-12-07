using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
namespace Bikewale.Sitemap.ServiceCenter
{
    class ServiceCenterSiteMap
    {
        public bool GenerateSiteMap()
        {
            bool isSuccess = false;
            string domain = ConfigurationManager.AppSettings["ServiceCenterSiteMapDomain"];
            string ServiceCenterSitemapLoc = ConfigurationManager.AppSettings["ServiceCenterSitemapLoc"];
            IEnumerable<ServiceCenterEnitity> SitemapList = null;

            if (domain != null)
                try
                {
                    // get data from database
                    ServiceCenterUrlsRepository urlObj = new ServiceCenterUrlsRepository();
                    SitemapList = urlObj.GetServiceCenterUrls();
                    // create directory if not exists
                    if (!String.IsNullOrEmpty(ServiceCenterSitemapLoc) && SitemapList != null && SitemapList.Any())
                    {
                        Logs.WriteInfoLog("All Service center List : " + SitemapList.Count());
                        System.IO.Directory.CreateDirectory(ServiceCenterSitemapLoc);

                        //call function to create urls
                        IEnumerable<string> urlList = CreateServiceCenterUrls(SitemapList);
                        Logs.WriteInfoLog("All Service center Urls List : " + urlList.Count());
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        int count = 1;
                        do
                        {

                            //create xml and write urls
                            using (XmlWriter writer = XmlWriter.Create(string.Format("{0}service-center-locator-{1}.xml ", ServiceCenterSitemapLoc, count), settings))
                            {
                                writer.WriteStartDocument();
                                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                                if (urlList != null && urlList.Any())
                                {
                                    int value = 0;
                                    foreach (var url in urlList)
                                    {
                                        writer.WriteStartElement("url");
                                        writer.WriteElementString("loc", String.Format("{0}{1}", domain, url));
                                        writer.WriteEndElement();
                                        value++;
                                        if (value == 40000)
                                            break;
                                    }

                                }
                                urlList = urlList.Skip(40000);
                                count++;
                                writer.WriteEndDocument();
                                writer.Flush();
                                writer.Close();
                            }


                        } while (urlList.Any());
                        isSuccess = true;
                    }

                }
                catch (Exception ex)
                {
                    Logs.WriteErrorLog("GenerateSiteMap: Exception " + ex.Message);
                }
            return isSuccess;
        }
        /// <summary>
        /// Modified by: Snehal Dange on 1st dec 2017
        /// Description: Changed "{make}-service-center-in-{city}" to "/service-centers/{make}/{city}/"
        /// </summary>
        /// <param name="SitemapList"></param>
        /// <returns></returns>
        public IEnumerable<string> CreateServiceCenterUrls(IEnumerable<ServiceCenterEnitity> SitemapList)
        {
            IList<string> urlList = new List<string>();
            try
            {
                urlList.Add("service-centers/");
                foreach (var value in SitemapList)
                {
                    urlList.Add(string.Format("service-centers/{0}/{1}", value.MakeMaskingName, string.IsNullOrEmpty(value.CityMaskingName) ? "" : value.CityMaskingName + "/"));
                }
                Logs.WriteInfoLog("All Service center Urls List Completed");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("CreateServiceCenterUrls: Exception " + ex.Message);
            }

            return urlList;
        }
    }
}
