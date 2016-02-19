using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DAL.Videos
{
    public class VideosRepository : IVideos
    {
        public IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory category ,uint totalCount)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoId , uint totalCount)
        {
            throw new NotImplementedException();
        }

        public BikeVideoEntity GetVideoDetails(uint videoBasicId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BikeVideoEntity> GetVideosByCategory(List<EnumVideosCategory> categoryIdList, uint pageSize, uint pageNo)
        {
            throw new NotImplementedException();
        }
    }
}
