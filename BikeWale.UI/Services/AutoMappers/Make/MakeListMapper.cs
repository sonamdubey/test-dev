using AutoMapper;
using Bikewale.DTO;
using Bikewale.DTO.Make;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Service.AutoMappers.Make
{
    public class MakeListMapper
    {
        internal static DTO.Make.MakeBase Convert(Entities.BikeData.BikeMakeEntityBase objMake)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            return Mapper.Map<BikeMakeEntityBase, MakeBase>(objMake);
        }

        internal static IEnumerable<MakeBase> Convert(List<BikeMakeEntityBase> objMakeList)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            return Mapper.Map<List<BikeMakeEntityBase>, List<MakeBase>>(objMakeList);
        }

        internal static IEnumerable<MakeBase> Convert(IEnumerable<BikeMakeEntityBase> objMakeList)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            return Mapper.Map<IEnumerable<BikeMakeEntityBase>, IEnumerable<MakeBase>>(objMakeList);
        }

        internal static SplashScreen Convert(SplashScreenEntity objSplash)
        {
            Mapper.CreateMap<SplashScreenEntity, SplashScreen>();
            return Mapper.Map<SplashScreenEntity, SplashScreen>(objSplash);
        }
    }
}