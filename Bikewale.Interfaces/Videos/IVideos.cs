using Bikewale.Entities.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Summary : Interface for videos section
    /// </summary>
    public interface IVideos
    {
        IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, uint totalCount);
        IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, uint totalCount);
        //List<BikeVideoEntity> GetVideosByCategory(int startIndex, int endIndex, int makeId, int modelId, List<EnumCMSContentType> categoryIdList, out int recordCount);
    }
}
