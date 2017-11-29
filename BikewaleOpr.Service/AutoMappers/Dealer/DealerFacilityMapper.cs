using AutoMapper;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entities;

namespace BikewaleOpr.Service.AutoMappers.Dealer
{
    public class DealerFacilityMapper
    {
        internal static FacilityEntity Convert(DealerFacilityDTO objMakes)
        {
            Mapper.CreateMap<DealerFacilityDTO, FacilityEntity>();
            return Mapper.Map<DealerFacilityDTO, FacilityEntity>(objMakes);
        }
    }
}