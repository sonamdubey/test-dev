using BikewaleOpr.Entities;
using BikewaleOpr.Entities.ContractCampaign;
using BikewaleOpr.Entities.ManufacturerCampaign;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.ManufacturerCampaign
{
    /// <summary>
    /// Created by : Sajal Gupta
    /// Description : This interface contains declaration of method used in Manufacturer Campaign ;
    /// Modified by:- Subodh Jain 01 march 2017
    /// Summary :-Added  UpdateBWDealerCampaign ,SaveManufacturerCampaignTemplate parameters
    /// </summary>
    public interface IManufacturerCampaignRepository
    {
        bool UpdateCampaignStatus(uint id, bool isactive);
        IEnumerable<ManufacturerEntity> GetManufacturersList();
        IEnumerable<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerid);
        IEnumerable<MfgCityEntity> GetManufacturerCities();
        IEnumerable<MfgCampaignRulesEntity> FetchManufacturerCampaignRules(int campaignId);
        int SaveManufacturerCampaignRules(MfgNewRulesEntity MgfRules);
        bool DeleteManufacturerCampaignRules(int userId, string ruleIds);
        int InsertBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId);
        bool UpdateBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId, int campaignId, List<ManuCamEntityForTemplate> objList, string LeadCapturePopupMessage, string LeadCapturePopupDescription, string LeadCapturePopupHeading, bool pinCodeRequired);
        bool SaveManufacturerCampaignTemplate(List<ManuCamEntityForTemplate> objList, int userId, int campaignId, string LeadCapturePopupMessage, string LeadCapturePopupDescription, string LeadCapturePopupHeading, int dealerId, bool pinCodeRequired);
        List<BikewaleOpr.Entities.ManufacturerCampaign.ManufacturerCampaignEntity> FetchCampaignDetails(int campaignId);
        bool ReleaseCampaignMaskingNumber(int campaignId);
    }
}
