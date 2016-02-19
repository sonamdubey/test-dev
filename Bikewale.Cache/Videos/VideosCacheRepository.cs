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

        public IEnumerable<BikeVideoEntity> GetVideosByCategory(List<EnumVideosCategory> categoryIdList, uint pageSize, uint pageNo)
        {
            IEnumerable<BikeVideoEntity> videosList = null;
            string key = string.Empty;
            try
            {
                //string contentTypeList = CommonApiOpn.GetContentTypesString(categoryIdList);
                key = String.Format("BW_Video_{0}", categoryIdList);
                videosList = _cache.GetFromCache<IEnumerable<BikeVideoEntity>>(key, new TimeSpan(1, 0, 0), () => _VideosRepository.GetVideosByCategory(categoryIdList, pageSize,pageNo));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetVideosByCategory");
                objErr.SendMail();
            }
            return videosList;
        }

        public IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, uint totalCount)
        {
            IEnumerable<BikeVideoEntity> videosList = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_SimilarVideos_{0}", videoBasicId);
                videosList = _cache.GetFromCache<IEnumerable<BikeVideoEntity>>(key, new TimeSpan(1, 0, 0), () => _VideosRepository.GetSimilarVideos(videoBasicId, totalCount));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetSimilarVideos");
                objErr.SendMail();
            }
            return videosList;
        }

        public BikeVideoEntity GetVideoDetails(uint videoBasicId)
        {
            BikeVideoEntity video = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_VideoDetails_{0}", videoBasicId);
                video = _cache.GetFromCache<BikeVideoEntity>(key, new TimeSpan(1, 0, 0), () => _VideosRepository.GetVideoDetails(videoBasicId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetSimilarVideos");
                objErr.SendMail();
            }
            return video;
        }
    }
}
