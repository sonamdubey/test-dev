using Bikewale.ElasticSearch.Entities;
using Bikewale.Sitemap.Entities;
using Consumer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml;

namespace Bikewale.Sitemap.MainSitemap
{
    /// <summary>
    /// Created by  :   Sumit Kate on 31 Oct 2017
    /// Description :   Class related to Generate various sitemap data
    /// </summary>
    public class Sitemap
    {
        private readonly string sitemapLoc = ConfigurationManager.AppSettings["SitemapLoc"];
        private readonly string domain = ConfigurationManager.AppSettings["SiteMapDomain"];
        private readonly int MaxUrlCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxUrlCount"]);
        private readonly string FileName = ConfigurationManager.AppSettings["FileName"];
        private readonly string sitemapSP = ConfigurationManager.AppSettings["sitemapSP"];
        private const string _extension = ".xml";

        private const string MakeUrl = "/{0}-bikes/";
        private const string ModelUrl = "/{0}-bikes/{1}/";
        private const string ModelImageUrl = "/{0}-bikes/{1}/images/";
        private const string ModelSpecUrl = "/{0}-bikes/{1}/specifications-features/";
        private const string ScooterMakeUrl = "/{0}-scooters/";
        private const string ModelComparisonUrl = "/comparebikes/{0}-{1}-vs-{2}-{3}/";
        private const string MakeExpertReviewsUrl = "/{0}-bikes/expert-reviews/";
        private const string ModelExpertReviewsUrl = "/{0}-bikes/{1}/expert-reviews/";
        private const string MakeNewsUrl = "/{0}-bikes/news/";
        private const string ModelNewsUrl = "/{0}-bikes/{1}/news/";
        private const string MakeUpcomingUrl = "/{0}-bikes/upcoming/";
        private const string MakeNewLaunchesUrl = "/new-{0}-bike-launches/";
        private const string MakeUserReviewsUrl = "/{0}-bikes/reviews/";
        private const string ModelUserReviewsUrl = "/{0}-bikes/{1}/reviews/";
        private const string MakeVideosUrl = "/{0}-bikes/videos/";
        private const string ModelVideosUrl = "/{0}-bikes/{1}/videos/";
        private const string NewsDetailsUrl = "/news/{0}-{1}.html";
        private const string ExpertReviewDetailsUrl = "/expert-reviews/{0}-{1}.html";
        private const string VideosDetailsUrl = "/bike-videos/{1}-{0}/";
        private const string FeaturesDetailsUrl = "/features/{1}-{0}/";

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Generate Sitemap
        /// </summary>
        public bool Generate()
        {
            bool isSuccess = false;
            try
            {
                SiteMapRepository repo = new SiteMapRepository(sitemapSP);

                IDictionary<UrlType, IDictionary<int, ICollection<KeyValuePair<int, string>>>> data = repo.GetData();
                SiteMapElasticSearch siteMapobj = new SiteMapElasticSearch();
                IDictionary<int, ICollection<KeyValuePair<int, string>>> siteMapESResult =  siteMapobj.GetSiteMapResult();
                data.Add(UrlType.ModelSpec,siteMapESResult);
                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        Logs.WriteInfoLog(String.Format("{0} - {1}", item.Key, ((item.Value != null && item.Value.Values != null && item.Value.Values.Any()) ? item.Value.Values.Count : 0)));
                    }
                    //call function to create urls
                    IEnumerable<string> urlList = CreateSitemapUrls(data);

                    Logs.WriteInfoLog(String.Format("Total Urls Fetched : {0}", urlList != null ? urlList.Count() : 0));

                    if (urlList != null && urlList.Any())
                    {
                        if (!Directory.Exists(sitemapLoc))
                            Directory.CreateDirectory(sitemapLoc);
                        ushort iteration = 1;
                        string outputFileName = String.Format("{0}{1}{2}", sitemapLoc, FileName, _extension);

                        Logs.WriteInfoLog(String.Format("Writing urls into {0}", outputFileName));

                        IEnumerable<string> urls = urlList.Take(MaxUrlCount);
                        do
                        {
                            CreateSitemapFile(urls, outputFileName);
                            urls = urlList.Skip(MaxUrlCount * iteration).Take(MaxUrlCount);
                            outputFileName = String.Format("{0}{1}-{2}{3}", sitemapLoc, FileName, iteration++, _extension);
                        } while (urls != null && urls.Any());
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Sitemap.Generate(): ", ex);
            }
            return isSuccess;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Created Sitemap file
        /// </summary>
        /// <param name="urlList"></param>
        /// <param name="fileName"></param>
        private void CreateSitemapFile(IEnumerable<string> urlList, string fileName)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                //create xml and write urls
                using (XmlWriter writer = XmlWriter.Create(fileName, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    if (urlList != null && urlList.Any())
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
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Sitemap.Generate(): ", ex);
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Created Sitemaps based on UrlType
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private IEnumerable<string> CreateSitemapUrls(IDictionary<UrlType, IDictionary<int, ICollection<KeyValuePair<int, string>>>> collection)
        {
            try
            {
                ICollection<string> urls = new List<string>();

                urls.Add("/");
                urls.Add("/new-bikes-in-india/");
                urls.Add("/best-cruiser-bikes-in-india/");
                urls.Add("/best-bikes-in-india/");
                urls.Add("/best-scooters-in-india/");
                urls.Add("/best-mileage-bikes-in-india/");
                urls.Add("/best-sports-bikes-in-india/");
                urls.Add("/scooters/");
                urls.Add("/comparebikes/");
                urls.Add("/expert-reviews/");
                urls.Add("/news/");
                urls.Add("/upcoming-bikes/");
                urls.Add("/new-bike-launches/");
                urls.Add("/reviews/");
                urls.Add("/bike-videos/");
                urls.Add("/aboutus.aspx");
                urls.Add("/bike-loan-emi-calculator/");
                urls.Add("/pricequote/");
                urls.Add("/features/");
                urls.Add("/bike-care/");
                urls.Add("/new/bike-search/bikes-under-50000/");
                urls.Add("/new/bike-search/bikes-between-50000-and-100000/");
                urls.Add("/new/bike-search/bikes-between-100000-and-250000/");
                urls.Add("/new/bike-search/bikes-above-250000/");
                urls.Add("/new/bike-search/bikes-between-50000-and-70000/");
                urls.Add("/new/bike-search/bikes-above-70000/");
                if (collection == null)
                {
                    collection = new Dictionary<UrlType, IDictionary<int, ICollection<KeyValuePair<int, string>>>>();
                }
                collection.Add(UrlType.NewsArticles, null);
                collection.Add(UrlType.ExpertReviews, null);
                collection.Add(UrlType.Features, null);
                collection.Add(UrlType.Videos, null);
                if (collection.Any())
                {
                    foreach (var item in collection)
                    {
                        switch (item.Key)
                        {
                            case UrlType.Invalid:
                                break;
                            case UrlType.Make:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(MakeUrl, urls, item);
                                }
                                break;
                            case UrlType.Model:
                                if (item.Value != null)
                                    PopulateUrlList(ModelUrl, urls, item);
                                break;
                            case UrlType.ModelImage:
                                if (item.Value != null)
                                    PopulateUrlList(ModelImageUrl, urls, item);
                                break;
                            case UrlType.ScooterMake:
                                if (item.Value != null)
                                    PopulateUrlList(ScooterMakeUrl, urls, item);
                                break;
                            case UrlType.ModelSpec:
                                if (item.Value != null)
                                    PopulateUrlList(ModelSpecUrl, urls, item);
                                break;
                            case UrlType.ModelComparison:
                                if (item.Value != null)
                                    PopulateUrlList(ModelComparisonUrl, urls, item);
                                break;
                            case UrlType.MakeExpertReviews:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(MakeExpertReviewsUrl, urls, item);
                                }
                                break;
                            case UrlType.ModelExpertReviews:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(ModelExpertReviewsUrl, urls, item);
                                }
                                break;
                            case UrlType.MakeNews:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(MakeNewsUrl, urls, item);
                                }
                                break;
                            case UrlType.ModelNews:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(ModelNewsUrl, urls, item);
                                }
                                break;
                            case UrlType.MakeUpcoming:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(MakeUpcomingUrl, urls, item);
                                }
                                break;
                            case UrlType.MakeNewLaunches:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(MakeNewLaunchesUrl, urls, item);
                                }
                                break;
                            case UrlType.MakeUserReviews:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(MakeUserReviewsUrl, urls, item);
                                }
                                break;
                            case UrlType.ModelUserReviews:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(ModelUserReviewsUrl, urls, item);
                                }
                                break;
                            case UrlType.MakeVideos:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(MakeVideosUrl, urls, item);
                                }
                                break;
                            case UrlType.ModelVideos:
                                if (item.Value != null)
                                {
                                    PopulateUrlList(ModelVideosUrl, urls, item);
                                }
                                break;
                            case UrlType.NewsArticles:
                                PopulateNewsUrls(urls);
                                break;
                            case UrlType.ExpertReviews:
                                PopulateExpertReviewsUrls(urls);
                                break;
                            case UrlType.Videos:
                                PopulateVideosUrls(urls);
                                break;
                            case UrlType.Features:
                                PopulateFeaturesUrls(urls);
                                break;
                            case UrlType.SeriesPage:
                                PopulateUrlList(ModelUrl, urls, item);
                                break;
                            case UrlType.SeriesNews:
                                PopulateUrlList(ModelNewsUrl, urls, item);
                                break;
                            case UrlType.SeriesExpertReview:
                                PopulateUrlList(ModelExpertReviewsUrl, urls, item);
                                break;
                            case UrlType.SeriesVideos:
                                PopulateUrlList(ModelVideosUrl, urls, item);
                                break;
                            default:
                                break;
                        }
                    }
                }
                return urls;
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Sitemap.CreateSitemapUrls(): ", ex);
            }
            return null;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Get Features data and create urls
        /// </summary>
        /// <param name="urls"></param>
        private void PopulateFeaturesUrls(ICollection<string> urls)
        {
            string response = String.Empty;
            try
            {
                using (HttpClient _httpClient = new HttpClient())
                {
                    using (HttpResponseMessage _response = _httpClient.GetAsync(ConfigurationManager.AppSettings["FeaturesApiUrl"]).Result)
                    {
                        if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //Check 200 OK Status      
                            response = _response.Content.ReadAsStringAsync().Result;
                            _response.Content.Dispose();
                            _response.Content = null;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(response))
                {
                    var articles = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ArticleEntity>>(response);
                    if (articles != null)
                    {
                        foreach (var article in articles)
                        {
                            urls.Add(String.Format(FeaturesDetailsUrl, article.BasicId, article.ArticleUrl));
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("PopulateFeaturesUrls ", ex);
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Get Videos data and create urls
        /// </summary>
        /// <param name="urls"></param>
        private void PopulateVideosUrls(ICollection<string> urls)
        {
            string response = String.Empty;
            try
            {
                using (HttpClient _httpClient = new HttpClient())
                {
                    using (HttpResponseMessage _response = _httpClient.GetAsync(ConfigurationManager.AppSettings["VideosApiUrl"]).Result)
                    {
                        if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //Check 200 OK Status      
                            response = _response.Content.ReadAsStringAsync().Result;
                            _response.Content.Dispose();
                            _response.Content = null;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(response))
                {
                    var videos = Newtonsoft.Json.JsonConvert.DeserializeObject<VideosResponse>(response);
                    if (videos != null)
                    {
                        foreach (var video in videos.videos)
                        {
                            urls.Add(String.Format(VideosDetailsUrl, video.BasicId, video.VideoTitleUrl));
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("PopulateVideosUrls ", ex);
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Get Expert reviews data and create urls
        /// </summary>
        /// <param name="urls"></param>
        private void PopulateExpertReviewsUrls(ICollection<string> urls)
        {
            string response = String.Empty;
            try
            {
                using (HttpClient _httpClient = new HttpClient())
                {
                    using (HttpResponseMessage _response = _httpClient.GetAsync(ConfigurationManager.AppSettings["ExpertReviewApiUrl"]).Result)
                    {
                        if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //Check 200 OK Status      
                            response = _response.Content.ReadAsStringAsync().Result;
                            _response.Content.Dispose();
                            _response.Content = null;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(response))
                {
                    var articles = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ArticleEntity>>(response);
                    if (articles != null)
                    {
                        foreach (var article in articles)
                        {
                            urls.Add(String.Format(ExpertReviewDetailsUrl, article.ArticleUrl, article.BasicId));
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("PopulateExpertReviewsUrls ", ex);
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Get News data and create urls
        /// </summary>
        /// <param name="urls"></param>
        private void PopulateNewsUrls(ICollection<string> urls)
        {
            string response = String.Empty;
            try
            {
                using (HttpClient _httpClient = new HttpClient())
                {
                    using (HttpResponseMessage _response = _httpClient.GetAsync(ConfigurationManager.AppSettings["NewsApiUrl"]).Result)
                    {
                        if (_response.IsSuccessStatusCode && _response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //Check 200 OK Status      
                            response = _response.Content.ReadAsStringAsync().Result;
                            _response.Content.Dispose();
                            _response.Content = null;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(response))
                {
                    var articles = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<ArticleEntity>>(response);
                    if (articles != null)
                    {
                        foreach (var article in articles)
                        {
                            urls.Add(String.Format(NewsDetailsUrl, article.BasicId, article.ArticleUrl));
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("PopulateNewsUrls ", ex);
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 31 Oct 2017
        /// Description :   Create urls based on url pattern passed in parameter
        /// </summary>
        /// <param name="urlPattern"></param>
        /// <param name="urls"></param>
        /// <param name="item"></param>
        private static void PopulateUrlList(String urlPattern, ICollection<string> urls, KeyValuePair<UrlType, IDictionary<int, ICollection<KeyValuePair<int, string>>>> item)
        {
            foreach (var vals in item.Value)
            {
                urls.Add(String.Format(urlPattern, vals.Value.Select(m => m.Value).ToArray()));
            }
        }
    }
}
