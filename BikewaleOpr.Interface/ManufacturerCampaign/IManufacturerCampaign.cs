using BikewaleOpr.Entities;
using BikewaleOpr.Entity.ManufacturerCampaign;
using BikewaleOpr.Entity.ContractCampaign;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.ManufacturerCampaign
{
    /// <summary>
    /// Created by Subodh Jain 29 aug 2016
    /// Description : Interface for manufactureCampaign
    /// </summary>
    public interface IManufacturerCampaignRepository
    {
        IEnumerable<ManufactureDealerCampaign> SearchManufactureCampaigns(uint dealerid);
        bool UpdateCampaignStatus(uint id, bool isactive);
        IEnumerable<ManufacturerEntity> GetManufacturersList();
        List<MfgCityEntity> GetManufacturerCities();
         List<MfgCampaignRulesEntity> FetchManufacturerCampaignRules(int campaignId);
         bool SaveManufacturerCampaignRules(MfgNewRulesEntity MgfRules);
         bool DeleteManufacturerCampaignRules(int userId, string ruleIds);
         
        
        /// <summary>
        /// Created by : Sajal Gupta
        /// Description : This interface contains declaration of method used in Manufacturer Campaign ;
        /// </summary>
        /// <param name="description"></param>
        /// <param name="isActive"></param>
        /// <param name="maskingNumber"></param>
        /// <param name="dealerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int InsertBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId);
        void UpdateBWDealerCampaign(string description, int isActive, string maskingNumber, int dealerId, int userId, int campaignId, string templateHtml1, int templateId1, string templateHtml2, int templateId2, string templateHtml3, int templateId3, string templateHtml4, int templateId4);
        void SaveManufacturerCampaignTemplate(string templateHtml1, int templateId1, string templateHtml2, int templateId2, string templateHtml3, int templateId3, string templateHtml4, int templateId4, int userId, int campaignId);
        //void SaveManufacturerCampaignTemplateMapping(int campaignId, int templateId, int pageId, int isActive, int userId);
        List<ManufacturerCampaignEntity> FetchCampaignDetails(int campaignId);
    }
}
