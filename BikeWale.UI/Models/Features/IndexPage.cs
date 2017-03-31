
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.Models.Features
{

    /// <summary>
    /// Created By :- Subodh Jain 31 March 2017
    /// Summary :- Model For Index Page
    /// </summary>
    public class IndexPage
    {
        private readonly ICMSCacheContent _Cache = null;
        private readonly IPager _objPager = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;

        public uint CurPageNo = 1, TopCount;
        private const int _pageSize = 10, _pagerSlotSize = 5;
        public StatusCodes status;
        public IndexPage(ICMSCacheContent Cache, IPager objPager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels)
        {
            _Cache = Cache;
            _objPager = objPager;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
        }
        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about feature article
        /// </summary>
        /// <returns></returns>
        public IndexFeatureVM GetData()
        {
            IndexFeatureVM objIndex = new IndexFeatureVM();
            GetFeaturesList(objIndex);
            BindLinkPager(objIndex);
            BindWidget(objIndex);
            BindPageMetas(objIndex);
            CreatePrevNextUrl(objIndex);
            return objIndex;
        }
        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching Meta Tags
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(IndexFeatureVM objPage)
        {

            try
            {
                objPage.PageMetaTags.Title = string.Format("Features - Stories, Specials & Travelogues | BikeWale");
                objPage.PageMetaTags.Description = string.Format("Features section of BikeWale brings specials, stories, travelogues and much more.");
                objPage.PageMetaTags.Keywords = string.Format("features, stories, travelogues, specials, drives.");

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindPageMetas()");
            }
        }
        /// <summary>
        /// Created By : Subodh Jain 31 March 2017
        /// Summary    : Bind Widgets
        /// </summary>
        private void BindWidget(IndexFeatureVM objIndex)
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                uint CityId = 0;
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;
                UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                objUpcomingBikes.Filters.StartIndex = 1;
                objUpcomingBikes.Filters.EndIndex = (int)TopCount;
                objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;

                objIndex.UpcomingBikes = objUpcomingBikes.GetData();


                objIndex.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                objIndex.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                objIndex.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, false, false);
                objPopularBikes.TopCount = (int)TopCount;
                objPopularBikes.CityId = CityId;


                objIndex.MostPopularBikes = objPopularBikes.GetData();
                objIndex.MostPopularBikes.WidgetHeading = "Popular bikes";
                objIndex.MostPopularBikes.WidgetHref = "/best-bikes-in-india/";
                objIndex.MostPopularBikes.WidgetLinkTitle = "Best Bikes in India";
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.BindWidget");

            }
        }

        /// <summary>
        /// Written By : Ashwini Todkar on 30 Sept 2014
        /// Summary    : PopulateWhere to get features list from web api asynchronously
        /// </summary>
        public void GetFeaturesList(IndexFeatureVM objIndex)
        {
            try
            {
                int _startIndex = 0, _endIndex = 0;

                _objPager.GetStartEndIndex(_pageSize, (int)CurPageNo, out _startIndex, out _endIndex);

                IList<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.Features);
                categorList.Add(EnumCMSContentType.SpecialFeature);
                string _featuresCategoryId = CommonApiOpn.GetContentTypesString(categorList);

                var _objFeaturesList = _Cache.GetArticlesByCategoryList(_featuresCategoryId, _startIndex, _endIndex, 0, 0);

                if (_objFeaturesList != null && _objFeaturesList.Articles.Count > 0)
                {

                    objIndex.ArticlesList = _objFeaturesList.Articles;
                    objIndex.TotalArticles = _objFeaturesList.RecordCount;
                    objIndex.StartIndex = (uint)_startIndex;
                    objIndex.EndIndex = (uint)_endIndex;
                    status = StatusCodes.ContentFound;

                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Models.Features.GetFeaturesList");
            }
        }
        /// <summary>
        /// Created By : Subodh Jain 31 March 2017
        /// Summary    : Bind previous and next url
        /// </summary>
        private void CreatePrevNextUrl(IndexFeatureVM objData)
        {
            string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
            int totalPages = _objPager.GetTotalPages((int)objData.TotalArticles, _pageSize);
            if (totalPages > 1)
            {
                if (CurPageNo == 1 && totalPages > 1)
                {
                    nextPageNumber = "2";
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
                else if (CurPageNo == totalPages)
                {
                    prevPageNumber = Convert.ToString(CurPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                }
                else
                {
                    prevPageNumber = Convert.ToString(CurPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                    nextPageNumber = Convert.ToString(CurPageNo + 1);
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
            }
        }

        /// <summary>
        /// Created By : Subodh Jain 31 March 2017
        /// Summary    : Bind link pager
        /// </summary>
        private void BindLinkPager(IndexFeatureVM objIndex)
        {
            try
            {
                string _baseUrl = RemoveTrailingPage(HttpContext.Current.Request.RawUrl.ToLower());
                objIndex.PagerEntity = new PagerEntity();

                objIndex.PagerEntity.PageNo = (int)CurPageNo;
                objIndex.PagerEntity.PagerSlotSize = _pagerSlotSize;
                objIndex.PagerEntity.BaseUrl = _baseUrl;
                objIndex.PagerEntity.PageUrlType = "page/";
                objIndex.PagerEntity.TotalResults = (int)objIndex.TotalArticles;
                objIndex.PagerEntity.PageSize = _pageSize;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception :Bikewale.Models.Features.IndexPage.BindLinkPager");
            }
        }
        /// <summary>
        /// Created By : Subodh Jain 31 March 2017
        /// Summary    : get raw url
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        private string RemoveTrailingPage(string rawUrl)
        {
            string retUrl = rawUrl;
            if (rawUrl.Contains("/page/"))
            {
                string[] urlArray = rawUrl.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                retUrl = string.Format("/{0}/", string.Join("/", urlArray.Take(urlArray.Length - 2).ToArray()));
            }
            return retUrl;
        }


    }
}