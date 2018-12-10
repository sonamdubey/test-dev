using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.TipsAndAdvice;
using Carwale.Entity.CMS.URIs;
using System.Collections.Generic;

namespace Carwale.Interfaces.CMS.Articles
{
    /// <summary>
    /// created by ashish kamble
    /// </summary>
    public interface ICMSContent
    {
        /// <summary>
        /// Function to get list of articles by category id.
        /// written by ashish kamble
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application">type of application. e.g. bikewale or carwale</param>
        /// <param name="categotyIdList">Category of article. e.g. news roadtest, features etc.</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount">Returns total no of records</param>
        /// <param name="filters">If any filters are required to filter the article.</param>
        /// <returns></returns>
        CMSContent GetContentListByCategory(ArticleByCatURI queryString,bool fetchSponosored = true, ArticleSummary sponsoredArticle = null, CMSContent results = null, bool makeApiCall = true);

        /// <summary>
        /// Function to get the list of articles by subcategory. e.g. tips and advices.
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application">type of application. e.g. bikewale or carwale</param>
        /// <param name="subCategotyId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount">Returns total no of records</param>
        /// <param name="filters">If any filters are required to filter the article.</param>
        /// <returns></returns>
        CMSContent GetContentListBySubCategory(ArticleBySubCatURI queryString);

        /// <summary>
        /// Function to get list of articles by category id related to a basicId.
        /// written by ashish kamble
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application">type of application. e.g. bikewale or carwale</param>
        /// <param name="categotyIdList">Category of article. e.g. news roadtest, features etc.</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="recordCount">Returns total no of records</param>
        /// <param name="filters">If any filters are required to filter the article.</param>
        /// <returns></returns>
        List<ArticleSummary> GetRelatedListByCategory(ArticleByCatAndIdURI queryString);

        /// <summary>
        /// Function to get details of the article having single page by basic id. e.g. news
        /// </summary>
        /// <typeparam name="ArticleDetails"></typeparam>
        /// <param name="basicId"></param>
        /// <returns></returns>
        ArticleDetails GetContentDetails(ArticleContentURI queryString);

        /// <summary>
        /// Function to get details of the articles having multiple pages by basic id. e.g. roadtest.
        /// </summary>
        /// <typeparam name="ArticlePageDetails"></typeparam>
        /// <param name="basicId"></param>
        /// <returns></returns>
        ArticlePageDetails GetContentPages(ArticleContentURI queryString);


        /// <summary>
        /// Function to get the list of featured articles. It can be one or multiple type of articles depending on contenttypes passed to the function.
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application"></param>
        /// <param name="contentTypes">list containing type of content. e.g. news roadtest. Data returned will be of given content type.</param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        List<ArticleSummary> GetFeaturedArticles(ArticleFeatureURI queryString);

        /// <summary>
        /// Function to get the list of recent articles. It can be one or multiple type of articles depending on contenttypes passed to the function.
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application"></param>
        /// <param name="contentTypes">list containing type of content. e.g. news roadtest. Data returned will be of given content type.</param>
        /// <param name="totalRecords"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        List<ArticleSummary> GetMostRecentArticles(ArticleRecentURI queryString, List<ArticleSummary> articles = null);


        /// <summary>
        /// Function to get the list of related articles. It can be one or multiple type of articles depending on contenttypes passed to the function.
        /// </summary>
        /// <typeparam name="ArticleSummary"></typeparam>
        /// <param name="application"></param>
        /// <param name="tag"></param>
        /// <param name="contentTypes">List containing type of content. e.g. news roadtest. Data returned will be of given content type.</param>
        /// <param name="Tags"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        List<ArticleSummary> GetRelatedArticles(ArticleRelatedURI queryString, List<ArticleSummary> articles = null);

        /// <summary>
        /// created by natesh on 13/11/14
        /// interface for getting makename and makeid of expertreviews available
        /// </summary>
        /// <param name="querystring"></param>
        /// <returns></returns>
        List<MakeAndModelDetail> GetMakeDetails(MakeDetailURI queryString);

        /// <summary>
        /// created by natesh on 13/11/14
        /// interface for getting modelname and modelid of expertreviews available
        /// </summary>
        /// <param name="querystring"></param>
        /// <returns></returns>
        List<MakeAndModelDetail> GetModelDetails(ModelDetailURI queryString);

        /// <summary>
        /// created by sachin bharti on 9/2/15
        /// interfce for getting content list meta tags details from XML file based on cms category id
        /// </summary>
        /// <param name="cmsCategoryId"></param>
        /// <returns></returns>
        MetaTags GetContentMetaTags(int cmsCategoryId);

        /// <summary>
        /// created by sachin bharti on 9/25/15
        /// interfce for getting cms subcategories
        /// </summary>
        /// <param name="cmsCategoryId"></param>
        /// <returns></returns>
        List<CMSSubCategory> GetCMSSubCategories(int cmsCategoryId);

        int GetCMSRoadTestCount(int makeId, int modelId, int versionId, int topCount, int applicationId);

        void QueueUpdateView(int basicId);



        List<RelatedArticles> GetRelatedContent(int basicId);
        List<RelatedArticles> GetTopArticlesByBasicId(int basicId);
        List<CMSSubCategoryV2> GetContentSegment(CommonURI queryString, int sourceId, bool getAllMedia = false);
        List<ContentFeedSummary> GetNewsFeedBySlug(ContentFeedURI queryString);
        List<ContentFeedSummary> GetNewsFeedBySubCategory(ContentFeedURI queryString);
        List<ContentFeedSummary> GetAllNewsFeed(ContentFeedURI queryString);

        CarSynopsisEntity GetCarSynopsis(int modelId, int applicationId, int priority = 0);
        /// <summary>
        /// to get the tagged model from basicId
        /// written Meet Shah on 29-12-2016
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        CarModelMaskingResponse GetTaggedModelId(int basicId);
        /// <summary>
        /// to clear the cache for editcms
        /// </summary>
        /// <param name="type"></param>
        /// <param name="modelId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        bool ClearEditCMSCache(string type, int modelId, int makeId, int basicId, string url, int applicationId);

        /// <summary>
        ///  to get nextPageUrl and prevPageUrl in TipsAndAdvice Appwebapi
        /// </summary>
        /// <param name="SubCategoryId"></param>
        /// <param name="BasicId"></param>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        List<TipsAndAdvicesEntity> TipsAdviceList(int SubCategoryId, int BasicId, int ApplicationId);

        List<ArticleSummary> GoogleSiteMapDetails(int applicationId);

        /// <summary>
        /// To get sponsored article. This article should be marked featured in opr and sponsored author id should read from web.config
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="sponsoredAuthorId"></param>
        /// <returns></returns>
        ArticleSummary GetSponsoredArticle(string categoryIds, string sponsoredAuthorId, ArticleSummary article = null, bool makeApiCall = true);

        List<ArticleSummary> GetExpertReviewByModel(int modelId, ushort count, List<ArticleSummary> expertReviews = null);
    }
}          