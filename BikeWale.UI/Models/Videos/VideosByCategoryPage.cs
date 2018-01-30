using Bikewale.Entities.Schema;
using Bikewale.Entities.Videos;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using Bikewale.Utility.StringExtention;
using System;
using System.Collections.Generic;

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

        public bool IsMobile { get; set; }

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
                SetBreadcrumList(objVM);
                objVM.Page = Entities.Pages.GAPages.Videos_Category_Page;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Models.Videos.VideosByCategoryPage.GetData: TotalCount {0}", totalCount));
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
                ErrorClass.LogError(ex, "Bikewale.Models.Videos.VideosByCategoryPage.BindPageMetas()");
            }
        }

        /// <summary>
        /// Created By : Snehal Dange on 10th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(VideosByCategoryPageVM objPageVM)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string bikeUrl, scooterUrl;
                bikeUrl = scooterUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    bikeUrl += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));

                bikeUrl = string.Format("{0}bike-videos/", bikeUrl);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Videos"));

                if (objPageVM != null)
                {
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objPageVM.PageHeading));
                    objPageVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
                }

            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.Videos.VideosByCategoryPage.SetBreadcrumList()");
            }




        }
        #endregion
    }
}