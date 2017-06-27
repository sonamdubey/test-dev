using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Bikewaleopr.ManufacturerCampaign.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.ManufacturerCampaign
{
    public class ConfigureCampaignPopup
    {
        private IManufacturerCampaignRepository _manufacurerCampaignRepo;
        public ConfigureCampaignPopup(IManufacturerCampaignRepository manufacurerCampaignRepo)
        {

            _manufacurerCampaignRepo = manufacurerCampaignRepo;
        }

        public ManufacturerCampaignPopup GetData(uint campaignId)
        {
            ManufacturerCampaignPopup obj = new ManufacturerCampaignPopup();
            try
            {
                obj = _manufacurerCampaignRepo.getManufacturerCampaignPopup(campaignId);
                obj.CampaignId = campaignId;
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Models.ManufacturerCampaign.GetData campaignId: {0}",campaignId));
            }
            return obj;
        }
    }
}