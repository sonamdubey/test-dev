using Bikewale.Notifications;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerFacilities;
using BikewaleOpr.Models.DealerFacility;
using System;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created by :  Snehal Dange on 05-08-2017
    /// Description : Added functionality for managing dealer facilities
    /// </summary>
    /// <returns></returns>

    public class DealerFacilitiesController : Controller
    {

        private readonly IDealers _dealerRepo;
        private readonly ILocation _location = null;
        /// <summary>
        /// Constuctor to initialize the dependencies
        /// </summary>
        /// <param name="dealer"></param>
        public DealerFacilitiesController(IDealers dealer, ILocation locationObject)
        {
            _dealerRepo = dealer;
            _location = locationObject;
        }

        /// <summary>
        /// Created by :  Snehal Dange on 05-08-2017
        /// Description :Action to get all facilities for dealer
        /// </summary>
        /// <param name="dealerId"></param> 
        /// <returns></returns>
        [HttpGet, Route("dealers/{dealerId}/facilities/")]
        public ActionResult Index(uint dealerId, uint? cityId, uint? makeId, string dealerName = null)
        {
            ManageDealerFacilityVM viewModel = null;
            DealerFacilitiesModel objModel = new DealerFacilitiesModel(_dealerRepo, _location);
            try
            {
                if (dealerId > 0)
                {
                    viewModel = objModel.GetData(dealerId, cityId.Value, makeId.Value, dealerName);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MakesController/Index");
            }

            return View(viewModel);
        }
    }
}