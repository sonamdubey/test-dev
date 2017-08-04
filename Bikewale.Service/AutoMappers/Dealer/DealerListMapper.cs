using AutoMapper;
using Bikewale.DTO.Dealer;
using Bikewale.Entities.Dealer;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Dealer
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 7th October 2015
    /// dealers Entity - DTO Mapping
    /// </summary>
    public class DealerListMapper
    {
        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 7th October 2015
        /// Get list of all dealers with details for a given make and city
        /// </summary>

        internal static IEnumerable<NewBikeDealerBase> Convert(IEnumerable<NewBikeDealerEntityBase> objDealers)
        {
            Mapper.CreateMap<NewBikeDealerEntityBase, NewBikeDealerBase>();
            return Mapper.Map<IEnumerable<NewBikeDealerEntityBase>, IEnumerable<NewBikeDealerBase>>(objDealers);
        }

        /// <summary>
        /// Craeted by  :   Sumit Kate on 20 May 2016
        /// Description :   AutoMapper for Dealer List api v2 DTO
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        internal static IEnumerable<DTO.Dealer.v2.NewBikeDealerBase> ConvertV2(IEnumerable<Entities.DealerLocator.DealersList> enumerable)
        {
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.DealerId));
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address));
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.CampaignId, opt => opt.MapFrom(s => s.CampaignId));
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.ContactNo, opt => opt.MapFrom(s => s.MaskingNumber));
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.Email, opt => opt.MapFrom(s => s.EMail));
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.DealerType, opt => opt.MapFrom(s => s.DealerType));
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.IsFeatured, opt => opt.MapFrom(s => s.IsFeatured));
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.objArea.Latitude));
            Mapper.CreateMap<Entities.DealerLocator.DealersList, DTO.Dealer.v2.NewBikeDealerBase>().ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.objArea.Longitude));
            return Mapper.Map<IEnumerable<Entities.DealerLocator.DealersList>, IEnumerable<DTO.Dealer.v2.NewBikeDealerBase>>(enumerable);
        }
    }
}



