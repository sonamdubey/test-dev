using Bikewale.Entities.Videos;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 18th February 2016
    /// Description : Cache Interface for videos section
    /// </summary>
    public interface IVideosCacheRepository
    {
        IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, ushort totalCount);
        IEnumerable<BikeVideoEntity> GetVideosByMakeModel(ushort pageNo, ushort pageSize, uint makeId, uint? modelId = null);
        IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, ushort totalCount);
        BikeVideoEntity GetVideoDetails(uint videoBasicId);
        BikeVideosListEntity GetVideosBySubCategory(string categoryIdList, ushort pageNo, ushort pageSize, VideosSortOrder? sortOrder = null);

    }
}
