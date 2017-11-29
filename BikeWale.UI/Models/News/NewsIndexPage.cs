using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Models.BestBikes;
using Bikewale.Models.Scooters;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
	/// <summary>
	/// Created by : Aditi Srivastava on 27 Mar 2017
	/// Summary    : Model to get data for news default page
	/// Modified by:Snehal Dange on 24 August,2017
	/// Description: Added _bikeMakesCacheRepository,_objBikeVersionsCache.
	///              Added PopularScooterBrandsWidget
	/// Modified by : Ashutosh Sharma on 27 Nov 2017
	/// Description : Added IBikeSeriesCacheRepository and IBikeSeries for series news page.             
	/// </summary>
	public class NewsIndexPage
    {
        #region Variables for dependency injection and constructor
        private readonly ICMSCacheContent _cacheContent = null;
        private readonly IArticles _articles = null;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
		private readonly IBikeSeriesCacheRepository _seriesCache;
		private readonly IBikeSeries _series;
		#endregion

		#region Page level variables
		private uint MakeId, ModelId, pageCatId = 0, CityId;
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        private uint _totalPagesCount;
        private string make = string.Empty, model = string.Empty, series = string.Empty;
        private MakeHelper makeHelper = null;
        private ModelHelper modelHelper = null;
        private GlobalCityAreaEntity currentCityArea = null;
        public string redirectUrl;
        public StatusCodes status;
        private BikeModelEntity objModel = null;
        private BikeMakeEntityBase objMake = null;
		private BikeSeriesEntityBase objSeries;
		private EnumBikeType bikeType = EnumBikeType.All;
        private bool showCheckOnRoadCTA = false;
        private PQSourceEnum pqSource = 0;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }

        public string CityName
        {
            get
            {
                if (currentCityArea == null)
                {
                    currentCityArea = GlobalCityArea.GetGlobalCityArea();
                    if (currentCityArea != null)
                        CityId = currentCityArea.CityId;
                }

                return string.IsNullOrEmpty(currentCityArea.City) ? string.Empty : currentCityArea.City;
            }
        }

        #endregion

        #region Constructor

        static string _newsContentType, _allContentTypes;
        static NewsIndexPage()
        {
            List<EnumCMSContentType> categoryList = new List<EnumCMSContentType>();
            categoryList.Add(EnumCMSContentType.News);
            _newsContentType = CommonApiOpn.GetContentTypesString(categoryList);

            categoryList.Add(EnumCMSContentType.AutoExpo2016);
            categoryList.Add(EnumCMSContentType.Features);
            categoryList.Add(EnumCMSContentType.RoadTest);
            categoryList.Add(EnumCMSContentType.ComparisonTests);
            categoryList.Add(EnumCMSContentType.SpecialFeature);
            categoryList.Add(EnumCMSContentType.TipsAndAdvices);

            _allContentTypes = CommonApiOpn.GetContentTypesString(categoryList);
        }

		/// <summary>
		/// Modified by : Ashutosh Sharma on 27 Nov 2017
		/// Description : Added IBikeSeriesCacheRepository and IBikeSeries for series news page.
		/// </summary>
		public NewsIndexPage(ICMSCacheContent cacheContent, IPager pager, IBikeMakesCacheRepository objMakeCache, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IPWACMSCacheRepository renderedArticles, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache, IArticles articles, IBikeSeriesCacheRepository seriesCache, IBikeSeries series)
        {
            _articles = articles;
            _cacheContent = cacheContent;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _renderedArticles = renderedArticles;
            _objMakeCache = objMakeCache;
            _objBikeVersionsCache = objBikeVersionsCache;
			_seriesCache = seriesCache;
			_series = series;
			ProcessQueryString();
        }
		#endregion

		#region Functions

		/// <summary>
		/// Created By : Aditi Srivastava on 27 Mar 2017
		/// Summary    : Get page data
		/// Modified by : Ashutosh Sharma on 27 Nov 2017
		/// Description : Added call to GetArticlesByCategoryList for news page of series.
		/// </summary>
		/// <returns></returns>
		public NewsIndexPageVM GetData(int widgetTopCount)
        {
            NewsIndexPageVM objData = new NewsIndexPageVM();

            try
            {
				string modelIds = string.Empty;

				int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                string contentTypeList = (MakeId == 0 && ModelId == 0) ? _allContentTypes : _newsContentType;

                if (objMake != null)
                    objData.Make = objMake;
                if (objModel != null)
                    objData.Model = objModel;
				if (objSeries != null)
				{
					objData.Series = objSeries;
				}
				if (objData.Series != null)
				{
					modelIds = _series.GetModelIdsBySeries(objData.Series.SeriesId);
					objData.Articles = _cacheContent.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, (int)MakeId, modelIds);

				}
				else
				{
					objData.Articles = _cacheContent.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, (int)MakeId, (int)ModelId);

				}

				_totalPagesCount = (uint)_pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    int recordCount = (int)objData.Articles.RecordCount;
                    status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > recordCount ? recordCount : _endIndex;
                    BindLinkPager(objData, recordCount);
                    CreatePrevNextUrl(objData, recordCount);
                    GetWidgetData(objData, widgetTopCount);
                    SetPageMetas(objData);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.News.NewsIndexPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created By : Prasad Gawde on 25 May 2017
        /// Summary    : Get page data for PWA
        /// </summary>
        /// <returns></returns>
        public NewsIndexPageVM GetPwaData(int widgetTopCount)
        {
            NewsIndexPageVM objData = new NewsIndexPageVM();

            try
            {
                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                string contentTypeList = (MakeId == 0 && ModelId == 0) ? _allContentTypes : _newsContentType;

                if (objMake != null)
                    objData.Make = objMake;
                if (objModel != null)
                    objData.Model = objModel;

                //objData.Articles
                var pwaCmsContent = _articles.GetArticlesByCategoryListPwa(contentTypeList, _startIndex, _endIndex, (int)MakeId, (int)ModelId);



                if (pwaCmsContent != null && pwaCmsContent.RecordCount > 0)
                {
                    _totalPagesCount = (uint)_pager.GetTotalPages((int)pwaCmsContent.RecordCount, pageSize);
                    status = StatusCodes.ContentFound;
                    int recordCount = (int)pwaCmsContent.RecordCount;

                    pwaCmsContent.StartIndex = (uint)_startIndex;
                    pwaCmsContent.EndIndex = (uint)(_endIndex > recordCount ? recordCount : _endIndex);
                    BindLinkPager(objData, recordCount); //needs the record coutn
                    SetPageMetas(objData); //needs nothing
                    CreatePrevNextUrl(objData, recordCount); // needs record count
                    GetWidgetData(objData, widgetTopCount); // needs nothing

                    try
                    {
                        if ((objData.Model == null || string.IsNullOrEmpty(objData.Model.ModelName)) &&
                            (objData.Make == null || string.IsNullOrEmpty(objData.Make.MakeName)))
                        {
                            //setting the store for Redux
                            objData.ReduxStore = new PwaReduxStore();
                            objData.ReduxStore.News.NewsArticleListReducer.ArticleListData.ArticleList = pwaCmsContent;                          
                            PopulateStoreForWidgetData(objData, CityName);

                            var storeJson = JsonConvert.SerializeObject(objData.ReduxStore);

                            objData.ServerRouterWrapper = _renderedArticles.GetNewsListDetails(PwaCmsHelper.GetSha256Hash(storeJson), objData.ReduxStore.News.NewsArticleListReducer,
                                "/m/news/", "root", "ServerRouterWrapper");
                            objData.WindowState = storeJson;
                        }
                    }
                    catch
                    {
                        status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.News.NewsIndexPage.GetPwaData");
            }
            return objData;
        }

        private void PopulateStoreForWidgetData(NewsIndexPageVM objData, string cityName)
        {
            List<PwaBikeNews> objPwaBikeNews = new List<PwaBikeNews>();
            if (objData.MostPopularBikes != null && objData.MostPopularBikes.Bikes != null)
            {
                PwaBikeNews popularBikes = new PwaBikeNews();
                popularBikes.Heading = "Popular bikes";
                popularBikes.CompleteListUrl = "/m/best-bikes-in-india/";
                popularBikes.CompleteListUrlAlternateLabel = "Best Bikes in India";
                popularBikes.CompleteListUrlLabel = "View all";
                popularBikes.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(objData.MostPopularBikes.Bikes,
                    cityName);

                objPwaBikeNews.Add(popularBikes);
            }

            if (objData.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes != null)
            {
                PwaBikeNews upcomingBikes = new PwaBikeNews();
                upcomingBikes.Heading = "Upcoming bikes";
                upcomingBikes.CompleteListUrl = "/m/upcoming-bikes/";
                upcomingBikes.CompleteListUrlAlternateLabel = "Upcoming Bikes in India";
                upcomingBikes.CompleteListUrlLabel = "View all";
                upcomingBikes.BikesList = ConverterUtility.MapUpcomingBikeEntityToPwaBikeDetails(objData.UpcomingBikes.UpcomingBikes
                    , cityName);
                objPwaBikeNews.Add(upcomingBikes);
            }

            objData.ReduxStore.News.NewsArticleListReducer.NewBikesListData.NewBikesList = objPwaBikeNews;
        }


		/// <summary>
		/// Created by : Aditi Srivastava on 27 Mar 2017
		/// Summary    : Process query string for news page
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
                make = queryString["make"];
                string maskingName = queryString["model"];

                ProcessMakeMaskingName(request, make);
                ProcessModelSeriesMaskingName(request, maskingName);
            }
        }
		/// <summary>
		/// Created by  :  Aditi Srivasava on 27 Mar 2017
		/// Summary     :  Processes model masking name
		/// Modifies by : Ashutosh Sharma on 27 Nov 2017
		/// Description : Process series and model masking name, get news data of series if series page is created otherwise	model news. Changed method name from 'ProcessModelMaskingName' to 'ProcessModelSeriesMaskingName'
		/// </summary>
		private void ProcessModelSeriesMaskingName(HttpRequest request, string maskingName)
        {
			try
			{
				SeriesMaskingResponse objResponse = null;
				if (!string.IsNullOrEmpty(maskingName))
				{
					objResponse = _seriesCache.ProcessMaskingName(maskingName);
				}
				if (objResponse != null)
				{
					if (objResponse.StatusCode == 200)
					{
						if (!objResponse.IsSeriesPageCreated)
						{
							modelHelper = new ModelHelper();
							model = objResponse.MaskingName;
							ModelId = objResponse.Id;
							objModel = modelHelper.GetModelDataById(objResponse.Id);
						}
						else
						{
							series = objResponse.MaskingName;
							objSeries = new BikeSeriesEntityBase
							{
								SeriesId = objResponse.Id,
								SeriesName = objResponse.Name,
								MaskingName = series,
								IsSeriesPageUrl = true
							};
						}
					}
					else if (objResponse.StatusCode == 301)
					{
						status = StatusCodes.RedirectPermanent;
						redirectUrl = request.RawUrl.Replace(objResponse.MaskingName, objResponse.NewMaskingName);
					}
					else
					{
						status = StatusCodes.ContentNotFound;
					}
				}
				else
				{
					status = StatusCodes.ContentNotFound;
				}
			}
			catch (Exception ex)
			{
				Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.News.NewsIndexPage.ProcessModelSeriesMaskingName");
			}
        }

        /// <summary>
        /// Created by  :  Aditi Srivasava on 27 Mar 2017
        /// Summary     :  Processes Make masking name
        /// </summary>
        private void ProcessMakeMaskingName(HttpRequest request, string make)
        {
            MakeMaskingResponse makeResponse = null;
            if (!string.IsNullOrEmpty(make))
            {
                makeHelper = new MakeHelper();
                makeResponse = makeHelper.GetMakeByMaskingName(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    MakeId = makeResponse.MakeId;
                    objMake = makeHelper.GetMakeNameByMakeId(MakeId);
                }
                else if (makeResponse.StatusCode == 301)
                {
                    status = StatusCodes.RedirectPermanent;
                    redirectUrl = request.RawUrl.Replace(make, makeResponse.MaskingName);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
        }

		/// <summary>
		/// Created by  :  Aditi Srivasava on 28 Mar 2017
		/// Summary     :  Set page metas and headings
		/// /// Modified by :- Subodh Jain 19 june 2017
		/// Summary :- Added TargetModels and Target Make
		/// Modified by :- Snehal Dange 24 August 2017
		/// Summary :- Added code for BodyStyle.Scooters
		/// Modifies by : Ashutosh Sharma on 27 Nov 2017
		/// Description : Added logic for series news page.
		/// </summary>
		private void SetPageMetas(NewsIndexPageVM objData)
        {
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrl, UrlFormatter.FormatNewsUrl(make, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            objData.PageMetaTags.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrl, UrlFormatter.FormatNewsUrl(make, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));

            EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;

			if (objData.Series != null && objData.Series.SeriesId > 0 && objMake != null)
			{
				objData.PageMetaTags.Title = string.Format("Latest news about all {0} {1} {2} | {0} {1} news - BikeWale", objMake.MakeName, objData.Series.SeriesName, bodyStyle.Equals(EnumBikeBodyStyles.Scooter) ? "scooters" : "bikes");

				objData.PageMetaTags.Description = String.Format("Read the latest news about all {0} {1} {2} on BikeWale. Catch up on the latest buzz around {0}", objMake.MakeName, objData.Series.SeriesName, bodyStyle.Equals(EnumBikeBodyStyles.Scooter) ? "scooters" : "bikes");

				objData.PageMetaTags.Keywords = string.Format("News about {0} {1}, {0} {1} News", objMake.MakeName, objData.Series.SeriesName);
				objData.PageH1 = string.Format("{0} {1} News", objMake.MakeName, objData.Series.SeriesName);
				objData.PageH2 = string.Format("Latest {0} Bikes News and Views", objMake.MakeName);
				objData.AdTags.TargetedSeries = objData.Series.SeriesName;
			}
			else if (ModelId > 0)
			{
				List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

				if (objVersionsList != null && objVersionsList.Count > 0)
					bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;

				if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
				{
					objData.PageMetaTags.Title = string.Format("Latest News about {0} {1} | {0} {1} News - BikeWale", objMake.MakeName, objModel.ModelName);
					objData.PageMetaTags.Description = String.Format("Read the latest news about {0} {1} scooters exclusively on BikeWale. Know more about {1}.", objMake.MakeName, objModel.ModelName);
					objData.PageH1 = string.Format("{0} {1} Scooters News", objMake.MakeName, objModel.ModelName);
					objData.PageH2 = string.Format("Latest {0} {1} Scooters News and Views", objMake.MakeName, objModel.ModelName);
					objData.AdTags.TargetedMakes = objMake.MakeName;
					objData.AdTags.TargetedModel = objModel.ModelName;
				}
				else
				{
					objData.PageMetaTags.Title = string.Format("Latest News about {0} {1} | {0} {1} News - BikeWale", objMake.MakeName, objModel.ModelName);
					objData.PageMetaTags.Description = String.Format("Read the latest news about {0} {1} bikes exclusively on BikeWale. Know more about {1}.", objMake.MakeName, objModel.ModelName);
					objData.PageH1 = string.Format("{0} {1} Bikes News", objMake.MakeName, objModel.ModelName);
					objData.PageH2 = string.Format("Latest {0} {1} Bikes News and Views", objMake.MakeName, objModel.ModelName);
					objData.AdTags.TargetedMakes = objMake.MakeName;
					objData.AdTags.TargetedModel = objModel.ModelName;
				}


			}
			else if (MakeId > 0 && objMake != null)
			{
				objData.PageMetaTags.Title = string.Format("Latest News about {0} Bikes | {0} Bikes News - BikeWale", objMake.MakeName);
				objData.PageMetaTags.Description = String.Format("Read the latest news about popular and upcoming {0} bikes exclusively on BikeWale. Know more about {0} bikes.", objMake.MakeName);
				objData.PageH1 = string.Format("{0} Bikes News", objMake.MakeName);
				objData.PageH2 = string.Format("Latest {0} Bikes News and Views", objMake.MakeName);
				objData.AdTags.TargetedMakes = objMake.MakeName;
			}
			else
			{
				objData.PageMetaTags.Title = "Bike News - Latest Indian Bike News & Views | BikeWale";
				objData.PageMetaTags.Description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
				objData.PageMetaTags.Keywords = "news, bike news, auto news, latest bike news, indian bike news, bike news of india";
				objData.PageH1 = "Bike News";
				objData.PageH2 = "Latest Indian Bikes News and Views";
			}

            if (curPageNo > 1)
            {
                objData.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Description);
                objData.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Title);
            }
            SetBreadcrumList(objData, bodyStyle);
		}

        /// <summary>
        /// Created by : Aditi Srivastava on 28 Mar 2017
        /// Summary    : Get view model for page widgets
        /// </summary>
        private void GetWidgetData(NewsIndexPageVM objData, int topCount)
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;

                if (ModelId > 0)
                {
                    List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

                    if (objVersionsList != null && objVersionsList.Count > 0)
                        bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;

                    if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                    {
                        PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_objMakeCache);
                        objPopularScooterBrands.TopCount = 4;
                        objData.PopularScooterMakesWidget = objPopularScooterBrands.GetData();
                        bikeType = EnumBikeType.Scooters;
                    }
                    else
                    {
                        PopularBikesByBodyStyle objPopularStyle = new PopularBikesByBodyStyle(_models);
                        objPopularStyle.ModelId = ModelId;
                        objPopularStyle.CityId = CityId;
                        objPopularStyle.TopCount = topCount;
                        objData.PopularBodyStyle = objPopularStyle.GetData();

                        if (objData.PopularBodyStyle != null)
                        {
                            objData.PopularBodyStyle.WidgetHeading = string.Format("Popular {0}", objData.PopularBodyStyle.BodyStyleText);
                            objData.PopularBodyStyle.WidgetLinkTitle = string.Format("Best {0} in India", objData.PopularBodyStyle.BodyStyleLinkTitle);
                            objData.PopularBodyStyle.WidgetHref = UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle);
                        }
                    }


                }
                else
                {
                    UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                    objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                    objUpcomingBikes.Filters.PageNo = 1;
                    objUpcomingBikes.Filters.PageSize = topCount;
                    if (MakeId > 0)
                    {
                        objUpcomingBikes.Filters.MakeId = (int)MakeId;
                    }
                    objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;
                    objData.UpcomingBikes = objUpcomingBikes.GetData();

                    if (objMake != null)
                    {
                        objData.UpcomingBikes.WidgetHeading = string.Format("Upcoming {0} bikes", objMake.MakeName);
                        objData.UpcomingBikes.WidgetHref = string.Format("/{0}-bikes/upcoming/", objMake.MaskingName);
                    }
                    else
                    {
                        objData.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                        objData.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                    }
                    objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
                }

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();

                if (bikeType.Equals(EnumBikeType.Scooters))
                {
                    objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} scooters", objMake.MakeName);
                    objData.MostPopularBikes.WidgetHref = string.Format("/{0}-scooters/", objMake.MaskingName);
                    objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Scooters", objMake.MakeName);
                }
                else if (MakeId > 0 && objMake != null)
                {
                    objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} bikes", objMake.MakeName);
                    objData.MostPopularBikes.WidgetHref = string.Format("/{0}-bikes/", objMake.MaskingName);
                    objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Bikes", objMake.MakeName);
                }
                else
                {
                    objData.MostPopularBikes.WidgetHeading = "Popular bikes";
                    objData.MostPopularBikes.WidgetHref = "/best-bikes-in-india/";
                    objData.MostPopularBikes.WidgetLinkTitle = "Best Bikes in India";
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Bikewale.Models.News.NewsIndexPage.GetWidgetData");
            }
        }

		/// <summary>
		/// Created By : Aditi Srivastava on 27 Mar 2017
		/// Summary    : Bind link pager
		/// Modifies by : Ashutosh Sharma on 27 Nov 2017
		/// Description : Added logic for series news page.
		/// </summary>
		private void BindLinkPager(NewsIndexPageVM objData, int recordCount)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
				if (objData.Series != null && objData.Series.IsSeriesPageUrl)
				{
					objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatNewsUrl(make, objData.Series.MaskingName));
				}
				else
				{
					objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatNewsUrl(make, model));
				}
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = recordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Bikewale.Models.News.NewsIndexPage.BindLinkPager");
            }
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Create previous and next page urls
        /// </summary>
        /// <param name="objData"></param>
        private void CreatePrevNextUrl(NewsIndexPageVM objData, int recordCount)
        {
            string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
            int totalPages = _pager.GetTotalPages(recordCount, pageSize);
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
        /// Created By : Snehal Dange on 12th Oct 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(NewsIndexPageVM objData, EnumBikeBodyStyles bodyStyle)
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


                if (objData.Make != null)
                {
                    bikeUrl = string.Format("{0}{1}-bikes/", bikeUrl, objData.Make.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} Bikes", objData.Make.MakeName)));
                }

                if (objData.Make != null && bodyStyle.Equals(EnumBikeBodyStyles.Scooter) && !objData.Make.IsScooterOnly)
                {
                    if (IsMobile)
                    {
                        scooterUrl += "m/";
                    }
                    scooterUrl = string.Format("{0}{1}-scooters/", scooterUrl, objData.Make.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, scooterUrl, string.Format("{0} Scooters", objData.Make.MakeName)));
                }

                if (objData.Model != null)
                {
                    bikeUrl = string.Format("{0}{1}/", bikeUrl, objData.Model.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} {1}", objData.Make.MakeName, objData.Model.ModelName)));
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "News"));

                objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

            }
            catch (Exception ex)
            {
                Notifications.ErrorClass objErr = new Notifications.ErrorClass(ex, "Exception : Bikewale.Models.News.NewsIndexPage.SetBreadcrumList");
            }

        }
        #endregion

    }
}