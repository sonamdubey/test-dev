using AutoMapper;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Enum;
using Google.Protobuf;
using Offers.Protos.ProtoFiles;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carwale.Adapters.Offers
{
    public class VersionOfferAvailableAdapter : ApiGatewayAdapterBase<Carwale.Entity.OffersV1.OfferInput, IEnumerable<int>, VersionIdList>
    {
        private static readonly string _moduleName = ConfigurationManager.AppSettings["OffersModuleName"] ?? string.Empty;
        private static readonly string _methodName = "GetVersionsWithOffers";
        public VersionOfferAvailableAdapter()
            : base(_moduleName, _methodName)
        {

        }

        /// <summary>
        /// Function to convert Entity to GRPC Message which will be passed to the APIGateway
        /// </summary>
        /// <param name="input">OfferInput entity</param>
        /// <returns>Returns GRPC message</returns>
        protected override IMessage GetRequest(Entity.OffersV1.OfferInput input)
        {
            if (input == null)
            {
                return new OfferCriteria();
            }
            var offerCriteria = new OfferCriteria
            {
                ApplicationId = (int)Application.CarWale,
                ModelRule = new ModelRule { ModelId = input.ModelId },
                CityRule = new CityRule { StateId = input.StateId, CityId = input.CityId }
            };

            IMessage message = offerCriteria;
            return message;
        }

        /// <summary>
        /// Function to convert output GRPC message to the output entity
        /// </summary>
        /// <param name="responseMessage">GRPC message</param>
        /// <returns>Returns versionIds with offers</returns>
        protected override IEnumerable<int> BuildResponse(IMessage responseMessage)
        {
            if (responseMessage == null)
            {
                return null;
            }
            var versionIdList = responseMessage as VersionIdList;
            if (versionIdList != null)
            {
                return versionIdList.Id.ToList();
            }
            return new List<int>();
        }
    }
}
