using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by : Aditi Srivasatva on 25 Mar 2017
    /// Summary    : Model to get videos by subcategory
    /// </summary>
    public class VideosBySubcategory
    {
        private readonly IVideos _videos = null;

        #region Constructor
        public VideosBySubcategory(IVideos videos)
        {
            _videos = videos;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivasatva on 25 Mar 2017
        /// Summary    : Get videos by category
        /// </summary>
        public VideosBySubcategoryVM GetData(string categoryIdList, ushort pageNo, ushort pageSize, VideosSortOrder? sortOrder = null)
        {
            VideosBySubcategoryVM objVideos = new VideosBySubcategoryVM();
            try
            {
                objVideos.VideoList = _videos.GetVideosBySubCategory(categoryIdList, pageNo, pageSize, sortOrder);
                objVideos.SectionTitle = VideoTitleDescription.VideoCategoryTitle(categoryIdList);
                objVideos.CategoryIdList = categoryIdList;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.Videos.VideosBySubCategory.GetData: CategoryId {0}, PageNo {1}, PageSize {2}, SortOrder {3}", categoryIdList,pageNo,pageSize,sortOrder));
            }
            return objVideos;
        }
        #endregion
    }
}