using AutoMapper;
using Bikewale.ManufacturerCampaign.Entities;
using BikewaleOpr.DTO.Location;
using BikewaleOpr.Entity;
using System.Collections.Generic;

namespace BikewaleOpr.Service.AutoMappers.Location
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   Maps between different city classes
    /// </summary>
    public class CityMapper
    {
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Maps CityEntity and CityDTO
        /// </summary>
        /// <param name="objModels"></param>
        /// <returns></returns>
        internal static IEnumerable<CityDTO> Convert(IEnumerable<CityEntity> objModels)
        {
            Mapper.CreateMap<CityEntity, CityDTO>();
            return Mapper.Map<IEnumerable<CityEntity>, IEnumerable<CityDTO>>(objModels);
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
        /// Description :   Maps CityNameEntity and CityNameDTO
        /// </summary>
        /// <param name="objModels"></param>
        /// <returns></returns>
        internal static IEnumerable<CityNameDTO> Convert(IEnumerable<CityNameEntity> objModels)
        {
            Mapper.CreateMap<CityNameEntity, CityNameDTO>();
            return Mapper.Map<IEnumerable<CityNameEntity>, IEnumerable<CityNameDTO>>(objModels);
        }
    }
}