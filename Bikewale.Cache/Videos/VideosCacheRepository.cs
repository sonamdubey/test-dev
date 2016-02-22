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
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetVideosByCategory(EnumVideosCategory categoryId, uint totalCount, uint pageNum)
        {
            IEnumerable<BikeVideoEntity> videosList = null;
            string key = string.Empty;
            try
            {
                switch ((int)categoryId)
                {
                    case 1:
                        key = String.Format("BW_Videos_FeaturedAndLatest_{0}", totalCount);
                        break;
                    case 2:
                        key = String.Format("BW_Videos_Popular_{0}", totalCount);
                        break;
                    case 3:
                        key = String.Format("BW_Videos_ExpertReviews_{0}", totalCount);
                        break;
                    case 4:
                        key = String.Format("BW_Videos_Miscelleneous_{0}", totalCount);
                        break;
                    case 5:
                        key = String.Format("BW_Videos_InteriorShow_{0}", totalCount);
                        break;
                    case 6:
                        key = String.Format("BW_Videos_Recent_{0}", totalCount);
                        break;
                    default:
                        key = String.Format("BW_Videos_CatId_{0}_{1}", categoryId, totalCount);
                        break;
                }

                videosList = _cache.GetFromCache<IEnumerable<BikeVideoEntity>>(key, new TimeSpan(1, 0, 0), () => _VideosRepository.GetVideosByCategory(categoryId, totalCount, pageNum));
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
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetVideosBySubCategory(string categoryIdList, uint pageSize, uint pageNo)
        {
            IEnumerable<BikeVideoEntity> videosList = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_Videos_SubCat_{0}_Cnt_{1}", categoryIdList, pageSize);
                videosList = _cache.GetFromCache<IEnumerable<BikeVideoEntity>>(key, new TimeSpan(1, 0, 0), () => _VideosRepository.GetVideosBySubCategory(categoryIdList, pageSize, pageNo));
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
        /// </summary>
        /// <param name="videoBasicId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<BikeVideoEntity> GetSimilarVideos(uint videoBasicId, uint totalCount)
        {
            IEnumerable<BikeVideoEntity> videosList = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_Videos_Similar_{0}", videoBasicId);
                videosList = _cache.GetFromCache<IEnumerable<BikeVideoEntity>>(key, new TimeSpan(1, 0, 0), () => _VideosRepository.GetSimilarVideos(videoBasicId, totalCount));
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
        /// </summary>
        /// <param name="videoBasicId"></param>
        /// <returns></returns>
        public BikeVideoEntity GetVideoDetails(uint videoBasicId)
        {
            BikeVideoEntity video = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_Videos_{0}", videoBasicId);
                video = _cache.GetFromCache<BikeVideoEntity>(key, new TimeSpan(1, 0, 0), () => _VideosRepository.GetVideoDetails(videoBasicId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikeVideosCacheRepository.GetVideoDetails");
                objErr.SendMail();
            }
            return video;
        }
        #endregion
    }
}
