using Bikewale.ManufacturerCampaign.Entities.Models;
using Bikewale.ManufacturerCampaign.Interface;
using BikewaleOpr.Models;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    public class ManufacturerCampaignController : Controller
    {
        private readonly IManufacturerCampaign _objManufacturer = null;

        public ManufacturerCampaignController(IManufacturerCampaign objManufacturer)
        {

            _objManufacturer = objManufacturer;
        }
        // GET: ManufacturerCampaign
        [Route("manu/Index/")]
        public ActionResult SearchManufacturerCampaign()
        {
            SearchCampaign objSearch = new SearchCampaign(_objManufacturer);
            if (objSearch != null)
            {
                SearchManufacturerCampaignVM objVM = new SearchManufacturerCampaignVM();
                objVM=objSearch.GetData();
                return View(objVM);
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }   
        }
    }
}