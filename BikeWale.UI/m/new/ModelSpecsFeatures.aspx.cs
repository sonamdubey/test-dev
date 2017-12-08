﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Mobile.Controls;
using Bikewale.Models;
using Bikewale.Utility;
using Microsoft.Practices.Unity;

namespace Bikewale.Mobile
{
    /// <summary>
    /// Created By : Lucky Rathore on 03 June 2016
    /// Description : class handle Binding of specification and Feature and other logics.
    /// 
    /// Modified By: Aditi Srivastava
    /// Description: Added a variable for make,model and version name together and also for checking if dealer offersd are available
    /// </summary>
    public class ModelSpecsFeatures : PageBase
    {
        protected uint cityId, areaId, modelId, versionId, dealerId, price = 0, _makeId;
        protected string cityName = "Mumbai", areaName, makeName, modelName, bikeName, versionName, makeMaskingName, modelMaskingName, modelImage, pgTitle;
        protected bool isDiscontinued, IsExShowroomPrice = true;
        protected BikeSpecificationEntity specs;
        protected BikeModelPageEntity modelDetail;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;
        protected bool IsScooter = false;
        protected bool IsScooterOnly = false;
        protected SimilarBikesWidgetVM similarBikes;
        protected PopularBodyStyleVM popularBodyStyle;
        protected EnumBikeBodyStyles bodyStyle;
        protected string bodyStyleText;
        protected string seriesUrl;
        protected BikeSeriesEntityBase Series;
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        /// <summary>
        /// Created By : Lucky Rathore on 03 June 2016
        /// Modified By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with _bwutmz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessQueryString();
            modelDetail = FetchModelPageDetails(modelId);
            IsScooterOnly = modelDetail.ModelDetails.MakeBase.IsScooterOnly;
            if (versionId > 0)
            {
                specs = FetchVariantDetails(versionId);
            }
            BindWidget();
            BindSimilarBikes();
            BindSeriesBreadCrum();
        }
        /// Created  By :- subodh Jain 10 Feb 2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        private void BindWidget()
        {
            if (ctrlGenericBikeInfo != null)
            {
                ctrlGenericBikeInfo.ModelId = modelId;
                ctrlGenericBikeInfo.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
                ctrlGenericBikeInfo.TabCount = 3;
                ctrlGenericBikeInfo.PageId = BikeInfoTabType.Specs;
            }
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// Modified by : Sajal gupta on 28-02-2017
        /// Description :" Fetch modelPage data from calling BAL function instead of cache function.
        /// </summary>
        private BikeModelPageEntity FetchModelPageDetails(uint modelID)
        {
            BikeModelPageEntity modelPg = new BikeModelPageEntity();
            try
            {
                if (modelID > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IPager, Pager>()
                            .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                            .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                 .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>();
                        var objBikeEntity = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                        modelPg = objBikeEntity.GetModelPageDetails(Convert.ToInt16(modelID));
                        if (modelPg != null && modelPg.ModelDetails != null)
                        {
                            if (modelPg.ModelDetails.ModelName != null)
                            {
                                modelName = modelPg.ModelDetails.ModelName;
                            }
                            if (modelPg.ModelDetails.MakeBase != null)
                            {
                                makeName = modelPg.ModelDetails.MakeBase.MakeName;
                                makeMaskingName = modelPg.ModelDetails.MakeBase.MaskingName;
                            }
                            IsScooter = (modelPg.ModelVersions.FirstOrDefault().BodyStyle.Equals(EnumBikeBodyStyles.Scooter));
                            bikeName = string.Format("{0} {1}", makeName, modelName);

                            if (!modelPg.ModelDetails.Futuristic && modelPg.ModelVersionSpecs != null)
                            {
                                // Check it versionId passed through url exists in current model's versions
                                if (this.versionId == 0)
                                {
                                    this.versionId = modelPg.ModelVersionSpecs.BikeVersionId;
                                }
                                modelImage = Bikewale.Utility.Image.GetPathToShowImages(modelPg.ModelDetails.OriginalImagePath, modelPg.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._272x153);
                                var selectedVersion = modelPg.ModelVersions.FirstOrDefault(p => p.VersionId == this.versionId);
                                if (selectedVersion != null)
                                {
                                    price = Convert.ToUInt32(selectedVersion.Price);
                                    versionName = selectedVersion.VersionName;
                                    bodyStyle = selectedVersion.BodyStyle;
                                    bodyStyleText = bodyStyle.Equals(EnumBikeBodyStyles.Scooter) ? "Scooters" : "Bikes";
                                }
                            }
                            // Added by Sangram on 21 Mar 2017 to fetch version id in case of discontinued bikes
                            else if (!modelPg.ModelDetails.New)
                            {
                                isDiscontinued = true;
                                List<BikeVersionMinSpecs> nonZeroValues = modelPg.ModelVersions.Where(x => x.Price > 0).ToList();
                                if (nonZeroValues != null && nonZeroValues.Count > 0)
                                {
                                    ulong minVal = nonZeroValues.Min(x => x.Price);
                                    var lowestVersion = modelPg.ModelVersions.First(x => x.Price == minVal);
                                    if (lowestVersion != null)
                                    {
                                        this.versionId = Convert.ToUInt16(lowestVersion.VersionId);
                                        price = Convert.ToUInt32(lowestVersion.Price);
                                        versionName = lowestVersion.VersionName;
                                    }
                                }
                                else
                                {
                                    BikeVersionMinSpecs selectedVersion = modelPg.ModelVersions.FirstOrDefault();
                                    if (selectedVersion != null)
                                    {
                                        price = Convert.ToUInt32(selectedVersion.Price);
                                        versionName = selectedVersion.VersionName;
                                    }
                                }
                            }
                        }
                    }
                    pgTitle = Bikewale.Utility.BWConfiguration.Instance.MetasMakeId.Split(',').Contains(_makeId.ToString()) ? string.Format("Specifications of {0} | Features of {1}- BikeWale", bikeName, modelName) : string.Format("{0} Specifications and Features - Check out mileage and other technical specifications - BikeWale", bikeName);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "FetchModelPageDetails");

            }
            return modelPg;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// Description     :   Sends the notification to Customer and Dealer
        /// </summary>
        private BikeSpecificationEntity FetchVariantDetails(uint versionId)
        {
            BikeSpecificationEntity specsFeature = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                            ;
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    specsFeature = objCache.MVSpecsFeatures((int)versionId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + "FetchVariantDetails");

            }
            return specsFeature;
        }

