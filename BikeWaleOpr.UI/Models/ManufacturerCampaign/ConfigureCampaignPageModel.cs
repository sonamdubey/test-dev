﻿using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using BikewaleOpr.Interface.ContractCampaign;
using System;

namespace BikewaleOpr.Models.ManufacturerCampaign
{
    /// <summary>
    /// Creeated by Sajal Gupta on 23-06-2017 To populate manufacturer campaign information page - Screen 1
    /// </summary>
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
                objData.CampaignInformation = _manufacurerCampaignRepo.GetManufacturerCampaign(_dealerId, _campaignId);
                objData.CampaignInformation.DealerDetails.Id = _dealerId;
                objData.CampaignId = _campaignId;
                objData.MaskingNumbers = _contractCampaign.GetAllMaskingNumbers(_dealerId);
                objData.NavigationWidget = new NavigationWidgetEntity();
                objData.NavigationWidget.ActivePage = 1;
                objData.NavigationWidget.DealerId = _dealerId;
                objData.NavigationWidget.CampaignId = _campaignId;
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "ConfigureCampaignPageModel.GetData");
            }
            return objData;
        }


    }
}