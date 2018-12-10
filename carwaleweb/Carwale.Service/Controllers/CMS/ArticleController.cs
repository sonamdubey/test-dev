using AutoMapper;
using Carwale.DTOs.CMS;
using Carwale.DTOs.CMS.Articles;
using Carwale.Entity;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Enum;
using Carwale.Interfaces.CMS.Articles;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Service.Filters.CMS;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;


namespace Carwale.Service.Controllers.CMS
{
    public class ArticleController : ApiController
    {
        private readonly ICMSContent _cmsContentCacheRepo;
        private readonly IUnityContainer _container;
        private const string _newsCategory = "1,2,6,8,19,22,26";
        private const string _ExpertReviewCategory = "2,8";
        private static string _hostUrl = ConfigurationManager.AppSettings["WebApiHostUrl"].Replace("api/", "") ?? string.Empty;

        public ArticleController(IUnityContainer container, ICMSContent cmsContentCacheRepo)
        {
            _cmsContentCacheRepo = cmsContentCacheRepo;
            _container = container;
        }

        /// <summary>
        /// api to show list by  list of categoryids 
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <returns></returns>
        [HttpGet, CMSArticleByCategoryValidator] //action filter for validation of querystring with categoryidlist
        public IHttpActionResult ListByCategory([FromUri]ArticleByCatURI queryString)
        {
            ICollection<EntityTagHeaderValue> etagsFromClient = Request.Headers.IfNoneMatch;
            if (queryString.PageNo > 0 && queryString.PageSize > 0)
            {
                queryString.StartIndex = queryString.PageSize * queryString.PageNo - queryString.PageSize + 1;
                queryString.EndIndex = queryString.PageSize * queryString.PageNo;
            }
            ArticleByCatURI pageQS = JsonConvert.DeserializeObject<ArticleByCatURI>(JsonConvert.SerializeObject(queryString));
            var content = _cmsContentCacheRepo.GetContentListByCategory(pageQS);

            if (content == null)//|| content.RecordCount <= 0)
                return NotFound();

            var tag = "\"" + (content.Articles.Count > 0 ? content.Articles[0].BasicId.ToString() : "0") + "\""; // ETag must be a quoted string.

            EntityTagHeaderValue etag = new EntityTagHeaderValue(tag);

            string referrerHost = string.Empty;
            if (this.Request.Headers.Referrer != null)
                referrerHost = this.Request.Headers.Referrer.Host ?? string.Empty;

            if (etagsFromClient.Count > 0 && etagsFromClient.Any(t => t.Tag == etag.Tag) && referrerHost.IndexOf("carwale.com") >= 0)
            {
                return StatusCode(HttpStatusCode.NotModified);
            }
            else
            {
                HttpContext.Current.Response.Headers.Add("ETag", tag);
            }
            var objDTOArticles = new DTOs.CMS.Articles.CMSContentDTOV2();
            queryString.CategoryIdList = string.IsNullOrEmpty(queryString.CategoryIdList) ? _newsCategory : queryString.CategoryIdList;
            objDTOArticles = Mapper.Map<Carwale.Entity.CMS.Articles.CMSContent, Carwale.DTOs.CMS.Articles.CMSContentDTOV2>(content);
            if (objDTOArticles != null && queryString.PageNo > 0 && queryString.PageSize > 0 && queryString.EndIndex < objDTOArticles.RecordCount)
                objDTOArticles.NextPageUrl = ConfigurationManager.AppSettings["WebApiHostUrl"].Replace("api/", "") + "api/article/ListByCategory/?categoryidlist=" + (queryString.CategoryIdList) + "&applicationid=" + (queryString.ApplicationId) + "&pageNo=" + (queryString.PageNo + 1) + "&pageSize=" + queryString.PageSize + (queryString.ModelId > 0 ? string.Format("&modelId={0}", queryString.ModelId) : string.Empty) + (queryString.MakeId > 0 ? string.Format("&makeId={0}", queryString.MakeId) : string.Empty);
            else
                objDTOArticles.NextPageUrl = "";

            string SourceId = Request.Headers.Contains("SourceId") ? Request.Headers.GetValues("SourceId").FirstOrDefault() : null;
            string appVersion = Request.Headers.Contains("appVersion") ? Request.Headers.GetValues("appVersion").FirstOrDefault() : null;

            //swap formatted datetime for ios for appversion 20 n below
            if (!string.IsNullOrWhiteSpace(SourceId) && !string.IsNullOrWhiteSpace(appVersion) && RegExValidations.IsPositiveNumber(SourceId) && RegExValidations.IsPositiveNumber(appVersion)
                && Convert.ToInt32(SourceId) == 83 && Convert.ToInt32(appVersion) <= 21)
            {
                string temp = string.Empty;
                foreach (var article in objDTOArticles.Articles)
                {
                    temp = article.FormattedDisplayDate;
                    article.FormattedDisplayDate = article.DisplayDate;
                    article.DisplayDate = temp;
                }
            }

            return Ok(objDTOArticles);
        }

