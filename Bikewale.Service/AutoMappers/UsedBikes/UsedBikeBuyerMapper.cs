
using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.Customer;
using Bikewale.Entities.Used;
namespace Bikewale.Service.AutoMappers.UsedBikes
{
    public class UsedBikeBuyerMapper
    {
        internal static Entities.Used.PhotoRequest Convert(DTO.UsedBikes.PhotoRequestDTO request)
        {
            AutoMapper.Mapper.CreateMap<DTO.UsedBikes.PhotoRequestDTO, Entities.Used.PhotoRequest>();
            AutoMapper.Mapper.CreateMap<DTO.Customer.CustomerBase, CustomerEntityBase>();
            return AutoMapper.Mapper.Map<DTO.UsedBikes.PhotoRequestDTO, Entities.Used.PhotoRequest>(request);
        }

        internal static DTO.UsedBikes.BikeInterestDetailsDTO Convert(Entities.Used.BikeInterestDetails interestDetails)
        {
            AutoMapper.Mapper.CreateMap<Entities.Used.BikeInterestDetails, DTO.UsedBikes.BikeInterestDetailsDTO>();
            AutoMapper.Mapper.CreateMap<CustomerEntityBase, DTO.Customer.CustomerBase>();
            AutoMapper.Mapper.CreateMap<UsedBikeSellerBase, UsedBikeSellerBaseDTO>();
            return AutoMapper.Mapper.Map<Entities.Used.BikeInterestDetails, DTO.UsedBikes.BikeInterestDetailsDTO>(interestDetails);
        }

        internal static CustomerEntityBase Convert(DTO.Customer.CustomerBase buyer)
        {
            AutoMapper.Mapper.CreateMap<DTO.Customer.CustomerBase, CustomerEntityBase>();
            return AutoMapper.Mapper.Map<DTO.Customer.CustomerBase, CustomerEntityBase>(buyer);
        }
    }
}