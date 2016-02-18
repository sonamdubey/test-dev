using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Cache.Videos
{
    public class VideosCacheRepository : IVideosCacheRepository
    {
         private readonly IVideos _VideosRepository = null;
        private readonly ICacheManager _cache = null;
        public VideosCacheRepository(ICacheManager cache, IVideos videosRepository)
        {
            _cache = cache;
            _VideosRepository = videosRepository;
        }

        public IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, uint totalCount)
        {
            IEnumerable<BikeVideoEntity> videosList = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_Video_{0}", categoryId);
                videosList = _cache.GetFromCache<IEnumerable<BikeVideoEntity>>(key, new TimeSpan(1, 0, 0), () => _VideosRepository.GetVideosByCategory(categoryId, totalCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetVideos");
                objErr.SendMail();
            }
            return videosList;
        }
    }
}
