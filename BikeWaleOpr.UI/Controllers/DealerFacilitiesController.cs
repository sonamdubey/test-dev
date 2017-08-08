using Bikewale.Notifications;
using BikewaleOpr.Interface;
using BikewaleOpr.Models.DealerFacilities;
using BikewaleOpr.Models.DealerFacility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public DealerFacilitiesController(IDealers dealer)
        {
            _dealerRepo = dealer;
        }


        // GET: DealerFacilities
        [Route("newbikebooking/ManageDealerFacilities/{dealerId}/")]
        public ActionResult ManageDealerFacilities(uint dealerId)
        {
            ManageDealerFacilityVM viewModel = new ManageDealerFacilityVM();
            DealerFacilitiesModel objModel = new DealerFacilitiesModel(_dealerRepo);
            try
            {
                if(dealerId >0)
                {
                    viewModel = objModel.GetData(dealerId);

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