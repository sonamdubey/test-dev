using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.EditorialWidgets;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models.BikeModels;
using Bikewale.Models.EditorialPages;
using Bikewale.Utility;
using System;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Aditi Srivastava on 8 May 2017
    /// Summary : Model for the comparison test landing page
    /// </summary>
    public class ComparisonTestsIndexPage : EditorialBasePage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _pager;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeSeries _series;
        #endregion

        #region Page level variables
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        private uint CityId, pageCatId = 0;
        public string redirectUrl;
        public StatusCodes status;
        private GlobalCityAreaEntity currentCityArea;
        private EnumBikeType bikeType = EnumBikeType.All;
        private bool showCheckOnRoadCTA = false;
        private PQSourceEnum pqSource = 0;
        private string _pageName = "Editorial List";
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public ComparisonTestsIndexPage(ICMSCacheContent cmsCache, IPager pager, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeMakesCacheRepository objMakeCache, IBikeModelsCacheRepository<int> models, IBikeSeries series)
            : base(objMakeCache, models, bikeModels, upcoming, series)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _objMakeCache = objMakeCache;
            _models = models;
            _series = series;
            ProcessQueryString();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 8 May 2017
        /// Summary    : Function to get the comparison tests landing page data
        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        public ComparisonTestsIndexPageVM GetData(int widgetTopCount)
        {
            ComparisonTestsIndexPageVM objData = new ComparisonTestsIndexPageVM();

            try
            {
                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

                objData.Articles = _cmsCache.GetArticlesByCategoryList(Convert.ToString((int)EnumCMSContentType.ComparisonTests), _startIndex, _endIndex, 0, 0);

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > objData.Articles.RecordCount ? Convert.ToInt32(objData.Articles.RecordCount) : _endIndex;
                    BindLinkPager(objData);
                    SetPageMetas(objData);
                    CreatePrevNextUrl(objData);

                    SetAdditionalVariables(objData);
                    objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Listing);

                    objData.Page = Entities.Pages.GAPages.Editorial_List_Page;
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ComparisonTestsIndexPage.GetData");
            }

            return objData;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 8 May 2017
        /// Summary    : Process query string for comparison test page
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            var queryString = request != null ? request.QueryString : null;

            if (queryString != null)
            {

                if (!string.IsNullOrEmpty(queryString["pn"]))
                {
                    string _pageNo = queryString["pn"];
                    if (!string.IsNullOrEmpty(_pageNo))
                    {
                        int.TryParse(_pageNo, out curPageNo);
                    }
                }
            }
        }

        /// <summary>
        /// Created by  :  Aditi Srivasava on 8 May 2017
        /// Summary     :  Set page metas and headings
        /// </summary>
        private void SetPageMetas(ComparisonTestsIndexPageVM objData)
        {
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}/comparison-tests/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            objData.PageMetaTags.AlternateUrl = string.Format("{0}/m/comparison-tests/{1}", BWConfiguration.Instance.BwHostUrl, (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));

            objData.PageMetaTags.Title = "Comparison Tests by BikeWale experts - BikeWale";
            objData.PageMetaTags.Description = "Find comparison of popular bikes from BikeWale experts. Know details about performance, engine, handling, fuel efficiency and many more features.";
            objData.PageMetaTags.Keywords = "Compare bikes, bike comparison, comparison test, comparison articles, expert review";
            objData.PageH1 = string.Format("Comparison Tests");
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 8 May 2017
        /// Summary    : Bind link pager
        /// </summary>
        private void BindLinkPager(ComparisonTestsIndexPageVM objData)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                objData.PagerEntity.BaseUrl = (IsMobile ? "/m/comparison-tests/" : "/comparison-tests/");
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = (int)objData.Articles.RecordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ComparisonTestsIndexPage.BindLinkPager");
            }
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 8 May 2017
        /// Summary    : Create previous and next page urls
        /// </summary>
        /// <param name="objData"></param>
        private void CreatePrevNextUrl(ComparisonTestsIndexPageVM objData)
        {
            string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
            int totalPages = _pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);
            if (totalPages > 1)
            {
                if (curPageNo == 1)
                {
                    nextPageNumber = "2";
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
                else if (curPageNo == totalPages)
                {
                    prevPageNumber = Convert.ToString(curPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                }
                else
                {
                    prevPageNumber = Convert.ToString(curPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                    nextPageNumber = Convert.ToString(curPageNo + 1);
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
            }
        }

        /// <summary>
        /// Created By  : Deepak Israni on 26 April 2018
        /// Description : Function to set additional Page level variables.
        /// </summary>
        /// <param name="objData"></param>
        private void SetAdditionalVariables(ComparisonTestsIndexPageVM objData)
        {
            try
            {
                objData.PageName = _pageName;
                EditorialWidgetEntity editorialWidgetData = new EditorialWidgetEntity
                {
                    IsMobile = IsMobile,
                    CityId = CityId
                };


                base.SetAdditionalData(editorialWidgetData);

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.News.ComparisonTestsIndexPage.SetAdditionalVariables");
            }
        }

        #endregion
    }
}