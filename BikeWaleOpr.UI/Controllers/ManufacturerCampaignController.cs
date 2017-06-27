using Bikewale.ManufacturerCampaign.Entities;
﻿
using Bikewale.ManufacturerCampaign.Interface;
using BikewaleOpr.common.ContractCampaignAPI;
using BikewaleOpr.Entities.ContractCampaign;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Models;
using BikewaleOpr.Models.ManufacturerCampaign;

﻿using Bikewale.ManufacturerCampaign.Entities.Models;


using BikeWaleOpr.Common;
using System;
using System.Web.Mvc;
using BikewaleOpr.Models.ManufacturerCampaign;
using Bikewale.ManufacturerCampaign.Interface;


namespace BikewaleOpr.Controllers
{
    [Authorize]
    public class ManufacturerCampaignController : Controller
    {

        private IManufacturerCampaignRepository _manufacurerCampaignRepo;
        private IContractCampaign _contractCampaign;

        public ManufacturerCampaignController (IManufacturerCampaignRepository manufacurerCampaignRepo, IContractCampaign contractCampaign)
        {
            _manufacurerCampaignRepo = manufacurerCampaignRepo;
            _contractCampaign = contractCampaign;
        }

        // GET: ManufacturerCampaign
        [Route("manufacturercampaign/search/index/")]
        public ActionResult SearchManufacturerCampaign()
        {
            SearchCampaign objSearch = new SearchCampaign(_manufacurerCampaignRepo);
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

        [Route("manufacturercampaign/information/{dealerId}/")]
        public ActionResult ConfigureCampaign(uint dealerId, uint? campaignId)
        {
            ConfigureCampaignPageModel objModel = new ConfigureCampaignPageModel(dealerId, (campaignId.HasValue ? campaignId.Value : 0), _manufacurerCampaignRepo, _contractCampaign);
            ManufacturerCampaignInformationModel objData = objModel.getData();
            return View(objData);
        }

        [HttpPost, Route("manufacturercampaign/save/campaign/")]
        public ActionResult saveCampaign([System.Web.Http.FromBody] ConfigureCampaignSave objData)
        {
            uint campaignId = _manufacurerCampaignRepo.saveManufacturerCampaign(objData);

            if (objData != null && objData.OldMaskingNumber != null && (objData.MaskingNumber != objData.OldMaskingNumber))
            {
                CwWebserviceAPI CWWebservice = new CwWebserviceAPI();
                CWWebservice.ReleaseMaskingNumber(objData.DealerId, Convert.ToInt32(objData.UserId), objData.OldMaskingNumber);

                ContractCampaignInputEntity ccInputs = new ContractCampaignInputEntity();
                ccInputs.ConsumerId = (int)objData.DealerId;
                ccInputs.DealerType = 2;
                ccInputs.LeadCampaignId = (int)campaignId;
                ccInputs.LastUpdatedBy = Convert.ToInt32(objData.UserId);
                ccInputs.OldMaskingNumber = objData.OldMaskingNumber;
                ccInputs.MaskingNumber = objData.MaskingNumber;
                ccInputs.NCDBranchId = -1;
                ccInputs.ProductTypeId = 3;
                ccInputs.Mobile = objData.MobileNumber;
                ccInputs.SellerMobileMaskingId = -1;

                CWWebservice.AddCampaignContractData(ccInputs);
            }
                                    
            return Redirect("/manufacturercampaign/properties/?campaignId=" + campaignId);
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