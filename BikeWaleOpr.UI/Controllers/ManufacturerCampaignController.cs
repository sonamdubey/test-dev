using BikewaleOpr.Models;
using BikewaleOpr.Models.ManufacturerCampaign;
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
        // GET: ManufacturerCampaign
        public ActionResult SearchManufacturerCampaign()
        {
            SearchCampaign objSearch = new SearchCampaign();
            if (objSearch != null)
            {
                objSearch.GetData();
                return View();
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }   
        }

        [Route("manufacturercampaign/information/")]
        public ActionResult ConfigureCampaign()
        {
            return View();
        }
        
        [Route("manufacturercampaign/properties/")]
        public ActionResult ConfigureCampaignProperties()
        {
            return View();
        }
        [HttpPost]
        [Route("manufacturercampaign/saveproperties/")]
        public ActionResult SaveConfiguredProperties(CampaignPropertiesVM model)
        {
            return RedirectToAction("ConfigureCampaignProperties");
        }

        [Route("manufacturercampaign/popup/")]
        public ActionResult ConfigureCampaignPopup()
        {
            return View();
        }
    }
}