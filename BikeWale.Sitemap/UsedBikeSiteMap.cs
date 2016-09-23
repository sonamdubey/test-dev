using Consumer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MoreLinq;

namespace BikeWale.Sitemap
{
    /// <summary>
    /// Created By: Aditi Srivastava on 22 Sep 2016
    /// Description: To generate xml files for used bikes urls
    /// </summary>
    public class UsedBikeSiteMap
    {
        /// <summary>
        /// Summary: Writes urls to xml
        /// </summary>
        public void GenerateSiteMap()
        {
            string domain = ConfigurationManager.AppSettings["UsedSiteMapDomain"];
            string usedSitemapLoc = ConfigurationManager.AppSettings["UsedSitemapLoc"];
            IEnumerable<UsedBikeEntity> SitemapList = null;
            
            if(domain!=null)
            try
            {
                // get data from database
                UsedBikeUrlsRepository urlObj = new UsedBikeUrlsRepository();
                SitemapList=urlObj.GetUsedBikeUrls();

                // create directory if not exists
                 if (usedSitemapLoc != null)
                {
                    System.IO.Directory.CreateDirectory(usedSitemapLoc);

                    //call function to create urls
                    IEnumerable<string> urlList = CreateUsedBikeUrls(SitemapList);

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;

                    //create xml and write urls
                    using (XmlWriter writer = XmlWriter.Create(string.Format("{0}usedbikes.xml ", usedSitemapLoc), settings))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                        if (urlList != null && urlList.Count() > 0)
                        {
                            foreach (var url in urlList)
                            {
                                writer.WriteStartElement("url");
                                writer.WriteElementString("loc", String.Format("{0}{1}", domain, url));
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndDocument();
                        writer.Flush();
                        writer.Close();
                    }
                }

           }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("GenerateSiteMap: Exception " + ex.Message);
            }
        }

        /// <summary>
        /// Summary: Creates all urls for used bikes sitemap
        /// </summary>
        /// <param name="SitemapList"></param>
        /// <returns></returns>
        public IEnumerable<string> CreateUsedBikeUrls(IEnumerable<UsedBikeEntity> SitemapList)
        {
            IList<string> urlList = new List<string>();
            try
            {
                StringBuilder Url = null;
                urlList.Add("bikes-in-india/");

                //used in cities
                 var cities = (from city in SitemapList
                              select city.CityName).Distinct();

                if(cities!=null) 
                foreach(var city in cities)
                {
                    Url = new StringBuilder();
                    Url.Append(String.Format("bikes-in-{0}/",city));
                    urlList.Add(Url.ToString());
                }

                //used makes in india
                var makes = (from make in SitemapList
                             select make.MakeName).Distinct();
                
                if (makes != null)
                foreach(var make in makes)
                {
                    Url = new StringBuilder();
                    Url.Append(String.Format("{0}-bikes-in-india/",make));
                    urlList.Add(Url.ToString());
                }

                //used makes in cities
                IEnumerable<UsedBikeEntity> makecity = SitemapList.DistinctBy(m => new { m.MakeName, m.CityName });
                
                if (makecity != null)
                foreach (var item in makecity)
                {
                    Url = new StringBuilder();
                    Url.Append(String.Format("{0}-bikes-in-{1}/", item.MakeName,item.CityName));
                    urlList.Add(Url.ToString());
                }

                //used make models in india
                IEnumerable<UsedBikeEntity> makemodels = SitemapList.DistinctBy(m => new { m.MakeName, m.ModelName });
                
                if(makemodels!=null)
                foreach (var item in makemodels)
                {
                    Url = new StringBuilder();
                    Url.Append(String.Format("{0}-{1}-bikes-in-india/", item.MakeName, item.ModelName));
                    urlList.Add(Url.ToString());
                }


                //used make models in cities
                if (SitemapList != null)
                foreach (var item in SitemapList)
                {
                    Url = new StringBuilder();
                    Url.Append(String.Format("{0}-{1}-bikes-in-{2}/", item.MakeName, item.ModelName,item.CityName));
                    urlList.Add(Url.ToString());
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("CreateUsedBikeUrls: Exception " + ex.Message);
            }

            return urlList;
        }
    }
}