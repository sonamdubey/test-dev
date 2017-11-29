using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace Bikewale.Sitemap.PriceInCity
{
    class PriceInCitySiteMap
    {
        public bool GenerateSiteMap()
        {
            bool isSuccess = false;
            string domain = ConfigurationManager.AppSettings["PriceInCitySiteMapDomain"];
            string PriceInCitySitemapLoc = ConfigurationManager.AppSettings["PriceInCitySitemapLoc"];
            IEnumerable<PriceInCityEnitity> SitemapList = null;

            if (domain != null)
                try
                {
                    // get data from database
                    PriceInCityUrlsRepository urlObj = new PriceInCityUrlsRepository();
                    SitemapList = urlObj.GetPriceInCityUrls();
                    Logs.WriteInfoLog("All Price In City List : " + SitemapList.Count());
                    // create directory if not exists
                    if (PriceInCitySitemapLoc != null)
                    {
                        System.IO.Directory.CreateDirectory(PriceInCitySitemapLoc);

                        //call function to create urls
                        IEnumerable<string> urlList = CreatePriceInCityUrls(SitemapList);
                        Logs.WriteInfoLog("All Price In City Urls  List : " + urlList.Count());
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        int count = 1;
                        do
                        {

                            //create xml and write urls
                            using (XmlWriter writer = XmlWriter.Create(string.Format("{0}new-bike-prices-{1}.xml ", PriceInCitySitemapLoc, count), settings))
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

        public IEnumerable<string> CreatePriceInCityUrls(IEnumerable<PriceInCityEnitity> SitemapList)
        {
            IList<string> urlList = new List<string>();
            try
            {
                foreach (var value in SitemapList)
                {
                    urlList.Add(string.Format("{0}-bikes/{1}/price-in-{2}/", value.MakeMaskingName, value.ModelMaskingName, value.CityMaskingName));
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("CreatePriceInCityUrls: Exception " + ex.Message);
            }

            return urlList;
        }

    }
}
