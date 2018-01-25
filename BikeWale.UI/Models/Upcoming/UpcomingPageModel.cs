using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
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
        private uint _totalPagesCount;
        private UpcomingBikesListInputEntity _filters;
        private readonly ICMSCacheContent _objArticles = null;

        #endregion


        #region Public members

        public EnumUpcomingBikesFilter SortBy { get; set; }
        public int PageSize { get; set; }
        public string BaseUrl { get; set; }
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor

        public UpcomingPageModel(uint topbrandCount, ushort? pageNumber, int pageSize, IUpcoming upcoming, INewBikeLaunchesBL newLaunches, string baseUrl, ICMSCacheContent objArticles)
        {
            _upcoming = upcoming;

            if (pageNumber.HasValue)
                _pageNumber = (ushort)pageNumber;
            else
                _pageNumber = 1;

            _newLaunches = newLaunches;
            _topBrandsCount = topbrandCount;

            _filters = new UpcomingBikesListInputEntity();
            _filters.PageSize = pageSize;
            _filters.PageNo = _pageNumber;

            SortBy = EnumUpcomingBikesFilter.LaunchDateSooner;
            BaseUrl = baseUrl;
            PageSize = pageSize;
            _objArticles = objArticles;

        }
        #endregion

        #region Functions

        /// <summary>
        /// Created by : Sangram Nandkhile on 07-Apr-2017 
        /// Gets the data.
        /// Modified by : snehal Dange on 28th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        public UpcomingPageVM GetData()
        {
            UpcomingPageVM objUpcoming = new UpcomingPageVM();
            try
            {
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                var upcomingBikes = _upcoming.GetModels(_filters, SortBy);
                objUpcoming.Brands = _upcoming.BindUpcomingMakes(_topBrandsCount);
                objUpcoming.NewLaunches = new NewLaunchedWidgetModel(9, location.CityId, _newLaunches).GetData();
                UpcomingBikeResult bikeResult = _upcoming.GetBikes(_filters, SortBy);

                _totalPagesCount = (uint)(bikeResult.TotalCount / _filters.PageSize);
                if ((bikeResult.TotalCount % _filters.PageSize) > 0)
                    _totalPagesCount += 1;

                BindPageMetaTags(objUpcoming.PageMetaTags);

                objUpcoming.UpcomingBikeModels = bikeResult.Bikes;
                objUpcoming.TotalBikes = bikeResult.TotalCount;
                //objUpcoming.NewLaunches.PageCatId = 1;
                objUpcoming.NewLaunches.PQSourceId = (uint)PQSourceEnum.Desktop_UpcomiingBikes_NewLaunchesWidget;
                objUpcoming.HasBikes = (objUpcoming.UpcomingBikeModels.Any());
                objUpcoming.YearsList = _upcoming.GetYearList();
                objUpcoming.MakesList = _upcoming.GetMakeList();
                BindCmsContent(objUpcoming);
                CreatePager(objUpcoming, objUpcoming.PageMetaTags);
                objUpcoming.Page = Entities.Pages.GAPages.Upcoming_Landing_Page;
                SetBreadcrumList(objUpcoming);
                SetPageJSONLDSchema(objUpcoming);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.UpcomingPageModel.GetData");
            }
            return objUpcoming;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 07-Apr-2017 
        /// Binds the page meta tags.
        /// </summary>        
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
                pageMetaTags.Description = string.Format("Find a list of upcoming bikes in India in {0}. Get details on expected launch date, prices for bikes expected to launch in {1}/{2}.", year, currentYear, nextYear);
                pageMetaTags.Title = string.Format("Upcoming Bikes in India | Expected Launches in {0}/{1} - BikeWale", currentYear, nextYear);

                if (_pageNumber > 1)
                {
                    pageMetaTags.Description = string.Format("Page {0} of {1} - {2}", _pageNumber, _totalPagesCount, pageMetaTags.Description);
                    pageMetaTags.Title = string.Format("Page {0} of {1} - {2}", _pageNumber, _totalPagesCount, pageMetaTags.Title);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.UpcomingPageModel.BindPageMetaTags");
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

                int pages = (int)_totalPagesCount;

                string prevUrl = string.Empty, nextUrl = string.Empty;
                Paging.CreatePrevNextUrl(pages, BaseUrl, (int)objUpcoming.Pager.PageNo, ref nextUrl, ref prevUrl);
                objMeta.NextPageUrl = nextUrl;
                objMeta.PreviousPageUrl = prevUrl;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.UpcomingPageModel.CreatePager()");
            }
        }

        /// <summary>
        /// Created By :Snehal Dange on 30th Oct 2017
        /// Description: Upcoming bikes news widget
        /// </summary>
        /// <param name="objUpcoming"></param>
        private void BindCmsContent(UpcomingPageVM objUpcoming)
        {
            try
            {
                if (objUpcoming != null && objUpcoming.UpcomingBikeModels != null)
                {
                    string modelId = string.Empty;
                    foreach (var obj in objUpcoming.UpcomingBikeModels)
                    {
                        modelId = string.IsNullOrEmpty(modelId) ? Convert.ToString(obj.ModelBase.ModelId) : string.Format("{0},{1}", modelId, obj.ModelBase.ModelId);
                    }
                    objUpcoming.News = new RecentNews(5, 0, modelId, _objArticles).GetData();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.UpcomingPageModel.BindCmsContent()");
            }

        }

        /// <summary>
        /// Created by : Snehal Dange on 12th Jan 2017
        /// Desc : Added breadcrum and webpage schema
        /// </summary>
        /// <param name="objPageMeta"></param>
        private void SetPageJSONLDSchema(UpcomingPageVM objPageMeta)
        {
            //set webpage schema 
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created by : Snehal Dange on 12th Dec 2017
        /// Desc : Added breadcrumb for upcoming page
        /// </summary>
        /// <param name="objData"></param>
        private void SetBreadcrumList(UpcomingPageVM objData)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    url += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
                url = string.Format("{0}new-bikes-in-india/", url);

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "New Bikes"));
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Upcoming Bikes"));

                objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.UpcomingPageModel.SetBreadcrumList()");
            }

        }
        #endregion
    }
}
