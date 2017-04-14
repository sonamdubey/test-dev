using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Utility;
using System;
using System.Linq;
using System.Web;

namespace Bikewale.Models.Upcoming
{
    public class UpcomingByMakePageModel
    {
        #region Private variables

        private IUpcoming _upcoming = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        public uint topbrandCount { get; set; }
        private UpcomingBikesListInputEntity _filters;
        private readonly ushort _pageNumber;
        private readonly string _makeMaskingName;

        #endregion

        #region Public members

        public EnumUpcomingBikesFilter SortBy { get; set; }
        public int PageSize { get; set; }
        public string BaseUrl { get; set; }

        public StatusCodes Status { get; private set; }
        public String RedirectUrl { get; private set; }
        public uint MakeId { get; private set; }

        #endregion

        #region Constructor

        public UpcomingByMakePageModel(string makeMaskingName, IUpcoming upcoming, ushort? pageNumber, int pageSize, INewBikeLaunchesBL newLaunches, string baseUrl)
        {
            _upcoming = upcoming;

            if (pageNumber.HasValue)
                _pageNumber = (ushort)pageNumber;
            else
                _pageNumber = 1;

            _filters = new UpcomingBikesListInputEntity();
            _filters.PageSize = pageSize;
            _filters.PageNo = _pageNumber;

            SortBy = EnumUpcomingBikesFilter.LaunchDateSooner;
            BaseUrl = baseUrl;
            PageSize = pageSize;

            _newLaunches = newLaunches;
            _makeMaskingName = makeMaskingName;
            ProcessQueryString();

            _filters.MakeId = (int)MakeId;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Created by : Sangram Nandkhile on 07-Apr-2017 
        /// Description : Gets the data.
        /// </summary>
        public UpcomingPageVM GetData()
        {
            UpcomingPageVM objUpcoming = new UpcomingPageVM();
            try
            {
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                objUpcoming.Make = new MakeHelper().GetMakeNameByMakeId(MakeId);
                BindPageMetaTags(objUpcoming.PageMetaTags, _makeMaskingName, objUpcoming.Make.MakeName);
                var upcomingBikes = _upcoming.GetModels(_filters, SortBy);
                objUpcoming.Brands = _upcoming.BindUpcomingMakes(topbrandCount);
                objUpcoming.NewLaunches = new NewLaunchedWidgetModel(MakeId, location.CityId, 9, _newLaunches).GetData();
                objUpcoming.NewLaunches.PQSourceId = (uint)PQSourceEnum.Desktop_UpcomiingBikes_NewLaunchesWidget;
                UpcomingBikeResult bikeResult = _upcoming.GetBikes(_filters, SortBy);
                objUpcoming.UpcomingBikeModels = bikeResult.Bikes;
                objUpcoming.TotalBikes = bikeResult.TotalCount;

                objUpcoming.HasBikes = (objUpcoming.UpcomingBikeModels.Count() > 0);
                objUpcoming.YearsList = _upcoming.GetYearList(MakeId);
                CreatePager(objUpcoming, objUpcoming.PageMetaTags);
                objUpcoming.OtherMakes = new OtherMakesVM();
                objUpcoming.OtherMakes.Makes = _upcoming.OtherMakes(MakeId,9);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Upcoming.UpcomingByMakePageModel.GetData");
            }
            return objUpcoming;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 07-Apr-2017 
        /// Description : Binds the page meta tags.
        /// </summary>
        /// <param name="pageMetaTags">The page meta tags.</param>
        private void BindPageMetaTags(PageMetaTags pageMetaTags, string makeMaskingName, string makeName)
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                pageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/upcoming/", makeMaskingName);
                pageMetaTags.AlternateUrl = string.Format("https://www.bikewale.com/m/{0}-bikes/upcoming/", makeMaskingName);
                pageMetaTags.Keywords = string.Format("{0} upcoming, Expected {0} Launch, upcoming {0}, Latest {0} bikes", makeName);
                pageMetaTags.Description = string.Format("Find {0} upcoming bikes in India. Get details on expected launch date, prices for {0} bikes expected to launch in {1}.", makeName, currentYear);
                pageMetaTags.Title = string.Format("Upcoming {0} Bikes| Expected {0} Launches in {1} - BikeWale", makeName, currentYear);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.Upcoming.UpcomingByMakePageModel.BindPageMetaTags");
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.Upcoming.UpcomingByMakePageModel.CreatePager()");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   ProcessQueryString to process make masking name
        /// </summary>
        private void ProcessQueryString()
        {
            String rawUrl = HttpContext.Current.Request.RawUrl;
            MakeMaskingResponse objMakeResponse = null;
            try
            {
                objMakeResponse = new MakeHelper().GetMakeByMaskingName(_makeMaskingName);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("Bikewale.Models.Upcoming.UpcomingByMakePageModel.ProcessQueryString({0})", _makeMaskingName));
                Status = StatusCodes.ContentNotFound;
            }
            finally
            {
                if (objMakeResponse != null)
                {
                    if (objMakeResponse.StatusCode == 200)
                    {
                        MakeId = objMakeResponse.MakeId;
                        Status = StatusCodes.ContentFound;
                    }
                    else if (objMakeResponse.StatusCode == 301)
                    {
                        rawUrl = rawUrl.Replace(_makeMaskingName, objMakeResponse.MaskingName);
                        Status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    RedirectUrl = rawUrl;
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
        }

        #endregion
    }
}