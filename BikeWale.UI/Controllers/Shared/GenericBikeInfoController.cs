using Bikewale.BAL;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Location;
using Bikewale.Models.Mobile.Videos;
using System.Web.Mvc;

namespace Bikewale.Controllers.Shared
{
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

            GenericBikeInfoModel bikeData = null;
            if (bikeInfo.ModelId > 0)
            {
                GenericBikeInfoHelper helper = new GenericBikeInfoHelper(_bikeInfo, _city);
                bikeData = helper.GetDetails(bikeInfo.ModelId, bikeInfo.CityId);
            }
            return PartialView("~/views/shared/_GenericBikeInfoCard.cshtml", bikeData);
        }
    }
}