using AutoMapper;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.DTO.PriceQuote.DealerPriceQuote;
using Bikewale.DTO.PriceQuote.DetailedDealerQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class PriceQuoteMapper
    {
        internal static DTO.PriceQuote.BikeQuotation.PQBikePriceQuoteOutput Convert(Entities.PriceQuote.BikeQuotationEntity quotation)
        {
            Mapper.CreateMap<BikeQuotationEntity, PQBikePriceQuoteOutput>();
            return Mapper.Map<BikeQuotationEntity, PQBikePriceQuoteOutput>(quotation);
        }

        internal static DTO.PriceQuote.DealerPriceQuote.DPQuotationOutput Convert(Entities.BikeBooking.PQ_QuotationEntity objPrice)
        {
            Mapper.CreateMap<BikeMakeEntityBase, DPQMakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, DPQModelBase>();
            Mapper.CreateMap<BikeVersionEntityBase, DPQVersionBase>();
            Mapper.CreateMap<StateEntityBase, DPQStateBase>();
            Mapper.CreateMap<CityEntityBase, DPQCityBase>();
            Mapper.CreateMap<Bikewale.Entities.BikeBooking.AreaEntityBase, DPQAreaBase>();
            Mapper.CreateMap<Bikewale.Entities.BikeBooking.PQ_Price, Bikewale.DTO.PriceQuote.DealerPriceQuote.DPQ_Price>();
            Mapper.CreateMap<NewBikeDealers, DPQNewBikeDealer>();
            Mapper.CreateMap<OfferEntity, DPQOfferBase>();
            Mapper.CreateMap<PQ_QuotationEntity, DPQuotationOutput>();
            return Mapper.Map<PQ_QuotationEntity, DPQuotationOutput>(objPrice);
        }

        internal static DTO.PriceQuote.DetailedDealerQuotation.DDQDealerDetailBase Convert(PQ_DealerDetailEntity dealerDetailEntity)
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

        internal static DTO.PriceQuote.CustomerDetails.PQCustomer Convert(Entities.BikeBooking.PQCustomerDetail entity)
        {
            Mapper.CreateMap<CustomerEntityBase, PQCustomerBase>();
            Mapper.CreateMap<VersionColor, PQColor>();
            Mapper.CreateMap<PQCustomerDetail, PQCustomer>();
            return Mapper.Map<PQCustomerDetail, PQCustomer>(entity);
        }

        internal static DTO.PriceQuote.PQOutput Convert(PQOutputEntity objPQOutput)
        {
            Mapper.CreateMap<PQOutputEntity, Bikewale.DTO.PriceQuote.PQOutput>();
            return Mapper.Map<PQOutputEntity, Bikewale.DTO.PriceQuote.PQOutput>(objPQOutput);
        }
    }
}