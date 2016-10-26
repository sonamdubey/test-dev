using BikewaleOpr.Entities;
using BikewaleOpr.Entities.ContractCampaign;
using BikewaleOpr.Entities.ManufacturerCampaign;
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
        IEnumerable<MfgCityEntity> GetManufacturerCities();
        IEnumerable<MfgCampaignRulesEntity> FetchManufacturerCampaignRules(int campaignId);
        int SaveManufacturerCampaignRules(MfgNewRulesEntity MgfRules);
        bool DeleteManufacturerCampaignRules(int userId, string ruleIds);
        int InsertBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId);
        bool UpdateBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId, int campaignId, List<ManuCamEntityForTemplate> objList);
        bool SaveManufacturerCampaignTemplate(List<ManuCamEntityForTemplate> objList, int userId, int campaignId);
        List<BikewaleOpr.Entities.ManufacturerCampaign.ManufacturerCampaignEntity> FetchCampaignDetails(int campaignId);
        bool ReleaseCampaignMaskingNumber(int campaignId);
    }
}
