using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private uint _campaignId, _dealerId;

        public ConfigurePropertiesModel(uint campaignId, uint dealerId, IManufacturerCampaignRepository manufacurerCampaignRepo)
        {
            _manufacurerCampaignRepo = manufacurerCampaignRepo;
            _campaignId = campaignId;
            _dealerId = dealerId;
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
                objData.CampaignId = _campaignId;
                objData.NavigationWidget = new Bikewale.ManufacturerCampaign.Entities.NavigationWidgetEntity();
                objData.NavigationWidget.ActivePage = 2;
                objData.NavigationWidget.CampaignId = _campaignId;
                objData.NavigationWidget.DealerId = _dealerId;
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "BikewaleOpr.Models.ManufacturerCampaign.ConfigurePropertiesModel.GetData()");
            }
            return objData;
        }

        public bool SaveData(CampaignPropertiesVM objModel)
        {
            //FormaModelData(objModel);
            bool isSaved = false;
            try
            {
                _manufacurerCampaignRepo.SaveManufacturerCampaignProperties(objModel);
                isSaved = true;
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "BikewaleOpr.Models.ManufacturerCampaign.ConfigurePropertiesModel.SaveData()");
            }
            return isSaved;
        }

        private void FormaModelData(CampaignPropertiesVM objModel)
        {
            objModel.FormattedHtmlMobile = WebUtility.HtmlEncode(objModel.FormattedHtmlMobile);
            objModel.FormattedHtmlDesktop = WebUtility.HtmlEncode(objModel.FormattedHtmlDesktop);
        }
    }
   
}