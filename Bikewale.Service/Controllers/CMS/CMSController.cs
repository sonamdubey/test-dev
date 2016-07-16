using Bikewale.BAL.GrpcFiles;
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
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);
        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);

        static readonly ILog _logger = LogManager.GetLogger(typeof(CMSController));

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
                
            try
            {
                List<ModelImage> objImageList = GetBikeModelPhotoGallery(modelId);
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


        private List<ModelImage> GetBikeModelPhotoGallery(int modelId)
        {
            try
            {
                if (_useGrpc)
                {
                    string contentTypeList = CommonApiOpn.GetContentTypesString(new List<EnumCMSContentType>() { EnumCMSContentType.PhotoGalleries, EnumCMSContentType.RoadTest, EnumCMSContentType.ComparisonTests });

                    var _objGrpcmodelPhotoList = GrpcMethods.GetModelPhotosList(Convert.ToUInt32(_applicationid), modelId, contentTypeList);

                    if (_objGrpcmodelPhotoList != null && _objGrpcmodelPhotoList.LstGrpcModelImage.Count > 0)
                    {
                        return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcmodelPhotoList);
                    }
                    else
                    {
                        return GetBikeModelPhotoGalleryOldWay(modelId);
                    }

                }
                else
                {
                    return GetBikeModelPhotoGalleryOldWay(modelId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                return GetBikeModelPhotoGalleryOldWay(modelId);
            }

        }

        private List<ModelImage> GetBikeModelPhotoGalleryOldWay(int modelId)
        {
            if (_logGrpcErrors)
            {
                _logger.Error(string.Format("Grpc did not work for GetBikeModelPhotoGalleryOldWay {0}", modelId));
            }

            List<ModelImage> objPhotos = null;
            string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];

            string _requestType = "application/json";

            try
            {
                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.PhotoGalleries);
                categorList.Add(EnumCMSContentType.ComparisonTests);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                categorList = null;

                string _apiUrl = String.Format("/webapi/image/modelphotolist/?applicationid={0}&modelid={1}&categoryidlist={2}", _applicationid, modelId, contentTypeList);
                using (BWHttpClient objClient = new BWHttpClient())
                {
                    objPhotos = objClient.GetApiResponseSync<List<ModelImage>>(APIHost.CW, _requestType, _apiUrl, objPhotos);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.BAL.BikeData.GetBikeModelPhotoGallery");
                objErr.SendMail();
            }

            return objPhotos;
        }

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
                ArticlePageDetails objFeaturedArticles = GetPageDetailsViaGrpc(basicId);

                
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

        private ArticlePageDetails GetPageDetailsViaGrpc(string basicId)
        {
            try
            {
                if (_useGrpc)
                {
                    var _objGrpcFeature = GrpcMethods.GetContentPages(Convert.ToUInt64(basicId));

                    if (_objGrpcFeature != null && _objGrpcFeature.ArticleSummary!=null)
                    {
                        return  GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcFeature);
                    }
                    else
                    {
                       return GetPageDetailsOldWay(basicId);
                    }

                }
                else
                {
                    return GetPageDetailsOldWay(basicId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                return GetPageDetailsOldWay(basicId);
            }
        }


        private ArticlePageDetails GetPageDetailsOldWay(string basicId)
        {

            if (_logGrpcErrors)
            {
                _logger.Error(string.Format("Grpc did not work for GetPageDetailsOldWay {0}", basicId));
            }

            string _apiUrl = "/webapi/article/contentpagedetail/?basicid=" + basicId;
            ArticlePageDetails objFeaturedArticles = null;
            using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
            {
                return objClient.GetApiResponseSync<ArticlePageDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objFeaturedArticles);
            }
        }

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
                ArticleDetails objNews = GetNewsDetailsViaGrpc(basicId);

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

        private ArticleDetails GetNewsDetailsViaGrpc(string basicId)
        {
            try
            {
                if (_useGrpc)
                {
                    var _objGrpcArticle = GrpcMethods.GetContentDetails(Convert.ToUInt64(basicId));

                    if (_objGrpcArticle != null)
                    {
                        return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);
                    }
                    else
                    {
                        return GetArticleDetailsPageOldWay(basicId);
                    }
                }
                else
                {
                    return GetArticleDetailsPageOldWay(basicId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                return GetArticleDetailsPageOldWay(basicId);
            }
        }

        public ArticleDetails GetArticleDetailsPageOldWay(string basicId)
        {
            try
            {
                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for GetArticlePhotos {0}", basicId));
                }

                string _apiUrl = "/webapi/article/contentdetail/?basicid=" + basicId;
                ArticleDetails objNews = null;
                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //objNews = objClient.GetApiResponseSync<ArticleDetails>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objNews);
                    return objClient.GetApiResponseSync<ArticleDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objNews);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
            }
            return null;
        }  

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
                if (_useGrpc)
                {
                    
                    var _objGrpcArticlePhotos = GrpcMethods.GetArticlePhotos(Convert.ToUInt64(basicId));

                    if (_objGrpcArticlePhotos != null && _objGrpcArticlePhotos.LstGrpcModelImage.Count > 0)
                    {
                        //following needs to be optimized
                        return Ok(CMSMapper.Convert(GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticlePhotos)));                 
                    }
                    else
                    {
                        return Ok(GetArticlePhotosOldWay(basicId));
                    }

                }
                else
                {
                    return Ok(GetArticlePhotosOldWay(basicId));
                }                                    
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
        }  //get Articles Photos       

        public List<CMSModelImageBase> GetArticlePhotosOldWay(string basicId)
        {
            List<CMSModelImageBase> objCMSModels = new List<CMSModelImageBase>();
            
            if (_logGrpcErrors)
            {
                _logger.Error(string.Format("Grpc did not work for GetArticlePhotosOldWay {0}", basicId));
            }

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
                  
                    objCMSModels = CMSMapper.Convert(objImg);

                    objImg.Clear();
                    objImg = null;

                    return objCMSModels;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                
            }
            return objCMSModels;
        }  //get Articles Photos  

        #endregion

    }   // class
}   // namespace
