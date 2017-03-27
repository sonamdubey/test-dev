using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Models;
using Bikewale.Models.Mobile.Videos;
using System.Web.Mvc;

namespace Bikewale.Controllers.Shared
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 01 Mar 2017
    /// Summary:  Desktop Generic bike controller
    /// </summary>
    public class GenericBikeInfoController : Controller
    {

        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _city;
        public GenericBikeInfoController(IBikeInfo bikeInfo, ICityCacheRepository city)
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
        [Route("GenericBikeInfo/BikeInfoCard/")]
        public ActionResult BikeInfoCard(GenericBikeInfoCard bikeInfo)
        {
            uint totalTabs = 4;
            BikeInfoVM objVM = null;
            if (bikeInfo.ModelId > 0)
            {
                BikeInfoWidget model = new BikeInfoWidget(_bikeInfo, _city, bikeInfo.ModelId, bikeInfo.CityId, totalTabs, bikeInfo.PageId);
                objVM = model.GetData();
            }
            return PartialView("~/views/BikeModels/_BikeInfoCard.cshtml", objVM);
        }
    }
}