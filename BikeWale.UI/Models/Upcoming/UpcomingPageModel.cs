using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 07-Apr-2017
    /// summary: Upcoming bikes page model
    /// </summary>
    public class UpcomingPageModel
    {
        #region Private variables

        private IUpcoming _upcoming = null;
        private EnumUpcomingBikesFilter filter = EnumUpcomingBikesFilter.Default;
        private readonly ushort _pageNumber;

        #endregion

        #region Public members

        public UpcomingBikesListInputEntity Filters { get; set; }
        public EnumUpcomingBikesFilter SortBy { get; set; }
        public int PageSize { get; set; }
        public string BaseUrl { get; set; }

        #endregion

        #region Constructor

        public UpcomingPageModel(IUpcoming upcoming, ushort pageNumber)
        {
            _upcoming = upcoming;
            _pageNumber = pageNumber;
        }
        #endregion

        #region Functions

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 07-Apr-2017 
        /// </returns>
        public UpcomingPageVM GetData()
        {
            UpcomingPageVM objUpcoming = new UpcomingPageVM();
            try
            {
                objUpcoming.UpcomingBikeModels = _upcoming.GetModels(Filters, SortBy);
                objUpcoming.TotalBikes = (uint)_upcoming.GetTotalBikes();
                objUpcoming.HasBikes = (objUpcoming.UpcomingBikeModels.Count() > 0);
                objUpcoming.YearsList = _upcoming.GetYearList();
                objUpcoming.MakesList = _upcoming.GetMakeList();
                CreatePager(objUpcoming, objUpcoming.PageMetaTags);
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.UpcomingPageModel.GetData");
            }
            return objUpcoming;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds Pager
        /// </summary>
        /// <param name="newLaunchesBikesVM"></param>
        /// <param name="objMeta"></param>
        private void CreatePager(UpcomingPageVM objUpcoming, PageMetaTags objMeta)
        {
            try
            {
                objUpcoming.Pager = new Entities.Pager.PagerEntity()
                {
                    PageNo = (int)_pageNumber,
                    PageSize = PageSize,
                    PagerSlotSize = 5,
                    BaseUrl = BaseUrl,
                    PageUrlType = "page/",
                    TotalResults = (int)(objUpcoming.TotalBikes)
                };
                int pages = (int)(objUpcoming.TotalBikes / PageSize);

                if ((objUpcoming.TotalBikes % PageSize) > 0)
                    pages += 1;
                string prevUrl = string.Empty, nextUrl = string.Empty;
                Paging.CreatePrevNextUrl(pages, BaseUrl, (int)objUpcoming.Pager.PageNo, ref nextUrl, ref prevUrl);
                objMeta.NextPageUrl = nextUrl;
                objMeta.PreviousPageUrl = prevUrl;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "NewLaunchedIndexModel.CreatePager()");
            }
        }

        #endregion
    }
}
