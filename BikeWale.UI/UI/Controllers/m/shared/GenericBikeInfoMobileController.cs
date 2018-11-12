using Bikewale.Entities.BikeData;
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
        private readonly IBikeSeries _bikeSeries = null;
        private readonly IBikeModels<BikeModelEntity, int> _models;

        public GenericBikeInfoMobileController(IBikeInfo bikeInfo, ICityCacheRepository city, IBikeModelsCacheRepository<int> modelCache, IBikeSeries bikeSeries, IBikeModels<BikeModelEntity, int> models)
        {
            _bikeInfo = bikeInfo;
            _city = city;
            _bikeSeries = bikeSeries;
            _models = models;
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
                BikeInfoWidget model = new BikeInfoWidget(_bikeInfo, _city, bikeInfo.ModelId, bikeInfo.CityId, tabsCount, bikeInfo.PageId, _models, _bikeSeries);
                objVM = model.GetData();
            }
            return PartialView("~/UI/views/BikeModels/_BikeInfoCard_Mobile.cshtml", objVM);
        }
    }
}