        [Route("api/content/list/")]
        [HttpGet, CMSArticleByCategoryValidator] //action filter for validation of querystring with categoryidlist
        public IHttpActionResult ListByCategoryV2([FromUri]ArticleByCatURI queryString)
        {
            ICollection<EntityTagHeaderValue> etagsFromClient = Request.Headers.IfNoneMatch;
            if (queryString.PageNo > 0 && queryString.PageSize > 0)
            {
                queryString.StartIndex = queryString.PageSize * queryString.PageNo - queryString.PageSize + 1;
                queryString.EndIndex = queryString.PageSize * queryString.PageNo;
            }
            ArticleByCatURI pageQS = JsonConvert.DeserializeObject<ArticleByCatURI>(JsonConvert.SerializeObject(queryString));
            var content = _cmsContentCacheRepo.GetContentListByCategory(pageQS);

            if (content == null)//|| content.RecordCount <= 0)
                return NotFound();

            var tag = "\"" + (content.Articles.Count > 0 ? content.Articles[0].BasicId.ToString() : "0") + "\""; // ETag must be a quoted string.

            EntityTagHeaderValue etag = new EntityTagHeaderValue(tag);

            string referrerHost = string.Empty;
            if (this.Request.Headers.Referrer != null)
                referrerHost = this.Request.Headers.Referrer.Host ?? string.Empty;

            if (etagsFromClient.Count > 0 && etagsFromClient.Any(t => t.Tag == etag.Tag) && referrerHost.IndexOf("carwale.com") >= 0)
            {
                return StatusCode(HttpStatusCode.NotModified);
            }
            else
            {
                HttpContext.Current.Response.Headers.Add("ETag", tag);
            }
            var objDTOArticles = new DTOs.CMS.Articles.CMSContentDTOV3();

            objDTOArticles = Mapper.Map<Carwale.Entity.CMS.Articles.CMSContent, Carwale.DTOs.CMS.Articles.CMSContentDTOV3>(content);
            if (objDTOArticles != null && queryString.PageNo > 0 && queryString.PageSize > 0 && queryString.EndIndex < objDTOArticles.RecordCount)
                objDTOArticles.NextPageUrl = ConfigurationManager.AppSettings["WebApiHostUrl"].Replace("api/", "") + "api/content/list/?categoryidlist=" + (queryString.CategoryIdList) + "&applicationid=" + (queryString.ApplicationId) + "&pageNo=" + (queryString.PageNo + 1) + "&pageSize=" + queryString.PageSize + (queryString.ModelId > 0 ? string.Format("&modelId={0}", queryString.ModelId) : string.Empty) + (queryString.MakeId > 0 ? string.Format("&makeId={0}", queryString.MakeId) : string.Empty);
            else
                objDTOArticles.NextPageUrl = "";

            return Ok(objDTOArticles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        [Route("api/content/list/related/{basicId}/")]
        [HttpGet] //action filter for validation of querystring with categoryidlist
        public IHttpActionResult RelatedListV2([FromUri]ArticleByCatAndIdURI queryString, int basicId)
        {
            if (queryString.BasicId < 1) return BadRequest("basicid missing");
            ICollection<EntityTagHeaderValue> etagsFromClient = Request.Headers.IfNoneMatch;
            if (queryString.PageNo > 0 && queryString.PageSize > 0)
            {
                queryString.StartIndex = queryString.PageSize * queryString.PageNo - queryString.PageSize + 1;
                queryString.EndIndex = queryString.PageSize * queryString.PageNo;
            }
            queryString.CategoryIdList = string.IsNullOrEmpty(queryString.CategoryIdList) ? _newsCategory : queryString.CategoryIdList;
            ArticleByCatAndIdURI pageQS = JsonConvert.DeserializeObject<ArticleByCatAndIdURI>(JsonConvert.SerializeObject(queryString));
            var content = _cmsContentCacheRepo.GetRelatedListByCategory(pageQS);

            if (content == null)//|| content.RecordCount <= 0)
                return NotFound();

            var tag = "\"" + (content.Count > 0 ? content[0].BasicId.ToString() : "0") + "\""; // ETag must be a quoted string.

            EntityTagHeaderValue etag = new EntityTagHeaderValue(tag);

            string referrerHost = string.Empty;
            if (this.Request.Headers.Referrer != null)
                referrerHost = this.Request.Headers.Referrer.Host ?? string.Empty;

            if (etagsFromClient.Count > 0 && etagsFromClient.Any(t => t.Tag == etag.Tag) && referrerHost.IndexOf("carwale.com") >= 0)
            {
                return StatusCode(HttpStatusCode.NotModified);
            }
            else
            {
                HttpContext.Current.Response.Headers.Add("ETag", tag);
            }
            RelatedArticlesDTOV2 objDTOArticles = new RelatedArticlesDTOV2();

            objDTOArticles.RelatedArticles = Mapper.Map<List<Carwale.Entity.CMS.Articles.ArticleSummary>, List<ArticleSummaryDTOV4>>(content);
            objDTOArticles.Header = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["relatedheader"]) ? "Related News" : ConfigurationManager.AppSettings["relatedheader"].Substring(0, 20);
            if (objDTOArticles != null && objDTOArticles.RelatedArticles != null && queryString.PageNo > 0 && queryString.PageSize > 0 && objDTOArticles.RelatedArticles.Count > queryString.PageSize-1)
                objDTOArticles.NextPageUrl = ConfigurationManager.AppSettings["WebApiHostUrl"].Replace("api/", "") + "api/content/list/related/" + basicId + "/?categoryidlist=" + (queryString.CategoryIdList??"") + "&applicationid=" + (queryString.ApplicationId) + "&pageNo=" + (queryString.PageNo + 1) + "&pageSize=" + queryString.PageSize;
            else
                objDTOArticles.NextPageUrl = "";

            return Ok(objDTOArticles);
        }

        ///<summary>
        ///api to show list by subcategory id
        ///written by Natesh Kumar on 20/9/14
        ///</summary>
        ///<param name="queryString"></param>
        ///<returns></returns>
        [HttpGet, CMSArticleBySubCatValidator] //action filter for validation of querystring with subcategoryid
        public IHttpActionResult ListBySubCategory([FromUri]ArticleBySubCatURI queryString)
        {
            var content = _cmsContentCacheRepo.GetContentListBySubCategory(queryString);

            if (content == null || content.RecordCount <= 0)
                return NotFound();

            DTOs.CMS.Articles.CMSContent objDTOArticles = Mapper.Map<Carwale.Entity.CMS.Articles.CMSContent, Carwale.DTOs.CMS.Articles.CMSContent>(content);

            return Ok(objDTOArticles);
        }

        /// <summary>
        /// api to show content details with basic id
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ContentDetail([FromUri]ArticleContentURI queryString)
        {
            if (queryString.BasicId == 0)
            {
                return BadRequest();
            }

            var content = _cmsContentCacheRepo.GetContentDetails(queryString);

            if (content == null || content.BasicId == 0)
            {
                return NotFound();
            }

            var objDTOArticlesDetails = Mapper.Map<Carwale.Entity.CMS.Articles.ArticleDetails, Carwale.DTOs.CMS.Articles.ArticleDetails>(content);
            if (objDTOArticlesDetails == null)
                return InternalServerError();

            return Ok(objDTOArticlesDetails);
        }

        /// <summary>
        /// api to get contentpage with basicid
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ContentPageDetail([FromUri]ArticleContentURI queryString)
        {
            if (queryString.BasicId == 0)
            {
                return BadRequest();
            }

            var content = _cmsContentCacheRepo.GetContentPages(queryString);

            if (content == null || content.BasicId == 0)
                return NotFound();

            DTOs.CMS.Articles.ArticlePageDetails objDTOArticlePage = Mapper.Map<Carwale.Entity.CMS.Articles.ArticlePageDetails, Carwale.DTOs.CMS.Articles.ArticlePageDetails>(content);

            return Ok(objDTOArticlePage);
        }

        /// <summary>
        /// api to get contentpage with basicid
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet, Route("api/v1/article/content/")]
        public IHttpActionResult ContentPageDetail_V1([FromUri]ArticleContentURI queryString)
        {
            if (queryString.BasicId == 0)
            {
                return BadRequest();
            }

            var content = _cmsContentCacheRepo.GetContentPages(queryString);

            if (content == null || content.BasicId == 0)
                return NotFound();

            var objDTOArticlePage = new DTOs.CMS.Articles.ArticlePageDetails_V1();


            objDTOArticlePage = Mapper.Map<Carwale.Entity.CMS.Articles.ArticlePageDetails, Carwale.DTOs.CMS.Articles.ArticlePageDetails_V1>(content);

            return Ok(objDTOArticlePage);
        }

