using Bikewale.Entities.Videos;
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
    /// Description : Cache Interface for videos section
    /// </summary>
    public interface IVideosCacheRepository
    {
        IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, uint totalCount);
        IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, uint totalCount);
        BikeVideoEntity GetVideoDetails(uint videoBasicId);
        IEnumerable<BikeVideoEntity> GetVideosBySubCategory(string categoryIdList, uint pageSize, uint pageNo); 
        
    }
}
