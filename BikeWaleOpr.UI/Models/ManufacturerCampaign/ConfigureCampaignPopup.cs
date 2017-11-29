using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Entities.Models;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using System;

namespace BikewaleOpr.Models.ManufacturerCampaign
{
    public class ConfigureCampaignPopup
    {
        private IManufacturerCampaignRepository _manufacurerCampaignRepo;
        public ConfigureCampaignPopup(IManufacturerCampaignRepository manufacurerCampaignRepo)
        {

            _manufacurerCampaignRepo = manufacurerCampaignRepo;
        }

        public ManufacturerCampaignPopupVM GetData(uint dealerId,uint campaignId)
        {
            ManufacturerCampaignPopupVM obj = new ManufacturerCampaignPopupVM();
            try
            {
               
              
                obj.objPopup = _manufacurerCampaignRepo.getManufacturerCampaignPopup(campaignId);
                obj.objPopup.CampaignId = campaignId;
                obj.NavigationWidget = new NavigationWidgetEntity();
                obj.NavigationWidget.ActivePage = 3;
                obj.NavigationWidget.DealerId = dealerId;
                obj.NavigationWidget.CampaignId = campaignId;
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("BikewaleOpr.Models.ManufacturerCampaign.GetData campaignId: {0}",campaignId));
            }
            return obj;
        }
    }
}