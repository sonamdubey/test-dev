using AutoMapper;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.PriceQuote.Version
{
    public class PQVersionListMapper
    {
        internal static List<PQVersionBase> Convert(List<Entities.BikeData.BikeVersionsListEntity> objVersionList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, PQVersionBase>();
            return Mapper.Map<List<BikeVersionsListEntity>, List<PQVersionBase>>(objVersionList);
        }
    }
}