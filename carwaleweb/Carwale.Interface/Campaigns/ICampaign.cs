using Carwale.DTOs.Campaigns;
using Carwale.DTOs.Geolocation;
using Carwale.Entity.Campaigns;
using Carwale.Entity.Dealers;
using System.Collections.Generic;
using System.Web;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Template;
using System;
using Carwale.Entity.Enum;

namespace Carwale.Interfaces.Campaigns
{
    public interface ICampaign
    {
        Campaign GetCampaignDetails(int campaignId);
        Campaign GetCampaignWithScore(Entity.Campaigns.Campaign campaign, HttpRequest httpRequest, int modelId, int cityId, int zoneId, int platformId);
        List<Campaign> GetAllCampaign(int modelId, Location locationObj, int platformId, bool usePriority = true);
        List<Campaign> GetAllAvailableCampaign(int modelId, Location locationObj, int platformId, int applicationId, int count = 0);
        IEnumerable<CampaignDTOv2> GetAllCampaignV2(int modelId, Location locationObj, int platformId, int applicationId, int count = 0);
        CampaignLeadDTO GetCampaignLeadInfo(int leadId);
        SponsoredDealer GetSponsorDealerAd(int modelId, int platformId, Location location);
        int GetPersistedCampaign(int modelId, Location locationObj);
        bool SetPersistedCampaign(int modelId, Location location, int campaignId);
        bool ChangeCampaignRunningStatus(int campaignId, CampaignStatus status);
        List<Campaign> GetAllRunningCampaignsOnCriteria(int modelId, Location locationObj, int platformId, int applicationId, int count);
        CvlDetailsDTO GetCampaignCvlDetails(int campaignId);
        List<CityDTO> GetCampaignCities(int campaignId, int modelId);
        Campaign GetDealerCampaign(int modelId, Location location, int platformId, int campaignId);     
        bool IsCityCampaignExist(int modelId, Location locationObj, int platformId, int applicationId);
        int FetchCampaignIdByDealerId(int dealerId);
        List<Campaign> FilterCampaignByPriority(List<Campaign> campaign);
        List<Tuple<int, Carwale.Entity.Campaigns.Campaign>> GetMultipleCampaign(List<int> modelIds, Location locationObj, int platformId, int count, int applicationId, bool isCheckRecommendation = true);
        Campaign GetCampaignByCarLocation(int modelId, Location locationObj, int platformId, bool recommendation, int applicationId = (int)Application.CarWale, int campaignId = 0);
        Dictionary<int, int> FetchCampaignByDealers(List<int> dealerIds);
        List<DealerAd> GetAllRunningCampaigns(int modelId, Location locationObj, int platformId, int applicationId, bool isDealerLocator, bool dealerAdminFilter);
        bool ValidateLocationOnArea(Location locationObj);
        bool IsTestDriveCampaign(int campaignId);
    }
}
