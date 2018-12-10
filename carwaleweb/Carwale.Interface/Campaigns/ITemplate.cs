using Carwale.Entity.Campaigns;
using Carwale.Entity.Common;
using Carwale.Entity.Dealers;
using Carwale.Entity.Template;
using System.Collections.Generic;
using Carwale.Entity.PageProperty;

namespace Carwale.Interfaces.Campaigns
{
    public interface ITemplate
    {
        SponsoredDealer CampaignDealerInfo(SponsoredDealer sponsoredDealer, string pageId, int sourceId);
        Templates GetCampaignTemplates(Entity.Campaigns.Campaign campaign, string pageId, int sourceId);
        int GetCampaignGroupTemplateId(int assignedTemplateId, int assignedGroupId, int platformId);
        void ResolveTemplate(Templates template, ref SponsoredDealer dealerSponsoredDetails);        
        string GetCampaignTemplate(Entity.Campaigns.Campaign campaign, string pageId, int sourceId);
        string GetRendredContent<T>(string templateName, string template, T model);
        Dictionary<int, IdName> GetTemplatesByPage<T>(CampaignInputv2 campaignInput, T model);
        Templates GetTemplateById(int id);
        Templates GetEmiCalculatorTemplate();
        List<PageProperty> GetPageProperties(int platformId, int pageId, Campaign campaignDetail);
        Dictionary<int, IdName> AddTemplatesToDict<T>(List<PageTemplates> pageTemplates, T model, int pageId);
        void AddTemplatesToDict<T>(Templates template, int propertyId, T model,ref Dictionary<int, IdName> templateDictionary);
    }
}
