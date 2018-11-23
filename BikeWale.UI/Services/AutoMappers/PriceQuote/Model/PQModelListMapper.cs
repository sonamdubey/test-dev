using AutoMapper;
using Bikewale.DTO.PriceQuote.Model;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.PriceQuote.Model
{
    public class PQModelListMapper
    {
        internal static IEnumerable<DTO.PriceQuote.Model.PQModelBase> Convert(List<Entities.BikeData.BikeModelEntityBase> objModelList)
        {
            return Mapper.Map<List<BikeModelEntityBase>, List<PQModelBase>>(objModelList);
        }
    }
}