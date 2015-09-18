using AutoMapper;
using Bikewale.DTO.Videos;
using Bikewale.Entities.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Service.AutoMappers.Videos
{
    public class VideosMapper
    {
        internal static List<VideoBase> Convert(List<BikeVideoEntity> objVideoList)
        {
            Mapper.CreateMap<BikeVideoEntity, VideoBase>();
            return Mapper.Map<List<BikeVideoEntity>, List<VideoBase>>(objVideoList);
        }
    }
}