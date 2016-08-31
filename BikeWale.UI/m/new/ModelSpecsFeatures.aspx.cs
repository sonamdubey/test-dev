﻿using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web;

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
        protected uint cityId, areaId, modelId, versionId, dealerId, price = 0;
        protected string cityName = "Mumbai", areaName, makeName, modelName, bikeName, versionName, makeMaskingName, modelMaskingName, modelImage;
        protected bool isDiscontinued, IsExShowroomPrice = true;
        protected BikeSpecificationEntity specs;
        protected BikeModelPageEntity modelDetail;


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
            modelDetail = FetchModelPageDetails(modelId, versionId);
            if (versionId > 0)
            {
                specs = FetchVariantDetails(versionId);
            }
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// </summary>
        private BikeModelPageEntity FetchModelPageDetails(uint modelID, uint versionId)
        {
            BikeModelPageEntity modelPg = new BikeModelPageEntity();
            try
            {
                if (modelID > 0)
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                            .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                 .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>();
                        var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                        modelPg = objCache.GetModelPageDetails(Convert.ToInt16(modelID));
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
                                //versionName = modelPg.ModelVersions.Find(item => item.VersionId == versionId).VersionName;
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
                            ModelMaskingResponse objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                            if (objResponse != null && objResponse.StatusCode == 200)
                            {
                                modelId = objResponse.ModelId;
                            }
                            else if (objResponse != null && objResponse.StatusCode == 301)
                            {
                                //redirect permanent to new page 
                                CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelMaskingName, objResponse.MaskingName));
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

        }

    }
}