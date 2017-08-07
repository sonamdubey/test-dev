using AutoMapper;
using Bikewale.ManufacturerCampaign.Entities;
using BikewaleOpr.DTO.Location;
using BikewaleOpr.Entity;
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

        internal static IEnumerable<CityNameDTO> Convert(IEnumerable<CityNameEntity> objModels)
        {
            Mapper.CreateMap<CityNameEntity, CityNameDTO>();
            return Mapper.Map<IEnumerable<CityNameEntity>, IEnumerable<CityNameDTO>>(objModels);
        }
    }
}