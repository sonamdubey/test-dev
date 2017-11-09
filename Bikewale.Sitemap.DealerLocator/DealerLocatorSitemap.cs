﻿using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace Bikewale.Sitemap.DealerLocator
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 01-Nov-2017
    /// Summary: function to generate sitemap
    /// 
    /// </summary>
    class DealerLocatorSitemap
    {
        public void GenerateSiteMap()
        {
            string domain = ConfigurationManager.AppSettings["SiteMapDomain"];
            string sitemapLoc = ConfigurationManager.AppSettings["SiteMapLoc"];
            IEnumerable<DealerLocatorEntity> SitemapList = null;

            if (domain != null)
                try
                {
                    // get data from database
                    DealerLocatorSitemapRepository objSitemap = new DealerLocatorSitemapRepository();
                    SitemapList = objSitemap.GetDealerLocatorMakeCities();
                    Logs.WriteInfoLog("All Dealer locator List : " + SitemapList.Count());
                    // create directory if not exists
                    if (sitemapLoc != null)
                    {
                        System.IO.Directory.CreateDirectory(sitemapLoc);

                        //call function to create urls
                        IEnumerable<string> urlList = CreateDealerLocatorUrls(SitemapList);
                        Logs.WriteInfoLog("All Dealer locator Urls List : " + urlList.Count());
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        int count = 1;
                        do
                        {

                            //create xml and write urls
                            using (XmlWriter writer = XmlWriter.Create(string.Format("{0}dealer-locator-showrooms-{1}.xml ", sitemapLoc, count), settings))
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
                    }

                }
                catch (Exception ex)
                {
                    Logs.WriteErrorLog("Bikewale.Sitemap.DealerLocator: GenerateSiteMap: Exception " + ex.Message);
                }
        }
        /// <summary>
        /// Creates the dealer locator urls.
        /// </summary>
        /// <param name="sitemapList">The sitemap list.</param>
        /// <returns></returns>
        public List<string> CreateDealerLocatorUrls(IEnumerable<DealerLocatorEntity> sitemapList)
        {
            List<string> urlList = new List<string>();
            try
            {
                urlList.Add("dealer-showroom-locator/");
                foreach (var value in sitemapList)
                {
                    urlList.Add(string.Format("{0}-dealer-showrooms-in-{1}/", value.MakeMaskingName, value.CityMaskingName));
                }
                List<string> urlIndiaList = CreateDealerLocatorUrlsForIndia(sitemapList);
                urlList.AddRange(urlIndiaList);

                Logs.WriteInfoLog("All Dealer locator Urls List Completed");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Bikewale.Sitemap.DealerLocator: CreateDealerLocatorUrls: Exception " + ex.Message);
            }

            return urlList;
        }
        /// <summary>
        /// Creates the dealer locator urls for india.
        /// </summary>
        /// <param name="sitemapList">The sitemap list.</param>
        /// <returns></returns>
        public List<string> CreateDealerLocatorUrlsForIndia(IEnumerable<DealerLocatorEntity> sitemapList)
        {
            List<string> urlList = new List<string>();
            try
            {
                var allMakes = sitemapList.Select(x => x.MakeMaskingName).Distinct();
                foreach (var make in allMakes)
                {
                    urlList.Add(string.Format("{0}-dealer-showrooms-in-{1}/", make, "india"));
                }
                Logs.WriteInfoLog("All Dealer locator Urls India List Completed");
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Bikewale.Sitemap.DealerLocator: CreateDealerLocatorUrlsForIndia: Exception " + ex.Message);
            }
            return urlList;
        }
    }
}