        /// <summary>
        ///  api to get mostrecentarticle list
        ///  written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet, CMSArticleApiValidator] // actionfilter for querystring validator
        public IHttpActionResult MostRecentList([FromUri]ArticleRecentURI queryString)
        {
            var content = _cmsContentCacheRepo.GetMostRecentArticles(queryString);

            if (content == null || content.Count <= 0)
                return NotFound();

            var objDTOArticleSummary = Mapper.Map<List<Carwale.Entity.CMS.Articles.ArticleSummary>, List<Carwale.DTOs.CMS.Articles.ArticleSummary>>(content);

            return Ok(objDTOArticleSummary);
        }

        /// <summary>
        /// api to get featured articles list
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet, CMSArticleApiValidator] //actionfilter for querystring validator
        public IHttpActionResult FeaturedList([FromUri]ArticleFeatureURI queryString)
        {
            var content = _cmsContentCacheRepo.GetFeaturedArticles(queryString);

            if (content == null || content.Count <= 0)
                return NotFound();


            var objDTOArticleSummary = Mapper.Map<List<Carwale.Entity.CMS.Articles.ArticleSummary>, List<Carwale.DTOs.CMS.Articles.ArticleSummary>>(content);

            return Ok(objDTOArticleSummary);
        }

        /// <summary>
        /// api to get related article list by tags
        /// written by Natesh Kumar on 20/9/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>

