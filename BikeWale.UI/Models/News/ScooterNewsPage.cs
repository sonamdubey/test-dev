using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Models.News
{
    /// <summary>
    /// Created by: Snehal Dange on 17th Aug 2017
    /// Summary: Model for News for scooters landing page
    /// </summary>
    public class ScooterNewsPage
    {
        #region Variables for dependency injection and constructor
        private readonly ICMSCacheContent _articles = null;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;

        #endregion

        #region Page level variables
        private uint MakeId, pageCatId = 0, CityId;
        private const int _pageSize = 10, _pagerSlotSize = 5;
        private int _curPageNo = 1;
        private uint _totalPagesCount;
        private string _make = string.Empty, _model = string.Empty;
        private GlobalCityAreaEntity _currentCityArea = null;
        public string RedirectUrl;
        public StatusCodes Status;
        private BikeModelEntity _objModel = null;
        private BikeMakeEntityBase _objMake = null;
        private EnumBikeType bikeType = EnumBikeType.Scooters;
        private bool _showCheckOnRoadCTA = false;
        private PQSourceEnum _pqSource = 0;

        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        public int WidgetTopCount { get; set; }

        #endregion

        #region Constructor
        public ScooterNewsPage(ICMSCacheContent articles, IPager pager, IBikeModelsCacheRepository<int> models, IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IPWACMSCacheRepository renderedArticles)
        {
            _articles = articles;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _renderedArticles = renderedArticles;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            ProcessQueryString();
        }
        #endregion

        /// <summary>
        /// Created by : Snehal Dange on 17th Aug 2017
        /// Summary    : Process query string for  scooters news page
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
                        int.TryParse(_pageNo, out _curPageNo);
                    }
                }
                _make = queryString["make"];


                ProcessMakeMaskingName(request, _make);

            }
        }




        /// <summary>
        /// Created by  : Snehal Dange on 17th Aug 2017
        /// Summary     :  Processes Make masking name for scooters
        /// </summary>
        private void ProcessMakeMaskingName(HttpRequest request, string make)
        {
            try
            {
                MakeMaskingResponse makeResponse = null;
                if (!string.IsNullOrEmpty(make))
                {
                    makeResponse = _bikeMakesCacheRepository.GetMakeMaskingResponse(make);
                }
                if (makeResponse != null)
                {
                    if (makeResponse.StatusCode == 200)
                    {
                        MakeId = makeResponse.MakeId;
                        _objMake = _bikeMakesCacheRepository.GetMakeDetails(MakeId);
                    }
                    else if (makeResponse.StatusCode == 301)
                    {
                        Status = StatusCodes.RedirectPermanent;
                        RedirectUrl = request.RawUrl.Replace(make, makeResponse.MaskingName);
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, string.Format("Bikewale.Models.News.ProcessMakeMaskingName Request:{0} Make:{1}", request, make));
            }
        }

        /// <summary>
        /// Created By : Snehal Dange on 17 August 2017
        /// Summary    : Bind link pager
        /// </summary>
        private void BindLinkPager(NewsScootersPageVM objData)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatScootersNewsUrl(_make, _model));
                objData.PagerEntity.PageNo = _curPageNo;
                objData.PagerEntity.PagerSlotSize = _pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = (int)objData.Articles.RecordCount;
                objData.PagerEntity.PageSize = _pageSize;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Models.News.NewsScootersPage.BindLinkPager Make:{0} ", objData.Make));
            }
        }


        /// <summary>
        /// Created by  :  Snehal Dange on 17 August 2017
        /// Summary     :  Set page metas and headings
        /// </summary>
        private void SetPageMetas(NewsScootersPageVM objData)
        {
            try
            {
                objData.PageMetaTags.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatScootersNewsUrl(_make, _model), (_curPageNo > 1 ? string.Format("page/{0}/", _curPageNo) : ""));
                objData.PageMetaTags.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatScootersNewsUrl(_make, _model), (_curPageNo > 1 ? string.Format("page/{0}/", _curPageNo) : ""));
                if (MakeId > 0)
                {
                    objData.PageMetaTags.Title = string.Format("{0} Scooter News | Latest news about {0} scooters - BikeWale", _objMake.MakeName);
                    objData.PageMetaTags.Description = String.Format("Read the latest news about scooters. Know more about {0} scooter new launch updates, and much more from two wheeler industry.", _objMake.MakeName);
                    objData.PageMetaTags.Keywords = string.Format("{0} Scooter news, {0} scooter updates,{0} scooty news, {0} scooty updates", _objMake.MakeName);
                    objData.PageH1 = string.Format("{0} Scooter News", _objMake.MakeName);
                    objData.PageH2 = string.Format("Latest news and views about {0} scooters", _objMake.MakeName);
                    objData.AdTags.TargetedMakes = _objMake.MakeName;
                }
                else
                {
                    objData.PageMetaTags.Title = "Scooter News | Latest news about scooters - BikeWale";
                    objData.PageMetaTags.Description = "Read the latest news about scooters. Know more about scooter new launch updates, and much more from two wheeler industry.";
                    objData.PageMetaTags.Keywords = "scooter news, scooty news, auto news, scooter launch, Indian scooter news";
                    objData.PageH1 = "Scooter News";
                    objData.PageH2 = "Latest News and Views about Scooters";
                }

                if (_curPageNo > 1)
                {
                    objData.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", _curPageNo, _totalPagesCount, objData.PageMetaTags.Description);
                    objData.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", _curPageNo, _totalPagesCount, objData.PageMetaTags.Title);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Models.News.NewsScootersPage.SetPageMetas Make:{0}", objData.Make));
            }
        }


        /// <summary>
        /// Created By : Snehal Dange on 17 August 2017
        /// Summary    : Create previous and next page urls
        /// </summary>
        /// <param name="objData"></param>
        private void CreatePrevNextUrl(NewsScootersPageVM objData)
        {
            string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
            int totalPages = _pager.GetTotalPages((int)objData.Articles.RecordCount, _pageSize);
            if (totalPages > 1)
            {
                if (_curPageNo == 1)
                {
                    nextPageNumber = "2";
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
                else if (_curPageNo == totalPages)
                {
                    prevPageNumber = Convert.ToString(_curPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                }
                else
                {
                    prevPageNumber = Convert.ToString(_curPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                    nextPageNumber = Convert.ToString(_curPageNo + 1);
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
            }
        }


        /// <summary>
        /// Created by : Snehal Dange on 17 August 2017
        /// Summary    : Get view model for page widgets
        /// </summary>
        private void GetWidgetData(NewsScootersPageVM objData)
        {
            try
            {
                _currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (_currentCityArea != null)
                    CityId = _currentCityArea.CityId;

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, _showCheckOnRoadCTA, false, _pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = WidgetTopCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();
                if (MakeId > 0 && _objMake != null)
                {
                    objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} Scooters", _objMake.MakeName);
                    objData.MostPopularBikes.WidgetHref = string.Format("/{0}-scooters/", _objMake.MaskingName);
                    objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Scooters", _objMake.MakeName);
                }
                else
                {
                    objData.MostPopularBikes.WidgetHeading = "Popular Scooters";
                    objData.MostPopularBikes.WidgetHref = "/best-scooters-in-india/";
                    objData.MostPopularBikes.WidgetLinkTitle = "Best Scooters in India";
                }

                Scooters.PopularScooterBrandsWidget objPopularScooterBrands = new Scooters.PopularScooterBrandsWidget(_bikeMakesCacheRepository);
                objPopularScooterBrands.TopCount = 4;

                if (MakeId > 0)
                {
                    objPopularScooterBrands.SkipMakeId = MakeId;
                    objData.PopularScooterBrandsWidgetHeading = "Other scooter brands";
                }
                else
                    objData.PopularScooterBrandsWidgetHeading = "Popular scooter brands";

                objData.PopularScooterMakesWidget = objPopularScooterBrands.GetData();

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Models.News.NewsScootersPage.GetWidgetData Make:{0}", objData.Make));
            }
        }


        /// <summary>
        /// Created By : Snehal Dange on 17 August 2017
        /// Summary    : Get news data for scooters
        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        /// <returns></returns>
        public NewsScootersPageVM GetData()
        {
            NewsScootersPageVM objData = new NewsScootersPageVM();

            try
            {
                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(_pageSize, _curPageNo, out _startIndex, out _endIndex);

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.News);
                if (MakeId == 0)
                {
                    categorList.Add(EnumCMSContentType.AutoExpo2016);
                    categorList.Add(EnumCMSContentType.Features);
                    categorList.Add(EnumCMSContentType.RoadTest);
                    categorList.Add(EnumCMSContentType.ComparisonTests);
                    categorList.Add(EnumCMSContentType.SpecialFeature);
                    categorList.Add(EnumCMSContentType.TipsAndAdvices);
                }
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                if (_objMake != null)
                    objData.Make = _objMake;
                if (_objModel != null)
                    objData.Model = _objModel;


                objData.Articles = _articles.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, Convert.ToString((int)EnumBikeBodyStyles.Scooter), (int)MakeId);

                _totalPagesCount = (uint)_pager.GetTotalPages((int)objData.Articles.RecordCount, _pageSize);

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    Status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > objData.Articles.RecordCount ? Convert.ToInt32(objData.Articles.RecordCount) : _endIndex;
                    BindLinkPager(objData);
                    SetPageMetas(objData);
                    CreatePrevNextUrl(objData);
                    GetWidgetData(objData);
                    SetBreadcrumList(objData);
                    objData.Page = Entities.Pages.GAPages.Editorial_List_Page;
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.News.NewsScootersPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created By : Snehal Dange on 10th Nov 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(NewsScootersPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null)
                {
                    IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                    string bikeUrl;
                    bikeUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                    ushort position = 1;
                    if (IsMobile)
                    {
                        bikeUrl += "m/";
                    }

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));
                    if (objPageVM.Make != null && objPageVM.Make.MakeId > 0)
                    {
                        bikeUrl = string.Format("{0}{1}-scooters/", bikeUrl, objPageVM.Make.MaskingName);
                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} Scooters", objPageVM.Make.MakeName)));
                    }
                    else
                    {
                        bikeUrl = string.Format("{0}scooters/", bikeUrl);
                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Scooters"));
                    }

                    if (objPageVM.Make != null && objPageVM.Make.MakeId > 0)
                    {
                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, string.Format("{0} Scooters News", objPageVM.Make.MakeName)));
                    }
                    else
                    {
                        BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Scooters News"));
                    }
                    objPageVM.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.News.NewsScootersPage.SetBreadcrumList()");
            }




        }

    }
}