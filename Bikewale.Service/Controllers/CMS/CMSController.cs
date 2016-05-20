using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.CMS.Photos;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.CMS;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.CMS
{
    /// <summary>
    /// Edit CMS Controller :  All Edit CMS related Operations 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class CMSController : CompressionApiController//ApiController
    {
        string _applicationid = Utility.BWConfiguration.Instance.ApplicationId;

        private readonly IPager _pager = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        public CMSController(IPager pager)
        {
            _pager = pager;
        }


        #region ModelImages List Api
        /// <summary>
        /// Modified By : Ashish G. Kamble.
        /// Summary : API to get list of photos for the specified model.
        /// </summary>
        /// <param name="modelId">Mandatory field. Value should be greater than 0.</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSModelImageBase>)), Route("api/cms/photos/model/{modelId}")]
        public IHttpActionResult Get(int modelId)
        {
            List<EnumCMSContentType> categorList = null;
            List<ModelImage> objImageList = null;

            try
            {
                categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.PhotoGalleries);
                categorList.Add(EnumCMSContentType.ComparisonTests);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                categorList = null;

                string _apiUrl = String.Format("/webapi/image/modelphotolist/?applicationid={0}&modelid={1}&categoryidlist={2}", _applicationid, modelId, contentTypeList);

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objImageList = objClient.GetApiResponseSync<List<ModelImage>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objImageList);
                }

                if (objImageList != null && objImageList.Count > 0)
                {
                    // Auto map the properties
                    List<CMSModelImageBase> objCMSModels = new List<CMSModelImageBase>();
                    objCMSModels = CMSMapper.Convert(objImageList);

                    objImageList.Clear();
                    objImageList = null;

                    return Ok(objCMSModels);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();

                return InternalServerError();

            }
            return NotFound();
        }  //get  ModelImages 
        #endregion


        #region Other Model Images List Api
        /// <summary>
        /// Modified By : Ashish G. Kamble.
        /// Summary : API to get list of models with main image of each model for the same make as of specified model.
        /// </summary>
        /// <param name="modelId">Mandatory.</param>
        /// <param name="posts">Mandatory. No of records on each page.</param>
        /// <param name="pageNumber">Page number for which records are required.</param>        
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSImageList>)), Route("api/cms/photos/othermodels/modelId/{modelId}/posts/{posts}/pn/{pageNumber}/")]
        public IHttpActionResult GetOtherModelsPhotos(int modelId, int posts, int pageNumber)
        {
            int startIndex = 0, endIndex = 0;
            CMSImage objPhotos = null;
            List<EnumCMSContentType> categorList = null;

            try
            {
                categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.PhotoGalleries);
                categorList.Add(EnumCMSContentType.ComparisonTests);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                categorList = null;

                _pager.GetStartEndIndex(Convert.ToInt32(posts), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                string _apiUrl = "/webapi/image/othermodelphotolist/?applicationid=2&startindex=" + startIndex + "&endindex=" + endIndex + "&modelid=" + modelId + "&categoryidlist=" + contentTypeList;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objPhotos = objClient.GetApiResponseSync<CMSImage>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objPhotos);
                }

                if (objPhotos != null)
                {
                    CMSImageList objCMSModelImageList = new CMSImageList();
                    objCMSModelImageList = CMSMapper.Convert(objPhotos);

                    if (objPhotos.Images != null)
                    {
                        objPhotos.Images.Clear();
                        objPhotos.Images = null;
                    }

                    return Ok(objCMSModelImageList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }

            return NotFound();
        }  //othermodelist api
        #endregion

        #region Article Content Details Api
        /// <summary>
        /// Modified By : Ashish G. Kamble.
        /// Summary : API to get details of article. This is api is used for the articles having multiple pages. e.g. Road Tests, Expert Reviews, Features.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>Article Details</returns>
        [ResponseType(typeof(CMSArticlePageDetails)), Route("api/cms/id/{basicId}/pages/")]
        public IHttpActionResult Get(string basicId)
        {
            try
            {
                ArticlePageDetails objFeaturedArticles = null;

                string _apiUrl = "/webapi/article/contentpagedetail/?basicid=" + basicId;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objFeaturedArticles = objClient.GetApiResponseSync<ArticlePageDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objFeaturedArticles);
                }

                if (objFeaturedArticles != null)
                {
                    CMSArticlePageDetails objCMSFArticles = new CMSArticlePageDetails();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);

                    if (objFeaturedArticles != null)
                    {
                        if (objFeaturedArticles.PageList != null)
                        {
                            objFeaturedArticles.PageList.Clear();
                            objFeaturedArticles.PageList = null;
                        }

                        if (objFeaturedArticles.TagsList != null)
                        {
                            objFeaturedArticles.TagsList.Clear();
                            objFeaturedArticles.TagsList = null;
                        }

                        if (objFeaturedArticles.VehiclTagsList != null)
                        {
                            objFeaturedArticles.VehiclTagsList.Clear();
                            objFeaturedArticles.VehiclTagsList = null;
                        }
                    }

                    objCMSFArticles.FormattedDisplayDate = objFeaturedArticles.DisplayDate.ToString("MMMM dd, yyyy hh:mm tt");

                    // If android, IOS client sanitize the article content 
                    string platformId = string.Empty;

                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }

                    if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                    {
                        foreach (var page in objCMSFArticles.PageList)
                        {
                            Bikewale.Entities.CMS.Articles.HtmlContent objContent = Bikewale.Utility.SanitizeHtmlContent.GetFormattedContent(page.Content);

                            if (objContent.HtmlItems != null && objContent.HtmlItems.Count > 0)
                            {
                                DTO.CMS.Articles.HtmlContent htmlContent = new DTO.CMS.Articles.HtmlContent();
                                htmlContent.HtmlItems = objContent.HtmlItems.Select(item => new DTO.CMS.Articles.HtmlItem() { Content = item.Content, ContentList = item.ContentList, SetMargin = item.SetMargin, Type = item.Type }).ToList();

                                page.htmlContent = htmlContent;
                                page.Content = "";

                                objContent.HtmlItems.Clear();
                                objContent.HtmlItems = null;
                            }
                        }
                    }
                    objCMSFArticles.ShareUrl = new CMSShareUrl().ReturnShareUrl(objCMSFArticles);
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
        }  //get article content
        #endregion


        #region News Details Api
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get details of article. This is api is used for the articles single page. e.g. News.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>News Details</returns>
        [ResponseType(typeof(CMSArticleDetails)), Route("api/cms/id/{basicId}/page/")]
        public IHttpActionResult GetArticleDetailsPage(string basicId)
        {
            DTO.CMS.Articles.HtmlContent htmlContent = null;
            CMSArticleDetails objCMSFArticles = null;
            try
            {
                ArticleDetails objNews = null;

                string _apiUrl = "/webapi/article/contentdetail/?basicid=" + basicId;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //objNews = objClient.GetApiResponseSync<ArticleDetails>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objNews);
                    objNews = objClient.GetApiResponseSync<ArticleDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objNews);
                }

                if (objNews != null)
                {
                    objCMSFArticles = new CMSArticleDetails();
                    objCMSFArticles = CMSMapper.Convert(objNews);

                    if (objNews.TagsList != null)
                    {
                        objNews.TagsList.Clear();
                        objNews.TagsList = null;
                    }

                    if (objNews.VehiclTagsList != null)
                    {
                        objNews.VehiclTagsList.Clear();
                        objNews.VehiclTagsList = null;
                    }

                    objCMSFArticles.FormattedDisplayDate = objNews.DisplayDate.ToString("MMMM dd, yyyy hh:mm tt");

                    // If android, IOS client execute this code
                    string platformId = string.Empty;

                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }

                    if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                    {
                        Bikewale.Entities.CMS.Articles.HtmlContent objContent = Bikewale.Utility.SanitizeHtmlContent.GetFormattedContent(objNews.Content);

                        if (objContent.HtmlItems != null && objContent.HtmlItems.Count > 0)
                        {
                            htmlContent = new DTO.CMS.Articles.HtmlContent();
                            htmlContent.HtmlItems = objContent.HtmlItems.Select(item => new DTO.CMS.Articles.HtmlItem() { Content = item.Content, ContentList = item.ContentList, SetMargin = item.SetMargin, Type = item.Type }).ToList();

                            if (objContent.HtmlItems != null)
                            {
                                objContent.HtmlItems.Clear();
                                objContent.HtmlItems = null;
                            }

                            objCMSFArticles.htmlContent = htmlContent;
                            objCMSFArticles.Content = "";
                        }
                    }
                    {
                        objCMSFArticles.ShareUrl = new CMSShareUrl().ReturnShareUrl(objCMSFArticles);
                    }
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
        }  //get News Details
        #endregion


        #region Article Photos Api
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get list of photos for the specified article.
        /// </summary>
        /// <param name="basicId">Mandatory field.</param>
        /// <returns>Returns list of photos.</returns>
        [ResponseType(typeof(IEnumerable<CMSModelImageBase>)), Route("api/cms/id/{basicId}/photos/")]
        public IHttpActionResult GetArticlePhotos(string basicId)
        {
            try
            {
                string _apiUrl = String.Format("/webapi/image/GetArticlePhotos/?basicid={0}", basicId);
                List<ModelImage> objImg = null;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objImg = objClient.GetApiResponseSync<List<ModelImage>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objImg);
                }

                if (objImg != null && objImg.Count > 0)
                {
                    // Auto map the properties
                    List<CMSModelImageBase> objCMSModels = new List<CMSModelImageBase>();
                    objCMSModels = CMSMapper.Convert(objImg);

                    objImg.Clear();
                    objImg = null;

                    return Ok(objCMSModels);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }  //get Articles Photos       
        #endregion

    }   // class
}   // namespace
