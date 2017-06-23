
﻿
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using BikewaleOpr.common.ContractCampaignAPI;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Interface.ContractCampaign;
using BikewaleOpr.Models;
using BikewaleOpr.Models.ManufacturerCampaign;

﻿using Bikewale.ManufacturerCampaign.Entities.Models;
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


        private readonly IManufacturerCampaign _objManufacturer = null;

        public ManufacturerCampaignController(IManufacturerCampaign objManufacturer)
        {

            _objManufacturer = objManufacturer;
        }

        // GET: ManufacturerCampaign
        [Route("manufacturercampaign/search/index/")]
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

        [Route("manufacturercampaign/popup/")]
        public ActionResult ConfigureCampaignPopup()
        {
            return View();
        }
    }
}