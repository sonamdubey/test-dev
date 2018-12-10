using AutoMapper;
using Carwale.DTOs.LeadForm;
using Carwale.Entity.Campaigns;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.LeadForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.BL.LeadForm
{
    public class LeadFormAdapter : ILeadFormAdapter
    {
        private readonly ICampaign _campaignBl;
        private readonly ICarModelCacheRepository _carModelCache;
        public LeadFormAdapter(ICampaign campaignBl, ICarModelCacheRepository carModelCache)
        {
            _campaignBl = campaignBl;
            _carModelCache = carModelCache;
        }
        public LeadFormDto GetDealerLeadFormDetails(DealerLeadFormInput campaignInput)
        {
            Location locationObj = new Location
            {
                CityId = campaignInput.CityId,
                CityName = campaignInput.CityName,
                AreaId = campaignInput.AreaId,
                ZoneId = campaignInput.ZoneId
            };
            LeadFormDto leadFormDetail = Mapper.Map<LeadFormDto>(campaignInput);
            leadFormDetail.CustLocation = locationObj;
            Campaign campaign = _campaignBl.GetCampaignByCarLocation(campaignInput.ModelId, locationObj, campaignInput.PlatformId, false, (int)Application.CarWale, campaignInput.CampaignId);
            if (campaign != null && campaign.DealerId > 0)
            {
                leadFormDetail.CampaignDetails = Mapper.Map<SponsoredDealer>(campaign);
                leadFormDetail.CampaignDetails.PredictionData = new PredictionModelResponse();
                leadFormDetail.CampaignDetails.PredictionData.Label = campaignInput.PredictionLabel;
                leadFormDetail.CampaignDetails.PredictionData.Score = campaignInput.PredictionScore;
            }
            leadFormDetail.CarDetails = _carModelCache.GetModelDetailsById(campaignInput.ModelId);
            return leadFormDetail;
        }
    }
}
