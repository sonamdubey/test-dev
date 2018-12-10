using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Campaigns;
using Google.Protobuf;
using ProtoBufClass.Campaigns;
using ProtoBufClass.Common;
using System.Configuration;

namespace Carwale.Adapters.Campaigns
{
    public class IsCityCampaignAdapter : ApiGatewayAdapterBase<DealerAdRequest, bool, BoolMessage>
    {
        private static readonly string _moduleName = ConfigurationManager.AppSettings["DealerCampaignModule"] ?? string.Empty;
        private static readonly string _methodName = "IsCampaignAvailable";

        /// <summary>
        /// Constructor to initialize the properties required call the GRPC method
        /// </summary>
        public IsCityCampaignAdapter() : base(_moduleName, _methodName)
        {}

        /// <summary>
        /// Function to convert Entity to GRPC Message which will be passed to the APIGateway
        /// </summary>
        /// <param name="input">input entity</param>
        /// <returns>Returns GRPC message</returns>
        protected override IMessage GetRequest(DealerAdRequest input)
        {
            return new PQRule
            {
                Model = new Item { Id = input.ModelId },
                City = new Item { Id = input.Location.CityId },
                Platform = new Item { Id = input.PlatformId },
                ApplicationId = input.ApplicationId
            };
        }

        /// <summary>
        /// Function to convert GRPC message to the respective entity
        /// </summary>
        /// <param name="responseMessage">GRPC message</param>
        /// <returns>Returns campaign</returns>
        protected override bool BuildResponse(IMessage responseMessage)
        {
            if(responseMessage == null)
            {
                return false;
            }

            return (responseMessage as BoolMessage).Status;
        }
    }
}
