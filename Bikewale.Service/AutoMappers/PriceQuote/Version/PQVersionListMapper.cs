using AutoMapper;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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