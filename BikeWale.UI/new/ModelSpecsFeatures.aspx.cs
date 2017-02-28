﻿using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.Controls;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.New
{
    /// <summary>
    /// Created By : Lucky Rathore on 03 June 2016
    /// Description : class handle Binding of specification and Feature and other logics.
    /// </summary>
    public class ModelSpecsFeatures : PageBase
    {
        protected uint cityId, areaId, modelId, versionId, dealerId, price = 0;
        protected string cityName, areaName, makeName, modelName, modelImage, bikeName, versionName, makeMaskingName, modelMaskingName, clientIP = CommonOpn.GetClientIP();
        protected IEnumerable<CityEntityBase> objCityList = null;
        protected IEnumerable<Bikewale.Entities.Location.AreaEntityBase> objAreaList = null;
        protected bool isDiscontinued, IsExShowroomPrice = true;
        protected BikeSpecificationEntity specs;
        protected BikeModelPageEntity modelDetail;
        protected DetailedDealerQuotationEntity dealerDetail;
        protected BikeModelPageEntity modelPg;
        protected LeadCaptureControl ctrlLeadPopUp;
        protected GenericBikeInfoControl ctrlGenericBikeInfo;

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
            // Modified By :Lucky Rathore on 12 July 2016.
            Form.Action = Request.RawUrl;
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            DeviceDetection dd = new DeviceDetection(originalUrl);
            dd.DetectDevice();
            ProcessQueryString();
            modelDetail = FetchModelPageDetails(modelId, versionId);
            if (modelDetail != null)
            {
                if (cityId > 0 && versionId > 0)
                {
                    string PQLeadId = (PQSourceEnum.Desktop_SpecsAndFeaturePage_OnLoad).ToString();
                    string UTMA = Request.Cookies["__utma"] != null ? Request.Cookies["__utma"].Value : string.Empty;
                    string UTMZ = Request.Cookies["_bwutmz"] != null ? Request.Cookies["_bwutmz"].Value : string.Empty;
                    string DeviceId = Request.Cookies["BWC"] != null ? Request.Cookies["BWC"].Value : string.Empty;

                }

            }
            if (versionId > 0)
            {
                specs = FetchVariantDetails(versionId);
            }
            else
            {
                specs = modelPg.ModelVersionSpecs;
            }
            BindWidget();
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// </summary>
        private BikeModelPageEntity FetchModelPageDetails(uint modelID, uint versionId)
        {
            modelPg = new BikeModelPageEntity();
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
                                }
                            }
                            if (!modelPg.ModelDetails.New)
                                isDiscontinued = true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchModelPageDetails");
                objErr.SendMail();
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "FetchVariantDetails");
                objErr.SendMail();
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

                    if (!string.IsNullOrEmpty(modelMaskingName)) // && versionId > 0
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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "ProcessQueryString");
                objErr.SendMail();
            }
            finally
            {
                if (isRedirect)
                {
                    //redirect permanent to new page 
                    CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelMaskingName, objResponse.MaskingName));
                }
            }
        }

        /// Created  By :- Sajal Gupta on 13-02-2017
        /// Summary :- BikeInfo Slug details
        /// </summary>
        private void BindWidget()
        {
            if (ctrlGenericBikeInfo != null)
            {
                ctrlGenericBikeInfo.ModelId = modelId;
                ctrlGenericBikeInfo.CityId = GlobalCityArea.GetGlobalCityArea().CityId;
                ctrlGenericBikeInfo.TabCount = 4;
                ctrlGenericBikeInfo.PageId = BikeInfoTabType.Specs;
            }
        }

    }
}