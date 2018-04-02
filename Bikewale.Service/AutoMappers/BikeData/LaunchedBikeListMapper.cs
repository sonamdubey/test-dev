using AutoMapper;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.City;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.BikeData
{
    /// <summary>
    /// Modified by :   Sumit Kate on 13 Feb 2017
    /// Description :   Added new Entity to DTO convert methods
    /// </summary>
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

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   Converts Input Filter DTO to entity
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal static Entities.BikeData.NewLaunched.InputFilter Convert(DTO.BikeData.NewLaunched.InputFilterDTO filter)
        {
            Mapper.CreateMap<DTO.BikeData.NewLaunched.InputFilterDTO, Entities.BikeData.NewLaunched.InputFilter>();
            return Mapper.Map<DTO.BikeData.NewLaunched.InputFilterDTO, Entities.BikeData.NewLaunched.InputFilter>(filter);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Feb 2017
        /// Description :   Converts New Launched Bike Entity to DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static DTO.BikeData.NewLaunched.NewLaunchedBikeResultDTO Convert(Entities.BikeData.NewLaunched.NewLaunchedBikeResult entity)
        {
            Mapper.CreateMap<Entities.BikeData.NewLaunched.NewLaunchedBikeResult, DTO.BikeData.NewLaunched.NewLaunchedBikeResultDTO>();
            Mapper.CreateMap<Entities.BikeData.NewLaunched.NewLaunchedBikeEntityBase, DTO.BikeData.NewLaunched.NewLaunchedBikeDTOBase>();
            Mapper.CreateMap<Entities.BikeData.NewLaunched.InputFilter, DTO.BikeData.NewLaunched.InputFilterDTO>();
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
            Mapper.CreateMap<SpecsItem, DTO.BikeData.v2.VersionMinSpecs>();
            Mapper.CreateMap<Entities.Location.CityEntityBase, CityBase>();
            return Mapper.Map<Entities.BikeData.NewLaunched.NewLaunchedBikeResult, DTO.BikeData.NewLaunched.NewLaunchedBikeResultDTO>(entity);
        }
    }
}