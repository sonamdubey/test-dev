using AutoMapper;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.PriceQuote.Version
{
    public class PQVersionEntityToDTO
    {
        internal static List<PQVersionBase> VersionList(List<Entities.BikeData.BikeVersionsListEntity> objVersionList)
        {
            Mapper.CreateMap<BikeVersionsListEntity, PQVersionBase>();
            return Mapper.Map<List<BikeVersionsListEntity>, List<PQVersionBase>>(objVersionList);
        }
    }
}