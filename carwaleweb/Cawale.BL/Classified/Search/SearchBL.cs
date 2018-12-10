using AutoMapper;
using Carwale.BL.Classified.CarValuation;
using Carwale.BL.Interface.Stock.Search;
using Carwale.BL.Stock;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.CarValuation;
using Carwale.Entity.Classified.Search;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Stock.Search;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classified.Search;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Carwale.BL.Classified.Search
{
    public class SearchBL : ISearchBL
    {
        private readonly IElasticSearchManager _searchManager;
        private readonly IGeoCitiesCacheRepository _geoCityCacheRepo;
        private readonly ICarMakesCacheRepository _carMakesCacheRepo;
        private static readonly string _elasticIndexName = ConfigurationManager.AppSettings["ElasticIndexName"];
        private readonly ICarModelRootsCacheRepository _modelRootsCacheRepo;
        private readonly IMetaKeywordsSearch _metaKeywords;
        private readonly ISearchParamsProcesser _searchParamsProc;
        private readonly ISearchUtility _searchUtility;
        private readonly IEnumerable<int> _ampCities = ConfigurationManager.AppSettings["UsedAmpCities"].Split(',').Select(int.Parse);
        private readonly IStockSearchLogic<SearchResultMobile> _stockSearch;
        private static readonly Dictionary<int, int> _popularMakesIdAndRank = new Dictionary<int, int>  //Key=MakeId, Value=RankOfPopularity 
        {
            { 10, 1 },         //Maruti Suzuki
            { 8, 2 },          //Hyundai
            { 7, 3 },          //Honda
            { 9, 4 },          //Mahindra
            { 16, 5 },         //Tata
            { 17, 6 },         //Toyota
            { 20, 7 },          //Volkswagen
            { 5, 8 },            //Ford
            { 21, 9 },          //Nissan
            { 45, 10 },          //Renault
            { 56, 11 },          //Datsun
            { 15, 12 },          //Skoda
            { 18, 13 },          //Audi
            { 1, 14 },           //BMW
            { 11, 15 }           //Mercedes Benz
        };
        private const int _rankValueForNonPopularMakes = 1000;

        public SearchBL(ICarModelRootsCacheRepository modelRootsCacheRepo, IElasticSearchManager searchManager, ICarMakesCacheRepository carMakesCacheRepo, IGeoCitiesCacheRepository geoCityCacheRepo,
            IMetaKeywordsSearch metaKeywords, ISearchParamsProcesser searchParamsProc, ISearchUtility searchUtility
            , IStockSearchLogic<SearchResultMobile> stockSearch)
        {
            _searchManager = searchManager;
            _modelRootsCacheRepo = modelRootsCacheRepo;
            _carMakesCacheRepo = carMakesCacheRepo;
            _geoCityCacheRepo = geoCityCacheRepo;
            _metaKeywords = metaKeywords;
            _searchParamsProc = searchParamsProc;
            _searchUtility = searchUtility;
            _stockSearch = stockSearch;
        }

        public SearchResultMobile FetchData(SearchParams searchParams, Platform source, bool isAjaxRequest, string queryString, out string redirectUrl)
        {
            SearchResultMobile stocks = null;
            _searchParamsProc.ProcessSearchParams(searchParams, source, isAjaxRequest, out redirectUrl);
            try
            {
                if (string.IsNullOrEmpty(redirectUrl))
                {
                    FilterInputs filterInputs = Mapper.Map<FilterInputs>(searchParams);
                    stocks = _stockSearch.Get(filterInputs);

                    if (stocks != null)
                    {
                        //For page number greater available pages redirect to last page.
                        if (searchParams.NearbyCityId <= 0 && stocks.TotalStockCount > 0 && stocks.TotalStockCount < filterInputs.lcr)
                        {
                            int totalPages = (int)Math.Ceiling((double)stocks.TotalStockCount / searchParams.Ps);
                            redirectUrl = _searchUtility.GetURL(searchParams.MakeName, searchParams.Root, searchParams.CityName, totalPages, queryString);
                            redirectUrl = Regex.Replace(redirectUrl, @"pn=[\d]*", m => $"pn={ totalPages }");
                        }
                        else if (searchParams.IsAmp)
                        {
                            foreach (var stock in stocks.ResultData)
	                        {
		                        stock.ValuationUrl = ValuationBL.GetValuationUrl(new ValuationUrlParameters()
                                {
                                    VersionId = Convert.ToInt32(stock.VersionId),
                                    Year = Convert.ToInt16(stock.MakeYear),
                                    Owners = Convert.ToInt32(stock.OwnerTypeId),
                                    AskingPrice = Convert.ToInt32(stock.PriceNumeric),
                                    CityId = Convert.ToInt32(stock.CityId),
                                    Kilometers = Convert.ToInt32(stock.KmNumeric),
                                    ProfileId = stock.ProfileId
                                });
                                stock.StockRecommendationsUrl = StockRecommendationsBL.GetStockRecommendationsUrl(
                                    stock.ProfileId,
                                    Convert.ToInt32(stock.RootId),
                                    Convert.ToInt32(stock.CityId),
                                    stock.DeliveryCity,
                                    Convert.ToInt32(stock.PriceNumeric),
                                    Convert.ToInt32(stock.VersionSubSegmentID));
                            }
                        }
                    }
                }
                else
                {
                    string currQS = GetCurrentPageQS(queryString, searchParams);
                    if (!string.IsNullOrEmpty(currQS))
                    {
                        currQS = $"?{ currQS }";
                    }
                    redirectUrl = $"{ redirectUrl }{ currQS }";
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return stocks;
        }

        public IEnumerable<Cities> GetCities()
        {
            List<Cities> cities = new List<Cities>();
            var cityList = _geoCityCacheRepo.GetCities(Modules.UsedSearch);
            if (cityList != null)
            {
                cities.Add(new Cities() { CityId = 0, CityName = "Search for all Cities" });
                cities.Add(new Cities() { CityId = 3001, CityName = "Delhi NCR" });
                cities.AddRange(cityList);

                var mumbaiCity = cities.FirstOrDefault(city => city.CityId == 1);
                if (mumbaiCity != null)
                {
                    mumbaiCity.CityId = 3000;
                }
            }
            return cities;
        }

        public int GetStocksCountByField(SearchParams searchParams, string field, double fieldValue, bool greaterThanFieldValue)
        {
            FilterInputs filterInputs = Mapper.Map<FilterInputs>(searchParams);
            return _searchManager.GetStocksCountByField(_elasticIndexName, filterInputs, field, fieldValue, greaterThanFieldValue);
        }

        public bool CheckSimilarCarsAvailability(int totalStockCount, int cityId)
        {
            string[] similarCarExcludedCities = ConfigurationManager.AppSettings["Used_SimilarCarExcludedCities"].Split(',');
            return (totalStockCount > 17 && cityId > 0 && !similarCarExcludedCities.Contains(cityId.ToString()));
        }

        public AdParams GetAdUnit(string budget, string car, IEnumerable<MakeEntity> makesInfo, IEnumerable<RootBase> rootsInfo)
        {
            AdParams adParams = new AdParams()
            {
                AdBudgetRange = GetBudgetForAd(budget),
                AdMakeOrRootName = GetMakeOrRootName(car, makesInfo, rootsInfo)
            };
            return adParams;
        }

        private string GetBudgetForAd(string budget)
        {
            string budgetRange;
            try
            {
                budget = budget ?? string.Empty;
                string[] tempBudget = budget.Split('-');

                if (string.IsNullOrEmpty(tempBudget[0]))
                {
                    budgetRange = "any";
                }
                else
                {
                    string low, high;
                    int min;
                    int.TryParse(tempBudget[0], out min);
                    min = (min / 5) * 5;
                    low = min == 20 ? "15" : min.ToString();

                    if (tempBudget.Length == 1 || string.IsNullOrEmpty(tempBudget[1]))
                    {
                        high = "20,above20";
                    }
                    else
                    {
                        int max;
                        int.TryParse(tempBudget[1], out max);
                        high = ((max / 5) * 5).ToString();

                        if (high == low)
                        {
                            high = (min + 5).ToString();
                        }
                    }
                    budgetRange = $"{low}-{high}";
                }
            }
            catch (Exception ex)
            {
                budgetRange = string.Empty;
                Logger.LogException(ex);
            }
            return budgetRange;
        }

        private string GetMakeOrRootName(string car, IEnumerable<MakeEntity> makesInfo, IEnumerable<RootBase> rootsInfo)
        {
            string rootsOrMake = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(car))
                {
                    var firstCarId = car.Trim().Split(' ')[0];
                    if (firstCarId.Contains("."))
                    {
                        string rootId = firstCarId.Split('.')[1];
                        RootBase root = rootsInfo?.Single(x => x.RootId.ToString() == rootId);
                        rootsOrMake = (root != null) ? $"{ root.MakeName } { root.Name }" : string.Empty;
                    }
                    else
                    {
                        rootsOrMake = makesInfo?.Single(x => x.MakeId.ToString() == firstCarId).MakeName ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return rootsOrMake;
        }

        public MetaKeywords GetMetaKeyWords(SearchParams searchParams, int totalStockCount, Platform source)
        {
            MetaKeywords objKeywords;
            string cityName = searchParams.CityName ?? string.Empty;
            string makeName = searchParams.MakeName ?? string.Empty;
            string rootName = searchParams.Root ?? string.Empty;

            if (!string.IsNullOrEmpty(searchParams.Car) && searchParams.Car.Contains("+"))
            {
                makeName = string.Empty;
                rootName = string.Empty;
            }
            int totalPages = (int)Math.Ceiling((double)(totalStockCount) / searchParams.Ps);
            objKeywords = _metaKeywords.GetMetaKeywordsSearchPage(makeName, rootName, cityName, searchParams.Pn, totalPages, source == Platform.CarwaleMobile);
            if (!_ampCities.Contains(searchParams.City))
            {
                objKeywords.AmpUrl = null;
            }

            return objKeywords;
        }

        public string GetCurrentPageQS(string qs, SearchParams searchParams)
        {
            string[] parameterTobeRemoved = { "cityname", "make", "makename", "root", "city", "isamp", "model", null };
            NameValueCollection queryString = _searchUtility.RemoveParamsFromQs(qs, parameterTobeRemoved);

            if (!string.IsNullOrEmpty(searchParams.Car) && string.IsNullOrEmpty(queryString["car"]))
            {
                queryString.Add("car", searchParams.Car);
            }
            if (searchParams.City > 0)
            {
                queryString.Add("city", searchParams.City.ToString());
            }
            return queryString.ToString();
        }

        public IEnumerable<RootBase> GetRootsName(string car)
        {
            IEnumerable<RootBase> rootDetails = null;
            if (!string.IsNullOrEmpty(car))
            {
                var carIds = car.Split(' ');
                string rootIds = string.Empty;
                for (int i = 0; i < carIds.Length; i++)
                {
                    if (carIds[i].Contains("."))
                    {
                        rootIds = $"{ rootIds },{ carIds[i].Split('.')[1] }";
                    }
                }
                if (!string.IsNullOrEmpty(rootIds))
                {
                    rootDetails = _modelRootsCacheRepo.GetRoots(rootIds.Substring(1));
                } 
            }
            return rootDetails;
        }

        public IEnumerable<MakeEntity> GetMakesName(string car)
        {
            IEnumerable<MakeEntity> makeDetails = null;
            if (!string.IsNullOrEmpty(car))
            {
                var carIds = car.Split(' ');
                string makeIds = string.Empty;
                for (int i = 0; i < carIds.Length; i++)
                {
                    if (!string.IsNullOrEmpty(carIds[i]) && !carIds[i].Contains("."))
                    {
                        makeIds = $"{ makeIds },{ carIds[i] }";
                    }
                }
                if (!string.IsNullOrEmpty(makeIds))
                {
                    makeDetails = Mapper.Map<IEnumerable<MakeEntity>>(_carMakesCacheRepo.GetMakes(makeIds.Substring(1)));
                } 
            }
            return makeDetails;
        }

        //Remove unnecessary QS params and return only valid and necessary qs params
        public string GetRedirectQsByModelIds(NameValueCollection qs)
        {
            string queryString = string.Empty;
            try
            {
                string[] modelIds = qs["model"]?.Split(',');
                string car = (modelIds != null) ? GetCarFromModelIds(Array.ConvertAll(modelIds, model => CustomParser.parseIntObject(model)))
                                                : string.Empty;
                string budget = GetBudgetQs(CustomParser.parseDoubleObject<string>(qs["priceFrom"]),
                                                                    CustomParser.parseDoubleObject<string>(qs["priceTo"]));
                StringBuilder qsBuilder = new StringBuilder();

                qsBuilder.Append("?budget=");
                qsBuilder.Append(budget);

                if (!string.IsNullOrEmpty(car))
                {
                    qsBuilder.Append("&car=");
                    qsBuilder.Append(car);
                }
                if (!string.IsNullOrEmpty(qs["city"]))
                {
                    qsBuilder.Append("&city=");
                    qsBuilder.Append(qs["city"]);
                }
                queryString = qsBuilder.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return queryString;
        }

        //get budget string using priceFrom and priceTo
        private static string GetBudgetQs(double priceFrom, double priceTo)
        {
            double minBudget = Math.Round((priceFrom / 100000), 2);
            double maxBudget = Math.Round((priceTo / 100000), 2);
            return $"{ minBudget }-{ (maxBudget == 0.00 ? string.Empty : maxBudget.ToString()) }";
        }

        //from modelIds create root base string
        private string GetCarFromModelIds(int[] modelIds)
        {
            string car = string.Empty;
            RootBase root;
            try
            {
                foreach (int model in modelIds)
                {
                    if (model > 0)
                    {
                        root = _modelRootsCacheRepo.GetRootByModel(model);
                        car = $"{ car }+{ root.MakeId }.{ root.RootId }";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return (string.IsNullOrEmpty(car)) ? string.Empty : car.Substring(1);
        }

        public IEnumerable<CarMakeEntityBase> GetCarMakes()
        {
            var carMakes = _carMakesCacheRepo.GetMakes();
            return carMakes.OrderBy(x => _popularMakesIdAndRank.ContainsKey(x.MakeId) ? _popularMakesIdAndRank[x.MakeId] : _rankValueForNonPopularMakes);
        }
    }
}
