using Bikewale.ManufacturerCampaign.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.ManufacturerCampaign
{
    public class ConfigureCampaignPageModel
    {
        private IManufacturerCampaignRepository _manufacurerCampaignRepo;
        private uint _dealerId, _campaignId;

        public ConfigureCampaignPageModel(uint dealerId, uint campaignId, IManufacturerCampaignRepository manufacurerCampaignRepo)
        {
            _manufacurerCampaignRepo = manufacurerCampaignRepo;
            _dealerId = dealerId;
            _campaignId = campaignId;
        }

        
        public ManufacturerCampaignInformationModel getData()
        {
            ManufacturerCampaignInformationModel objData = null;
            try
            {
                objData = new ManufacturerCampaignInformationModel();
                objData.CampaignInformation = _manufacurerCampaignRepo.GetManufacturerCampaign(_dealerId, _campaignId);
                objData.CampaignInformation.DealerDetails.Id = _dealerId;
                objData.CampaignId = _campaignId;
            }
            catch(Exception ex)
            {

            }
            return objData;
        }
    }
}