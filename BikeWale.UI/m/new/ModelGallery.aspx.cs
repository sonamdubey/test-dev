using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.m.controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.m.New
{
    public partial class ModelGalleryPage : System.Web.UI.Page
    {

        protected ModelGallery ctrlModelGallery;
        protected string bikeName = string.Empty, modelName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty;
        protected int modelId = 0;
        protected BikeModelPageEntity modelPage = default(BikeModelPageEntity);

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                    .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                    .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                    .RegisterType<ICacheManager, MemcacheManager>();
                var objCache = container.Resolve<IBikeModelsCacheRepository<int>>();


                using (IUnityContainer modelContainer = new UnityContainer())
                {
                    modelContainer.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    IBikeModels<BikeModelEntity, int> objClient = modelContainer.Resolve<IBikeModels<BikeModelEntity, int>>();
                    BikeModelEntity bikemodelEnt = objClient.GetById(Convert.ToInt32(modelId));
                    if (bikemodelEnt != null)
                    {
                        modelName = bikemodelEnt.ModelName;
                        bikeName = string.Format("{0} {1}", bikemodelEnt.MakeBase.MakeName, modelName);
                    }
                }

                List<ModelImage> objImageList = objCache.GetModelPhotoGallery(modelId);
                if (objImageList != null && objImageList.Count > 0)
                {
                    ctrlModelGallery.bikeName = bikeName;
                    ctrlModelGallery.modelId = Convert.ToInt32(modelId);
                    ctrlModelGallery.Photos = objImageList;
                }
            }
        }

        /// <summary>
        /// Function to get the required parameters from the query string.
        /// Desc: It sets variantId and modelId
        /// </summary>
        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            try
            {
                modelMaskingName = Request.QueryString["model"];
                makeMaskingName = Request.QueryString["make"];
                if (!string.IsNullOrEmpty(modelMaskingName))
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                        objResponse = objCache.GetModelMaskingResponse(modelMaskingName);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : FetchModelPageDetails");
                objErr.SendMail();
                Response.Redirect("/m/new/", true);
            }
            finally
            {
                if (!string.IsNullOrEmpty(Request.QueryString["model"]))
                {
                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            modelId = Convert.ToInt32(objResponse.ModelId);
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            //redirect permanent to new page 
                            CommonOpn.RedirectPermanent(Request.RawUrl.Replace(modelMaskingName, objResponse.MaskingName));
                        }
                        else
                        {
                            Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                        }
                    }
                    else
                    {
                        Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                    }
                }
                else
                {
                    Response.Redirect(CommonOpn.AppPath + "pageNotFound.aspx", true);
                }
            }
        }
    }
}