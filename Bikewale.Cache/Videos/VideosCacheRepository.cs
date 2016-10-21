﻿using Bikewale.Entities.Videos;
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
        public VideosCacheRepository(ICacheManager cache, IVideos videosRepository)
        {
            _cache = cache;
            _VideosRepository = videosRepository;
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
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetVideosByCategory");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetVideosByCategory");
                objErr.SendMail();
            }
            return videosList;
        }
        #endregion

        #region Get Similar Bike Videos
        /// <summary>
        /// Created By : Sushil Kumar K
        /// Created On : 18th February 2016
        /// Description : Cache Layer for GetSimilar Videos
        /// Modified by :   Sumit Kate on 17 Oct 2016
        /// Description :   Removed the caching
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
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetSimilarVideos");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetVideoDetails");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetVideosByCategory");
                objErr.SendMail();
            }
            return videosList;
        }

        #endregion
    }
}
