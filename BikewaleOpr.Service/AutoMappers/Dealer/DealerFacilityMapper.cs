using AutoMapper;
using BikewaleOpr.DTO.Dealers;
using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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