using AutoMapper;
using Bikewale.ManufacturerCampaign.Entities;
using System.Collections.Generic;

namespace BikewaleOpr.Service.AutoMappers.Location
{
    public class CityMapper
    {
        internal static IEnumerable<CityDTO> Convert(IEnumerable<CityEntity> objModels)
        {
            Mapper.CreateMap<CityEntity, CityDTO>();
            return Mapper.Map<IEnumerable<CityEntity>, IEnumerable<CityDTO>>(objModels);
        }
    }
}