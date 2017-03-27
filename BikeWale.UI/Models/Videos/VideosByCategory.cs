using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by : Aditi Srivasatva on 25 Mar 2017
    /// Summary    : Model to get videos by category
    /// </summary>
    public class VideosByCategory
    {
        private readonly IVideos _videos = null;
        
        #region Constructor
        public VideosByCategory(IVideos videos)
        {
            _videos = videos;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivasatva on 25 Mar 2017
        /// Summary    : Get videos by category
        /// </summary>
        public IEnumerable<BikeVideoEntity> GetData(EnumVideosCategory categoryId, ushort totalCount)
        {
            IEnumerable<BikeVideoEntity> objVideos = null;
            try
            {
                objVideos = _videos.GetVideosByCategory(categoryId,totalCount);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.Videos.VideosByCategory.GetData: CategoryId {0},TotalCount {1}", categoryId, totalCount)); 
            }
            return objVideos;
        }
        #endregion
    }
}