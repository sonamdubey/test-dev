﻿using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Photos;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by  : Sushil Kumar on 30th Sep 2017
    /// Description :  Photos controller to set photos related methods
    /// </summary>
    public class PhotosController : Controller
    {
        private readonly IBikeModelsCacheRepository<int> _objModelCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IBikeInfo _objGenericBike = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly IVideos _objVideos = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;

        public PhotosController(IBikeModelsCacheRepository<int> objModelCache, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache,
            IBikeModels<BikeModelEntity, int> objModelEntity, ICityCacheRepository objCityCache, IBikeInfo objGenericBike, IBikeVersions<BikeVersionEntity, uint> objVersion,
            IVideos objVideos, IBikeMakesCacheRepository objMakeCache)
        {

            _objModelCache = objModelCache;
            _objModelMaskingCache = objModelMaskingCache;
            _objModelEntity = objModelEntity;
            _objCityCache = objCityCache;
            _objGenericBike = objGenericBike;
            _objVersion = objVersion;
            _objVideos = objVideos;
            _objMakeCache = objMakeCache;
        }

        /// <summary>
        /// Description : Image landing page for Desktop
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [Route("photos/"), Filters.DeviceDetection]
        public ActionResult Index(uint? pageNo)
        {
            Models.Photos.v1.PhotosPage objModel = new Models.Photos.v1.PhotosPage(false, _objModelEntity, _objMakeCache, _objVideos, pageNo);
            objModel.PageSize = 30;
            objModel.VideosCount = 4;
            Models.Photos.v1.PhotosPageVM objData = objModel.GetData();
            return View(objData);
        }

        /// <summary>
        /// Description : Image landing page for mobile
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        [Route("m/photos/")]
        public ActionResult Index_Mobile(uint? pageNo)
        {
            Models.Photos.v1.PhotosPage objModel = new Models.Photos.v1.PhotosPage(true, _objModelEntity, _objMakeCache, _objVideos, pageNo);
            objModel.PageSize = 30;
            objModel.VideosCount = 2;
            Models.Photos.v1.PhotosPageVM objData = objModel.GetData();
            return View(objData);
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 30 Jan 2018
        /// Description : Make images landing page for Desktop 
        /// </summary>
        /// <returns></returns>
        [Route("photos/{makeMasking}-bikes/"), Filters.DeviceDetection]
        public ActionResult Make(string makeMasking)
        {
            MakePhotosPage obj = new MakePhotosPage(false, makeMasking, _objModelEntity, _objMakeCache, _objVideos);
            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.VideosCount = 4;
                MakePhotosPageVM objData = obj.GetData();
                return View(objData);
            }
            else if (obj.Status.Equals(StatusCodes.RedirectPermanent))
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
        }

        /// <summary>
        /// Created By  : Rajan Chauhan on 30 Jan 2018
        /// Description : Make images landing page for mobile 
        /// </summary>
        /// <returns></returns>
        [Route("m/photos/{makeMasking}-bikes/")]
        public ActionResult Make_Mobile(string makeMasking)
        {
            MakePhotosPage obj = new MakePhotosPage(true, makeMasking, _objModelEntity, _objMakeCache, _objVideos);
            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.VideosCount = 2;
                MakePhotosPageVM objData = obj.GetData();
                return View(objData);
            }
            else if (obj.Status.Equals(StatusCodes.RedirectPermanent))
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else
            {
                return Redirect("/m/pagenotfound.aspx");
            }
        }

        /// <summary>
        /// Created by  : Sushil Kumar on 30th Sep 2017
        /// Description :  Photos page for desktop
        /// </summary>
        /// <param name="makeMasking"></param>
        /// <param name="modelMasking"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("photos/{makeMasking}-bikes/{modelMasking}/"), Filters.DeviceDetection]
        public ActionResult Model(string makeMasking, string modelMasking, string q)
        {
            PhotosPage obj = new PhotosPage(makeMasking, modelMasking, _objModelCache, _objModelMaskingCache, _objModelEntity, _objCityCache, _objGenericBike, _objVersion, _objVideos);

            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                PhotosPageVM objData = obj.GetData(24, 8, q);
                return View(objData);

            }
            else if (obj.Status.Equals(StatusCodes.RedirectPermanent))
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else
            {
                return Redirect("/pagenotfound.aspx");
            }
        }

        /// <summary>
        /// Modified by: Dhruv Joshi
        /// Dated: 28th March 2018
        /// Description: New Images Page not rendered if no images are present for a model, instead redirected to pagenotfound.aspx
        /// </summary>
        /// <param name="makeMasking"></param>
        /// <param name="modelMasking"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("m/photos/{makeMasking}-bikes/{modelMasking}/")]
        public ActionResult Model_Mobile(string makeMasking, string modelMasking, string q)
        {
            PhotosPage obj = new PhotosPage(makeMasking, modelMasking, _objModelCache, _objModelMaskingCache, _objModelEntity, _objCityCache, _objGenericBike, _objVersion, _objVideos);

            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.IsMobile = true;
                PhotosPageVM objData = obj.GetData(30, 6, q);
                if (objData != null && objData.TotalPhotos > 0)
                {
                    return View(objData);
                }
                else
                {
                    return Redirect("/m/pagenotfound.aspx");
                }

            }
            else if (obj.Status.Equals(StatusCodes.RedirectPermanent))
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else
            {
                return Redirect("/m/pagenotfound.aspx");
            }
        }
    }
}