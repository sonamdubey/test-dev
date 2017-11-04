using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Entities.Models;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewaleopr.ManufacturerCampaign.Entities;
using BikewaleOpr.common.ContractCampaignAPI;
using BikewaleOpr.Entities.ContractCampaign;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Models;
using BikewaleOpr.Models.ManufacturerCampaign;
using BikeWaleOpr.Common;
using System;
using System.Web.Mvc;

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

        /// <summary>
        /// Modified by :- Subodh Jain 10 july 2017
        /// summary :- Get manufacturer list
        /// </summary>
        /// <returns></returns>
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
                                    
            return Redirect(string.Format("/manufacturercampaign/properties/{0}/?campaignId={1}" , objData.DealerId, campaignId));
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 26 Jun 2017
        /// Summary    : method for fetcthing manufacturer properties
        /// </summary>
        [Route("manufacturercampaign/properties/{dealerid}/")]
        public ActionResult ConfigureCampaignProperties(uint campaignId, uint dealerId)
        {
            ConfigurePropertiesModel objvm = new ConfigurePropertiesModel(campaignId, dealerId, _manufacurerCampaignRepo);
            CampaignPropertyEntity retData = objvm.GetData();
            retData.DealerId = dealerId;
            return View(retData);
        }


        /// <summary>
        /// Created by : Sangram Nandkhile on 26 Jun 2017
        /// Summary    : Post method for saving manufacturer properties
        /// </summary>
        [HttpPost]
        [Route("manufacturercampaign/saveproperties/{dealerid}/")]
        public ActionResult SaveConfiguredProperties(CampaignPropertiesVM model, uint campaignId, uint dealerId)
        {
            ConfigurePropertiesModel objvm = new ConfigurePropertiesModel(campaignId, model, _manufacurerCampaignRepo);
            objvm.SaveData(model);
            return Redirect("/manufacturercampaign/popup/"+ dealerId + "/?campaignId=" + campaignId);
        }

        [Route("manufacturercampaign/popup/{dealerId}")]
        public ActionResult ConfigureCampaignPopup(uint dealerId, uint? campaignId)
        {
            ConfigureCampaignPopup objPopup = new ConfigureCampaignPopup(_manufacurerCampaignRepo);
            ManufacturerCampaignPopupVM objVM = null;
            if (objPopup!=null)
            {
                objVM = objPopup.GetData(dealerId,campaignId ?? 0);
            }
            return View(objVM);
        }

        [HttpPost, Route("manufacturercampaign/save/popup/")]
        public ActionResult saveCampaignPopup([System.Web.Http.FromBody] ManufacturerCampaignPopup objData)
        {
            if (objData != null)
            {
                _manufacurerCampaignRepo.saveManufacturerCampaignPopup(objData);
            }
            return Redirect("/manufacturercampaign/rules/campaignId/" + objData.CampaignId+"?dealerId="+objData.DealerId);
        }

        /// <summary>
        /// Created by : Aditi Srivastava 23 Jun 2017
        /// Summary    : Action method for manufacturer campaign rules page
        /// </summary>
        [Route("manufacturercampaign/rules/campaignId/{campaignId}")]
        public ActionResult ManufacturerCampaignRules(uint campaignId, uint? dealerId)
        {
            ManufacturerCampaignRules obj = new ManufacturerCampaignRules(_manufacurerCampaignRepo);
            if(dealerId.HasValue)
            obj.DealerId = dealerId.Value;
            obj.CampaignId = campaignId;
            ManufacturerCampaignRulesVM objData = obj.GetData();
            return View(objData);
        }

        /// <summary>
        /// Created by : Aditi Srivastava 23 Jun 2017
        /// Summary    : Action method to save new manufacturer campaign rules
        /// </summary>
        [Route("manufacturercampaign/rules/campaignid/{campaignId}/add/"), HttpPost]
        public ActionResult AddManufacturerCampaignRules(uint campaignId, string modelIds, string stateIds, string cityIds, bool isAllIndia, uint userId, uint? dealerId)
        {
            bool isSuccess = false;
            isSuccess = _manufacurerCampaignRepo.SaveManufacturerCampaignRules(campaignId, modelIds, stateIds, cityIds, isAllIndia, userId);
            if(isSuccess)
                TempData["msg"] = "Rules added successfully!";
            else
                TempData["msg"] = "Could not add rules.";
            return RedirectToAction("ManufacturerCampaignRules", routeValues: new { campaignId = campaignId , dealerId= dealerId });
        }
    }
}