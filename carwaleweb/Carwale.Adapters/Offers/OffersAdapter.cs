using AutoMapper;
using Carwale.DAL.ApiGateway.ApiGatewayHelper;
using Carwale.Entity.Enum;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Offers.Protos.ProtoFiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Adapters.Offers
{
    public class OffersOnCriteria : ApiGatewayAdapterBase<Carwale.Entity.OffersV1.OfferInput, Carwale.Entity.OffersV1.Offer, OfferWithCategoryDetailList>
    {
        public OffersOnCriteria()
            : base(ConfigurationManager.AppSettings["OffersModuleName"], "GetOffersOnCriteria")
        {

        }

        protected override IMessage GetRequest(Carwale.Entity.OffersV1.OfferInput offerInput)
        {
            IMessage message = new OfferCriteria
            {
                ApplicationId = (int)Application.CarWale,
                RequiredOfferCount = 1, //Only one offer is required currently
                ModelRule = new ModelRule { MakeId = offerInput.MakeId, ModelId = offerInput.ModelId, VersionId = offerInput.VersionId },
                CityRule = new CityRule { StateId = offerInput.StateId, CityId = offerInput.CityId }
            };

            return message;
        }

        protected override Carwale.Entity.OffersV1.Offer BuildResponse(IMessage responseMessage)
        {
            var modelNews = responseMessage as OfferWithCategoryDetailList;
            return Mapper.Map<Carwale.Entity.OffersV1.Offer>(modelNews.ListOfferWithCategoryDetail[0]);
        }
    }

    public class OfferAvailability : ApiGatewayAdapterBase<List<Carwale.Entity.OffersV1.OfferInput>, List<Carwale.Entity.OffersV1.OfferAvailabiltyDetails>, OfferAvailabiltyDetailsList>
    {
        public OfferAvailability()
            : base(ConfigurationManager.AppSettings["OffersModuleName"], "CheckModelCityOfferAvailability")
        {

        }

        protected override IMessage GetRequest(List<Carwale.Entity.OffersV1.OfferInput> offerInputList)
        {

            var offerCriteriaList = new OfferCriteriaList();

            foreach (var input in offerInputList)
            {
                var offerInput = new OfferCriteria
                {
                    ApplicationId = (int)Application.CarWale,
                    RequiredOfferCount = 1, //Only one offer is required currently
                    ModelRule = new ModelRule { MakeId = input.MakeId, ModelId = input.ModelId, VersionId = input.VersionId },
                    CityRule = new CityRule { StateId = input.StateId, CityId = input.CityId }
                };
                offerCriteriaList.ListOfferCriteria.Add(offerInput);
            }
            IMessage message = offerCriteriaList;

            return message;
        }

        protected override List<Carwale.Entity.OffersV1.OfferAvailabiltyDetails> BuildResponse(IMessage responseMessage)
        {
            var offerAvailabiltyDetailsList = responseMessage as OfferAvailabiltyDetailsList;
            return Mapper.Map<RepeatedField<OfferAvailabiltyDetails>, List<Carwale.Entity.OffersV1.OfferAvailabiltyDetails>>(offerAvailabiltyDetailsList.ListOfferAvailabiltyDetails);
        }
    }
}
