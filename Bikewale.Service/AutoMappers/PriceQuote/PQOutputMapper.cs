using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.PriceQuote
{
    public class PQOutputMapper
    {
        internal static DTO.PriceQuote.PQOutput Convert(Entities.BikeBooking.PQOutputEntity objPQOutput)
        {
            Mapper.CreateMap<PQOutputEntity, Bikewale.DTO.PriceQuote.PQOutput>();
            return Mapper.Map<PQOutputEntity, Bikewale.DTO.PriceQuote.PQOutput>(objPQOutput);
        }

        internal static DTO.Model.ModelDetail Convert(Entities.BikeData.BikeModelEntity objModel)
        {
            Mapper.CreateMap<BikeModelEntity, ModelDetail>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            return Mapper.Map<BikeModelEntity, ModelDetail>(objModel);
        }
    }
}