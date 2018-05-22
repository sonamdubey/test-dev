using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.CMS.Photos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.CMS;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private readonly ICMS _cms = null;
        private readonly IPager _pager = null;
        private readonly ICMSCacheContent _CMSCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModelEntity = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="cms"></param>
        /// <param name="cmsCache"></param>
        /// <param name="objModelCache"></param>
        /// <param name="bikeModelEntity"></param>
        public CMSController(IPager pager, ICMS cms, ICMSCacheContent cmsCache, IBikeModelsCacheRepository<int> objModelCache, IBikeModels<BikeModelEntity, int> bikeModelEntity)
        {
            _cms = cms;
            _pager = pager;
            _CMSCache = cmsCache;
            _bikeModelEntity = bikeModelEntity;
        }


        #region ModelImages List Api
        /// <summary>
        /// Modified By : Ashish G. Kamble.
        /// Summary : API to get list of photos for the specified model.
        /// Modified By : Sajal Gupta on 28-02-2017
        /// Description : Called function of BAL instead of Cache
        /// </summary>
        /// <param name="modelId">Mandatory field. Value should be greater than 0.</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSModelImageBase>)), Route("api/cms/photos/model/{modelId}")]
        public IHttpActionResult Get(int modelId)
        {
            IEnumerable<ModelImage> objImageList = null;
            try
            {
                objImageList = _bikeModelEntity.GetBikeModelPhotoGallery(modelId);
                if (objImageList != null && objImageList.Any())
                {
                    // Auto map the properties
                    List<CMSModelImageBase> objCMSModels = new List<CMSModelImageBase>();
                    objCMSModels = CMSMapper.Convert(objImageList);

                    return Ok(objCMSModels);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");


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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");

                return InternalServerError();
            }

            return NotFound();
        }  //othermodelist api

        /// <summary>
        /// Created By  : Rajan Chauhan on 13 Jan 2018
        /// Description : API for images landing page
        /// Modified By : Rajan Chauhan on 13 Mar 2018
        /// Description : Added ComparisonTests in categoryIds
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Route("api/images/pages/{pageNo}/"), HttpGet, ResponseType(typeof(ModelImageList))]
        public IHttpActionResult GetModelsImagesList(int pageNo, int? pageSize = null)
        {
            try
            {

                int _pageSize = pageSize != null ? Convert.ToInt32(pageSize) : 30;
                ImagePager pager = new ImagePager()
                {
                    PageNo = pageNo,
                    PageSize = _pageSize,
                    StartIndex = (pageNo - 1) * _pageSize + 1,
                    EndIndex = pageNo * _pageSize

                };
                IEnumerable<ModelIdWithBodyStyle> objModelIds = _bikeModelEntity.GetModelIdsForImages(0, EnumBikeBodyStyles.AllBikes, ref pager);
                string modelIds = string.Join(",", objModelIds.Select(m => m.ModelId));
                int requiredImageCount = 4;
                string categoryIds = Bikewale.Utility.CommonApiOpn.GetContentTypesString(
                    new List<EnumCMSContentType>()
                    {
                        EnumCMSContentType.PhotoGalleries,
                        EnumCMSContentType.RoadTest,
                        EnumCMSContentType.ComparisonTests
                    }
                );

                ModelImageWrapper ImageWrapper = _bikeModelEntity.GetBikeModelsPhotos(modelIds, categoryIds, requiredImageCount, pager);
                ImageWrapper.RecordCount = pager.CurrentSetResults;
                ModelImageList obj = CMSMapper.Convert(ImageWrapper);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Service.Controller.ImageController : GetModelsImagesList({0}, {1})", pageNo, pageSize));
                return BadRequest();
            }

        }
        #endregion

        #region Article Content Details Api
        /// <summary>
        /// Modified By : Ashish G. Kamble.
        /// Summary : API to get details of article. This is api is used for the articles having multiple pages. e.g. Road Tests, Expert Reviews, Features.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// Modified By : Ashish G. Kamble on 2 June 2017
        /// Modified : Removed all business logic from controller to the BAL
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>Article Details</returns>
        [ResponseType(typeof(CMSArticlePageDetails)), Route("api/cms/id/{basicId}/pages/")]
        public HttpResponseMessage Get(string basicId)
        {
            uint _basicId = default(uint);
            string articleDetailsJson = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(basicId) && uint.TryParse(basicId, out _basicId))
                {
                    articleDetailsJson = _cms.GetArticleDetailsPages(_basicId);

                    if (string.IsNullOrEmpty(articleDetailsJson))
                    {
                        return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                    }
                    else
                    {
                        return new System.Net.Http.HttpResponseMessage()
                        {
                            Content = new System.Net.Http.StringContent(articleDetailsJson, System.Text.Encoding.UTF8, "application/json")
                        };
                    }
                }
                else
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }  //get article content          

        #endregion


        #region News Details Api
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get details of article. This is api is used for the articles single page. e.g. News.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// Modified By : Ashish G. Kamble on 2 June 2017
        /// Modified : Removed all business logic from controller to the BAL
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>News Details</returns>
        [HttpGet, ResponseType(typeof(CMSArticleDetails)), Route("api/cms/id/{basicId}/page/")]
        //public IHttpActionResult GetArticleDetailsPage(string basicId)
        public HttpResponseMessage ArticlePages(string basicId)
        {
            uint _basicId = default(uint);
            string articleDetailsJson = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(basicId) && uint.TryParse(basicId, out _basicId))
                {
                    // Get data from BAL (returns json from cache)
                    articleDetailsJson = _cms.GetArticleDetailsPage(_basicId);

                    if (String.IsNullOrEmpty(articleDetailsJson))
                    {
                        return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                    }
                    else
                    {
                        return new System.Net.Http.HttpResponseMessage()
                        {
                            Content = new System.Net.Http.StringContent(articleDetailsJson, System.Text.Encoding.UTF8, "application/json")
                        };
                    }
                }
                else
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
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
            IEnumerable<CMSModelImageBase> objCMSModels = null;
            IEnumerable<ModelImage> objImg = null;
            int _basicId = default(int);
            try
            {

                if (!string.IsNullOrEmpty(basicId) && int.TryParse(basicId, out _basicId))
                {
                    objImg = _CMSCache.GetArticlePhotos(_basicId);

                    if (objImg != null && objImg.Any())
                    {
                        objCMSModels = CMSMapper.Convert(objImg);
                        objImg = null;

                    }

                    return Ok(objCMSModels);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController.GetArticlePhotos");

                return InternalServerError();
            }

            return NotFound();
        }  //get Articles Photos  

        #endregion

    }   // class
}   // namespace
