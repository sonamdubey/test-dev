using Carwale.BL.Interface.Stock.Search;
using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Entity.Stock.Finance;
using Carwale.Entity.Stock.Search;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Geolocation;
using Carwale.Interfaces.Stock;
using Carwale.Utility;
using Carwale.Utility.Classified;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Carwale.BL.Stock.Search
{
    public class MobileStockSearchLogic : StockSearchLogicBase<SearchResultMobile>
    {
        private readonly IProcessFilters _processFilters;
        private readonly IStockManager _stockManager;
        private readonly IESStockQueryRepository _esStockQueryRepository;
        private readonly INearbyCityLogic _nearbyCityLogic;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;
        public MobileStockSearchLogic(IProcessFilters processFilters, IStockManager stockManager, IESStockQueryRepository esStockQueryRepository, INearbyCityLogic nearbyCityLogic, IGeoCitiesCacheRepository geoCitiesCacheRepository)
        {
            _processFilters = processFilters;
            _stockManager = stockManager;
            _esStockQueryRepository = esStockQueryRepository;
            _nearbyCityLogic = nearbyCityLogic;
            _geoCitiesCacheRepository = geoCitiesCacheRepository;
        }
        public override SearchResultMobile Get(FilterInputs filterInputs)
        {
            if (filterInputs == null)
            {
                return null;
            }
            SearchResultMobile searchResultMobile = new SearchResultMobile();
            searchResultMobile.PrimaryCity = GetPrimaryCity(filterInputs.city);
            var elasticInputs = _processFilters.ProcessFilterParams(filterInputs);

            int totalPageSize = GetTotalPageSize(filterInputs.ps);

            elasticInputs.sessionId = SessionId;

            string[] citiesOriginal = elasticInputs.cities;

            bool isRequestFromNearbyCity = false;
            bool updateMainCityTotalStockCount = true;
            //If client already has nearbyCityId, fetch results from that city.
            if (elasticInputs.nearbyCityId != 0)
            {
                searchResultMobile.TotalStockCount = _esStockQueryRepository.GetTotalStockCount(elasticInputs);
                elasticInputs.cities = new[] { elasticInputs.nearbyCityId.ToString() };
                isRequestFromNearbyCity = true;
                updateMainCityTotalStockCount = false;
            }

            int stockCountFetched = 0;
            bool isNearbyCityMessageRequired = false;
            
            int currentNearbyCityStockCount = 0;
            searchResultMobile.NearbyCityId = elasticInputs.nearbyCityId;
            searchResultMobile.NearbyCityIds = elasticInputs.nearbyCityIds;
            searchResultMobile.NearbyCityIdsStockCount = elasticInputs.nearbyCityIdsStockCount;

            while (stockCountFetched < totalPageSize)
            {
                var results = _stockManager.GetStocks(elasticInputs); 
                
                var currentCityListings = results.ResultData;
                if (isRequestFromNearbyCity)
                {
                    currentCityListings = currentCityListings.Select(currentCityListing => 
                    { currentCityListing.IsNearbyCityListing = true;
                        return currentCityListing;
                    }).ToList();
                }
                if (isNearbyCityMessageRequired)
                {
                    currentCityListings[0].NearbyCityText = _nearbyCityLogic.GetNearbyCityText(elasticInputs.nearbyCityId, currentNearbyCityStockCount);
                    currentCityListings = currentCityListings.Select(currentCityListing =>
                    {
                        currentCityListing.IsNearbyCityListing = true;
                        return currentCityListing;
                    }).ToList();
                    updateMainCityTotalStockCount = false;
                }
                searchResultMobile.ResultData.AddRange(currentCityListings);
                if (updateMainCityTotalStockCount)
                {
                    searchResultMobile.TotalStockCount = results.TotalStockCount;
                }
                
                searchResultMobile.LastNonFeaturedSlotRank = results.LastNonFeaturedSlotRank;
                stockCountFetched += currentCityListings.Count;

                if (stockCountFetched < totalPageSize) //If with current city query, we were not able to fetch listings equal to page size, then try fetching from nearby cities.
                {
                    //Get nearby city id or next city id to fetch nearby city listings (NOT for mumbai-all, delhi ncr, all india case)
                    if (elasticInputs.cities != null && elasticInputs.cities.Length > 0 && elasticInputs.cities[0] != Constants.AllIndiaCityId && elasticInputs.multiCityId == 0)
                    {
                        ProcessNearByCityLogic(elasticInputs, searchResultMobile, stockCountFetched, totalPageSize, ref isNearbyCityMessageRequired, ref currentNearbyCityStockCount);
                    }               

                    if (elasticInputs.nearbyCityId == 0) //if no nextCityId, break.
                    {
                        searchResultMobile.IsAllStocksFetched = true;
                        break;
                    }
                }
            }

            elasticInputs.cities = citiesOriginal;


            if (searchResultMobile.ResultData.Count < Convert.ToInt32(elasticInputs.ps)) //If we are unable to fetch listings = page size, then we don't have more listings.
            {
                searchResultMobile.IsAllStocksFetched = true; //This would be used by client to not send further requests as listings have ended.
            }
            

            if (!searchResultMobile.IsAllStocksFetched)
            {
                var excludeStocks = GetExcludedStocksFromResultListings(filterInputs, searchResultMobile.ResultData);
                string nextPageQueryParam = SearchUtility.GetNextPageQueryParameter(filterInputs, excludeStocks, searchResultMobile);

                searchResultMobile.NextPageUrl = $"/m/used/cars-for-sale/?{nextPageQueryParam}";
            }
            if (searchResultMobile.ResultData != null)
            {
                for (int i = 0; i < searchResultMobile.ResultData.Count; i++)
                {
                    searchResultMobile.ResultData[i].PriceNumeric = searchResultMobile.ResultData[i].Price;
                    searchResultMobile.ResultData[i].Price = Format.FormatFullPrice(searchResultMobile.ResultData[i].Price);
                    searchResultMobile.ResultData[i].KmNumeric = searchResultMobile.ResultData[i].Km;
                    searchResultMobile.ResultData[i].Km = Format.Numeric(searchResultMobile.ResultData[i].Km);
                    searchResultMobile.ResultData[i].MakeMonth = searchResultMobile.ResultData[i].MfgDate.Month;
                    searchResultMobile.ResultData[i].ValuationUrl = GetValuationPath(searchResultMobile.ResultData[i]);
                    if (searchResultMobile.ResultData[i].IsEligibleForFinance && searchResultMobile.ResultData[i].CwBasePackageId != Entity.Enum.CwBasePackageId.Franchise)
                    {
                        var stockFinanceData = StockFinanceBL.GetFinanceData(new FinanceUrlParameter
                        {
                            HostUrl = ConfigurationManager.AppSettings["CTFinanceMsite"],
                            ProfileId = searchResultMobile.ResultData[i].ProfileId,
                            MakeId = Convert.ToInt32(searchResultMobile.ResultData[i].MakeId),
                            ModelId = Convert.ToInt32(searchResultMobile.ResultData[i].ModelId),
                            MakeYear = Convert.ToInt32(searchResultMobile.ResultData[i].MakeYear),
                            CityId = Convert.ToInt32(searchResultMobile.ResultData[i].CityId),
                            PriceNumeric = Convert.ToInt32(searchResultMobile.ResultData[i].PriceNumeric),
                            OwnerNumeric = Convert.ToInt16(searchResultMobile.ResultData[i].OwnerTypeId),
                            MakeMonth = searchResultMobile.ResultData[i].MakeMonth
                        });
                        searchResultMobile.ResultData[i].FinanceUrl = stockFinanceData.FinanceUrl;
                        searchResultMobile.ResultData[i].EmiFormatted = Format.GetValueInINR(searchResultMobile.ResultData[i].Emi);
                    }
                    if (!string.IsNullOrEmpty(searchResultMobile.ResultData[i].DeliveryText))
                    {
                        searchResultMobile.ResultData[i].Url = $"{ searchResultMobile.ResultData[i].Url }?dc={ searchResultMobile.ResultData[i].DeliveryCity }";
                    }
                        
                }
            }

            return searchResultMobile;

        }

        private static string GetValuationPath(StockBaseEntity stockBaseEntity)
        {
            StringBuilder str = new StringBuilder("/m/used/valuation/v1/report/?");
            str.Append($"car={ stockBaseEntity.VersionId}");
            str.Append($"&year={stockBaseEntity.MakeYear}");
            str.Append($"&city={stockBaseEntity.CityId}");
            str.Append($"&askingPrice={stockBaseEntity.PriceNumeric}");
            str.Append($"&owner={stockBaseEntity.OwnerTypeId}");
            str.Append($"&kms={stockBaseEntity.KmNumeric}");
            str.Append($"&isChatAvail={(stockBaseEntity.IsChatAvailable ? 1 : 0)}");
            string dealerRating = string.IsNullOrWhiteSpace(stockBaseEntity.DealerRatingText) 
                ? string.Empty
                : $"&ratingText={HttpUtility.UrlEncode(stockBaseEntity.DealerRatingText)}";
            str.Append(dealerRating);
            return str.ToString();
        }

        private void ProcessNearByCityLogic(ElasticOuptputs elasticInputs
            , SearchResultMobile searchResultMobile
            , int stockCountFetched
            , int totalPageSize
            , ref bool isNearbyCityMessageRequired
            ,ref int currentNearbyCityStockCount)
        {
            int selectedNearbyCityId = 0;
            int selectedNearbyCityCount = 0;
            List<string> cityIdList;
            List<string> cityCountList;
            if (string.IsNullOrWhiteSpace(elasticInputs.nearbyCityIds))
            {
                List<City> nearbyCities = _nearbyCityLogic.GetCities(elasticInputs);
                if (nearbyCities != null && nearbyCities.Count > 0)
                {
                    cityIdList = nearbyCities
                            .Select(nbc => nbc.CityId.ToString())
                            .ToList();
                    cityCountList = nearbyCities
                        .Select(nbc => nbc.CityCount.ToString())
                        .ToList();
                    elasticInputs.nearbyCityIds = searchResultMobile.NearbyCityIds = string.Join(",", cityIdList);
                    elasticInputs.nearbyCityIdsStockCount = searchResultMobile.NearbyCityIdsStockCount = string.Join(",", cityCountList);
                    
                    
                        selectedNearbyCityId = Convert.ToInt32(cityIdList[0]);
                        selectedNearbyCityCount = Convert.ToInt32(cityCountList[0]);
                    
                }
            }
            else
            {
                cityIdList = elasticInputs.nearbyCityIds.Trim().Split(',').ToList();
                cityCountList = elasticInputs.nearbyCityIdsStockCount.Trim().Split(',').ToList();
                
                int index = cityIdList.IndexOf(elasticInputs.nearbyCityId.ToString());
                if (index < cityIdList.Count - 1 && index != -1)
                {
                    selectedNearbyCityId = Convert.ToInt32(cityIdList[index + 1]);
                    selectedNearbyCityCount = Convert.ToInt32(cityCountList[index + 1]);
                }
            }
            
            
            elasticInputs.nearbyCityId = searchResultMobile.NearbyCityId = selectedNearbyCityId;
            if (elasticInputs.nearbyCityId > 0)
            {
                elasticInputs.lcr = 0;
                elasticInputs.ldr = 0;
                elasticInputs.ps = (totalPageSize - stockCountFetched).ToString();
                elasticInputs.cities = new[] { selectedNearbyCityId.ToString() };
                isNearbyCityMessageRequired = true;
                currentNearbyCityStockCount = selectedNearbyCityCount;
            }
        }

        private City GetPrimaryCity(string cityId)
        {
            int CityId;
            City city = new City();
            if (!string.IsNullOrEmpty(cityId))
            {
                if (cityId == "3000")
                {
                    city.CityName = "Mumbai";
                }
                else if (cityId == "3001")
                {
                    city.CityName = "Delhi NCR";
                }
                else
                {
                    city.CityName = _geoCitiesCacheRepository.GetCityNameById(cityId);
                }
                int.TryParse(cityId, out CityId);
                city.CityId = CityId;
            }
            return city;
        }
    }
}
