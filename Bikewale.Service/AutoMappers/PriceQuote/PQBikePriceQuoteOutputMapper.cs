using AutoMapper;
using Bikewale.DTO.PriceQuote.BikeQuotation;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class PQBikePriceQuoteOutputMapper
    {
        internal static DTO.PriceQuote.BikeQuotation.PQBikePriceQuoteOutput Convert(Entities.PriceQuote.BikeQuotationEntity quotation)
        {
            Mapper.CreateMap<OtherVersionInfoEntity, OtherVersionInfoDTO>();
            Mapper.CreateMap<BikeQuotationEntity, PQBikePriceQuoteOutput>();
            return Mapper.Map<BikeQuotationEntity, PQBikePriceQuoteOutput>(quotation);
        }

        internal static List<DTO.PriceQuote.v2.DPQDealerBase> Convert(IEnumerable<NewBikeDealerBase> enumerable)
        {
            Mapper.CreateMap<NewBikeDealerBase, DTO.PriceQuote.v2.DPQDealerBase>();
            return Mapper.Map<IEnumerable<NewBikeDealerBase>, List<DTO.PriceQuote.v2.DPQDealerBase>>(enumerable);
        }

        internal static DTO.PriceQuote.v2.PQPrimaryDealer Convert(Entities.BikeBooking.NewBikeDealers newBikeDealers)
        {
            Mapper.CreateMap<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.DealerId));
            Mapper.CreateMap<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>().ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address));
            Mapper.CreateMap<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>().ForMember(d => d.ContactNo, opt => opt.MapFrom(s => s.MaskingNumber));
            Mapper.CreateMap<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>().ForMember(d => d.DealerType, opt => opt.MapFrom(s => s.DealerPackageType));
            Mapper.CreateMap<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>().ForMember(d => d.Email, opt => opt.MapFrom(s => s.EmailId));
            Mapper.CreateMap<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>().ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.objArea.Latitude));
            Mapper.CreateMap<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>().ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.objArea.Longitude));
            Mapper.CreateMap<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>().ForMember(d => d.Name, opt => opt.MapFrom(s => s.Organization));
            return Mapper.Map<Entities.BikeBooking.NewBikeDealers, DTO.PriceQuote.v2.PQPrimaryDealer>(newBikeDealers);
        }

        internal static DTO.PriceQuote.v2.DPQuotationOutput Convert(DetailedDealerQuotationEntity objDealerQuotation, IEnumerable<PQ_BikeVarient> varients)
        {
            DTO.PriceQuote.v2.DPQuotationOutput output = new DTO.PriceQuote.v2.DPQuotationOutput();
            output.emi = ConvertEMI(objDealerQuotation.PrimaryDealer.EMIDetails);
            output.Benefits = ConvertBenefits(objDealerQuotation.PrimaryDealer.Benefits);
            output.Offers = ConvertOffers(objDealerQuotation.PrimaryDealer.OfferList);
            output.Versions = ConvertVersions(varients);
            return output;
        }

        private static IEnumerable<DTO.PriceQuote.v2.DPQVersionBase> ConvertVersions(IEnumerable<PQ_BikeVarient> varients)
        {
            Mapper.CreateMap<Entities.BikeBooking.PQ_Price, DTO.PriceQuote.v2.DPQ_Price>().ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.CategoryName));
            Mapper.CreateMap<Entities.BikeBooking.PQ_Price, DTO.PriceQuote.v2.DPQ_Price>().ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price));
            Mapper.CreateMap<PQ_BikeVarient, DTO.PriceQuote.v2.DPQVersionBase>().ForMember(d => d.VersionId, opt => opt.MapFrom(s => s.objVersion.VersionId));
            Mapper.CreateMap<PQ_BikeVarient, DTO.PriceQuote.v2.DPQVersionBase>().ForMember(d => d.VersionName, opt => opt.MapFrom(s => s.objVersion.VersionName));
            Mapper.CreateMap<PQ_BikeVarient, DTO.PriceQuote.v2.DPQVersionBase>().ForMember(d => d.PriceList, opt => opt.MapFrom(s => s.PriceList));
            return Mapper.Map<IEnumerable<PQ_BikeVarient>, IEnumerable<DTO.PriceQuote.v2.DPQVersionBase>>(varients);
        }

        private static IEnumerable<DTO.PriceQuote.v2.DPQ_Price> ConvertPriceList(IEnumerable<Entities.BikeBooking.PQ_Price> enumerable)
        {
            Mapper.CreateMap<Entities.BikeBooking.PQ_Price, DTO.PriceQuote.v2.DPQ_Price>().ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.CategoryName));
            Mapper.CreateMap<Entities.BikeBooking.PQ_Price, DTO.PriceQuote.v2.DPQ_Price>().ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price));
            return Mapper.Map<IEnumerable<Entities.BikeBooking.PQ_Price>, IEnumerable<DTO.PriceQuote.v2.DPQ_Price>>(enumerable);
        }

        private static IEnumerable<DTO.PriceQuote.v2.DPQOffer> ConvertOffers(IEnumerable<Entities.OfferEntityBase> enumerable)
        {
            Mapper.CreateMap<Entities.OfferEntityBase, DTO.PriceQuote.v2.DPQOffer>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.OfferId));
            Mapper.CreateMap<Entities.OfferEntityBase, DTO.PriceQuote.v2.DPQOffer>().ForMember(d => d.OfferCategoryId, opt => opt.MapFrom(s => s.OfferCategoryId));
            Mapper.CreateMap<Entities.OfferEntityBase, DTO.PriceQuote.v2.DPQOffer>().ForMember(d => d.Text, opt => opt.MapFrom(s => s.OfferText));
            return Mapper.Map<IEnumerable<Entities.OfferEntityBase>, IEnumerable<DTO.PriceQuote.v2.DPQOffer>>(enumerable);
        }

        private static IEnumerable<DTO.PriceQuote.v2.DPQBenefit> ConvertBenefits(IEnumerable<Entities.DealerBenefitEntity> enumerable)
        {
            Mapper.CreateMap<Entities.DealerBenefitEntity, DTO.PriceQuote.v2.DPQBenefit>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.BenefitId));
            Mapper.CreateMap<Entities.DealerBenefitEntity, DTO.PriceQuote.v2.DPQBenefit>().ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.CatId));
            Mapper.CreateMap<Entities.DealerBenefitEntity, DTO.PriceQuote.v2.DPQBenefit>().ForMember(d => d.Text, opt => opt.MapFrom(s => s.BenefitText));
            return Mapper.Map<IEnumerable<Entities.DealerBenefitEntity>, IEnumerable<DTO.PriceQuote.v2.DPQBenefit>>(enumerable);
        }


        private static DTO.PriceQuote.v2.EMI ConvertEMI(Entities.BikeBooking.EMI eMI)
        {
            Mapper.CreateMap<Entities.BikeBooking.EMI, DTO.PriceQuote.v2.EMI>();
            return Mapper.Map<Entities.BikeBooking.EMI, DTO.PriceQuote.v2.EMI>(eMI);
        }

        internal static IEnumerable<DTO.Version.VersionBase> Convert(IEnumerable<OtherVersionInfoEntity> enumerable)
        {
            Mapper.CreateMap<OtherVersionInfoEntity, DTO.Version.VersionBase>();
            return Mapper.Map<IEnumerable<OtherVersionInfoEntity>, IEnumerable<DTO.Version.VersionBase>>(enumerable);
        }
    }
}