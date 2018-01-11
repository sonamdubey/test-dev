using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.Upcoming
{
    public class UpcomingByMakePageModel
    {
        #region Private variables

        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly UpcomingBikesListInputEntity _filters;
        private readonly ushort _pageNumber;
        private uint _totalPagesCount;
        private readonly string _makeMaskingName;
        private readonly IBikeMakesCacheRepository _bikeMakesCache = null;

        #endregion

        #region Public members

        public EnumUpcomingBikesFilter SortBy { get; set; }
        public int PageSize { get; set; }
        public string BaseUrl { get; set; }
        public bool IsMobile { get; set; }

        public StatusCodes Status { get; private set; }
        public String RedirectUrl { get; private set; }
        public uint MakeId { get; private set; }
        public uint topbrandCount { get; set; }

        #endregion

        #region Constructor

        public UpcomingByMakePageModel(string makeMaskingName, IUpcoming upcoming, ushort? pageNumber, int pageSize, IBikeModels<BikeModelEntity, int> bikeModels, string baseUrl, IBikeMakesCacheRepository bikeMakesCache)
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

            _bikeModels = bikeModels;
            _makeMaskingName = makeMaskingName;
            _bikeMakesCache = bikeMakesCache;
            ProcessQueryString();

            _filters.MakeId = (int)MakeId;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Created by : Sangram Nandkhile on 07-Apr-2017 
        /// Description : Gets the data.
        /// Modified by : snehal Dange on 28th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        public UpcomingPageVM GetData()
        {
            UpcomingPageVM objUpcoming = new UpcomingPageVM();
            try
            {
                objUpcoming.Make = _bikeMakesCache.GetMakeDetails(MakeId);
                objUpcoming.Brands = _upcoming.BindUpcomingMakes(topbrandCount);
                objUpcoming.PopularBikes = BindMostPopularBikes();
                UpcomingBikeResult bikeResult = _upcoming.GetBikes(_filters, SortBy);


                _totalPagesCount = (uint)(bikeResult.TotalCount / _filters.PageSize);

                if ((bikeResult.TotalCount % _filters.PageSize) > 0)
                    _totalPagesCount += 1;

                objUpcoming.UpcomingBikeModels = bikeResult.Bikes;
                objUpcoming.TotalBikes = bikeResult.TotalCount;

                objUpcoming.HasBikes = (objUpcoming.UpcomingBikeModels != null && objUpcoming.UpcomingBikeModels.Any());
                objUpcoming.YearsList = _upcoming.GetYearList(MakeId);
                CreatePager(objUpcoming, objUpcoming.PageMetaTags);
                objUpcoming.OtherMakes = new OtherMakesVM();
                objUpcoming.OtherMakes.Makes = _upcoming.OtherMakes(MakeId, 9);
                objUpcoming.Page = Entities.Pages.GAPages.Upcoming_MakeWise_Page;

                if (objUpcoming.Make != null)
                {
                    BindPageMetaTags(objUpcoming);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Upcoming.UpcomingByMakePageModel.GetData");
            }
            return objUpcoming;
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To fetch data for most popular bikes
        /// </summary>
        /// <returns></returns>
        private MostPopularBikeWidgetVM BindMostPopularBikes()
        {
            MostPopularBikeWidgetVM objPopularBikes = new MostPopularBikeWidgetVM();
            try
            {
                MostPopularBikesWidget popularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, true, false, PQSourceEnum.Desktop_Scooter_MakePage_PopularBikes, 0, MakeId);
                popularBikes.TopCount = 9;
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
                popularBikes.CityId = location.CityId;
                objPopularBikes = popularBikes.GetData();
                objPopularBikes.PageCatId = 5;
                objPopularBikes.PQSourceId = PQSourceEnum.Desktop_HP_MostPopular;
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "DealerShowroomDealerDetail.BindMostPopularBikes()");
            }
            return objPopularBikes;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 07-Apr-2017 
        /// Description : Binds the page meta tags.
        /// </summary>
        /// <param name="pageMetaTags">The page meta tags.</param>
        private void BindPageMetaTags(UpcomingPageVM objUpcoming)
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                objUpcoming.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/{0}-bikes/upcoming/", objUpcoming.Make.MaskingName);
                objUpcoming.PageMetaTags.AlternateUrl = string.Format("https://www.bikewale.com/m/{0}-bikes/upcoming/", objUpcoming.Make.MaskingName);
                objUpcoming.PageMetaTags.Keywords = string.Format("{0} upcoming, Expected {0} Launch, upcoming {0}, Latest {0} bikes", objUpcoming.Make.MakeName);
                objUpcoming.PageMetaTags.Description = string.Format("Find {0} upcoming bikes in India. Get details on expected launch date, prices for {0} bikes expected to launch in {1}.", objUpcoming.Make.MakeName, currentYear);
                objUpcoming.PageMetaTags.Title = string.Format("Upcoming {0} Bikes| Expected {0} Launches in {1} - BikeWale", objUpcoming.Make.MakeName, currentYear);

                if (_pageNumber > 1)
                {
                    objUpcoming.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", _pageNumber, _totalPagesCount, objUpcoming.PageMetaTags.Description);
                    objUpcoming.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", _pageNumber, _totalPagesCount, objUpcoming.PageMetaTags.Title);
                }

                objUpcoming.Page_H1 = string.Format("Upcoming {0} bikes", objUpcoming.Make.MakeName);

                SetBreadcrumList(objUpcoming);
                SetPageJSONLDSchema(objUpcoming);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.Upcoming.UpcomingByMakePageModel.BindPageMetaTags");
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(UpcomingPageVM objData)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objData.PageMetaTags, objData.BreadcrumbList);

            if (webpage != null)
            {
                objData.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(UpcomingPageVM objData)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", url), "New Bikes"));
            url += "upcoming-bikes/";
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Upcoming bikes"));

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objData.Page_H1));

            objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

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
                ErrorClass.LogError(ex, "Bikewale.Models.Upcoming.UpcomingByMakePageModel.CreatePager()");
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
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.Upcoming.UpcomingByMakePageModel.ProcessQueryString({0})", _makeMaskingName));
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