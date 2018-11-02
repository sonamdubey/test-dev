using Bikewale.BAL.MVC.UI;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   Scooters Controller
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance for comparison list
    /// </summary>
    public class ScootersController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _objBikeModel = null;
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompare _compareScooters = null;
        private readonly IDealerCacheRepository _dealerCache = null;
        private readonly IServiceCenter _serviceCenter = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IBikeSeries _bikeSeries;

        public ScootersController(IBikeMakesCacheRepository bikeMakes, INewBikeLaunchesBL newLaunches, IUpcoming upcoming, IBikeCompare compareScooters, IDealerCacheRepository dealerCache, IBikeMakesCacheRepository objMakeCache, IBikeModels<BikeModelEntity, int> objBikeModel, IBikeMakes<BikeMakeEntity, int> objMakeRepor, IServiceCenter serviceCenter, ICMSCacheContent articles, IVideos videos, IBikeSeries bikeSeries)
        {
            _newLaunches = newLaunches;
            _upcoming = upcoming;
            _compareScooters = compareScooters;
            _objMakeCache = objMakeCache;
            _objBikeModel = objBikeModel;
            _dealerCache = dealerCache;
            _serviceCenter = serviceCenter;
            _bikeMakes = bikeMakes;
            _articles = articles;
            _videos = videos;
            _bikeSeries = bikeSeries;
        }




        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns view model Desktop Scooter landing page
        /// Modified by : Aditi Srivastava on 5 June 2017
        /// Summary     : Added comparison source
        /// </summary>
        /// <returns></returns>
        [Route("scooters/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult Index()
        {
            ScootersIndexPageModel model = new ScootersIndexPageModel(
                _bikeMakes, _objBikeModel, _newLaunches, _upcoming, _compareScooters, _articles, _videos);
            model.BrandTopCount = 10;
            model.PqSource = PQSourceEnum.Desktop_Scooters_Landing_Check_on_road_price;
            model.CompareSource = CompareSources.Desktop_Featured_Compare_Widget;
            model.EditorialTopCount = 3;
            return View(model.GetData());
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns view model Mobile Scooter landing page
        /// Modified by : Aditi Srivastava on 5 June 2017
        /// Summary     : Added comparison source
        /// </summary>
        /// <returns></returns>
        [Route("m/scooters/")]
        public ActionResult Index_Mobile()
        {
            ScootersIndexPageModel model = new ScootersIndexPageModel(
                _bikeMakes, _objBikeModel, _newLaunches, _upcoming, _compareScooters, _articles, _videos);
            model.BrandTopCount = 6;
            model.PqSource = PQSourceEnum.Mobile_Scooters_Landing_Check_on_road_price;
            model.CompareSource = CompareSources.Mobile_Featured_Compare_Widget;
            model.EditorialTopCount = 3;
            return View(model.GetData());
        }

        /// <summary>
        /// Created By :- Subodh Jain 17 March 2017
        /// Summary :- Added statuscode check
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Route("scooters/make/{makeMaskingName}/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult BikesByMake(string makeMaskingName)
        {
            ScootersMakePageModel obj = new ScootersMakePageModel(makeMaskingName, _objBikeModel, _upcoming, _compareScooters, _objMakeCache, _dealerCache, _serviceCenter, _articles, _videos, _bikeSeries);
            obj.EditorialTopCount = 2;
            obj.CompareSource = CompareSources.Desktop_Featured_Compare_Widget;

            ScootersMakePageVM objData = null;

            if (obj.Status == StatusCodes.ContentFound)
            {
                objData = obj.GetData();
                return View(objData);
            }
            else if (obj.Status == StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else if (obj.Status == StatusCodes.RedirectTemporary)
            {
                return Redirect(obj.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }

        }

        /// <summary>
        /// Created By :- Subodh Jain 17 March 2017
        /// Summary :- Added statuscode check
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Route("m/scooters/make/{makemaskingname}/")]
        public ActionResult BikesByMake_Mobile(string makeMaskingName)
        {
            ScootersMakePageModel obj = new ScootersMakePageModel(makeMaskingName, _objBikeModel, _upcoming, _compareScooters, _objMakeCache, _dealerCache, _serviceCenter, _articles, _videos, _bikeSeries);
            obj.EditorialTopCount = 2;
            obj.IsMobile = true;
            obj.CompareSource = CompareSources.Mobile_Featured_Compare_Widget;

            ScootersMakePageVM objData;

            if (obj.Status == StatusCodes.ContentFound)
            {
                objData = obj.GetData();
                return View(objData);
            }
            else if (obj.Status == StatusCodes.RedirectPermanent)
            {
                return Redirect(obj.RedirectUrl);
            }
            else if (obj.Status == StatusCodes.RedirectTemporary)
            {
                return Redirect(obj.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }

        }

        [Route("scooters/otherBrands/")]
        public ActionResult OtherBrands(uint makeId)
        {
            ScooterBrands scooters = new ScooterBrands();
            IEnumerable<BikeMakeEntityBase> otherBrand = scooters.GetOtherScooterBrands(_objMakeCache, makeId, 9);
            return View("~/UI/views/shared/_otherbrands.cshtml", otherBrand);
        }

        [Route("m/scooters/otherBrands/")]
        public ActionResult OtherBrands_Mobile(uint makeId)
        {
            ScooterBrands scooters = new ScooterBrands();
            IEnumerable<BikeMakeEntityBase> otherBrand = scooters.GetOtherScooterBrands(_objMakeCache, makeId, 9);
            return View("~/UI/views/m/shared/_otherbrands.cshtml", otherBrand);
        }


    }
}