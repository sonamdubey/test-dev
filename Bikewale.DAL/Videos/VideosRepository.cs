using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using System;
using System.Collections.Generic;

namespace Bikewale.DAL.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 18th February 2016
    /// Description : Data Access Layer for videos section 
    /// </summary>
    public class VideosRepository : IVideos
    {
        public IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, uint totalCount, uint pageNum)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoId, uint totalCount)
        {
            throw new NotImplementedException();
        }

        public BikeVideoEntity GetVideoDetails(uint videoBasicId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BikeVideoEntity> GetVideosBySubCategory(string categoryIdList, uint pageSize, uint pageNo)
        {
            throw new NotImplementedException();
        }
    }
}
