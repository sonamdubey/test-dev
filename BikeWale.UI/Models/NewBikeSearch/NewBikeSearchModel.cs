using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.DTO.NewBikeSearch;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.NewBikeSearch;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;

namespace Bikewale.Models.NewBikeSearch
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 08-Nov-2017
    /// Summary: Model for New BIke Search
    /// </summary>
    public class NewBikeSearchModel
    {
        public bool IsMobile { get; set; }
        public string BaseUrl { get; set; }
        private string _baseUrl = string.Empty;
        public uint EditorialTopCount { get; set; }
        public int PageSize { get; set; }
        private int _totalBikeCount;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IBikeMakesCacheRepository _makes;
        private readonly IBikeSearchResult _searchResult = null;
        private readonly IProcessFilter _processFilter = null;
        private readonly string _queryString;
        private readonly HttpRequestBase _request;
        private readonly PQSourceEnum _pqSource;
        NewSearchPage currentPage;
        NewBikeSearchVM viewModel;
        private readonly IApiGatewayCaller _apiGatewayCaller;


        public NewBikeSearchModel(HttpRequestBase Request, ICMSCacheContent objArticles, IVideos objVideos, IBikeMakesCacheRepository makes, IBikeSearchResult searchResult, IProcessFilter processFilter, PQSourceEnum pqSource, IApiGatewayCaller apiGatewayCaller)
        {
            _request = Request;
            _makes = makes;
            _queryString = Request.Url.Query;
            _articles = objArticles;
            _videos = objVideos;
            _searchResult = searchResult;
            _processFilter = processFilter;
            _pqSource = pqSource;
            _apiGatewayCaller = apiGatewayCaller;
        }

        /// <summary>
        /// Created by : Sangram Nandakhile on 08 Nov 2017
        /// Summary : Get model for bike search
        /// </summary>
        /// <returns></returns>
        public NewBikeSearchVM GetData()
        {
            viewModel = new NewBikeSearchVM();
            viewModel.BikeSearch = BindBikes(this.PageSize.ToString());
            SetPageType();
            if (viewModel.BikeSearch != null)
            {
                viewModel.BikeSearch.PqSource = Convert.ToInt32(_pqSource);
                viewModel.Page = Entities.Pages.GAPages.Search_Page;
                BindEditorialWidget(viewModel);
                BindPageMetas(viewModel.PageMetaTags);
                CreatePager(viewModel);
                BindBrands(viewModel);
                BindDropDowns(viewModel);
            }
            return viewModel;
        }

        /// <summary>
        /// Created by: Sangram Nandakhile on 08 Nov 2017
        /// Summary : Bind list of bikes
        /// </summary>
        /// <returns></returns>
        private SearchOutput BindBikes(string pageSize)
        {
            SearchOutput objResult = null;
            InputBaseEntity input = MapQueryString(_queryString);
            if (null != input)
            {
                input.PageSize = pageSize;
                FilterInput filterInputs = _processFilter.ProcessFilters(input);
                objResult = _searchResult.GetSearchResult(filterInputs, input);
                if (objResult != null)
                {
                    _totalBikeCount = objResult.TotalCount;
                }
            }
            return objResult;
        }

        /// <summary>
        /// Maps the query string into an object
        /// </summary>
        /// <param name="qs">The qs.</param>
        /// <returns></returns>
        private InputBaseEntity MapQueryString(string qs)
        {
            InputBaseEntity input = null;
            try
            {
                var dict = HttpUtility.ParseQueryString(qs);
                string jsonStr = JsonConvert.SerializeObject(
                    dict.AllKeys.ToDictionary(k => k, k => dict[k])
                );
                input = JsonConvert.DeserializeObject<InputBaseEntity>(jsonStr);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "NewBikeSearchModel.MapQueryString()");
            }
            return input;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 09-Nov-2017 
        /// Binds the page metas.
        /// </summary>
        /// <param name="objPage">The object page.</param>
        private void BindPageMetas(PageMetaTags objPage)
        {
            try
            {
                // construct trailing page query
                string pageQuery = currentPage.IsPageNoInUrl ? string.Format("page-{0}/", currentPage.PageNo) : string.Empty;
                switch (currentPage.PageType)
                {
                    case SearchPageType.Default:
                        objPage.Title = "Search New Bikes by Brand, Budget, Mileage and Ride Style - BikeWale";
                        objPage.Keywords = "search new bikes, search bikes by brand, search bikes by budget, search bikes by price, search bikes by style, street bikes, scooters, commuter bikes, cruiser bikes";
                        objPage.Description = "Search through all the new bike models by various criteria. Get instant on-road price for the bike of your choice";
                        _baseUrl = "/new/bike-search/";
                        objPage.CanonicalUrl = string.Format("{0}/new/bike-search/", BWConfiguration.Instance.BwHostUrlForJs);
                        objPage.AlternateUrl = string.Format("{0}/m/new/bike-search/", BWConfiguration.Instance.BwHostUrlForJs);
                        break;
                    case SearchPageType.Under:
                        objPage.Title = string.Format("Bikes Under Rs. {0} - Explore Latest Bikes Below Rs. {0} on BikeWale", currentPage.MaxPriceStr);
                        objPage.Keywords = string.Format("best bikes under Rs. {0}, latest bikes under Rs. {0}", currentPage.MaxPriceStr);
                        objPage.Description = string.Format("Explore best bikes under Rs. {0}. Choose from bike models like {1} and more. Check specifications, prices and reviews to buy the best bike.", currentPage.MaxPriceStr, currentPage.ModelNameList);
                        _baseUrl = string.Format("{0}bikes-under-{1}/", BaseUrl, currentPage.MaxPrice);
                        objPage.CanonicalUrl = string.Format("{0}/new/bike-search/bikes-under-{1}/{2}", BWConfiguration.Instance.BwHostUrlForJs, currentPage.MaxPrice, pageQuery);
                        objPage.AlternateUrl = string.Format("{0}/m/new/bike-search/bikes-under-{1}/{2}", BWConfiguration.Instance.BwHostUrlForJs, currentPage.MaxPrice, pageQuery);
                        viewModel.MinMaxBudget = string.Format("0-{0}", currentPage.MaxPrice);
                        break;
                    case SearchPageType.Above:
                        objPage.Title = string.Format("Bikes Above Rs. {0} - Explore Latest Bikes Above Rs. {0} on BikeWale", currentPage.MinPriceStr);
                        objPage.Keywords = string.Format("best bikes above Rs. {0}, latest bikes above Rs. {0}", currentPage.MinPriceStr);
                        objPage.Description = string.Format("Explore best bikes above Rs. {0}. Choose from bike models like {1} and more. Check specifications, prices and reviews to buy the best bike.", currentPage.MinPriceStr, currentPage.ModelNameList);
                        _baseUrl = string.Format("{0}bikes-above-{1}/", BaseUrl, currentPage.MinPrice);
                        objPage.CanonicalUrl = string.Format("{0}/new/bike-search/bikes-above-{1}/{2}", BWConfiguration.Instance.BwHostUrlForJs, currentPage.MinPrice, pageQuery);
                        objPage.AlternateUrl = string.Format("{0}/m/new/bike-search/bikes-above-{1}/{2}", BWConfiguration.Instance.BwHostUrlForJs, currentPage.MinPrice, pageQuery);
                        viewModel.MinMaxBudget = string.Format("{0}-", currentPage.MinPrice);
                        break;
                    case SearchPageType.Between:
                        objPage.Title = string.Format("Bikes Between Rs. {0}  and Rs. {1} - Explore Latest Bikes between Rs. {0} and {1} on BikeWale", currentPage.MinPriceStr, currentPage.MaxPriceStr);
                        objPage.Keywords = string.Format("best bikes between Rs. {0} and Rs. {1}, latest bikes between Rs. {0} and Rs. {1}", currentPage.MinPriceStr, currentPage.MaxPriceStr);
                        objPage.Description = string.Format("Explore best bikes between Rs. {0} and Rs. {1}. Choose from bike models like {2} and more. Check specifications, prices and reviews to buy the best bike.", currentPage.MinPriceStr, currentPage.MaxPriceStr, currentPage.ModelNameList);
                        _baseUrl = string.Format("{0}bikes-between-{1}-and-{2}/", BaseUrl, currentPage.MinPrice, currentPage.MaxPrice);
                        objPage.CanonicalUrl = string.Format("{0}/new/bike-search/bikes-between-{1}-and-{2}/{3}", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, currentPage.MinPrice, currentPage.MaxPrice, pageQuery);
                        objPage.AlternateUrl = string.Format("{0}/m/new/bike-search/bikes-between-{1}-and-{2}/{3}", Bikewale.Utility.BWConfiguration.Instance.BwHostUrlForJs, currentPage.MinPrice, currentPage.MaxPrice, pageQuery);
                        viewModel.MinMaxBudget = string.Format("{0}-{1}", currentPage.MinPrice, currentPage.MaxPrice);
                        break;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "NewBikeSearchModel.BindMetas()");
            }
        }

        private void SetPageType()
        {
            try
            {
                currentPage = new NewSearchPage();
                string minMaxStr = _request.QueryString["budget"];
                currentPage.IsPageNoInUrl = !string.IsNullOrEmpty(_request.QueryString["pageno"]);
                currentPage.PageNo = !currentPage.IsPageNoInUrl ? 1 : Convert.ToInt16(_request.QueryString["pageno"]);
                if (!string.IsNullOrEmpty(minMaxStr))
                {
                    string[] budgetArray = minMaxStr.Split('-');
                    if (budgetArray != null && budgetArray.Length > 1)
                    {
                        currentPage.MinPrice = Convert.ToInt32(budgetArray[0]);
                        currentPage.MinPriceStr = Utility.Format.FormatPrice(currentPage.MinPrice.ToString());
                        currentPage.MaxPrice = budgetArray[1].Length > 0 ? Convert.ToInt32(budgetArray[1]) : 0;
                        currentPage.MaxPriceStr = Utility.Format.FormatPrice(currentPage.MaxPrice.ToString());

                        if (currentPage.MinPrice == 0 && currentPage.MaxPrice > 0)
                        {
                            currentPage.PageType = SearchPageType.Under;
                        }
                        else if (currentPage.MinPrice > 0 && currentPage.MaxPrice == 0)
                        {
                            currentPage.PageType = SearchPageType.Above;
                        }
                        else if (currentPage.MinPrice > 0 && currentPage.MaxPrice > 0)
                        {
                            currentPage.PageType = SearchPageType.Between;
                        }
                    }
                }
                if (viewModel.BikeSearch != null && viewModel.BikeSearch.SearchResult != null)
                {
                    currentPage.ModelNameList = string.Join(",", viewModel.BikeSearch.SearchResult.Take(5).Select(x => x.BikeModel.ModelName).ToList());
                    currentPage.ModelIdList = string.Join(",", viewModel.BikeSearch.SearchResult.Take(10).Select(x => x.BikeModel.ModelId).ToList());
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "NewBikeSearchModel.SetPageType()");
            }
        }

        /// <summary>
        /// Binds the editorial widget.
        /// </summary>
        /// <param name="objVM">The object vm.</param>
        private void BindEditorialWidget(NewBikeSearchVM objVM)
        {
            try
            {
                RecentNews objNews = new RecentNews((ushort)EditorialTopCount, 0, currentPage.ModelIdList, _articles);
                objVM.News = objNews.GetData();

                RecentExpertReviews objReviews = new RecentExpertReviews((ushort)EditorialTopCount, currentPage.ModelIdList, _articles);
                objVM.ExpertReviews = objReviews.GetData();

                RecentVideos objVideos = new RecentVideos(1, (ushort)EditorialTopCount, currentPage.ModelIdList, _videos);
                objVM.Videos = objVideos.GetData();

                objVM.TabCount = 0;
                objVM.IsNewsActive = false;
                objVM.IsExpertReviewActive = false;
                objVM.IsVideoActive = false;

                if (objVM.News != null && objVM.News.FetchedCount > 0)
                {
                    objVM.TabCount++;
                    objVM.IsNewsActive = true;
                }
                if (objVM.ExpertReviews.FetchedCount > 0)
                {
                    objVM.TabCount++;
                    if (!objVM.IsNewsActive)
                    {
                        objVM.IsExpertReviewActive = true;
                    }
                }
                if (objVM.Videos.FetchedCount > 0)
                {
                    objVM.TabCount++;
                    if (!objVM.IsExpertReviewActive && !objVM.IsNewsActive)
                    {
                        objVM.IsVideoActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "NewBikeSearchModel.BindEditorialWidget()");
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 9th Oct 2017
        /// Summary : Fetch list of other makes to bind filters
        /// </summary>
        /// <param name="objVM"></param>
        private void BindBrands(NewBikeSearchVM objVM)
        {
            try
            {
                var makes = _makes.GetMakesByType(EnumBikeType.New);
                if (makes != null && makes.Any())
                {
                    objVM.PopularBrands = makes.Take(9);
                    objVM.OtherBrands = makes.Skip(9).OrderBy(m => m.MakeName);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "NewBikeSearchModel.BindBrands");
            }
        }

        /// <summary>
        /// Creates the pager.
        /// </summary>
        /// <param name="newBikeSearchVM">The new bike search vm.</param>
        /// <param name="objMeta">The object meta.</param>
        private void CreatePager(NewBikeSearchVM newBikeSearchVM)
        {
            int _totalPagesCount = (int)(_totalBikeCount / PageSize);

            if ((_totalPagesCount % PageSize) > 0)
                _totalPagesCount += 1;
            try
            {
                newBikeSearchVM.Pager = new Entities.Pager.PagerEntity()
                {
                    PageNo = currentPage.PageNo,
                    PageSize = PageSize,
                    PagerSlotSize = 5,
                    BaseUrl = _baseUrl,
                    PageUrlType = "page-",
                    TotalResults = _totalBikeCount
                };
                string prevUrl = string.Empty, nextUrl = string.Empty;
                Paging.CreatePrevNextUrl(_totalPagesCount, _baseUrl, (int)newBikeSearchVM.Pager.PageNo, newBikeSearchVM.Pager.PageUrlType, ref nextUrl, ref prevUrl);
                newBikeSearchVM.PageMetaTags.NextPageUrl = nextUrl;
                newBikeSearchVM.PageMetaTags.PreviousPageUrl = prevUrl;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "NewBikeSearchModel.CreatePager()");
            }
        }

        /// <summary>
        /// Created By : Deepak Israni on 12 April 2018
        /// Description: Bind data for brake type, start type and wheel type dropdown menus.
        /// </summary>
        /// <param name="objData"></param>
        private void BindDropDowns(NewBikeSearchVM objData)
        {
            //Brake Type -- Change the enum to whatever is decided for brake types
            GetCustomDataTypesByItemIdAdapter adapt1 = new GetCustomDataTypesByItemIdAdapter();
            adapt1.AddApiGatewayCall(_apiGatewayCaller, new GetCustomDataType_Input() { InputType = Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.FuelType });

            //Start Type -- Change the enum to whatever is decided for start types
            GetCustomDataTypesByItemIdAdapter adapt2 = new GetCustomDataTypesByItemIdAdapter();
            adapt2.AddApiGatewayCall(_apiGatewayCaller, new GetCustomDataType_Input() { InputType = Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.FuelType });

            //Wheel Type -- Change the enum to whatever is decided for wheel types
            GetCustomDataTypesByItemIdAdapter adapt3 = new GetCustomDataTypesByItemIdAdapter();
            adapt3.AddApiGatewayCall(_apiGatewayCaller, new GetCustomDataType_Input() { InputType = Bikewale.BAL.ApiGateway.Entities.BikeData.EnumSpecsFeaturesItems.FuelType });

            _apiGatewayCaller.Call();

            objData.BrakeTypes = adapt1.Output;
            objData.StartTypes = adapt2.Output;
            objData.WheelTypes = adapt3.Output;
        }

    }
    /// <summary>
    /// Enum for type of Search Page
    /// </summary>
    public enum SearchPageType
    {
        Default = 0,
        Under = 1,
        Above = 2,
        Between = 3
    }

    /// <summary>
    /// Class to hold properties of New Search page
    /// </summary>
    public class NewSearchPage
    {
        public SearchPageType PageType { get; set; }
        public int MinPrice { get; set; }
        public string MinPriceStr { get; set; }
        public int MaxPrice { get; set; }
        public string MaxPriceStr { get; set; }
        public string ModelIdList { get; set; }
        public string ModelNameList { get; set; }
        public int PageNo { get; set; }
        public bool IsPageNoInUrl { get; set; }
    }
}