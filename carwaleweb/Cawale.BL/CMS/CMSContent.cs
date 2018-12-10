using AutoMapper;
using Carwale.BL.GrpcFiles;
using Carwale.Entity;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.CMS;
using System.Collections.Specialized;
using RabbitMqPublishing;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Notifications;
using Carwale.Utility;
using Grpc.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.TipsAndAdvice;
using System.Xml.Linq;
using System.Web;
using Carwale.Entity.SEO;
using AEPLCore.Cache;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.CarData;
using AEPLCore.Cache.Interfaces;
using Carwale.Interfaces.Customer;

namespace Carwale.BL.CMS
{
    public class CMSContentBL : ICMSContent
    {
        public static readonly List<int> ExpertCategories = System.Configuration.ConfigurationManager.AppSettings["ExpertCategories"].Split(',').Select(int.Parse).ToList();
        public static readonly List<int> FeatureCategories = System.Configuration.ConfigurationManager.AppSettings["FeatureCategories"].Split(',').Select(int.Parse).ToList();
        public static readonly List<int> NewsCategories = System.Configuration.ConfigurationManager.AppSettings["NewsCategoriesListing"].Split(',').Select(int.Parse).ToList();
        private static readonly uint pageSize=9;
        private readonly IVideosBL _videoBL;
        private ICacheManager cachecore;
        private  readonly ICustomerTracking _customerTracking;
        public CMSContentBL(ICacheManager cachecore, IVideosBL videoBL, ICarModelCacheRepository carModelCacheRepos, ICustomerTracking customerTracking)
        {
            this.cachecore = cachecore;
            this._videoBL = videoBL;
            this._customerTracking = customerTracking;
        }

        //#
        public ArticleDetails GetContentDetails(ArticleContentURI queryString)
        {
            try
            {
                queryString.ApplicationId = queryString.ApplicationId > 0 ? queryString.ApplicationId : (ushort)Carwale.Entity.Enum.Application.CarWale;
                QueueUpdateView(Convert.ToInt32(queryString.BasicId));
                var result = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetContentDetails(queryString));
                if (result != null)
                {
                    if (result.Description != null)
                        result.Description = result.Description.Replace("&#x20B9;", "₹");
                    if (result.Title != null)
                        result.Title = result.Title.Replace("&#x20B9;", "₹");
                    if (result.Content != null)
                        result.Content = result.Content.Replace("&#x20B9;", "₹");

                    string adHtml = string.Format("<div id='dfp-tw-{0}'></div>", result.BasicId);
                    result.Content = InsertAdHtml(result.Content, adHtml);
                    //call customer tracking only for news
                    if (result.CategoryId==(ushort)CMSContentType.News)
                    {
                        _customerTracking.TrackNewsTags(result.VehiclTagsList);
                    }
                    //check modified date
                    if(result.ModifiedDate < result.DisplayDate)
                    {
                        result.ModifiedDate = result.DisplayDate;
                    }
                }
                return result;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetContentDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        //# 
        public ArticlePageDetails GetContentPages(ArticleContentURI queryString)
        {
            try
            {
                queryString.ApplicationId = queryString.ApplicationId > 0 ? queryString.ApplicationId : (ushort)Carwale.Entity.Enum.Application.CarWale;
                QueueUpdateView(Convert.ToInt32(queryString.BasicId));
                var result = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetContentPages(queryString));
                if (result == null)
                    return null;
                if (result.Description != null)
                    result.Description = result.Description.Replace("&#x20B9;", "₹");
                if (result.Title != null)
                    result.Title = result.Title.Replace("&#x20B9;", "₹");
                foreach (var page in result.PageList)
                {
                    if (page.Content != null)
                        page.Content = page.Content.Replace("&#x20B9;", "₹");
                }
                //call customer tracking only for news
                if (result.CategoryId == (ushort)CMSContentType.News)
                {
                    _customerTracking.TrackNewsTags(result.VehiclTagsList);
                }
                //check modified date
                if (result.ModifiedDate < result.DisplayDate)
                {
                    result.ModifiedDate = result.DisplayDate;
                }
                return result;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetContentPages Exception");
                objErr.LogException();
                return null;
            }
        }

