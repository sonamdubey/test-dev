using AutoMapper;
using Bikewale.DTO.PriceQuote.Model;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.PriceQuote.Model
{
    public class PQModelEntityToDTO
    {
        internal static IEnumerable<DTO.PriceQuote.Model.PQModelBase> ConvertModelList(List<Entities.BikeData.BikeModelEntityBase> objModelList)
        {
            Mapper.CreateMap<BikeModelEntityBase, PQModelBase>();
            return Mapper.Map<List<BikeModelEntityBase>, List<PQModelBase>>(objModelList);
        }
    }
}