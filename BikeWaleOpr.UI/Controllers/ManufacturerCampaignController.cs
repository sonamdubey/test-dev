
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
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
    [Authorize]
    public class ManufacturerCampaignController : Controller
    {
        private IManufacturerCampaignRepository _manufacurerCampaignRepo;

        public ManufacturerCampaignController (IManufacturerCampaignRepository manufacurerCampaignRepo)
        {
            _manufacurerCampaignRepo = manufacurerCampaignRepo;
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

        [Route("manufacturercampaign/information/{dealerId}")]
        public ActionResult ConfigureCampaign(uint dealerId, uint? campaignId)
        {
            ConfigureCampaignPageModel objModel = new ConfigureCampaignPageModel(dealerId, (campaignId.HasValue ? campaignId.Value : 0), _manufacurerCampaignRepo);
            ManufacturerCampaignInformationModel objData = objModel.getData();
            return View(objData);
        }
        
        [Route("manufacturercampaign/properties/")]
        public ActionResult ConfigureCampaignProperties(uint ? campaignId)
        {
            CampaignPropertyEntity retData = new CampaignPropertyEntity();
            if (campaignId.HasValue)
            {
                ConfigurePropertiesModel objvm = new ConfigurePropertiesModel(Convert.ToUInt32(campaignId), _manufacurerCampaignRepo);
                retData = objvm.GetData();
            }
            return View(retData);
        }
        [HttpPost]
        [Route("manufacturercampaign/saveproperties/{campaignId}/")]
        public ActionResult SaveConfiguredProperties(CampaignPropertiesVM model, uint campaignId)
        {
            ConfigurePropertiesModel objvm = new ConfigurePropertiesModel(campaignId, model, _manufacurerCampaignRepo);
            objvm.GetData();
            return RedirectToAction("ConfigureCampaignProperties");
        }

        [Route("manufacturercampaign/popup/")]
        public ActionResult ConfigureCampaignPopup()
        {
            return View();
        }
    }
}