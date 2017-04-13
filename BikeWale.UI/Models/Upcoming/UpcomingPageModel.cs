using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData.NewLaunched;
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
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly uint _topBrandsCount;
        private readonly ushort _pageNumber;

        #endregion


        #region Public members

        private UpcomingBikesListInputEntity filters;
        public EnumUpcomingBikesFilter SortBy { get; set; }
        public int PageSize { get; set; }
        public string BaseUrl { get; set; }

        #endregion

        #region Constructor

        public UpcomingPageModel(uint topbrandCount, ushort? pageNumber, int pageSize, IUpcoming upcoming, INewBikeLaunchesBL newLaunches, string baseUrl)
        {
            _upcoming = upcoming;

            if (pageNumber.HasValue)
                _pageNumber = (ushort)pageNumber;
            else
                _pageNumber = 1;

            _newLaunches = newLaunches;
            _topBrandsCount = topbrandCount;
            filters = new UpcomingBikesListInputEntity();
            filters.PageSize = pageSize;
            SortBy = EnumUpcomingBikesFilter.LaunchDateSooner;
            BaseUrl = baseUrl;
            PageSize = pageSize;
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
                BindPageMetaTags(objUpcoming.PageMetaTags);
                var upcomingBikes = _upcoming.GetModels(filters, SortBy);
                objUpcoming.Brands = _upcoming.BindUpcomingMakes(_topBrandsCount);
                objUpcoming.NewLaunches = new NewLaunchedWidgetModel(9, _newLaunches).GetData();
                filters.PageNo = _pageNumber;
                UpcomingBikeResult bikeResult = _upcoming.GetBikes(filters, SortBy);
                objUpcoming.UpcomingBikeModels = bikeResult.Bikes;
                objUpcoming.TotalBikes = bikeResult.TotalCount;
                //objUpcoming.NewLaunches.PageCatId = 1;
                objUpcoming.NewLaunches.PQSourceId = (uint)PQSourceEnum.Desktop_UpcomiingBikes_NewLaunchesWidget;
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
        /// Binds the page meta tags.
        /// </summary>
        /// <param name="pageMetaTags">The page meta tags.</param>
        private void BindPageMetaTags(PageMetaTags pageMetaTags)
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                string nextYear = DateTime.Now.AddYears(1).Year.ToString().Substring(2);
                string year = string.Format("{0}-{1}", currentYear, nextYear);
                pageMetaTags.CanonicalUrl = "https://www.bikewale.com/upcoming-bikes/";
                pageMetaTags.AlternateUrl = "https://www.bikewale.com/m/upcoming-bikes/";
                pageMetaTags.Keywords = "Upcoming bikes, expected launch, new bikes, upcoming scooter, upcoming, to be released bikes, bikes to be launched";
                pageMetaTags.Description = string.Format("Find a list of upcoming bikes in India in {0}. Get details on expected launch date, prices for bikes expected to launch in {1}.", year, currentYear);
                pageMetaTags.Title = string.Format("Upcoming Bikes in India | Expected Launches in {0} - BikeWale", currentYear);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.UpcomingPageModel.BindPageMetaTags");
            }
        }
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
