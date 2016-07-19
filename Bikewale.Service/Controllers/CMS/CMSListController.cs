using Bikewale.BAL.EditCMS;
using Bikewale.Cache.Core;
using Bikewale.Cache.News;
using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.News;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.CMS;
using Bikewale.Service.Utilities;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.CMS
{

    /// <summary>
    /// Edit CMS List Controller :  Operations related to list of content 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class CMSListController : CompressionApiController//ApiController
    {
        string _applicationid = Utility.BWConfiguration.Instance.ApplicationId;

        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);
        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);
        static readonly ILog _logger = LogManager.GetLogger(typeof(CMSListController));

        private readonly IPager _pager = null;
        public CMSListController(IPager pager)
        {
            _pager = pager;
        }

        #region List Recent Categories Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of a specified category.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>      
        /// <param name="posts">No of records needed. should be greater than 0.</param>
        /// <returns>Recent Articles List Summary</returns>
        [ResponseType(typeof(IEnumerable<CMSArticleSummary>)), Route("api/cms/cat/{categoryId}/posts/{posts}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, uint posts)
        {
            try
            {
                List<ArticleSummary> objRecentArticles = GetArticlesViaGrpc(categoryId, posts, null, null);

                if (objRecentArticles != null && objRecentArticles.Count > 0)
                {

                    List<CMSArticleSummary> objCMSRArticles = new List<CMSArticleSummary>();
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);
                    objRecentArticles.Clear();
                    objRecentArticles = null;
                    objCMSRArticles = new CMSShareUrl().GetShareUrl(objCMSRArticles);
                    return Ok(objCMSRArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }  //get 
        #endregion

        #region List Recent Categories Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of a specified category for a particular make or model
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>
        /// <param name="posts">No of records needed. should be greater than 0.</param>
        /// <param name="makeId">Mandetory field.</param>
        /// <param name="modelId">Optional.</param>        
        /// <returns>Recent Articles List Summary</returns>
        [ResponseType(typeof(IEnumerable<CMSArticleSummary>)), Route("api/cms/cat/{categoryId}/posts/{posts}/make/{makeId}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, uint posts, string makeId, string modelId = null)
        {
            try
            {
                List<ArticleSummary> objRecentArticles = GetArticlesViaGrpc(categoryId, posts, makeId, modelId);

                if (objRecentArticles != null && objRecentArticles.Count > 0)
                {
                    List<CMSArticleSummary> objCMSRArticles;
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);

                    objRecentArticles.Clear();
                    objRecentArticles = null;
                    objCMSRArticles = new CMSShareUrl().GetShareUrl(objCMSRArticles);
                    return Ok(objCMSRArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }  //get 


        //Modified By Vivek Gupta on 19-07-2016, added caching for grpc method call
        private List<ArticleSummary> GetArticlesViaGrpc(EnumCMSContentType categoryId, uint posts, string makeId, string modelId)
        {
            List<ArticleSummary> _objArticleList = null;

            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArticles, Articles>();
                    IArticles _articles = container.Resolve<IArticles>();
                    _objArticleList = (List<ArticleSummary>)_articles.GetArticlesViaGrpc(categoryId, posts, Convert.ToUInt32(makeId), Convert.ToUInt32(modelId));
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return _objArticleList;
        }

        #endregion


        #region List Category Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of specified category. This api returns data for given page number.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>        
        /// <param name="posts">No of records per page. Should be greater than 0.</param>
        /// <param name="pageNumber">page number for which data is required.</param>
        /// <returns>Category Content List</returns>
        [ResponseType(typeof(IEnumerable<Bikewale.DTO.CMS.Articles.CMSContent>)), Route("api/cms/cat/{categoryId}/posts/{posts}/pn/{pageNumber}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, int posts, int pageNumber)
        {
            Bikewale.Entities.CMS.Articles.CMSContent objFeaturedArticles = null;
            try
            {
                int startIndex = 0, endIndex = 0;
                _pager.GetStartEndIndex(Convert.ToInt32(posts), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                objFeaturedArticles = GetNewsViaGrpc(categoryId, startIndex, endIndex);

                if (objFeaturedArticles != null && objFeaturedArticles.Articles.Count > 0)
                {
                    Bikewale.DTO.CMS.Articles.CMSContent objCMSFArticles = new Bikewale.DTO.CMS.Articles.CMSContent();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);

                    objFeaturedArticles.Articles.Clear();
                    objFeaturedArticles.Articles = null;
                    objCMSFArticles = new CMSShareUrl().GetShareUrl(objCMSFArticles);
                    return Ok(objCMSFArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }

        private Entities.CMS.Articles.CMSContent GetNewsViaGrpc(EnumCMSContentType categoryId, int startIndex, int endIndex, int makeId = 0, int modelId = 0)
        {
            Entities.CMS.Articles.CMSContent objNews = null;
            try
            {
                string contentTypeList;
                switch (categoryId)
                {
                    case EnumCMSContentType.RoadTest:
                        contentTypeList = (int)categoryId + "," + (int)EnumCMSContentType.ComparisonTests;
                        break;

                    case EnumCMSContentType.News:
                        contentTypeList = (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
                        break;
                    default:
                        contentTypeList = ((int)categoryId).ToString();
                        break;

                }

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<INewsCache, NewsCache>()
                    .RegisterType<ICacheManager, MemcacheManager>()
                    .RegisterType<INews, Bikewale.BAL.News.News>();
                    INewsCache _objNews = container.Resolve<INewsCache>();
                    objNews = _objNews.GetNews(startIndex, endIndex, contentTypeList, modelId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return objNews;
        }

        //get 
        #endregion


        #region List Category Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of specified category for the given make or model. This api return data for given page number.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>
        /// <param name="makeId">Mandatory parameter.</param>
        /// <param name="modelId">Optional parameter.</param>        
        /// <param name="posts">No of records per page. Should be greater than 0.</param>
        /// <param name="pageNumber">page number for which data is required.</param>
        /// <returns>Category Content List</returns>
        [ResponseType(typeof(IEnumerable<Bikewale.DTO.CMS.Articles.CMSContent>)), Route("api/cms/cat/{categoryId}/posts/{posts}/pn/{pageNumber}/make/{makeId}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, int posts, int pageNumber, string makeId, string modelId = null)
        {
            Bikewale.Entities.CMS.Articles.CMSContent objFeaturedArticles = null;
            try
            {
                int startIndex = 0, endIndex = 0;
                _pager.GetStartEndIndex(Convert.ToInt32(posts), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                int intMakeId = string.IsNullOrEmpty(makeId) ? 0 : Convert.ToInt32(makeId);
                int intModelId = string.IsNullOrEmpty(modelId) ? 0 : Convert.ToInt32(modelId);

                objFeaturedArticles = GetNewsViaGrpc(categoryId, startIndex, endIndex, intMakeId, intModelId);

                if (objFeaturedArticles != null && objFeaturedArticles.Articles.Count > 0)
                {
                    Bikewale.DTO.CMS.Articles.CMSContent objCMSFArticles = new Bikewale.DTO.CMS.Articles.CMSContent();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);

                    if (objFeaturedArticles.Articles != null)
                    {
                        objFeaturedArticles.Articles.Clear();
                        objFeaturedArticles.Articles = null;
                    }

                    objCMSFArticles = new CMSShareUrl().GetShareUrl(objCMSFArticles);
                    return Ok(objCMSFArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }  //get 
        #endregion
    }   // class
}   // namespace
