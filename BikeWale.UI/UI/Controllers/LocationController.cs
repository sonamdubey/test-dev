using Bikewale.Entities.Location;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by  :   Sumit Kate on 27 Mar 2017
    /// Description :   Location Controller includes views related to Location functionality
    /// </summary>
    public class LocationController : Controller
    {
        /// <summary>
        /// Created by  :   Sumit Kate on 27 Mar 2017
        /// Description :   ChangeLocation action method for Desktop
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        [Route("location/change/{cityId}/{cityName}")]
        public ActionResult ChangeLocation(uint cityId, string cityName)
        {
            ChangeLocationPopupVM objVM = (new ChangeLocationPopup(new GlobalCityAreaEntity() { CityId = cityId, City = cityName })).GetData();
            return PartialView("~/UI/views/Location/_changelocation.cshtml", objVM);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 27 Mar 2017
        /// Description :   Change Location action method for Mobile
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        [Route("m/location/change/{cityId}/{cityName}")]
        public ActionResult ChangeLocation_Mobile(uint cityId, string cityName)
        {
            ChangeLocationPopupVM objVM = (new ChangeLocationPopup(new GlobalCityAreaEntity() { CityId = cityId, City = cityName })).GetData();
            return PartialView("~/UI/views/Location/_changelocation_mobile.cshtml", objVM);
        }
    }
}