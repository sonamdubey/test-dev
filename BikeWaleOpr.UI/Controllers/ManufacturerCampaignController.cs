

using Bikewale.ManufacturerCampaign.Interface;
using BikewaleOpr.common.ContractCampaignAPI;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Models;
using BikewaleOpr.Models.ManufacturerCampaign;

using Bikewale.ManufacturerCampaign.Entities.Models;


using BikeWaleOpr.Common;
using System;
using System.Web.Mvc;
using Bikewaleopr.ManufacturerCampaign.Entities;

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
        public void saveCampaign([System.Web.Http.FromBody] ConfigureCampaignSave objData)
        {
            if (objData != null && objData.OldMaskingNumber != null && (objData.MaskingNumber != objData.OldMaskingNumber))
            {
                CwWebserviceAPI CWWebservice = new CwWebserviceAPI();
                CWWebservice.ReleaseMaskingNumber(objData.DealerId, Convert.ToInt32(objData.UserId), objData.OldMaskingNumber);
            }

            uint campaignId = _manufacurerCampaignRepo.saveManufacturerCampaign(objData);           
        }

        [Route("manufacturercampaign/properties/")]
        public ActionResult ConfigureCampaignProperties()
        {
            return View();
        }

        [Route("manufacturercampaign/popup/{dealerId}/")]
        public ActionResult ConfigureCampaignPopup(uint dealerId,uint? campaignId)
        {
            ConfigureCampaignPopup objPopup = new ConfigureCampaignPopup(_manufacurerCampaignRepo);
            ManufacturerCampaignPopup objVM = null;
            if (objPopup!=null)
            {
                objVM= objPopup.GetData(campaignId??0);
                
            }
            return View(objVM);
        }

        [HttpPost, Route("manufacturercampaign/save/popup/")]
        public void saveCampaignPopup([System.Web.Http.FromBody] ManufacturerCampaignPopup objData)
        {
            if (objData != null)
            {
                _manufacurerCampaignRepo.saveManufacturerCampaignPopup(objData);
            }

          
        }
    }
}