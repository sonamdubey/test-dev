using Bikewale.ManufacturerCampaign.Entities;
using BikewaleOpr.Models;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BikewaleOpr.Models.ManufacturerCampaign;
using Bikewale.ManufacturerCampaign.Interface;

namespace BikewaleOpr.Controllers
{
    public class ManufacturerCampaignController : Controller
    {
        private readonly IManufacturerCampaign _mfgCampaign = null;

        public ManufacturerCampaignController(IManufacturerCampaign mfgCampaign)
        {
            _mfgCampaign = mfgCampaign;
        }
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

        [Route("manufacturercampaign/popup/")]
        public ActionResult ConfigureCampaignPopup()
        {
            return View();
        }

        [Route("manufacturercampaign/rules/campaignId/{campaignId}")]
        public ActionResult ManufacturerCampaignRules(uint campaignId)
        {
            MfgCampaignRules obj = new MfgCampaignRules(_mfgCampaign);
            obj.CampaignId = campaignId;
            ManufacturerCampaignRulesVM objData = obj.GetData();
            return View(objData);
        }

        [Route("manufacturercampaign/rules/campaignid/{campaignId}/add/"), HttpPost]
        public ActionResult AddManufacturerCampaignRules(uint campaignId, string modelIds, string stateIds, string cityIds, bool isAllIndia, uint userId)
        {
            bool isSuccess = false;
            isSuccess = _mfgCampaign.SaveManufacturerCampaignRules(campaignId, modelIds, stateIds, cityIds, isAllIndia, userId);
            if(isSuccess)
                TempData["msg"] = "Rules added successfully!";
            else
                TempData["msg"] = "Could not add rules";
            return RedirectToAction("ManufacturerCampaignRules", routeValues: new { campaignId = campaignId });
        }
    }
}