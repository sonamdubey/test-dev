using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ContractCampaign;
using BikewaleOpr.Entity.ManufacturerCampaign;
using System.Collections.Generic;

namespace BikewaleOpr.Interface.ManufacturerCampaign
{
    /// <summary>
    /// Created by : Sajal Gupta
    /// Description : This interface contains declaration of method used in Manufacturer Campaign ;
    /// </summary>
    public interface IManufacturerCampaignRepository
    {
        bool UpdateCampaignStatus(uint id, bool isactive);
        IEnumerable<ManufacturerEntity> GetManufacturersList();
        IEnumerable<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerid);
        List<MfgCityEntity> GetManufacturerCities();
        List<MfgCampaignRulesEntity> FetchManufacturerCampaignRules(int campaignId);
        bool SaveManufacturerCampaignRules(MfgNewRulesEntity MgfRules);
        bool DeleteManufacturerCampaignRules(int userId, string ruleIds);
        int InsertBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId);
        void UpdateBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId, int campaignId, List<ManuCamEntityForTemplate> objList);
        void SaveManufacturerCampaignTemplate(List<ManuCamEntityForTemplate> objList, int userId, int campaignId);
        List<BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity> FetchCampaignDetails(int campaignId);
        bool ReleaseCampaignMaskingNumber(int campaignId);
    }
}