        public CMSContent GetContentListByCategory(ArticleByCatURI queryString, bool fetchSponosored = true, ArticleSummary sponsoredArticle = null, CMSContent results = null, bool makeApiCall = true)
        {
            try
            {
                if (makeApiCall)
                {
                    sponsoredArticle = new ArticleSummary();
                }
                bool addsponsored = false;
                queryString.PageSize = queryString.PageSize > 0 ? queryString.PageSize : pageSize;
                queryString.PageNo = queryString.PageNo > 0 ? queryString.PageNo : (uint)(queryString.StartIndex / queryString.PageSize) + 1;

                if (!string.IsNullOrEmpty(CWConfiguration.SponsoredAuthorId) && queryString.PageNo == 1 && fetchSponosored)
                {
                    sponsoredArticle = GetSponsoredArticle(queryString.CategoryIdList, CWConfiguration.SponsoredAuthorId, sponsoredArticle, makeApiCall);
                    if (sponsoredArticle != null && sponsoredArticle.BasicId > 0 && ((queryString.MakeId == 0 && queryString.ModelId == 0) || queryString.MakeId == 9))
                    {
                        queryString.EndIndex = queryString.StartIndex + ((2 * queryString.PageSize) - 1) - 1;
                        addsponsored = true;
                    }
                }
                if (makeApiCall)
                {
                    results = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetArticleListByCategory(queryString), queryString.CategoryIdList);
                }
                results = ConvertRupeeUnicode(results);
                if (results != null)
                {
                    if (addsponsored)
                    {
                        results.Articles.Insert(0, sponsoredArticle);
                    }
                }
                return results;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetContentListByCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<ArticleSummary> GetRelatedListByCategory(ArticleByCatAndIdURI queryString)
        {
            try
            {
                List<ArticleSummary> articles = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetRelatedListByCategory(queryString));

                articles = ConvertRupeeUnicode(articles);

                return articles;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetRelatedListByCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        public CMSContent GetContentListBySubCategory(ArticleBySubCatURI queryString)
        {
            try
            {
                var results = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetContentListBySubCategory(queryString),queryString.CategoryIdList);
                foreach (var article in results.Articles)
                {
                    if (article.Description != null)
                        article.Description = article.Description.Replace("&#x20B9;", "₹");
                    if (article.Title != null)
                        article.Title = article.Title.Replace("&#x20B9;", "₹");
                }
                return results;              
            }
            catch(Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CMSContentBL GetContentListBySubCategory Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<ArticleSummary> GetFeaturedArticles(ArticleFeatureURI queryString)
        {
            try
            {
                var articles = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetFeaturedArticles(queryString));

                articles = ConvertRupeeUnicode(articles);

                return articles;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetFeaturedArticles Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<ArticleSummary> GetMostRecentArticles(ArticleRecentURI queryString, List<ArticleSummary> articles = null)
        {
            try
            {
                if (queryString != null)
                {
                    articles = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.MostRecentList(queryString));
                }

                articles = ConvertRupeeUnicode(articles);

                return articles;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetMostRecentArticles Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<ArticleSummary> GetRelatedArticles(ArticleRelatedURI queryString, List<ArticleSummary> articles = null)
        {
            try
            {
                if (queryString != null)
                {
                    articles = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetRelatedArticlesByBasicId(queryString));
                }

                articles = ConvertRupeeUnicode(articles);

                return articles;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetRelatedArticles Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<MakeAndModelDetail> GetMakeDetails(MakeDetailURI queryString)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetMakeDetails(queryString));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetMakeDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<MakeAndModelDetail> GetModelDetails(ModelDetailURI queryString)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetModelDetails(queryString));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetModelDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        public MetaTags GetContentMetaTags(int cmsCategoryId)
        {
            var result = new MetaTags();
            try
            {
                string cacheKey = "CMS-Meta-Tags-" + cmsCategoryId;
                result = cachecore.GetFromCache<MetaTags>(cacheKey);
                if (result == null)
                {
                    XDocument xDocument = XDocument.Load(HttpContext.Current.Server.MapPath("~/data/cms/cms-Content-Metatags.xml"));

                    result = (from c in xDocument.Descendants("category").Where
                                           (c => (int)c.Attribute("id") == cmsCategoryId)
                            select new MetaTags()
                            {
                                Title = c.Element("title").Value,
                                Description = c.Element("description").Value,
                                Heading = c.Element("heading").Value,
                                Keywords = c.Element("keywords").Value,
                                Annotation = new Annotations()
                                {
                                    AlternateUrl = c.Element("alternateUrl").Value,
                                    Canonical = c.Element("canonical").Value
                                }
                            }).First();
                    cachecore.StoreToCache<MetaTags>(cacheKey, CacheRefreshTime.DefaultRefreshTime(), result);
                }
                if (result.Description != null)
                    result.Description = result.Description.Replace("&#x20B9;", "₹");
                if (result.Title != null)
                    result.Title = result.Title.Replace("&#x20B9;", "₹");
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "Content List Meta Tags BL in Exception- GetContentMetaTags()");
                objErr.LogException();
            }
            return result;
        }

        public List<CMSSubCategory> GetCMSSubCategories(int cmsCategoryId)
        {
            try
            {
                return Mapper.Map<List<CMSSubCategoryV2>,List<CMSSubCategory>>(GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetCMSSubCategories(cmsCategoryId)));
            }
            catch(Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetTopArticlesByBasicId Exception");
                objErr.LogException();
                return null;
            }
        }

        public int GetCMSRoadTestCount(int makeId, int modelId, int versionId, int topCount, int applicationId)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetCMSRoadTestCount(makeId, modelId,versionId,topCount,applicationId));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetCMSRoadTestCount Exception");
                objErr.LogException();
                return 0;
            }
        }

