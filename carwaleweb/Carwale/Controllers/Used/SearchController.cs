using AutoMapper;
using Carwale.BL.Stock;
using Carwale.DTOs.Classified.Stock;
using Carwale.DTOs.Stock.SimiliarCars;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Search;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock;
using Carwale.Entity.Stock.Search;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.Search;
using Carwale.Interfaces.Dealers.Used;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Stock;
using Carwale.Service.Filters;
using Carwale.UI.Common.CustomModelBinder;
using Carwale.UI.ViewModels.Used.Search;
using Carwale.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Carwale.UI.Controllers.Used
{
    public class SearchController : Controller
    {
        private readonly ISearchBL _searchBL;
        private readonly ISearchUtility _searchUtility;
        private readonly IStockRecommendationsBL _stockRecommendationsBL;
        private readonly IUsedDealerStocksBL _usedDealerStocksBL;

        public SearchController(
            ISearchBL searchBL,
            ISearchUtility searchUtility,
            IStockRecommendationsBL stockRecommendationsBL,
            IUsedDealerStocksBL usedDealerStocksBL
        )
        {
            _searchBL = searchBL;
            _searchUtility = searchUtility;
            _stockRecommendationsBL = stockRecommendationsBL;
            _usedDealerStocksBL = usedDealerStocksBL;
        }

        public ActionResult GetSearchResults([ModelBinder(typeof(SearchParamsModelBinder))]SearchParams searchParams)
        {
            Platform source = Platform.CarwaleMobile;
            searchParams.Pn = GetPageNo(Request.QueryString["pn"]);
            string redirectUrl;
            var stocks = _searchBL.FetchData(searchParams, source, Request.IsAjaxRequest(), Request.QueryString.ToString(), out redirectUrl);
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = string.Format("/m{0}", redirectUrl);
                return RedirectPermanent(redirectUrl);
            }

            SearchViewModel searchVM = GetSearchViewModel(stocks, searchParams, source);

            if (searchParams.IsAmp)
            {
                return View("~/Views/m/Used/SearchAmp/SearchAmp.cshtml", searchVM);
            }
            else if (!Request.IsAjaxRequest())               //In case of PageLoad
            {
                return View("~/Views/m/Used/Search/Search_v1.cshtml", searchVM);
            }
            else if (searchParams.IsFilter)             //In case of Filter(Ajax request for stock applying some filter)
            {
                return PartialView("~/Views/m/Used/Search/SearchMainContent_v1.cshtml", searchVM);
            }
            else                                        //In case of sort/nextPage(Ajax)
            {
                if (!searchParams.IsSort)               //In case of scroll nextPage
                    searchVM.StockFetched = searchParams.StockFetched;
                return PartialView("~/Views/m/Used/Search/StockList_v1.cshtml", searchVM);
            }
        }

        private int GetPageNo(string page)
        {
            int pn = 1;
            if (!string.IsNullOrEmpty(page))
            {
                string[] pages = page.Split(',');
                if (!int.TryParse(pages[pages.Length - 1], out pn))
                    pn = 1;
            }
            return pn;
        }

        private SearchViewModel GetSearchViewModel(SearchResultMobile stocks, SearchParams searchParams, Platform source)
        {
            SearchViewModel searchViewModel = new SearchViewModel
            {
                StockList = Mapper.Map<IList<StockBaseData>>(stocks.ResultData),
                FilterCityName = stocks.PrimaryCity.CityName,
                FilterCityId = stocks.PrimaryCity.CityId,
                NextPageUrl = stocks.NextPageUrl,
                TotalStockCount = stocks.TotalStockCount,
                RootsInfo = _searchBL.GetRootsName(searchParams.Car),
                MakesInfo = _searchBL.GetMakesName(searchParams.Car)
            };
            searchViewModel.MakesInfoInJson = searchViewModel.MakesInfo == null ? "[]" : JsonConvert.SerializeObject(searchViewModel.MakesInfo);
            searchViewModel.RootsInfoInJson = searchViewModel.RootsInfo == null ? "[]" : JsonConvert.SerializeObject(searchViewModel.RootsInfo);
            searchViewModel.AdUnit = _searchBL.GetAdUnit(searchParams.Budget, searchParams.Car, searchViewModel.MakesInfo, searchViewModel.RootsInfo);
            searchViewModel.IsSimilarCarAvailable = _searchBL.CheckSimilarCarsAvailability(searchViewModel.TotalStockCount, searchParams.City);
            searchParams.CityName = stocks.PrimaryCity.CityName;
            searchViewModel.MetaKeywords = _searchBL.GetMetaKeyWords(searchParams, searchViewModel.TotalStockCount, source);
            searchViewModel.IsSoldOut = searchParams.IsSold;
            searchViewModel.SearchParams = searchParams;
            searchViewModel.LastNearbyCarsBucket = searchParams.LastNearbyCarsBucket;
            searchViewModel.IsCustomerAreaAvailable = !string.IsNullOrWhiteSpace(searchParams.CustAreaName);
            searchViewModel.CustAreaName = searchParams.CustAreaName;
            searchViewModel.CarsNearMeLabel = "Cars Near " + (searchParams.IsLocationDetected ? "You" : searchParams.CustAreaName);
            searchViewModel.ShouldFetchNearbyCars = searchParams.ShouldFetchNearbyCars;

            if (!Request.IsAjaxRequest())   //In case of PageLoad
            {
                searchViewModel.CurrQS = _searchBL.GetCurrentPageQS(Request.QueryString.ToString(), searchParams);
                if (searchParams.IsAmp)
                {
                    searchViewModel.SortBaseUrlAmp = "/m/used/cars-for-sale/?" + _searchUtility.RemoveParamsFromQs(searchViewModel.CurrQS, new string[] { "so", "sc" }).ToString();
                }
            }
            if (searchParams.StockFetched >= searchParams.Ps * 9)
            {
                searchViewModel.NextPageUrl = null;
            }
            else if (!string.IsNullOrEmpty(searchViewModel.NextPageUrl))
            {
                searchViewModel.NextPageUrl = searchViewModel.NextPageUrl.Substring(searchViewModel.NextPageUrl.IndexOf('?'));
            }

            return searchViewModel;
        }

        [OutputCache(Duration = 1800)]
        public ActionResult GetFilterData()
        {
            return PartialView("~/Views/m/Used/Search/FilterPopup.cshtml", _searchBL.GetCarMakes());
        }

        [OutputCache(Duration = 1800)]
        public ActionResult GetAllCities()
        {
            var cities = _searchBL.GetCities();
            return PartialView("~/Views/m/Used/Search/CityPopup.cshtml", cities);
        }

        [ValidateModel("recoParams")]
        public ActionResult GetSimilarCars(StockRecoParams recoParams)
        {
                List<StockBaseEntity> recommendations = _stockRecommendationsBL.GetRecommendations(recoParams, (int)Platform.CarwaleMobile);

                if (recommendations != null && recommendations.Count != 0)
                {
                   var result = recommendations.Select((stock, i) =>
                   {
                        var dto = Mapper.Map<StockSummaryDTO>(stock);
                        dto.Url = StockBL.AddRankInUrl(stock.Url, (i + 1));
                        dto.Rank = i;
                        return dto;
                   }).ToList();
                    return PartialView("~/Views/m/Used/Search/SimilarCarsPopup.cshtml", result);
                }
            return null;
        }

        public ActionResult GetDealerStocks(int dealerId, int from = 0, int size = 0)
        {
            if (_usedDealerStocksBL.ValidateDealerStocksApiInputs(dealerId, from, size).Count == 0)
            {
                var usedDealerStocks = _usedDealerStocksBL.GetDealerStocksEntity(dealerId, from, size);
                if(usedDealerStocks?.Stocks != null && usedDealerStocks.Stocks.Any())
                {
                    var result = usedDealerStocks.Stocks.Select((stock, i) =>
                    {
                        var dto = Mapper.Map<StockSummaryDTO>(stock);
                        dto.Url = StockBL.AddRankInUrl(stock.Url, (i + 1));
                        dto.Rank = i;
                        return dto;
                    }).ToList();
                    return PartialView("~/Views/m/Used/Search/SimilarCarsPopup.cshtml", result);
                }
            }
            return null;
        }
    }
}