﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Bikewale.Entities.BikeData;
using System.Web.Http.Description;
using Bikewale.Entities.CMS.Photos;
using Bikewale.DTO.CMS;
using Bikewale.Utility;
using System.Net.Http.Formatting;
using Bikewale.Interfaces.Pager;
using Microsoft.Practices.Unity;
using Bikewale.BAL.Pager;
using Bikewale.Entities.CMS;
using Bikewale.DTO.CMS.Photos;
using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using Bikewale.Entities.CMS.Articles;
using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.Version;
using Bikewale.Service.AutoMappers.CMS;

namespace Bikewale.Service.Controllers.CMS
{
    public class CMSController : ApiController
    {
        string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        string _requestType = "application/json";

        #region ModelImages List Api
        /// <summary>
        /// EditCMS Api to get List of ModelImages
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSModelImageBase>))]
        public IHttpActionResult Get(int modelId)
        {
            //sets the base URI for HTTP requests
            List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
            categorList.Add(EnumCMSContentType.RoadTest);
            categorList.Add(EnumCMSContentType.PhotoGalleries);
            categorList.Add(EnumCMSContentType.ComparisonTests);
            string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);


            string _apiUrl = "webapi/image/modelphotolist/?applicationid=" + _applicationid + "&modelid=" + Convert.ToInt32(modelId) + "&categoryidlist=" + contentTypeList;

            List<ModelImage> objImageList = null;

            objImageList = BWHttpClient.GetApiResponseSync<List<ModelImage>>(_cwHostUrl, _requestType, _apiUrl, objImageList);
            if (objImageList != null && objImageList.Count > 0)
            {
                // Auto map the properties
                List<CMSModelImageBase> objCMSModels = new List<CMSModelImageBase>();
                //Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                //Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                //Mapper.CreateMap<ModelImage, CMSModelImageBase>();
                //objCMSModels = Mapper.Map<List<ModelImage>, List<CMSModelImageBase>>(objImageList);
                objCMSModels = CMSMapper.Convert(objImageList);

                return Ok(objImageList);
            }
            else
            {
                return NotFound();
            }

        }  //get  
        #endregion 

        #region Article Content Details Api
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        [ResponseType(typeof(CMSArticlePageDetails))]
        public IHttpActionResult Get(string basicId, bool article)
        {
            ArticlePageDetails objFeaturedArticles = null;
            if(Convert.ToBoolean(article))
            {
                string _apiUrl = "webapi/article/contentpagedetail/?basicid=" + basicId;

                objFeaturedArticles = BWHttpClient.GetApiResponseSync<ArticlePageDetails>(_cwHostUrl, _requestType, _apiUrl, objFeaturedArticles);

                if (objFeaturedArticles != null)
                {
                    // Auto map the properties
                    CMSArticlePageDetails objCMSFArticles = new CMSArticlePageDetails();
                    //Mapper.CreateMap<ArticlePageDetails, CMSArticlePageDetails>();
                    //Mapper.CreateMap<ArticleBase, CMSArticleBase>();
                    //Mapper.CreateMap<VehicleTag, CMSVehicleTag>();
                    //Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                    //Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                    //Mapper.CreateMap<BikeVersionEntityBase, VersionBase>();
                    //Mapper.CreateMap<Page, CMSPage>();
                    ////objCMSFArticles.PageList = Mapper.Map<List<Page>, List<CMSPage>>(objFeaturedArticles.PageList);
                    ////objCMSFArticles.VehicleTagList = Mapper.Map<List<ArticlePageDetails>, List<CMSArticlePageDetails>>(objFeaturedArticles.VehicleTagList);                    
                    //objCMSFArticles = Mapper.Map<ArticlePageDetails, CMSArticlePageDetails>(objFeaturedArticles);
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);
                    return Ok(objFeaturedArticles);
                }
            }

            return NotFound();

        }  //get 
        #endregion

        #region Article Photos Api
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicId"></param>
        /// <param name="articlePhotos"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSModelImageBase>))]
        public IHttpActionResult Get(string basicId, bool article, bool photos)
        {
            string _apiUrl = "webapi/image/GetArticlePhotos/?basicid=" + basicId;
            List<ModelImage> objImg = null;

            objImg = BWHttpClient.GetApiResponseSync<List<ModelImage>>(_cwHostUrl, _requestType, _apiUrl, objImg);

            if (objImg != null && objImg.Count > 0)
            {
                // Auto map the properties
                List<CMSModelImageBase> objCMSModels = new List<CMSModelImageBase>();
                //Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                //Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                //Mapper.CreateMap<ModelImage, CMSModelImageBase>();
                //objCMSModels = Mapper.Map<List<ModelImage>, List<CMSModelImageBase>>(objImg);
                objCMSModels = CMSMapper.Convert(objImg);
                return Ok(objImg);
            }

        return NotFound();

        }  //get 
        #endregion 

        #region News Details Api
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicId"></param>
        /// <param name="article"></param>
        /// <param name="news"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        [ResponseType(typeof(CMSArticleDetails))]
        public IHttpActionResult Get(string basicId, bool article, bool news, bool all)
        {
            ArticleDetails objNews = null;
            if (Convert.ToBoolean(article))
            {
                string _apiUrl = "webapi/article/contentdetail/?basicid=" + basicId;

                objNews = BWHttpClient.GetApiResponseSync<ArticleDetails>(_cwHostUrl, _requestType, _apiUrl, objNews);

                if (objNews != null)
                {
                    CMSArticleDetails objCMSFArticles = new CMSArticleDetails();
                    objCMSFArticles = CMSMapper.Convert(objNews);
                    return Ok(objNews);
                }
            }          

            return NotFound();

        }  //get 
        #endregion      
        
        #region Other Model Images List Api
        /// <summary>
        /// Edit CMS Api to get List of other model images list
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSImageList>))]
        public IHttpActionResult Get(int modelId, int pageSize, int pageNumber, int startIndex, int endIndex)
        {
            //sets the base URI for HTTP requests
            List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
            categorList.Add(EnumCMSContentType.RoadTest);
            categorList.Add(EnumCMSContentType.PhotoGalleries);
            categorList.Add(EnumCMSContentType.ComparisonTests);
            string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

            CMSImage objPhotos = null;

            // get pager instance
            IPager objPager = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPager, Pager>();
                objPager = container.Resolve<IPager>();

                objPager.GetStartEndIndex(Convert.ToInt32(pageSize), Convert.ToInt32(pageNumber), out startIndex, out endIndex);


                string _apiUrl = "webapi/image/othermodelphotolist/?applicationid=2&startindex=" + startIndex + "&endindex=" + endIndex + "&modelid=" + modelId + "&categoryidlist=" + contentTypeList;

                objPhotos = BWHttpClient.GetApiResponseSync<CMSImage>(_cwHostUrl, _requestType, _apiUrl, objPhotos);

                if (objPhotos != null)
                {
                    CMSImageList objCMSModelImageList = new CMSImageList();
                    objCMSModelImageList = CMSMapper.Convert(objPhotos);
                    return Ok(objPhotos);
                }
                else
                {
                    return NotFound();
                }
            }

        }  //othermodelist api
        
        #endregion   


        #region List Recent Categories Content
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="posts"></param>
        /// <param name="recent"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSArticleSummary>))]
        public IHttpActionResult Get(EnumCMSContentType categoryId, string makeId, string modelId, uint posts, bool recent, bool all)
        {
            List<ArticleSummary> objRecentArticles = null;
            try
            {
                string apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=";
                if (!String.IsNullOrEmpty(makeId) || !String.IsNullOrEmpty(modelId))
                {
                    if (!String.IsNullOrEmpty(modelId))
                        apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + categoryId + "&totalrecords=" + posts + "&makeid=" + makeId + "&modelid=" + modelId;
                    else
                        apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + categoryId + "&totalrecords=" + posts + "&makeid=" + makeId;
                }

                objRecentArticles = BWHttpClient.GetApiResponseSync<List<ArticleSummary>>(_cwHostUrl, _requestType, apiUrl, objRecentArticles);

                if (objRecentArticles != null && objRecentArticles.Count > 0)
                {
                    // Auto map the properties
                    List<CMSArticleSummary> objCMSRArticles = new List<CMSArticleSummary>();
                    //Mapper.CreateMap<ArticleBase, CMSArticleBase>();
                    //Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
                    //objCMSRArticles = Mapper.Map<List<ArticleSummary>, List<CMSArticleSummary>>(objRecentArticles);
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);
                    return Ok(objRecentArticles);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return NotFound();

        }  //get 
        #endregion       

        #region List Category Content
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSContent>))]
        public IHttpActionResult Get(EnumCMSContentType CategoryId, string makeId, string modelId, int startIndex, int endIndex, int? pageSize, int? pageNumber)
        {
            List<CMSContentBase> objFeaturedArticles = null;
            try
            {
                // get pager instance
                IPager objPager = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IPager, Pager>();
                    objPager = container.Resolve<IPager>();

                    objPager.GetStartEndIndex(Convert.ToInt32(pageSize), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                    string apiUrl = "webapi/article/listbycategory/?applicationid=2&categoryidlist=";
                    if (CategoryId != EnumCMSContentType.RoadTest)
                    {
                        apiUrl += (int)CategoryId + "&startindex=" + startIndex + "&endindex=" + endIndex;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(makeId))
                        {
                            apiUrl += (int)CategoryId + "&startindex=" + startIndex + "&endindex=" + endIndex;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(modelId))
                            {
                                apiUrl += (int)CategoryId + "&startindex=" + startIndex + "&endindex=" + endIndex + "&makeid=" + makeId;
                            }
                            else
                            {
                                apiUrl += (int)CategoryId + "&startindex=" + startIndex + "&endindex=" + endIndex + "&makeid=" + makeId + "&modelid=" + modelId;

                            }
                        }
                    }

                    objFeaturedArticles = BWHttpClient.GetApiResponseSync<List<CMSContentBase>>(_cwHostUrl, _requestType, apiUrl, objFeaturedArticles);

                    if (objFeaturedArticles != null && objFeaturedArticles.Count > 0)
                    {
                        // Auto map the properties
                        List<CMSContent> objCMSFArticles = new List<CMSContent>();
                        //Mapper.CreateMap<CMSContentBase, CMSContent>();
                        //Mapper.CreateMap<ArticleBase, CMSArticleBase>();
                        //Mapper.CreateMap<ArticleSummary, CMSArticleSummary>();
                        //Mapper.CreateMap<VehicleTag, CMSVehicleTag>();
                        //objCMSFArticles = Mapper.Map<List<CMSContentBase>, List<CMSContent>>(objFeaturedArticles);
                        objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);

                        return Ok(objFeaturedArticles);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return NotFound();

        }  //get 
        #endregion

        

    }
}
