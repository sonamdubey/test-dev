using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.BikeData
{
    public class LaunchedBikeListMapper
    {
        internal static IEnumerable<DTO.BikeData.LaunchedBike> Convert(IEnumerable<Entities.BikeData.NewLaunchedBikeEntity> objRecent)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
            Mapper.CreateMap<NewLaunchedBikeEntity, LaunchedBike>();
            Mapper.CreateMap<MinSpecsEntity, MinSpecs>();
            return Mapper.Map<IEnumerable<NewLaunchedBikeEntity>, IEnumerable<LaunchedBike>>(objRecent);
        }
    }
}