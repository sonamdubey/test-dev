using Bikewale.Entities.Videos;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 18th February 2016
    /// Description : Interface for videos section
    /// Modified by : Aditi Srivastava on 14 June 2017
    /// Summary     : Added overload for GetVideosByMakeModel with bodyStyleId
    /// Modified by : Ashutosh Sharma on 17 Nov 2017
    /// Description : Added overload of GetSimilarVideos with modelIdList.
    /// Modified by : Ashutosh Sharma on 11 Dec 2017
    /// Description : Removed videoBasicId from GetSimilarVideos with modelIdList.
    /// </summary>
    public interface IVideos
    {
        IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, ushort totalCount);
        IEnumerable<BikeVideoEntity> GetVideosByMakeModel(ushort pageNo, ushort pageSize, uint makeId, uint? modelId = null);
        IEnumerable<BikeVideoEntity> GetVideosByMakeModel(ushort pageNo, ushort pageSize, string bodyStyleId, uint makeId, uint? modelId = null);
        IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, ushort totalCount);
		IEnumerable<BikeVideoEntity> GetSimilarVideos(ushort totalCount, string modelIdList);
		BikeVideoEntity GetVideoDetails(uint videoBasicId);
        BikeVideosListEntity GetVideosBySubCategory(string categoryIdList, ushort pageNo, ushort pageSize, VideosSortOrder? sortOrder = null);
        IEnumerable<BikeVideoEntity> GetSimilarModelsVideos(uint videoId, uint ModelId, ushort totalCount);
        IEnumerable<BikeVideoEntity> GetVideosByModelId(uint ModelId);
    }
}
