using AutoMapper;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Enum;
using Carwale.Entity.Offers;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Offers.Protos.ProtoFiles;
using System.Collections.Generic;
using System.Configuration;

namespace Carwale.Adapters.Offers
{
    public class OfferAvailabilityAdapter : ApiGatewayAdapterBase<OfferAvailabilityInput, List<Carwale.Entity.OffersV1.OfferAvailabiltyDetails>, OfferAvailabiltyDetailsList>
    {
        public OfferAvailabilityAdapter()
            : base(ConfigurationManager.AppSettings["OffersModuleName"], "CheckModelCityOfferAvailability")
        {

        }

        protected override IMessage GetRequest(OfferAvailabilityInput offerAvailabilityInput)
        {
            var offerCriteriaList = new OfferCriteriaList();
            if(offerAvailabilityInput.CityId < 1)
            {
                offerAvailabilityInput.CityId = -1;
                offerAvailabilityInput.StateId = -1;
            }
            foreach (var input in offerAvailabilityInput.ModelIds)
            {
                var offerInput = new OfferCriteria
                {
                    ApplicationId = (int)Application.CarWale,
                    RequiredOfferCount = 1, //Only one offer is required currently
                    ModelRule = new ModelRule { ModelId = input, VersionId = -1 }, // fetching offer for any version providing model
                    CityRule = new CityRule { StateId = offerAvailabilityInput.StateId, CityId = offerAvailabilityInput.CityId }
                };
                offerCriteriaList.ListOfferCriteria.Add(offerInput);
            }
            IMessage message = offerCriteriaList;

            return message;
        }

        protected override List<Carwale.Entity.OffersV1.OfferAvailabiltyDetails> BuildResponse(IMessage responseMessage)
        {
            if (responseMessage != null)
            {
                var offerAvailabiltyDetailsList = responseMessage as OfferAvailabiltyDetailsList;
                return Mapper.Map<RepeatedField<OfferAvailabiltyDetails>, List<Carwale.Entity.OffersV1.OfferAvailabiltyDetails>>(offerAvailabiltyDetailsList.ListOfferAvailabiltyDetails);
            }
            return null;
        }
    }
}
