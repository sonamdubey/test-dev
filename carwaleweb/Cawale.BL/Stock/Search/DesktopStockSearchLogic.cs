using Carwale.BL.Dealers.Used;
using Carwale.BL.Interface.Stock.Search;
using Carwale.Entity.Classified;
using Carwale.Entity.Enum;
using Carwale.Entity.Stock.Finance;
using Carwale.Entity.Stock.Search;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Stock;
using Carwale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Carwale.BL.Stock.Search
{
    public class DesktopStockSearchLogic : StockSearchLogicBase<SearchResultDesktop>
    {
        private readonly IProcessFilters _processFilters;
        private readonly IStockManager _stockManager;
        private readonly INearbyCityLogic _nearbyCityLogic;
        private readonly IAggregationsRepository _aggregationRepository;
        public DesktopStockSearchLogic(IProcessFilters processFilters
            , IStockManager stockManager
            , INearbyCityLogic nearbyCityLogic
            , IAggregationsRepository aggregationRepository)
        {
            _processFilters = processFilters;
            _stockManager = stockManager;
            _nearbyCityLogic = nearbyCityLogic;
            _aggregationRepository = aggregationRepository;
        }
        public override SearchResultDesktop Get(FilterInputs filterInputs)
        {
            if (filterInputs == null)
            {
                return null;
            }
            SearchResultDesktop searchResultDesktop = new SearchResultDesktop();
            var elasticInputs = _processFilters.ProcessFilterParams(filterInputs);
            elasticInputs.sessionId = SessionId;

            elasticInputs.ps = GetTotalPageSize(elasticInputs.ps).ToString();

            SearchResultBase results = new SearchResultBase();

            if (elasticInputs.multiCityName == null && elasticInputs.cities != null && elasticInputs.cities.Length > 1)
            {
                int noOfNBCities = elasticInputs.cities.Length;
                string[] nbCities = elasticInputs.cities;
                for (int i = 0; i < noOfNBCities; i++)
                {
                    if (nbCities != null)
                    {
                        elasticInputs.cities = new string[] { nbCities[i] };
                    }
                    results = _stockManager.GetStocks(elasticInputs);
                    if (results.ResultData.Count > 0 && nbCities != null)
                    {
                        results.ResultData[results.ResultData.Count - 1].NBCityStripId = nbCities[i];
                    }


                }
            }
            else
            {
                results = _stockManager.GetStocks(elasticInputs);
                if (results.ResultData.Count > 0 && elasticInputs.cities != null)
                    results.ResultData[results.ResultData.Count - 1].NBCityStripId = elasticInputs.cities[0];

            }
            SetRecommendationUrl(results.ResultData);
            CountData countData = _aggregationRepository.GetAllFilterCount(elasticInputs);

            List<City> nearbyCities = _nearbyCityLogic.GetCities(elasticInputs);

            searchResultDesktop.ResultData.AddRange(results.ResultData);
            searchResultDesktop.FiltersData = countData;
            //searchResultDesktop.PagerData = countData; TODO:this field is not required, need to check front end
            if (nearbyCities != null && nearbyCities.Count > 0)
            {
                searchResultDesktop.NearByCitiesWithCount.AddRange(nearbyCities);
            }
            searchResultDesktop.LastNonFeaturedSlotRank = results.LastNonFeaturedSlotRank;
            searchResultDesktop.ExcludeStocks = GetExcludedStocksFromResultListings(filterInputs, results.ResultData);

            foreach (var resultStock in searchResultDesktop.ResultData)
            {
                resultStock.PriceNumeric = resultStock.Price;
                resultStock.KmNumeric = resultStock.Km;
                resultStock.Price = Format.FormatFullPrice(resultStock.Price);
                resultStock.Km = Format.Numeric(resultStock.Km);
                resultStock.LastUpdatedOn = Format.GetDisplayTimeSpan(resultStock.LastUpdatedOn);
                resultStock.Color = Format.UppercaseWords(resultStock.Color);
                resultStock.MaskingNumber = string.Empty;
                resultStock.MakeMonth = resultStock.MfgDate.Month;
                if (resultStock.IsEligibleForFinance && resultStock.CwBasePackageId != Entity.Enum.CwBasePackageId.Franchise)
                {
                    var stockFinanceData = StockFinanceBL.GetFinanceData(new FinanceUrlParameter
                    {
                        HostUrl = ConfigurationManager.AppSettings["CTFinanceDesktop"],
                        ProfileId = resultStock.ProfileId,
                        MakeId = Convert.ToInt32(resultStock.MakeId),
                        ModelId = Convert.ToInt32(resultStock.ModelId),
                        MakeYear = Convert.ToInt32(resultStock.MakeYear),
                        CityId = Convert.ToInt32(resultStock.CityId),
                        PriceNumeric = Convert.ToInt32(resultStock.PriceNumeric),
                        OwnerNumeric = Convert.ToInt16(resultStock.OwnerTypeId),
                        MakeMonth = resultStock.MakeMonth
                    });
                    resultStock.FinanceUrl = stockFinanceData.FinanceUrl;
                    resultStock.FinanceUrlText = stockFinanceData.FinanceUrlText;
                    resultStock.EmiFormatted = Format.GetValueInINR(resultStock.Emi);
                }
                resultStock.ValuationUrl = GetValuationPath(resultStock);
                resultStock.CertProgLogoUrl = StockBL.GetLogoUrlForStock(resultStock.CwBasePackageId, resultStock.CertProgLogoUrl);
            }

            return searchResultDesktop;
        }

        private static string GetValuationPath(StockBaseEntity resultStock)
        {
            StringBuilder strB = new StringBuilder("/used/valuation/v1/report/?");
            strB.Append($"car={resultStock.VersionId}");
            strB.Append($"&year={resultStock.MakeYear}");
            strB.Append($"&city={resultStock.CityId}");
            strB.Append($"&askingPrice={resultStock.PriceNumeric}");
            strB.Append($"&owner={resultStock.OwnerTypeId}");
            strB.Append($"&kms={resultStock.KmNumeric}");
            return strB.ToString();
        }

        private void SetRecommendationUrl(ICollection<StockBaseEntity> stockList)
        {
            if (stockList != null)
            {
                foreach (var item in stockList)
                {
                    if (item.CwBasePackageId == CwBasePackageId.Franchise)
                    {
                        item.DealerCarsUrl = UsedDealerStocksBL.GetDealerOtherCarsUrl(item.SellerName, item.DealerId);
                    }
                    else
                    {
                        item.StockRecommendationsUrl = StockRecommendationsBL.GetStockRecommendationsUrl(
                                                                        item.ProfileId,
                                                                        CustomParser.parseIntObject(item.RootId),
                                                                        CustomParser.parseIntObject(item.CityId),
                                                                        item.DeliveryCity,
                                                                        CustomParser.parseIntObject(item.Price),
                                                                        CustomParser.parseIntObject(item.VersionSubSegmentID)
                                                                );
                    }
                }
            }
        }
    }
}
