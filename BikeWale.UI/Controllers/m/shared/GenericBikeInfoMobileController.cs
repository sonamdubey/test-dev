using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Models;
using Bikewale.Models.Mobile.Videos;
using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile.Shared
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 01 Mar 2017
    /// Summary:  Mobile Generic bike controller
    /// </summary>
    public class GenericBikeInfoMobileController : Controller
    {

        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _city;
        public GenericBikeInfoMobileController(IBikeInfo bikeInfo, ICityCacheRepository city)
        {
            _bikeInfo = bikeInfo;
            _city = city;
        }
        // GET: GenericBikeInfo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 01 Mar 2017
        /// Summary: Partial view to show generic bike info card
        /// </summary>
        [Route("m/GenericBikeInfo/BikeInfoCard/")]
        public ActionResult BikeInfoCard(GenericBikeInfoCard bikeInfo)
        {
            uint tabsCount = 3;
            BikeInfoVM objVM = null;
            if (bikeInfo.ModelId > 0)
            {
                BikeInfoWidget model = new BikeInfoWidget(_bikeInfo, _city, bikeInfo.ModelId, bikeInfo.CityId, tabsCount, bikeInfo.PageId);
                objVM = model.GetData();
            }
            return PartialView("~/views/BikeModels/_BikeInfoCard_Mobile.cshtml", objVM);
        }
    }
}