using AutoMapper;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class DDQDealerDetailBaseMapper
    {
        internal static DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase Convert(Entities.BikeBooking.PQ_DealerDetailEntity dealerDetailEntity)
        {
            Mapper.CreateMap<BikeMakeEntityBase, DDQMakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, DDQModelBase>();
            Mapper.CreateMap<BikeVersionEntityBase, DDQVersionBase>();
            Mapper.CreateMap<StateEntityBase, DDQStateBase>();
            Mapper.CreateMap<CityEntityBase, DDQCityBase>();
            Mapper.CreateMap<Bikewale.Entities.BikeBooking.AreaEntityBase, DDQAreaBase>();
            Mapper.CreateMap<Bikewale.Entities.BikeBooking.PQ_Price, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQPQ_Price>();
            Mapper.CreateMap<NewBikeDealers, DDQNewBikeDealer>();
            Mapper.CreateMap<OfferEntity, DDQOfferBase>();
            Mapper.CreateMap<FacilityEntity, DDQFacilityBase>();
            Mapper.CreateMap<PQ_QuotationEntity, DDQQuotationBase>();
            Mapper.CreateMap<Bikewale.Entities.BikeBooking.EMI, Bikewale.DTO.PriceQuote.DetailedDealerQuotation.DDQEMI>();
            Mapper.CreateMap<BookingAmountEntityBase, DDQBookingAmountBase>();
            Mapper.CreateMap<PQ_DealerDetailEntity, DDQDealerDetailBase>();
            return Mapper.Map<PQ_DealerDetailEntity, DDQDealerDetailBase>(dealerDetailEntity);
        }
    }
}