        public void QueueUpdateView(int basicId)
        {
            RabbitMqPublish ra = new RabbitMqPublish();
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("basicId", basicId.ToString());
            ra.PublishToQueue(CWConfiguration.ArticleViewQueue, nvc);
        }

        public List<RelatedArticles> GetTopArticlesByBasicId(int basicId)
        {
            try
            {
                return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetTopArticlesByBasicId(basicId));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetTopArticlesByBasicId Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<RelatedArticles> GetRelatedContent(int basicId)
        {
            try
            {
                int count = CWConfiguration.RelatedArticleCount;
                var results = new List<RelatedArticles>();
                var videoResult = _videoBL.GetRelatedVideoContent(basicId);
                if (videoResult != null)
                {
                    results.AddRange(videoResult);
                    count = count - videoResult.Count;
                }
                var relatedArticle = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetRelatedContent(basicId, count));
                if (relatedArticle != null) results.AddRange(relatedArticle);
                if (results == null || results.Count < 1)
                {
                    var topArticles = GetTopArticlesByBasicId(basicId);
                    if (topArticles != null) results.AddRange(topArticles);
                }
                return GetSequenceRelatedContent(results.GroupBy(x => x.CategoryMaskingName != null ? x.CategoryMaskingName.Replace("-", "").ToLower() : "NA").ToDictionary(y => y.Key, y => y.ToList()));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetRelatedContent Exception");
                objErr.LogException();
            }
            return new List<RelatedArticles>();
        }

