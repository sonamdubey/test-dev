using System;
using ProtoBufClass.Common;
using Carwale.Utility;
using System.Diagnostics;
using Carwale.DAL.Campaigns;
using Carwale.DAL.ApiGateway;
using System.Configuration;
using Google.Protobuf;
using ProtoBufClass.Campaigns;

namespace Campaigns.DealerCampaignClient
{
    public static class DealerCampaignClient
    {
        private static readonly string _campaignModule = ConfigurationManager.AppSettings["DealerCampaignModule"] ?? string.Empty;

        #region GRPC calls
        public static Campaign GetCampaignDetailsById(CampaignId campaignId)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = campaignId;
            apiGatewayCaller.Add(_campaignModule, "GetCampaignDetailsById", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<Campaign>(0);
        }

        public static PQTemplateMapping GetCampaignTemplateId(PQTemplateMapping PQTemplateMapping)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = PQTemplateMapping;
            apiGatewayCaller.Add(_campaignModule, "GetCampaignTemplateId", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<PQTemplateMapping>(0);
        }

        public static DealersList GetDealersOnMakeCity(MakeCity MakeCity)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = MakeCity;
            apiGatewayCaller.Add(_campaignModule, "GetDealersOnMakeCity", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<DealersList>(0);
        }

        public static Dealer GetPremiumDealerDetails(DealerId DealerId)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = DealerId;
            apiGatewayCaller.Add(_campaignModule, "GetPremiumDealerDetails", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<Dealer>(0);
        }

        public static DealersList GetDealerDetailsByCampaignId(CampaignId CampaignId)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = CampaignId;
            apiGatewayCaller.Add(_campaignModule, "GetDealerDetailsByCampaignId", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<DealersList>(0);
        }

        public static CrossSellCampaignList GetPaidCrossSellCampaign(CrossSellCampaignInput CrossSellCampaignInput)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = CrossSellCampaignInput;
            apiGatewayCaller.Add(_campaignModule, "GetPaidCrossSellCampaign", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<CrossSellCampaignList>(0);
        }

        public static FeaturedVersionsList GetHouseCrossSellVersions(CrossSellCampaignInput CrossSellCampaignInput)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = CrossSellCampaignInput;
            apiGatewayCaller.Add(_campaignModule, "GetHouseCrossSellVersions", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<FeaturedVersionsList>(0);
        }

        public static BoolMessage UpdateCampaignRunningStatus(CampaignStatus CampaignStatus)
        {           
                IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
                IMessage message = CampaignStatus;
                apiGatewayCaller.Add(_campaignModule, "UpdateCampaignRunningStatus", message);
                apiGatewayCaller.Call();

                return apiGatewayCaller.GetResponse<BoolMessage>(0);           
        }

        public static CampaignList GetCampaignsOnModelCityPlatform(PQRule PQRule)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = PQRule;
            apiGatewayCaller.Add(_campaignModule, "GetCampaignsOnModelCityPlatform", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<CampaignList>(0);
        }

        public static Campaign GetCampaign(CampaignRequest request)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = request;
            apiGatewayCaller.Add(_campaignModule, "GetCampaign", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<Campaign>(0);
        }

        public static CampaignList GetAllRunningCampaigns(CampaignRequest request)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = request;
            apiGatewayCaller.Add(_campaignModule, "GetAllRunningCampaigns", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<CampaignList>(0);
        }

        public static PropertyTemplates GetAllTemplatesByPage(TemplateInput templateInput)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = templateInput;
            apiGatewayCaller.Add(_campaignModule, "GetAllTemplatesByPage", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<PropertyTemplates>(0);
        }

        public static CampaignCities GetCampaignCities(CampaignCitiesInput campaignCitiesInput)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = campaignCitiesInput;
            apiGatewayCaller.Add(_campaignModule, "GetCampaignCities", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<CampaignCities>(0);
        }

        public static Campaign GetRunningCampaignDetailsById(CampaignId campaignId)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = campaignId;
            apiGatewayCaller.Add(_campaignModule, "GetRunningCampaignDetailsById", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<Campaign>(0);
        }

        public static SponsoredCarComparisonData GetSponsoredCarComparision(SponsoredComparisonInput sponsoredComparisionInput)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = sponsoredComparisionInput;
            apiGatewayCaller.Add(_campaignModule, "GetSponsoredCarComparision", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<SponsoredCarComparisonData>(0);
        }

        public static BoolMessage IsCampaignAvailable(PQRule pqRule)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = pqRule;
            apiGatewayCaller.Add(_campaignModule, "IsCampaignAvailable", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<BoolMessage>(0);
        }

        public static CampaignList GetCampaignDetailsByCriteria(CampaignCriteria pqRule)
        {
            IApiGatewayCaller apiGatewayCaller = new ApiGatewayCaller();
            IMessage message = pqRule;
            apiGatewayCaller.Add(_campaignModule, "GetCampaignDetailsByCriteria", message);
            apiGatewayCaller.Call();

            return apiGatewayCaller.GetResponse<CampaignList>(0);
        }
        #endregion
    }
}
