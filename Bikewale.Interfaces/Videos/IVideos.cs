using Bikewale.Entities.Videos;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 18th February 2016
    /// Description : Interface for videos section
    /// </summary>
    public interface IVideos
    {
        IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, uint totalCount, uint pageNum);
        IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, uint totalCount);
        BikeVideoEntity GetVideoDetails(uint videoBasicId);
        IEnumerable<BikeVideoEntity> GetVideosBySubCategory(string categoryIdList, uint pageSize, uint pageNo);
    }
}
