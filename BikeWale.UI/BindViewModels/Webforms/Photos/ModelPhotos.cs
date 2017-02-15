
using Bikewale.BAL.BikeData;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Common;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.SEO;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.Photos
{
    /// <summary>
    /// Created By : Sushil Kumar on 5th Jan 2017
    /// Description : Viewmodel for Model's Photos page
    /// Modified by : Sangram Nandkhile on 10 Feb 2017
    /// Description: Changed Image entity to ColorImageBaseEntity
    /// </summary>
    public class BindModelPhotos
    {
        private uint _modelId = 0, _noOfGrid = 6;
        private IBikeModelsCacheRepository<int> objModelCache = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache = null;
        public string bikeName = string.Empty, modelImage = string.Empty;
        public int totalPhotosCount, gridPhotosCount, nongridPhotosCount;
        public bool isPageNotFound = false, isPermanentRedirection = false;
        public bool isRedirectToModelPage = false;
        public string pageRedirectUrl = "/";
        public BikeMakeEntityBase objMake = null;
        public BikeModelEntityBase objModel = null;
        public List<ColorImageBaseEntity> objImageList = null;
        public PageMetaTags pageMetas = null;
        public uint gridSize = 30;  //show more photos available after grid size more than 30
        public bool IsUpcoming = false, IsDiscontinued = false;
        public bool isModelpage;

        /// <summary>
        /// Created By : Sushil Kumar on 5th Jan 2016
        /// Description: ModelPhotos constructor to resolve unity containers on intialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public BindModelPhotos()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>();


                    objModelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                    objModelMaskingCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                }

                ParseQueryString();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : BindModelPhotos");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 5th Jan 2016
        /// Description: To get Model Details
        /// </summary>
        public void GetModelDetails()
        {
            try
            {
                var bikemodelEnt = new ModelHelper().GetModelDataById(_modelId);

                if (bikemodelEnt != null)
                {
                    objMake = bikemodelEnt.MakeBase;
                    objModel = new BikeModelEntityBase();
                    objModel.ModelId = bikemodelEnt.ModelId;
                    objModel.ModelName = bikemodelEnt.ModelName;
                    objModel.MaskingName = bikemodelEnt.MaskingName;
                    bikeName = string.Format("{0} {1}", objMake.MakeName, bikemodelEnt.ModelName);
                    IsUpcoming = bikemodelEnt.Futuristic;
                    IsDiscontinued = !bikemodelEnt.Futuristic && !bikemodelEnt.New;
                }

                GetModelImages();
                SetPageMetas();

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : GetModelImages");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 5th Jan 2016
        /// Description: To get model images .Calculate grid and non grid images count based on total count
        /// Modified By :- Subodh Jain 20 jan 2017
        /// Summary :- take only 1 element if model page gallery is binding
        /// </summary>
        public void GetModelImages()
        {
            try
            {
                objImageList = objModelCache.GetAllPhotos((int)_modelId) as List<ColorImageBaseEntity>;

                if (objImageList != null && objImageList.Count > 0)
                {
                    totalPhotosCount = objImageList.Count;

                    nongridPhotosCount = (int)(totalPhotosCount % _noOfGrid);
                    gridPhotosCount = totalPhotosCount - nongridPhotosCount;
                    modelImage = Utility.Image.GetPathToShowImages(objImageList[0].OriginalImgPath, objImageList[0].HostUrl, Bikewale.Utility.ImageSize._476x268);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : GetModelImages");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 6th Jan 2017
        /// Description : Set mode photos page metas
        /// </summary>
        private void SetPageMetas()
        {
            try
            {
                if (objMake != null && objModel != null)
                {
                    pageMetas = new PageMetaTags();
                    pageMetas.Title = String.Format("{0} Images | {1} Photos - BikeWale", bikeName, objModel.ModelName);
                    pageMetas.Keywords = string.Format("{0} photos, {0} pictures, {0} images, {1} {0} photos", objModel.ModelName, objMake.MakeName);
                    pageMetas.Description = String.Format("View images of {0} in different colors and angles. Check out {2} photos of {1} on BikeWale", objModel.ModelName, bikeName, totalPhotosCount);
                    pageMetas.CanonicalUrl = String.Format("https://www.bikewale.com/{0}-bikes/{1}/images/", objMake.MaskingName, objModel.MaskingName);
                    pageMetas.AlternateUrl = String.Format("https://www.bikewale.com/m/{0}-bikes/{1}/images/", objMake.MaskingName, objModel.MaskingName);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : SetPageMetas");
            }

        }

        /// <summary>
        /// Created By : Sushil Kumar on 5th Jan 2016
        /// Description: Parse query string to validate model masking name
        /// </summary>
        private void ParseQueryString()
        {
            ModelMaskingResponse objResponse = null;
            string modelMaskingName = string.Empty;
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : ParseQueryString");
                isRedirectToModelPage = true;
            }
            finally
            {
                if (!string.IsNullOrEmpty(request.QueryString["model"]))
                {
                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            _modelId = objResponse.ModelId;
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            pageRedirectUrl = request.RawUrl.Replace(modelMaskingName, objResponse.MaskingName);
                            isPermanentRedirection = true;
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
                else
                {
                    isPageNotFound = true;
                }
            }
        }
    }
}