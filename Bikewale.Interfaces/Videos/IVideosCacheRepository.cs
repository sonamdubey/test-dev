using Bikewale.Entities.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Videos
{
    public interface IVideosCacheRepository
    {
        IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, uint totalCount);
        IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, uint totalCount);
    }
}
