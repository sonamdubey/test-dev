using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewale.Utility.StringExtention;
using System;

namespace Bikewale.Models.Videos
{
    /// <summary>
    /// Created by : Aditi Srivasatva on 25 Mar 2017
    /// Summary    : Model to get videos by category
    /// </summary>
    public class VideosByCategoryPage
    {
        private readonly IVideosCacheRepository _objVideoCache = null;
        private string _categoryIdList;
        private string _title;


        #region Constructor
        public VideosByCategoryPage(IVideosCacheRepository objVideoCache, string categoryIdList, string title)
        {
            _objVideoCache = objVideoCache;
            _categoryIdList = categoryIdList;
            _title = title;

            ParseQuery();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivasatva on 25 Mar 2017
        /// Summary    : Get videos by category
        /// </summary>
        public VideosByCategoryPageVM GetData(ushort totalCount)
        {
            VideosByCategoryPageVM objVM = null;
            try
            {
                objVM = new VideosByCategoryPageVM();
                if (!string.IsNullOrEmpty(_title))
                {
                    objVM.CanonicalTitle = _title.ToLower();
                    objVM.TitleName = (StringHelper.Capitlization(_title)).Replace('-', ' ');
                    objVM.PageHeading = string.Format("{0} Video", objVM.TitleName);
                    //titleName = string.Format("{0} Video Review - BikeWale", titleName);
                }
                objVM.CategoryId = _categoryIdList;
                objVM.Videos = _objVideoCache.GetVideosBySubCategory(_categoryIdList, 1, totalCount, VideosSortOrder.JustLatest);
                BindPageMetas(objVM);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.Videos.VideosByCategoryPage.GetData: TotalCount {0}", totalCount));
            }
            return objVM;
        }

        private void ParseQuery()
        {
            _categoryIdList = _categoryIdList.Replace('-', ' ');
        }


        private void BindPageMetas(VideosByCategoryPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null)
                {
                    string descrip, title;
                    VideoTitleDescription.VideoTitleDesc(_categoryIdList, out title, out descrip, null, null);

                    objPageVM.PageMetaTags.Title = title;
                    objPageVM.PageMetaTags.Description = descrip;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterIndiaPage.BindPageMetas()");
            }
        }
        #endregion
    }
}