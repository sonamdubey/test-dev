using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.Interface.ManufacturerCampaign
{
    public interface IManufacturerCampaign
    {
         List<MfgCityEntity> GetManufacturerCities();
         List<MfgCampaignRulesEntity> FetchManufacturerCampaignRules(int campaignId);
         bool SaveManufacturerCampaignRules(MfgNewRulesEntity MgfRules);
         bool DeleteManufacturerCampaignRules(int userId, string ruleIds);
        
        void SearchCampaign();
    }
}