        [HttpGet, CMSArticleApiValidator] // actionfilter for querystring validator
        public IHttpActionResult RelatedList([FromUri] ArticleRelatedURI queryString)
        {
            var content = _cmsContentCacheRepo.GetRelatedArticles(queryString);

            if (content == null || content.Count <= 0)
                return NotFound();


            var objDTOArticleSummary = Mapper.Map<List<Carwale.Entity.CMS.Articles.ArticleSummary>, List<Carwale.DTOs.CMS.Articles.ArticleSummary>>(content);
            return Ok(objDTOArticleSummary);
        }

        /// <summary>
        /// api to get list of makes and makeids for which expert review are available
        /// wrritten by natesh kumar on 13/11/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>

        [HttpGet, CMSArticleByCategoryValidator]
        public IHttpActionResult MakeList([FromUri] MakeDetailURI queryString)
        {
            var content = _cmsContentCacheRepo.GetMakeDetails(queryString);

            if (content == null || content.Count <= 0)
                return NotFound();

            var objDTOModelDetails = Mapper.Map<List<Carwale.Entity.CMS.Articles.MakeAndModelDetail>, List<Carwale.DTOs.CMS.Articles.MakeAndModelDetail>>(content);

            return Ok(objDTOModelDetails);
        }

        /// <summary>
        /// api to get list of models and modelids for which expert review are available
        /// wrritten by natesh kumar on 13/11/14
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>

