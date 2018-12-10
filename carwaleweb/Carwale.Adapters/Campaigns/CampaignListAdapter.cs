using AutoMapper;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Google.Protobuf;
using ProtoBufClass.Campaigns;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.Adapters.Campaigns
{
    public class CampaignListAdapter : ApiGatewayAdapterBase<DealerAdRequest,List<DealerAd>, CampaignList>
    {
        private static readonly string _moduleName = ConfigurationManager.AppSettings["DealerCampaignModule"] ?? string.Empty;
        private static readonly string _methodName = "GetAllRunningCampaigns";

        private DealerAdRequest _request;

        private readonly IElasticLocation _elasticLocation;
        private readonly ICampaign _campaignBl;
        private readonly ITemplate _campaignTemplateBl;
        private readonly IDealers _dealerBl;
        private readonly IDealerCache _dealersCache;

        /// <summary>
        /// Constructor to initialize the properties required call the GRPC method
        /// </summary>
        public CampaignListAdapter(IElasticLocation elasticLocation, ICampaign campaignBl,
            IDealers dealerBl, ITemplate campaignTemplateBl, IDealerCache dealersCache) : base(_moduleName, _methodName)
        {
            _elasticLocation = elasticLocation;
            _campaignBl = campaignBl;
            _dealerBl = dealerBl;
            _campaignTemplateBl = campaignTemplateBl;
            _dealersCache = dealersCache;
        }

        /// <summary>
        /// Function to convert Entity to GRPC Message which will be passed to the APIGateway.
        /// </summary>
        /// <param name="input">input entity</param>
        /// <returns>Returns GRPC message</returns>
        protected override IMessage GetRequest(DealerAdRequest input)
        {
            _request = input;
            input.Location = _elasticLocation.FormCompleteLocation(input.Location);
            var campaignRequest = Mapper.Map<CampaignRequest>(input);
            return campaignRequest;
        }

        /// <summary>
        /// Function to convert GRPC message to the output entity.
        /// </summary>
        /// <param name="responseMessage">GRPC message</param>
        /// <returns>Returns List of campaigns</returns>
        protected override List<DealerAd> BuildResponse(IMessage responseMessage)
        {
            var campaignList = responseMessage as CampaignList;
            if(campaignList == null || campaignList.Campaigns == null)
            {
                return null;
            }

            var dealerAdList = MapDealerAd(campaignList);
            SetDealerDetails(dealerAdList);
            return dealerAdList;
        }

        /// <summary>
        /// Maps campaignList from microservice to list of DealerAd
        /// </summary>
        /// <param name="campaignList"></param>
        /// <returns></returns>
        private List<DealerAd> MapDealerAd(CampaignList campaignList)
        {
            var dealerAdList = new List<DealerAd>();

            HashSet<int> distinctDealerIds = new HashSet<int>();

            foreach (var campaign in campaignList.Campaigns)
            {
                if (campaign != null && !distinctDealerIds.Contains(campaign.DealerId))
                {
                    var dealerAd = new DealerAd();
                    var dealerDetails = new DealerDetails();
                    var campaignDetails = Mapper.Map<ProtoBufClass.Campaigns.Campaign, Entity.Campaigns.Campaign>(campaign);

                    distinctDealerIds.Add(campaign.DealerId);
                    dealerDetails.DealerId = campaign.DealerId;
                    dealerDetails.Distance = campaign.Distance != 0 ? campaign.Distance : 1;
                    dealerAd.DealerDetails = dealerDetails;
                    dealerAd.Campaign = campaignDetails;
                    dealerAd.CampaignType = CampaignAdType.Pq;
                    dealerAd.FeaturedCarData = new CarVersionDetails { ModelId = _request.ModelId };
                    dealerAdList.Add(dealerAd);
                }
            }

            return dealerAdList;
        }

        /// <summary>
        /// Set dealer details for each campaign in the list
        /// </summary>
        /// <param name="dealerAdList"></param>
        private void SetDealerDetails(List<DealerAd> dealerAdList)
        {
            //getting details of all unique dealers
            var dealerIds = dealerAdList.Select(x => x.DealerDetails.DealerId).ToList();
            Dictionary<int, DealerDetails> dealerDetails = _dealersCache.MultiGetDealerDetails(dealerIds);

            //setting dealerName and area for each campaign
            foreach (DealerAd dealerAd in dealerAdList)
            {
                if (dealerDetails.ContainsKey(dealerAd.DealerDetails.DealerId) && dealerDetails[dealerAd.DealerDetails.DealerId] != null)
                {
                    dealerAd.DealerDetails.DealerArea = dealerDetails[dealerAd.DealerDetails.DealerId].DealerArea;
                    dealerAd.DealerDetails.Name = dealerDetails[dealerAd.DealerDetails.DealerId].Name;
                }
            }

            //sorting campaigns by dealer distance and priority
            dealerAdList = dealerAdList.OrderBy(x => x.DealerDetails.Distance).ThenBy(x => x.Campaign.Priority).ToList();

            //setting infinite distance to -1
            dealerAdList.ForEach(dealerAd =>
                dealerAd.DealerDetails.Distance = dealerAd.DealerDetails.Distance == Int16.MaxValue 
                ? -1 : dealerAd.DealerDetails.Distance
            );
        }
    }
}
