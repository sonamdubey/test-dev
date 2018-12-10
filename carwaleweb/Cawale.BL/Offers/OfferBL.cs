using AutoMapper;
using Carwale.DAL.ApiGateway;
using Carwale.DAL.ApiGateway.Extensions.Offers;
using Carwale.Entity.Offers;
using Carwale.Entity.OffersV1;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Offers;
using Carwale.Notifications.Logs;
using Microsoft.Practices.Unity;
using Offers.Protos.ProtoFiles;
using System;

namespace Carwale.BL.Offers
{
    public class OfferBL : IOfferBL
    {
        private readonly IGeoCitiesCacheRepository _geoCitiesCache;
        private readonly IUnityContainer _unityContainer;
        
        public OfferBL(IGeoCitiesCacheRepository geoCitiesCache,
                        IUnityContainer unityContainer)
        {
            _geoCitiesCache = geoCitiesCache;
            _unityContainer = unityContainer;
        }

        public Carwale.Entity.OffersV1.Offer GetOffer(OfferInput offerInput)
        {
            try
            {
                if (ValidateOfferInput(offerInput))
                {
                    IApiGatewayCaller apiGatewayCaller = _unityContainer.Resolve<IApiGatewayCaller>();
                    if (apiGatewayCaller.AggregateGetOffersOnCriteria(offerInput))
                    {
                        apiGatewayCaller.Call();
                    }

                    var offersList = apiGatewayCaller.GetResponse<OfferWithCategoryDetailList>(0);
                    if (offersList != null && offersList.ListOfferWithCategoryDetail != null && offersList.ListOfferWithCategoryDetail.Count > 0)
                    {
                        var offer = Mapper.Map<Carwale.Entity.OffersV1.Offer>(offersList.ListOfferWithCategoryDetail[0]);
                        return offer;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        public bool ValidateOfferInput(OfferInput offerInput)
        {
            
            if (ValidateOfferCarInput(offerInput))
            {
                if (offerInput.StateId == 0)
                {
                    offerInput.StateId = _geoCitiesCache.GetStateByCityId(offerInput.CityId).StateId;
                }
                return true;
            }

            return false;
        }

        private static bool ValidateOfferCarInput(OfferInput offerInput)
        {
            if (offerInput.MakeId > 0 && offerInput.ModelId > 0 && offerInput.VersionId > 0)
            {
                return true;
            }

            return false;
        }
    }
}