        /// <summary>
        /// Created By : Lucky Rathore on 03 June 2016
        /// Description : Private Method to proceess mpq queryString and set the values 
        /// for queried parameters versionId, ModelMaskingName and set value of modelId from Model Masking Name. 
        /// </summary>
        private void ProcessQueryString()
        {
            bool isRedirect = false;
            ModelMaskingResponse objResponse = null;
            try
            {
                if (HttpContext.Current.Request.QueryString != null && HttpContext.Current.Request.QueryString.HasKeys())
                {
                    UInt32.TryParse(Request.QueryString["vid"], out versionId);
                    modelMaskingName = Request.QueryString["model"];

                    if (!string.IsNullOrEmpty(modelMaskingName))
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                     .RegisterType<ICacheManager, MemcacheManager>()
                                     .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                    ;
                            var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                            objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                            if (objResponse != null && objResponse.StatusCode == 200)
                            {
                                modelId = objResponse.ModelId;
                            }
                            else if (objResponse != null && objResponse.StatusCode == 301)
                            {
                                isRedirect = true;
                            }
                            else
                            {
                                Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", false);
                                HttpContext.Current.ApplicationInstance.CompleteRequest();
                                this.Page.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Trace.Warn("GetLocationCookie Ex: ", ex.Message);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + "ProcessQueryString");

            }
            finally
            {
                if (isRedirect)
                    CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelMaskingName, objResponse.MaskingName));
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 12th Oct 2017
        /// Summary : Bind similar bikes
        /// </summary>
        private void BindSimilarBikes()
        {
            try
            {
                if (modelId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeVersionCacheRepository<BikeVersionEntity, uint>, BikeVersionsCacheRepository<BikeVersionEntity, uint>>()
                                .RegisterType<ICacheManager, MemcacheManager>()
                                .RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
                        var objVersionCache = container.Resolve<IBikeVersionCacheRepository<BikeVersionEntity, uint>>();
                        var objSimilarBikes = new SimilarBikesWidget(objVersionCache, versionId, PQSourceEnum.Desktop_DPQ_Alternative);

                        objSimilarBikes.TopCount = 9;
                        objSimilarBikes.CityId = cityId;
                        objSimilarBikes.IsNew = modelDetail.ModelDetails.New;
                        objSimilarBikes.IsUpcoming = modelDetail.ModelDetails.Futuristic;
                        objSimilarBikes.IsDiscontinued = !modelDetail.ModelDetails.New && modelDetail.ModelDetails.Used;
                        similarBikes = objSimilarBikes.GetData();
                        if (similarBikes != null && similarBikes.Bikes != null && similarBikes.Bikes.Any())
                        {
                            similarBikes.Make = modelDetail.ModelDetails.MakeBase;
                            similarBikes.Model = modelDetail.ModelDetails;
                            similarBikes.VersionId = versionId;
                            similarBikes.BodyStyle = bodyStyle;
                            similarBikes.Page = GAPages.Model_Page;
                        }
                        else
                        {
                            if (objSimilarBikes.IsNew || objSimilarBikes.IsUpcoming)
                            {
                                BindPopularBodyStyle();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.New.ModelSpecsFeatures.BindSimilarBikes({0})", modelId));
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 13th Oct 2017
        /// Summary : Bind Popular Body Style
        /// </summary>
        private void BindPopularBodyStyle()
        {
            try
            {
                if (modelId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                                .RegisterType<ICacheManager, MemcacheManager>()
                                .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                .RegisterType<IPager, Pager>();
                        var objBestBikes = container.Resolve<IBikeModelsCacheRepository<int>>();
                        var modelPopularBikesByBodyStyle = new Models.BestBikes.PopularBikesByBodyStyle(objBestBikes);
                        modelPopularBikesByBodyStyle.CityId = cityId;
                        modelPopularBikesByBodyStyle.ModelId = modelId;
                        modelPopularBikesByBodyStyle.TopCount = 9;

                        popularBodyStyle = modelPopularBikesByBodyStyle.GetData();
                        popularBodyStyle.PQSourceId = PQSourceEnum.Desktop_ModelPage;
                        popularBodyStyle.ShowCheckOnRoadCTA = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.New.ModelSpecsFeatures.BindPopularBodyStyle({0})", modelId));
            }
        }

        /// <summary>
        /// Created by  : Vivek Singh Tomar on 29th Nov 2017
        /// Description : Bind series url for if available
        /// </summary>
        private void BindSeriesBreadCrum()
        {
            try
            {
                if (modelId > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                                    .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                                    .RegisterType<ICacheManager, MemcacheManager>()
                                    .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                    .RegisterType<IPager, Pager>();
                        var models = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                        Series = models.GetSeriesByModelId(modelId);
                        if (Series != null && Series.IsSeriesPageUrl)
                        {
                            seriesUrl = string.Format("{0}-bikes/{1}/", makeMaskingName, Series.MaskingName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("Bikewale.New.ModelSpecsFeatures.BindSeriesBreadCrum model id = {0}", modelId));
            }
        }

    }
}