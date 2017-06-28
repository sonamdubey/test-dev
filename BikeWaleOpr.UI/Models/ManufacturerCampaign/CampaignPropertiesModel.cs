using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.ManufacturerCampaign
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 22 Jun 2017
    /// Summary: Model for configuring campaign properties
    /// </summary>
    public class ConfigurePropertiesModel
    {

        private IManufacturerCampaignRepository _manufacurerCampaignRepo;
        private CampaignPropertiesVM _objModel;
        private uint _campaignId;

        public ConfigurePropertiesModel(uint campaignId, IManufacturerCampaignRepository manufacurerCampaignRepo)
        {
            _manufacurerCampaignRepo = manufacurerCampaignRepo;
            _campaignId = campaignId;
        }

        public ConfigurePropertiesModel(uint campaignId, CampaignPropertiesVM objModel, IManufacturerCampaignRepository manufacurerCampaignRepo)
        {
            _manufacurerCampaignRepo = manufacurerCampaignRepo;
            _campaignId = campaignId;
            _objModel = objModel;
        }


        public CampaignPropertyEntity GetData()
        {
            CampaignPropertyEntity objData = null;
            try
            {
                objData = new CampaignPropertyEntity();
                objData = _manufacurerCampaignRepo.GetManufacturerCampaignProperties(_campaignId);
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "BikewaleOpr.Models.ManufacturerCampaign.ConfigurePropertiesModel.GetData()");
            }
            return objData;
        }

        public void SaveData(CampaignPropertiesVM objModel)
        {

        }

    }
   
}