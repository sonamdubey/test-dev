using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.AppDeepLinking
{
    public class DeepLinkingMapper
    {
        internal static DTO.AppDeepLinking.DeepLinking Convert(Entities.AppDeepLinking.DeepLinkingEntity entity)
        {
            Mapper.CreateMap<Entities.AppDeepLinking.DeepLinkingEntity, DTO.AppDeepLinking.DeepLinking>();
            return Mapper.Map<Entities.AppDeepLinking.DeepLinkingEntity, DTO.AppDeepLinking.DeepLinking>(entity);
        }
    }
}