        public List<RelatedArticles> GetSequenceRelatedContent(Dictionary<string, List<RelatedArticles>> dict)
        {
            List<RelatedArticles> relatedContentlist = new List<RelatedArticles>();
            List<RelatedArticles> newsList = new List<RelatedArticles>();
            List<RelatedArticles> videoList = new List<RelatedArticles>();
            List<RelatedArticles> reviewList = new List<RelatedArticles>();
            List<RelatedArticles> featuresList = new List<RelatedArticles>();
            List<RelatedArticles> photoGalleries = new List<RelatedArticles>();
            //if want to add more category add here
            Dictionary<int, List<RelatedArticles>> result = new Dictionary<int, List<RelatedArticles>>();
            if (dict != null)
            {
                foreach (var key in dict.Keys)
                {
                    EnumGenericContentType category;
                    if (Enum.TryParse<EnumGenericContentType>(key.ToLower(), out category))
                        result[(int)category] = dict[key];
                }
            }
            if (result != null)
            {
                if (result.ContainsKey((int)EnumGenericContentType.news))
                {
                    newsList.AddRange(result[(int)EnumGenericContentType.news]);
                }
                if (result.ContainsKey((int)EnumGenericContentType.videos))
                {
                    videoList.AddRange(result[(int)EnumGenericContentType.videos]);
                }
                if (result.ContainsKey((int)EnumGenericContentType.expertreviews))
                {
                    reviewList.AddRange(result[(int)EnumGenericContentType.expertreviews]);
                }
                if (result.ContainsKey((int)EnumGenericContentType.features))
                {
                    featuresList.AddRange(result[(int)EnumGenericContentType.features]);
                }
                if (result.ContainsKey((int)EnumGenericContentType.galleries))
                {
                    photoGalleries.AddRange(result[(int)EnumGenericContentType.galleries]);
                }
                //if want to add more category add here

            }
            var maxCount = Math.Max(newsList.Count, Math.Max(reviewList.Count, videoList.Count));
            int parentCategory = newsList.Count > 0 ? newsList[0].ParentCatId : (videoList.Count > 0 ? videoList[0].ParentCatId : (reviewList.Count > 0 ? reviewList[0].ParentCatId : (int)EnumGenericContentType.news));
            for (int articleNo = 0; articleNo < maxCount; articleNo++)
            {
                if (parentCategory == (int)CMSContentType.ComparisonTests || parentCategory == (int)CMSContentType.RoadTest)
                {
                    if (newsList.Count > articleNo)
                        relatedContentlist.Add(newsList[articleNo]);
                    if (videoList.Count > articleNo)
                        relatedContentlist.Add(videoList[articleNo]);
                    if (reviewList.Count > articleNo)
                        relatedContentlist.Add(reviewList[articleNo]);
                }

                else if (parentCategory == (int)CMSContentType.Videos)
                {
                    if (reviewList.Count > articleNo)
                        relatedContentlist.Add(reviewList[articleNo]);
                    if (newsList.Count > articleNo)
                        relatedContentlist.Add(newsList[articleNo]);
                    if (videoList.Count > articleNo)
                        relatedContentlist.Add(videoList[articleNo]);

                }

                else
                {
                    if (videoList.Count > articleNo)
                        relatedContentlist.Add(videoList[articleNo]);
                    if (reviewList.Count > articleNo)
                        relatedContentlist.Add(reviewList[articleNo]);
                    if (newsList.Count > articleNo)
                        relatedContentlist.Add(newsList[articleNo]);
                }
                //if want to add more category add here
            }

            return relatedContentlist;
        }
        public List<CMSSubCategoryV2> GetContentSegment(CommonURI queryString, int sourceId = -1, bool getAllMedia = false)
        {
            List<CMSSubCategoryV2> content = new List<CMSSubCategoryV2>();
            try
            {
                content = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetContentSegment(queryString ?? new CommonURI { ApplicationId = 1}, getAllMedia));                

                if (content == null || content.Count <= 0)
                    return null;

                int sum = content.Where(x => NewsCategories.Contains(x.SubCategoryId)).Sum(x => x.RecordCount);
                content.Where(x => x.SubCategoryId == 1).Select(x => { x.RecordCount = sum; return x; }).ToList();
                sum = content.Where(x => ExpertCategories.Contains(x.SubCategoryId)).Sum(x => x.RecordCount);
                content.Where(x => x.SubCategoryId == 2).Select(x => { x.RecordCount = sum; return x; }).ToList();

                if (sourceId == (int)Platform.CarwaleAndroid || sourceId == (int)Platform.CarwaleiOS)
                {
                    content.Where(x => x.SubCategoryId == 1).Select(x => { x.SubCategoryName = "All"; return x; }).ToList();
                }

                if (!string.IsNullOrEmpty(CWConfiguration.WebContentSegmentList))
                {
                    var _webSegmentContent = CWConfiguration.WebContentSegmentList.Split(',');
                    return content.Where(c => _webSegmentContent.Contains(c.SubCategoryId.ToString())).ToList();
                }
                
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetContentSegment Exception");
                objErr.LogException();
            }
            return content;
        }

        public List<ContentFeedSummary> GetNewsFeedBySlug(ContentFeedURI queryString)
        {
            try
            {
                var feedSummary = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetNewsFeedBySlug(queryString));

                feedSummary = ConvertRupeeUnicode(feedSummary);

                return feedSummary;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetNewsFeedBySlug Exception");
                objErr.LogException();
                return null;
            }
        }
        public List<ContentFeedSummary> GetNewsFeedBySubCategory(ContentFeedURI queryString)
        {
            try
            {
                var feedSummary = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetNewsFeedBySubCategory(queryString));

                feedSummary = ConvertRupeeUnicode(feedSummary);

                return feedSummary;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetNewsFeedBySubCategory Exception");
                objErr.LogException();
                return null;
            }
        }
        public List<ContentFeedSummary> GetAllNewsFeed(ContentFeedURI queryString)
        {
            try
            {
                var feedSummary = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetAllNewsFeed(queryString));

                feedSummary = ConvertRupeeUnicode(feedSummary);

                return feedSummary;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetAllNewsFeed Exception");
                objErr.LogException();
                return null;
            }
        }
        public CarModelMaskingResponse GetTaggedModelId(int basicId)
        {
            try
            {
                var result = GrpcMethods.GetTaggedModelId(basicId);
                return new CarModelMaskingResponse { ModelId = result.IntOutput};
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetTaggedModelId Exception");
                objErr.LogException();
                return null;
            }
        }

        public CarSynopsisEntity GetCarSynopsis(int modelId, int applicationId,int priority)
        {
            try
            {
                var synopsis = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetCarSynopsis(modelId, applicationId, priority));

                if (synopsis != null && synopsis.Description != null)
                    synopsis.Description = synopsis.Description.Replace("&#x20B9;", "₹");

                return synopsis;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetCarSynopsis Exception");
                objErr.LogException();
                return null;
            }
        }
        public bool ClearEditCMSCache(string type, int modelId, int makeId, int basicId, string url, int applicationId)
        {
            try
            {
               return GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.ClearMemCacheKey(type, modelId, makeId, basicId, url,applicationId));
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL ClearEditCMSCache Exception");
                objErr.LogException();
            }
            return false;
        }

        public List<TipsAndAdvicesEntity> TipsAdviceList(int SubCategoryId,int BasicId,int ApplicationId)
        {
            try
            {
                return new List<TipsAndAdvicesEntity>();
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetCarSynopsis Exception");
                objErr.LogException();
                return null;
            }
        }

        public List<ArticleSummary> GoogleSiteMapDetails(int applicationId)
        {
            try
            {
                var articles = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GoogleSiteMapDetails(applicationId));

                articles = ConvertRupeeUnicode(articles);

                return articles;
            }
            catch (Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GoogleSiteMapDetails Exception");
                objErr.LogException();
                return null;
            }
        }

        public ArticleSummary GetSponsoredArticle(string categoryIds, string sponsoredAuthorId, ArticleSummary article = null, bool makeApiCall = true)
        {
            try
            {
                if (makeApiCall)
                {
                    article = GrpcToCarwaleConvert.ConvertFromGrpcToCarwale(GrpcMethods.GetSponsoredArticle(categoryIds, sponsoredAuthorId));
                }

                if (article != null)
                {
                    if (article.Title != null)
                        article.Title = article.Title.Replace("&#x20B9;", "₹");
                    if (article.Description != null)
                        article.Description = article.Description.Replace("&#x20B9;", "₹");
                }

                return article;
            }
            catch(Exception err)
            {
                ExceptionHandler objErr = new ExceptionHandler(err, "CMSContentBL GetSponsoredArticle Exception");
                objErr.LogException();
                return null;
            }
        }
        
        public CMSContent ConvertRupeeUnicode(CMSContent content)
        {
            if (content != null && content.Articles != null && content.Articles.Count > 0)
            {
                foreach (var article in content.Articles)
                {
                    if (article.Title != null)
                        article.Title = article.Title.Replace("&#x20B9;", "₹");
                    if (article.Description != null)
                        article.Description = article.Description.Replace("&#x20B9;", "₹");
                }
            }
            return content;
        }

        public List<ArticleSummary> ConvertRupeeUnicode(List<ArticleSummary> articles)
        {
            if (articles != null && articles.Count > 0)
            {
                foreach (var article in articles)
                {
                    if (article.Title != null)
                        article.Title = article.Title.Replace("&#x20B9;", "₹");
                    if (article.Description != null)
                        article.Description = article.Description.Replace("&#x20B9;", "₹");
                }
            }
            return articles;
        }

        public List<ContentFeedSummary> ConvertRupeeUnicode(List<ContentFeedSummary> feedSummary)
        {
            if (feedSummary != null && feedSummary.Count > 0)
            {
                foreach (var feed in feedSummary)
                {
                    if (feed.Title != null)
                        feed.Title = feed.Title.Replace("&#x20B9;", "₹");
                    if (feed.Description != null)
                        feed.Description = feed.Description.Replace("&#x20B9;", "₹");
                    if (feed.Content != null)
                        feed.Content = feed.Content.Replace("&#x20B9;", "₹");
                }
            }
            return feedSummary;
        }

        public List<ArticleSummary> GetExpertReviewByModel(int modelId, ushort count, List<ArticleSummary> expertReviews = null)
        {
            try
            {
                if (expertReviews == null)
                {
                    var articleURI = new ArticleRecentURI()
                    {
                        ApplicationId = (ushort)CMSAppId.Carwale,
                        ContentTypes = ((int)CMSContentType.RoadTest).ToString(),
                        TotalRecords = count,
                        ModelId = modelId,
                    };
                    expertReviews = GetMostRecentArticles(articleURI);
                }
                if (expertReviews != null)
                {
                    foreach (var x in expertReviews)
                    {
                        x.FormattedDisplayDate = x.DisplayDate.ConvertDateToDays();
                    }
                }

                return expertReviews;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        private string InsertAdHtml(string content, string adHtml)
        {
            string completeContent = "";
            try
            {
                if (content != null)
                {
                    int totalStrippedHTMLLength = 0, requiredLength = 0;
                    
                    var tuple = StringHtmlHelpers.StripHtmlTagsWithLength(content);
                    totalStrippedHTMLLength += tuple.Item2;
                   
                    requiredLength = Convert.ToInt16(totalStrippedHTMLLength * 0.25);

                    if (!string.IsNullOrEmpty(adHtml))
                    {
                        completeContent = StringHtmlHelpers.InsertHTMLBetweenHTML(content, adHtml, requiredLength);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return completeContent;
        }

        
    }//class
}//namespace