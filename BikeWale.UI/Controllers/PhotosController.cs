using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.Photos;
using System.Web.Mvc;
using Bikewale.Models;

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
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IVideos _objVideos = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;

        public PhotosController(IBikeModelsCacheRepository<int> objModelCache, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IBikeModels<BikeModelEntity, int> objModelEntity, ICityCacheRepository objCityCache, IBikeInfo objGenericBike, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, IVideos objVideos, IBikeMakesCacheRepository objMakeCache)
        {

            _objModelCache = objModelCache;
            _objModelMaskingCache = objModelMaskingCache;
            _objModelEntity = objModelEntity;
            _objCityCache = objCityCache;
            _objGenericBike = objGenericBike;
            _objVersionCache = objVersionCache;
            _objVideos = objVideos;
            _objMakeCache = objMakeCache;
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
            PhotosPage obj = new PhotosPage(makeMasking, modelMasking, _objModelCache, _objModelMaskingCache, _objModelEntity, _objCityCache, _objGenericBike, _objVersionCache, _objVideos);

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
        /// Created by  : Sushil Kumar on 30th Sep 2017
        /// Description :  Photos page for mobile
        /// </summary>
        /// <param name="makeMasking"></param>
        /// <param name="modelMasking"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("m/photos/{makeMasking}-bikes/{modelMasking}/")]
        public ActionResult Model_Mobile(string makeMasking, string modelMasking, string q)
        {
            PhotosPage obj = new PhotosPage(makeMasking, modelMasking, _objModelCache, _objModelMaskingCache, _objModelEntity, _objCityCache, _objGenericBike, _objVersionCache, _objVideos);

            if (obj.Status.Equals(StatusCodes.ContentFound))
            {
                obj.IsMobile = true;
                PhotosPageVM objData = obj.GetData(30, 6, q);
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

		[Route("photos/")]
		public ActionResult Index()
		{
            // false sent for Desktop view
            Bikewale.Models.Photos.v1.PhotosPage _objData = new Bikewale.Models.Photos.v1.PhotosPage(false, _objMakeCache);
            Bikewale.Models.Photos.v1.PhotosPageVM obj = _objData.GetData();
			return View(obj);
		}

		[Route("m/photos/")]
		public ActionResult Index_Mobile()
		{
			ModelBase objModel = new ModelBase();
			return View(objModel);
		}

		[Route("photos/make/")]
		public ActionResult Make()
		{
			ModelBase objModel = new ModelBase();
			return View(objModel);
		}

		[Route("m/photos/make/")]
		public ActionResult Make_Mobile()
		{
			ModelBase objModel = new ModelBase();
			return View(objModel);
		}
    }
}