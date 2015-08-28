using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Make
{
    public class MakeEntityToDTO
    {
        internal static DTO.Make.MakeBase ConvertMakeEntity(Entities.BikeData.BikeMakeEntityBase objMake)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            return Mapper.Map<BikeMakeEntityBase, MakeBase>(objMake);
        }

        internal static IEnumerable<MakeBase> ConvertMakeEntityList(List<BikeMakeEntityBase> objMakeList)
        {
            Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
            return Mapper.Map<List<BikeMakeEntityBase>, List<MakeBase>>(objMakeList);
        }
    }
}