
using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.Photos
{
    /// <summary>
    /// Created By : Sushil Kumar 
    /// </summary>
    public class ModelPhotos
    {
        protected string bikeName = string.Empty, modelName = string.Empty, makeName = string.Empty, makeMaskingName = string.Empty, modelMaskingName = string.Empty, modelImage = string.Empty;
        protected int modelId = 0, imgCount = 0;
        public bool IsPageNotFound = false, IsPermanentRedirection = false;
        protected BikeModelPageEntity modelPage = null;
        private IBikeModelsCacheRepository<int> objModelCache = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache = null;
        IBikeModels<BikeModelEntity, int> objModel = null;
        public bool IsRedirectToModelPage = false;


        /// <summary>
        /// Created By : Sushil Kumar on 5th Jan 2016
        /// Description: ModelPhotos constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public ModelPhotos()
        {

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                    .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                    .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                    .RegisterType<ICacheManager, MemcacheManager>()
                    .RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                    .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();

                objModelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                objModelMaskingCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                objModel = container.Resolve<IBikeModels<BikeModelEntity, int>>();
            }

            ParseQueryString();
        }


        public void GetModelImages()
        {
            try
            {
                BikeModelEntity bikemodelEnt = null;
                bikemodelEnt = objModel.GetById(Convert.ToInt32(modelId));
                List<ModelImage> objImageList = (List<ModelImage>)objModelCache.GetModelPhotos(modelId);

                if (bikemodelEnt != null)
                {
                    modelName = bikemodelEnt.ModelName;
                    makeMaskingName = bikemodelEnt.MakeBase.MaskingName;
                    makeName = bikemodelEnt.MakeBase.MakeName;
                    bikeName = string.Format("{0} {1}", makeName, modelName);
                }


                if (objImageList != null && objImageList.Count > 0)
                {
                    imgCount = objImageList.Count;
                    modelImage = Utility.Image.GetPathToShowImages(objImageList[0].OriginalImgPath, objImageList[0].HostUrl, Bikewale.Utility.ImageSize._476x268);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Photos : GetModelImages");
            }
        }

        /// <summary>
        /// Function to get the required parameters from the query string.
        /// Desc: It sets variantId and modelId
        /// </summary>
        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            var request = HttpContext.Current.Request;
            try
            {
                modelMaskingName = request.QueryString["model"];
                if (!string.IsNullOrEmpty(modelMaskingName) && objModelMaskingCache != null)
                {
                    objResponse = objModelMaskingCache.GetModelMaskingResponse(modelMaskingName);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.Photos : ParseQueryString");
                IsRedirectToModelPage = true;
            }
            finally
            {
                if (!string.IsNullOrEmpty(request.QueryString["model"]))
                {
                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            modelId = Convert.ToInt32(objResponse.ModelId);
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            IsPermanentRedirection = true;
                        }
                        else
                        {
                            IsPageNotFound = true;
                        }
                    }
                    else
                    {
                        IsPageNotFound = true;
                    }
                }
                else
                {
                    IsPageNotFound = true;
                }
            }
        }
    }
}