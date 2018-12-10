using Carwale.DTOs;
using Carwale.DTOs.Campaigns;
using Carwale.Entity;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using System.Collections.Generic;
using Carwale.Entity.Geolocation;

namespace Carwale.Interfaces.Campaigns
{
    /// <summary>
    /// For getting alternate Cars with active campaigns on submitting a lead
    /// Created: Vicky Lund, 01/12/2015
    /// </summary>
    /// <returns></returns>
    public interface ICampaignRecommendationsBL
    {
        List<CampaignRecommendationEntity> GetCampaignRecommendationsByLead(string historyModelList, string mobileNumber, int noOfRecommendations);
        List<CampaignRecommendationEntity> SimilarCampaignRecommend(int modelId, Location locationObj, int subsegmentRange, int recommendationCount, bool isSameBodystyle);
        void FilterBySubsegment(ref List<CarModelDetails> modelDetailList, int segment, int range);
        List<MakeModelEntity> GetPQRecommendations(string cwcCookie, string modelList, int referenceModel, int noOfRecommendations, Location locationObj, int platformId);
        List<CampaignRecommendation> GetCampaignRecommendation(string mobile, int recommendationCount, CampaignInput campaignInput, string cwcCookie, bool boost, bool isCheckRecommendation = true, bool isSameMakeFilter = true);
    }
}
