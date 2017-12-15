using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.Videos
{
    /// <summary>
    /// Created By : Sushil Kumar K
    /// Created On : 18th February 2016
    /// Description : Cache Layer for  Videos  Section
    /// </summary>
    public class VideosCacheRepository : IVideosCacheRepository
    {
        private readonly IVideos _VideosRepository = null;
        private readonly ICacheManager _cache = null;
        private readonly IVideoRepository _videoRepo = null;
        public VideosCacheRepository(ICacheManager cache, IVideos videosRepository,IVideoRepository videoRepo)
        {
            _cache = cache;
            _VideosRepository = videosRepository;
            _videoRepo=videoRepo;
        }

        #region Get Bike Videos by Category
        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : Cache Layer for Get Bike Videos by category earlier version 
        /// Modified by :   Sumit Kate on 17 Oct 2016
        /// Description :   Removed the caching
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, ushort totalCount)
        {
            IEnumerable<BikeVideoEntity> videosList = null;
            try
            {
                videosList = _VideosRepository.GetVideosByCategory(categoryId, totalCount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeVideosCacheRepository.GetVideosByCategory");
                
            }
            return videosList;
        }

        #endregion

        #region Get Bike Videos by Sub Category/Categories
        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : Cache Layer for Get Videos by Sub Categories according to new methodology
        /// Modified by :   Sumit Kate on 17 Oct 2016
        /// Description :   Removed the caching
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public BikeVideosListEntity GetVideosBySubCategory(string categoryIdList, ushort pageNo, ushort pageSize, VideosSortOrder? sortOrder = null)
        {
            BikeVideosListEntity videosList = null;
            try
            {
                videosList = _VideosRepository.GetVideosBySubCategory(categoryIdList, pageNo, pageSize, sortOrder);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeVideosCacheRepository.GetVideosByCategory");
                
            }
            return videosList;
        }
        #endregion

        #region Get Similar Bike Videos
        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : Cache Layer for GetSimilar Videos
        /// Modified by :  Sumit Kate on 17 Oct 2016
        /// Description :  Removed the caching
        /// </summary>
        /// <param name="videoBasicId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, ushort totalCount)
        {
            IEnumerable<BikeVideoEntity> videosList = null;
            try
            {
                videosList = _VideosRepository.GetSimilarVideos(videoBasicId, totalCount);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeVideosCacheRepository.GetSimilarVideos");
            }
            return videosList;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Method to get similar videos of multiple models.
        /// Modified by : Ashutosh Sharma on 11 Dec 2017
        /// Description : Removed videoBasicId from call of GetSimilarVideos.
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="modelIdList"></param>
        /// <param name="videoBasicId"></param>
        /// <returns></returns>
		public IEnumerable<BikeVideoEntity> GetSimilarVideos(ushort totalCount, string modelIdList)
		{
			IEnumerable<BikeVideoEntity> videosList = null;
			try
			{
				videosList = _VideosRepository.GetSimilarVideos(totalCount, modelIdList);
			}
			catch (Exception ex)
			{
				Bikewale.Notifications.ErrorClass.LogError(ex, "BikeVideosCacheRepository.GetSimilarVideos");
			}
			return videosList;
		}
		#endregion

		#region Get Video Details
		/// <summary>
		/// Created By : Sushil Kumar K
		/// Created On : 18th February 2016
		/// Description : Cache Layer for Videos Details page
		/// Modified by :   Sumit Kate on 17 Oct 2016
		/// Description :   Removed the caching
		/// </summary>
		/// <param name="videoBasicId"></param>
		/// <returns></returns>
		public BikeVideoEntity GetVideoDetails(uint videoBasicId)
        {
            BikeVideoEntity video = null;
            try
            {
                video = _VideosRepository.GetVideoDetails(videoBasicId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeVideosCacheRepository.GetVideoDetails");
                
            }
            return video;
        }
        #endregion

        #region Get Bike Videos by Make Model
        /// <summary>
        /// Created By : Lucky Rathore
        /// Created On : 1st March 2016
        /// Description : Cache Layer for Get Bike Videos by Make. 
        /// Modified by :   Sumit Kate on 17 Oct 2016
        /// Description :   Removed the caching
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="totalCount"></param>
        /// <returns>IEnumerable of BikeVideoEntity</returns>
        public IEnumerable<BikeVideoEntity> GetVideosByMakeModel(ushort pageNo, ushort pageSize, uint makeId, uint? modelId = null)
        {
            IEnumerable<BikeVideoEntity> videosList = null;
            try
            {
                videosList = _VideosRepository.GetVideosByMakeModel(pageNo, pageSize, makeId, modelId);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeVideosCacheRepository.GetVideosByCategory");
                
            }
            return videosList;
        }

        #endregion

        /// <summary>
        /// Created by : Aditi Srivastava on 27 Feb 2017
        /// Summary    : Get model wise video count
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoModelEntity> GetModelVideos(uint makeId)
        {
            IEnumerable<BikeVideoModelEntity> modelVideos = null;
            string key = String.Format("BW_VideoModels_Count_MK_{0}", makeId);
            try
            {
                modelVideos = _cache.GetFromCache<IEnumerable<BikeVideoModelEntity>>(key, new TimeSpan(0, 30, 0), () => _videoRepo.GetModelVideos(makeId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Cache.Videos.GetModelVideos MakeId:{0}",makeId));
            }
            return modelVideos;
        }
    }
}
