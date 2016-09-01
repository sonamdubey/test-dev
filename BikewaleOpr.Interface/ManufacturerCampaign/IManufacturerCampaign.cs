using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Entity.ContractCampaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.ManufacturerCampaign
{
    public interface IManufacturerCampaignRepository
    {
        bool UpdateCampaignStatus(uint id, bool isactive);
        IEnumerable<ManufacturerEntity> GetManufacturersList();
        IEnumerable<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerid);
        List<MfgCityEntity> GetManufacturerCities();
         List<MfgCampaignRulesEntity> FetchManufacturerCampaignRules(int campaignId);
         bool SaveManufacturerCampaignRules(MfgNewRulesEntity MgfRules);
         bool DeleteManufacturerCampaignRules(int userId, string ruleIds);
        /// <summary>
        /// Created by : Sajal Gupta
        /// Description : This interface contains declaration of method used in Manufacturer Campaign ;
        /// </summary>
        int InsertBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId);
        void UpdateBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId, int campaignId, List<ManuCamEntityForTemplate> objList);
        void SaveManufacturerCampaignTemplate(List<ManuCamEntityForTemplate> objList, int userId, int campaignId);
       List<BikewaleOpr.Entity.ManufacturerCampaign.ManufacturerCampaignEntity> FetchCampaignDetails(int campaignId);
         bool ReleaseCampaignMaskingNumber(int campaignId);
    }
}