        [HttpGet, CMSModelDetailValidator]
        public IHttpActionResult ModelListByMake([FromUri] ModelDetailURI queryString)
        {
            var content = _cmsContentCacheRepo.GetModelDetails(queryString);

            if (content == null || content.Count <= 0)
                return NotFound();

            var objDTOModelDetails = Mapper.Map<List<Carwale.Entity.CMS.Articles.MakeAndModelDetail>, List<Carwale.DTOs.CMS.Articles.MakeAndModelDetail>>(content);

            return Ok(content);
        }

        /// <summary>
        ///  api to get latest 3 article gist for new menu
        ///  written by Meet Shah on 5/8/16
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet, CMSArticleByCategoryValidator] // actionfilter for querystring validator
        public IHttpActionResult LatestArticlesGist([FromUri]ArticleByCatURI queryString)
        {
            ICollection<EntityTagHeaderValue> etagsFromClient = Request.Headers.IfNoneMatch;
            var content = _cmsContentCacheRepo.GetContentListByCategory(queryString,false);

            if (content == null || content.RecordCount <= 0 || content.Articles.Count <= 0)
            {
                return NotFound();
            }

            var tag = "\"" + string.Join("",content.Articles.Select(x => x.BasicId)) + "\""; // ETag must be a quoted string.

            EntityTagHeaderValue etag = new EntityTagHeaderValue(tag);

            if (etagsFromClient.Count > 0 && etagsFromClient.Any(t => t.Tag == etag.Tag))
            {
                return StatusCode(HttpStatusCode.NotModified);
            }
            else
            {
                var objDTOArticleGist = Mapper.Map<List<Carwale.Entity.CMS.Articles.ArticleSummary>, List<Carwale.DTOs.CMS.Articles.ArticleGist>>(content.Articles.ToList());
                HttpContext.Current.Response.Headers.Add("ETag", tag);
                return Ok(objDTOArticleGist);
            }
        }

        /// <summary>
        /// api to get content segment
        /// written by Jitendra singh on 10/10/16
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpGet, Route("api/content/segment"), Route("api/v1/content/segment"), Route("api/v2/content/segment")]
        public IHttpActionResult GetContentSegment([FromUri]CommonURI queryString, bool getAllMedia = false)
        {
            if (Request.Headers.Contains("sourceId"))
            {
                int sourceId = Convert.ToInt32(Request.Headers.GetValues("sourceId").First());
                var content = _cmsContentCacheRepo.GetContentSegment(queryString, sourceId, getAllMedia);
                if (content == null || content.Count <= 0)
                    return NotFound();
                var contentSegments = Mapper.Map<List<CMSSubCategoryV2>, List<ContentSegmentDTO>>(content);
                var contentApiPath = (sourceId == (int)Platform.CarwaleAndroid) ? "content/list" : "article/ListByCategory";

                contentSegments.ForEach(c =>
                {
                    string categoryList = c.CategoryId.ToString();
                    if (c.CategoryId == (int)CMSContentType.News)
                    { categoryList = _newsCategory; }
                    else if (c.CategoryId == (int)CMSContentType.ComparisonTests) 
                    { categoryList = _ExpertReviewCategory; }
                    if (c.CategoryId == (int)CMSContentType.Images || c.CategoryId == (int)CMSContentType.Videos)
                    {
                        c.Url = string.Format("{0}api/media/?categoryidlist={1}&applicationid={2}&pageNo=1&pageSize=10{3}{4}{5}",
                            _hostUrl, categoryList, (int)(Application.CarWale),
                            (queryString != null && queryString.MakeId > 0) ? "&makeid=" + queryString.MakeId.ToString() : string.Empty,
                            (queryString != null && queryString.ModelId > 0) ? "&modelid=" + queryString.ModelId.ToString() : string.Empty,
                            getAllMedia ? "&getAllMedia=true" : string.Empty);
                    }
                    else
                    {
                        c.Url = string.Format("{0}api/{1}/?categoryidlist={2}&applicationid={3}&pageNo=1&pageSize=10{4}{5}", 
                            _hostUrl, contentApiPath, categoryList, (int)(Application.CarWale), 
                            (queryString != null && queryString.MakeId > 0) ? "&makeid=" + queryString.MakeId.ToString() : string.Empty,
                            (queryString != null && queryString.ModelId > 0) ? "&modelid=" + queryString.ModelId.ToString() : string.Empty);
                    }
                });

                if ((HttpContext.Current.Request.Path == "/api/content/segment/" && sourceId == (int)Platform.CarwaleiOS) ||
                    (HttpContext.Current.Request.Path == "/api/v1/content/segment/" && sourceId == (int)Platform.CarwaleAndroid))
                {
                    contentSegments = contentSegments.Where((item) => item.CategoryId != (int)CMSContentType.Images && item.CategoryId != (int)CMSContentType.Videos).ToList();
                }
                return Ok(contentSegments);
            }
            else
            {
                return BadRequest("sourceid is missing");
            }
        }

        /// <summary>
        //This is a generic api which is implemented for the purpose to serve all type of articles by id and categoryname
        ///  Ajay Singh on 06 oct 2016
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpGet, Route("api/content/{categoryName}/{id:int:min(1)}")]
        public IHttpActionResult GetContent(string categoryName, int id)
        {
            try
            {
                EnumGenericContentType categoryId = EnumGenericContentType.news;
                if (!Enum.TryParse<EnumGenericContentType>(categoryName.Replace("-", "").ToLower(), out categoryId))
                {
                    return BadRequest();
                }

                GenericContentDetailDTO contentPage = new GenericContentDetailDTO();
                _container.RegisterInstance<int>(Convert.ToInt32(id));
                _container.RegisterInstance<EnumGenericContentType>(categoryId);
                IServiceAdapter genArticleAdapter = _container.Resolve<IServiceAdapter>("GenericContentDetailAdaptor");
                contentPage = genArticleAdapter.Get<GenericContentDetailDTO>();
                if (contentPage != null)
                    return Ok(contentPage);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "ArticleController.GetContent()");
                objErr.LogException();
            }
            return InternalServerError();
        }


        /// <summary>
        /// Written by Ajay Singh on 30 sep 2016
        /// Purpose of this action method is to fetch a sequence of related articles by basic id.
        /// This is a generic method which return the sequece in a cycle like news-video-expertreview-video...
        /// If there is not any related article for specific basicid then it will return those top articles of same categoryid which are recently addde(oredr by dispaly date)
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>

        [HttpGet, Route("api/content/related/{id:int:min(1)}")]
        public IHttpActionResult GetRelatedContents(int id)
        {
            var content = _cmsContentCacheRepo.GetRelatedContent(id);
            if (content == null || content.Count <= 0)
                return BadRequest();
            var result = Mapper.Map<List<Carwale.Entity.CMS.Articles.RelatedArticles>, List<Carwale.DTOs.CMS.Articles.RelatedArticlesDTO>>(content);
            return Ok(result);

        }

        [HttpGet, Route("api/cms/clearcache/")]
        public IHttpActionResult ClearCache(string type, int modelId = 0, int makeId = 0, int basicId = 0, string articleUrl = "", int applicationId=0)
        {
            return Ok(_cmsContentCacheRepo.ClearEditCMSCache(type, modelId, makeId, basicId, articleUrl, applicationId));
        }

    }

}