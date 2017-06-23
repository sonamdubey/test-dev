using Bikewale.ManufacturerCampaign.Interface;
using BikewaleOpr.Interface.ContractCampaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.ManufacturerCampaign
{
    public class ConfigureCampaignPageModel
    {
        private IManufacturerCampaignRepository _manufacurerCampaignRepo;
        private IContractCampaign _contractCampaign;
        private uint _dealerId, _campaignId;

        public ConfigureCampaignPageModel(uint dealerId, uint campaignId, IManufacturerCampaignRepository manufacurerCampaignRepo, IContractCampaign contractCampaign)
        {
            _manufacurerCampaignRepo = manufacurerCampaignRepo;
            _contractCampaign = contractCampaign;
            _dealerId = dealerId;
            _campaignId = campaignId;
        }

        
        public ManufacturerCampaignInformationModel getData()
        {
            ManufacturerCampaignInformationModel objData = null;
            try
            {
                objData = new ManufacturerCampaignInformationModel();
                objData.CampaignInformation = _manufacurerCampaignRepo.getManufacturerCampaign(_dealerId, _campaignId);
                objData.CampaignInformation.DealerDetails.Id = _dealerId;
                objData.CampaignId = _campaignId;
                objData.MaskingNumbers = _contractCampaign.GetAllMaskingNumbers(_dealerId);
            }
            catch(Exception ex)
            {

            }
            return objData;
        }


    }
}