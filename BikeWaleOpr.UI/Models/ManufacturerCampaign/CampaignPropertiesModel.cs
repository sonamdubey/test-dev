using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
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
        private uint _dealerId, _campaignId;

        public ConfigurePropertiesModel(uint dealerId, uint campaignId, IManufacturerCampaignRepository manufacurerCampaignRepo)
        {
            _manufacurerCampaignRepo = manufacurerCampaignRepo;
            _campaignId = campaignId;
        }


        public CampaignPropertyEntity getData()
        {
            CampaignPropertyEntity objData = null;
            try
            {
                objData = new CampaignPropertyEntity();
                objData = _manufacurerCampaignRepo.GetManufacturerCampaignProperties(_campaignId);
            }
            catch (Exception ex)
            {

            }
            return objData;
        }

    }
   
}