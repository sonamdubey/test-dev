﻿using Bikewale.Entities.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 18th February 2016
    /// Description : Interface for videos section
    /// </summary>
    public interface IVideos
    {
        IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, ushort totalCount);
        IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, ushort totalCount);
        BikeVideoEntity GetVideoDetails(uint videoBasicId);
        BikeVideosListEntity GetVideosBySubCategory(string categoryIdList, ushort pageNo, ushort pageSize);
    }
}
