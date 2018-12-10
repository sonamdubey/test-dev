using AutoMapper;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.Dealers;
using Carwale.Interfaces.Geolocation;
using Carwale.Utility;
using Google.Protobuf;
using ProtoBufClass.Campaigns;
using System.Configuration;

namespace Carwale.Adapters.Campaigns
{
    public class CampaignAdapter : ApiGatewayAdapterBase<DealerAdRequest, DealerAd, ProtoBufClass.Campaigns.Campaign>
    {
        private static readonly string _moduleName = ConfigurationManager.AppSettings["DealerCampaignModule"] ?? string.Empty;
        private static readonly string _methodName = "GetCampaign";
        private DealerAdRequest _request;

        private readonly IElasticLocation _elasticLocation;
        private readonly ICampaign _campaignBl;
        private readonly ITemplate _campaignTemplateBl;
        private readonly IDealers _dealerBl;

        /// <summary>
        /// Constructor initializes the properties required to call the GRPC method
        /// </summary>
        public CampaignAdapter(IElasticLocation elasticLocation, ICampaign campaignBl, 
            IDealers dealerBl, ITemplate campaignTemplateBl) : base(_moduleName, _methodName)
        {
            _elasticLocation = elasticLocation;
            _campaignBl = campaignBl;
            _dealerBl = dealerBl;
            _campaignTemplateBl = campaignTemplateBl;
        }

        /// <summary>
        /// Function to convert Entity to GRPC Message which will be passed to the APIGateway
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
        /// Function to convert output GRPC message to the output entity
        /// </summary>
        /// <param name="responseMessage">GRPC message</param>
        /// <returns>Returns campaign</returns>
        protected override DealerAd BuildResponse(IMessage responseMessage)
        {
            if (responseMessage == null)
            {
                return null;
            }

            var dealerAd = new DealerAd();
            dealerAd.Campaign = Mapper.Map<ProtoBufClass.Campaigns.Campaign, Entity.Campaigns.Campaign>(responseMessage as ProtoBufClass.Campaigns.Campaign);

            //setting contact numbers
            SetContactNumber(dealerAd);

            //setting up dealerAd properties
            dealerAd.DealerDetails = _dealerBl.GetDealerDetailsOnDealerId(dealerAd.Campaign.DealerId);
            dealerAd.FeaturedCarData = new CarVersionDetails { ModelId = _request.ModelId };
            dealerAd.PageProperty = _campaignTemplateBl.GetPageProperties(_request.PlatformId, _request.PageId, dealerAd.Campaign);
            dealerAd.CampaignType = CampaignAdType.Pq;

            //setting campaign persistence
            SetPersistence(dealerAd);

            return dealerAd;
        }

        /// <summary>
        /// Set contact number for Campaign based on platform
        /// </summary>
        /// <param name="dealerAd"></param>
        private void SetContactNumber(DealerAd dealerAd)
        {
            if ((_request.PlatformId == (int)Platform.CarwaleAndroid || _request.PlatformId == (int)Platform.CarwaleiOS) 
                && string.IsNullOrWhiteSpace(dealerAd.Campaign.ContactNumber))
            {
                dealerAd.Campaign.ContactNumber = CWConfiguration.tollFreeNumber;
            }
        }

        /// <summary>
        /// Set persistence cookie for campaign
        /// </summary>
        /// <param name="dealerAd"></param>
        private void SetPersistence(DealerAd dealerAd)
        {
            if (_request.PlatformId == (int)Platform.CarwaleDesktop || _request.PlatformId == (int)Platform.CarwaleMobile)
            {
                _campaignBl.SetPersistedCampaign(_request.ModelId, _request.Location, dealerAd.Campaign.Id);
            }
        }

    }
}
