
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.DAL.BikeData;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.Schema;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private uint _modelId;
        public uint NoOfGrid;
        private IBikeModelsCacheRepository<int> objModelCache = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache = null;
        public string bikeName = string.Empty, modelImage = string.Empty;
        public ImageBaseEntity firstImage = null;
        public int totalPhotosCount, gridPhotosCount, nongridPhotosCount;
        public bool isPageNotFound = false, isPermanentRedirection = false;
        public bool isRedirectToModelPage = false;
        public string pageRedirectUrl = "/";
        public BikeMakeEntityBase objMake = null;
        public BikeModelEntityBase objModel = null;
        public List<ColorImageBaseEntity> objImageList = null;
        public List<BikeVideoEntity> objVideosList = null;
        public PageMetaTags pageMetas = null;
        public uint GridSize;  //show more photos available after grid size more than gridSize
        public bool IsUpcoming = false, IsDiscontinued = false;
        public bool isDesktop;
        private IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        public ModelPhotoGalleryEntity photoGalleryEntity = null;
        public BreadcrumbList Breadcrumb { get; set; }
        public string SchemaJSON { get; set; }

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
                    container.RegisterType<IPager, Pager>()
                        .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                        .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                        .RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                        .RegisterType<ICacheManager, MemcacheManager>();

                    objModelCache = container.Resolve<IBikeModelsCacheRepository<int>>();
                    objModelMaskingCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    _objModelEntity = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                }

                ParseQueryString();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : BindModelPhotos");
            }
        }

        /// <summary>
        /// Created BY : Sajal Gupta on 28-02-2017
        /// Description : Function to get photo gallery data from bal
        /// </summary>
        public void GetPhotoGalleryData()
        {
            try
            {
                photoGalleryEntity = _objModelEntity.GetPhotoGalleryData(Convert.ToInt32(_modelId));

                if (photoGalleryEntity != null && photoGalleryEntity.ObjModelEntity != null)
                {
                    var bikeEntity = photoGalleryEntity.ObjModelEntity;
                    objMake = bikeEntity.MakeBase;
                    objModel = new BikeModelEntityBase();
                    objModel.ModelId = bikeEntity.ModelId;
                    objModel.ModelName = bikeEntity.ModelName;
                    objModel.MaskingName = bikeEntity.MaskingName;
                    bikeName = string.Format("{0} {1}", objMake.MakeName, bikeEntity.ModelName);
                    IsUpcoming = bikeEntity.Futuristic;
                    IsDiscontinued = !bikeEntity.Futuristic && !bikeEntity.New;
                }

                if (photoGalleryEntity != null)
                {
                    objImageList = photoGalleryEntity.ImageList as List<ColorImageBaseEntity>;
                    objVideosList = photoGalleryEntity.VideosList as List<BikeVideoEntity>;
                }

                if (objImageList != null && objImageList.Count > 0)
                {
                    modelImage = Utility.Image.GetPathToShowImages(objImageList[0].OriginalImgPath, objImageList[0].HostUrl, Bikewale.Utility.ImageSize._476x268);
                    firstImage = objImageList[0];
                    if (isDesktop)
                    {
                        objImageList = objImageList.Skip(1).ToList();
                    }

                    totalPhotosCount = objImageList.Count;

                    nongridPhotosCount = (int)(totalPhotosCount % NoOfGrid);
                    gridPhotosCount = totalPhotosCount - nongridPhotosCount;
                }

                SetPageMetas();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : GetPhotoGalleryData");
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
                    pageMetas.Description = String.Format("View images of {0} in different colours and angles. Check out {2} photos of {1} on BikeWale", objModel.ModelName, bikeName, totalPhotosCount);
                    pageMetas.CanonicalUrl = String.Format("https://www.bikewale.com/{0}-bikes/{1}/images/", objMake.MaskingName, objModel.MaskingName);
                    pageMetas.AlternateUrl = String.Format("https://www.bikewale.com/m/{0}-bikes/{1}/images/", objMake.MaskingName, objModel.MaskingName);

                    SetBreadcrumList();
                    SetPageJSONLDSchema(pageMetas);

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : SetPageMetas");
            }

        }


        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(PageMetaTags objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta, Breadcrumb);

            if (webpage != null)
            {
                SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList()
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (!isDesktop)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));


            if (objMake != null)
            {
                url = string.Format("{0}{1}-bikes/", url, objMake.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, string.Format("{0} Bikes", objMake.MakeName)));
            }

            if (objModel != null)
            {
                url = string.Format("{0}{1}/", url, objModel.MaskingName);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, objModel.ModelName));
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, null, "Images"));

            Breadcrumb = new BreadcrumbList() { BreadcrumListItem = BreadCrumbs };

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
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.BindViewModels.Webforms.BindModelPhotos : ParseQueryString");
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