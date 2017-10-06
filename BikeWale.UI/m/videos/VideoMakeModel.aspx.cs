﻿using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Videos;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Videos;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 4th March 2016
    /// </summary>
    public class VideoMakeModel : System.Web.UI.Page
    {
        protected Repeater rptVideos;
        protected int totalRecords = 0;
        protected bool isModel = false;
        protected string make = string.Empty, model = string.Empty, titleName = string.Empty, canonTitle = string.Empty, pageHeading = string.Empty, metaDescription = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty, canonicalUrl = string.Empty, metaKeywords = string.Empty;
        protected ushort makeId = 0;
        protected uint? modelId;
        protected SimilarBikeVideos ctrlSimilarBikeVideos;
        protected PopularBikesByBodyStyle ctrlBikesByBodyStyle;
        private GlobalCityAreaEntity _currentCityArea;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;
        protected override void OnInit(EventArgs e)
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();
            BindVideos();
            CreateTitleMeta();
            BindControl();
        }
        /// <summary>
        /// Created By:- Subodh Jain 3 feb 2017
        /// Summary :- bind similar bike widget
        /// Modified By :- Subodh Jain 6 feb 2017
        /// Summary :- Added popular body style widget
        /// Modified  By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        private void BindControl()
        {
            try
            {
                _currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (ctrlSimilarBikeVideos != null)
                {
                    ctrlSimilarBikeVideos.ModelId = (uint)(modelId ?? 0);
                    ctrlSimilarBikeVideos.TotalCount = 9;
                }
                if (ctrlBikesByBodyStyle != null)
                {
                    ctrlBikesByBodyStyle.ModelId = (modelId ?? 0);
                    ctrlBikesByBodyStyle.topCount = 9;
                    ctrlBikesByBodyStyle.CityId = _currentCityArea.CityId;
                }
                if (ctrlGenericBikeInfo != null)
                {
                    ctrlGenericBikeInfo.ModelId = (modelId ?? 0);
                    ctrlGenericBikeInfo.CityId = _currentCityArea.CityId;
                    ctrlGenericBikeInfo.PageId = BikeInfoTabType.Videos;
                    ctrlGenericBikeInfo.TabCount = 3;
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Mobile.Videos.BindControl");
            }
        }
        /// <summary>
        /// Function to create Title, meta tags and description
        /// Written By : Sushil Kumar on 4th March 2016
        /// Modified By  : Sushil Kumar on 4th March 2016
        /// Description : Added titleName for pageTitle
        /// </summary>
        private void CreateTitleMeta()
        {
            if (isModel)
            {
                pageHeading = String.Format("{0} {1} Videos", make, model);
                titleName = String.Format("{0} {1} Videos - BikeWale", make, model);
                canonicalUrl = string.Format("https://www.bikewale.com/bike-videos/{0}-{1}/", makeMaskingName, modelMaskingName);
                metaDescription = string.Format("Check latest {0} {1} videos, watch BikeWale expert's take on {0} {1} - features, performance, price, fuel economy, handling and more.", make, model);
                metaKeywords = string.Format("{0},{1},{0} {1},{0} {1} Videos", make, model);
            }
            else
            {
                pageHeading = String.Format("{0} Bike Videos", make);
                titleName = String.Format("{0} Bike Videos - BikeWale", make);
                canonicalUrl = string.Format("https://www.bikewale.com/bike-videos/{0}/", makeMaskingName);
                metaDescription = string.Format("Check latest {0} bikes videos, watch BikeWale expert's take on {0} bikes - features, performance, price, fuel economy, handling and more.", make);
                metaKeywords = string.Format("{0},{0} Videos", make);
            }
        }   // page load

        /// <summary>
        /// Written By : Sushil Kumar on 4th March 2016
        /// Summary : function to read the query string values.
        /// Modified By  : Sushil Kumar on 4th March 2016
        /// Description : Added try catch for parseString function
        ///               Make check for null objects
        /// </summary>
        private void ParseQueryString()
        {
            string _makeId = String.Empty;
            ModelMaskingResponse objModelResponse = null;
            MakeMaskingResponse objMakeResponse = null;
            bool isMake301 = false, isModel301 = false, isPageNotFound = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                        .RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    makeMaskingName = Request.QueryString["make"];
                    modelMaskingName = Request.QueryString["model"];
                    if (!String.IsNullOrEmpty(makeMaskingName))
                    {

                        var objMakeCache = container.Resolve<IBikeMakesCacheRepository<int>>();

                        objMakeResponse = objMakeCache.GetMakeMaskingResponse(makeMaskingName);

                        if (objMakeResponse != null)
                        {
                            if (objMakeResponse.StatusCode == 200)
                            {
                                _makeId = Convert.ToString(objMakeResponse.MakeId);
                            }
                            else if (objMakeResponse.StatusCode == 301)
                            {
                                isMake301 = true;
                            }
                            else
                            {
                                isPageNotFound = true;
                            }
                        }
                        else
                        {
                            isPageNotFound = true;
                        }

                        if (!string.IsNullOrEmpty(_makeId) && ushort.TryParse(_makeId, out makeId))
                        {
                            if (!string.IsNullOrEmpty(modelMaskingName))
                                isModel = true;

                            if (makeId > 0 && isModel)
                            {

                                var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                                objModelResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                                if (objModelResponse != null)
                                {
                                    if (objModelResponse.StatusCode == 200)
                                    {
                                        modelId = objModelResponse.ModelId;
                                    }
                                    else if (objModelResponse.StatusCode == 301)
                                    {
                                        isModel301 = true;
                                    }
                                    else
                                    {
                                        isPageNotFound = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            isPageNotFound = true;
                        }
                    }
                    else
                    {
                        isPageNotFound = true;
                    }
                }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.RawUrl + "ParseQueryString()");
                objErr.SendMail();
            }
            finally
            {
                if (isMake301)
                {
                    CommonOpn.RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objMakeResponse.MaskingName));
                }
                else if (isModel301)
                {
                    CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelMaskingName, objModelResponse.MaskingName));
                }
                else if (isPageNotFound)
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    this.Page.Visible = false;
                }
            }
        }

        /// <summary>
        /// Writtten By : Sushil Kumar on 4th March 2016
        /// Summary : Function to bind the videos to the videos repeater.
        ///           Initially 9 records are binded.
        /// Modified By : Sushil Kumar on 4th March 2016
        /// Description : Made check for makeId for unnecessary call  if query string is wrong or invalid
        /// Modified by :   Sumit Kate on 17 Oct 2016
        /// Description :   Added condition to check modelid > 0
        /// </summary>
        private void BindVideos()
        {
            IEnumerable<BikeVideoEntity> objVideosList = null;
            try
            {
                if (makeId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IVideosCacheRepository, VideosCacheRepository>()
                                 .RegisterType<IVideos, Bikewale.BAL.Videos.Videos>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IVideoRepository, ModelVideoRepository>();

                        var objCache = container.Resolve<IVideosCacheRepository>();
                        if (modelId.HasValue && modelId.Value > 0)
                        {
                            objVideosList = objCache.GetVideosByMakeModel(1, 9, makeId, modelId);
                        }
                        else
                        {
                            objVideosList = objCache.GetVideosByMakeModel(1, 9, makeId);
                        }
                        if (objVideosList != null && objVideosList.Any())
                        {
                            rptVideos.DataSource = objVideosList;
                            rptVideos.DataBind();
                            // Set make and modelName
                            if (objVideosList.FirstOrDefault() != null)
                            {
                                make = objVideosList.FirstOrDefault().MakeName;
                                if (isModel)
                                    model = objVideosList.FirstOrDefault().ModelName;
                            }
                        }
                        else
                        {
                            // As no videos are found, please redirect to 404 error

                            Response.Redirect(CommonOpn.AppPath + "m/pageNotFound.aspx", false);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            this.Page.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.RawUrl + "BindVideos()");
                objErr.SendMail();
            }
        }
    }
}