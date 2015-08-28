﻿using AutoMapper;
using Bikewale.DTO.UsedBikes;
using Bikewale.Entities.UsedBikes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.UsedBikes
{
    public class PopularUsedBikesMapper
    {
        internal static IEnumerable<PopularUsedBikesBase> Convert(IEnumerable<PopularUsedBikesEntity> objUsedBikesList)
        {
            Mapper.CreateMap<PopularUsedBikesEntity, PopularUsedBikesBase>();
            return Mapper.Map<IEnumerable<PopularUsedBikesEntity>, IEnumerable<PopularUsedBikesBase>>(objUsedBikesList);
        }
    }
}