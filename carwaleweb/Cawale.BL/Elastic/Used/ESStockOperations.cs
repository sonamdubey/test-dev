using AutoMapper;
using Carwale.BL.Elastic.Used;
using Carwale.BL.Stock;
using AEPLCore.Cache;
using Carwale.Cache.Geolocation;
using Carwale.DAL.Geolocation;
using Carwale.DTOs.Classified;
using Carwale.DTOs.Classified.Stock;
using Carwale.DTOs.Classified.Stock.Ios;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Enum;
using Carwale.Entity.Elastic;
using Carwale.Entity.Stock.Finance;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using Carwale.Utility.Classified;
using log4net;
using Microsoft.Practices.Unity;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity.Enum;
using System.Text;

namespace Carwale.BL.Elastic
{
    public class ESStockOperations : IESOperations
    {
        /// <summary>
        /// Class Added By Jugal || 04 Nov 2014
        /// It will Call the methods that will fetch the data from ElasticSearchIndex.
        /// </summary>
        IUnityContainer _container;
        IESStockQuery searchQuery;
        private readonly IProcessFilters processFilters;
        public List<SortCriteriaAndroid> sortCriterias = new List<SortCriteriaAndroid>();
        public List<SortCriteriaIos> SortCriteria = new List<SortCriteriaIos>();
        private readonly IGeoCitiesRepository _geoCitiesRepo;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepository;
        private readonly IESStockQueryRepository _esStockQueryRepository;

        public ESStockOperations(IUnityContainer container, IProcessFilters _processFilters, IESStockQueryRepository esStockQueryRepository)
        {
            _container = container;
            processFilters = _processFilters;
            _esStockQueryRepository = esStockQueryRepository;
            _container.RegisterType<IESStockQuery, ESStockQuery>()
                .RegisterType<IProcessElasticJson, ProcessElasticJson>();

            searchQuery = container.Resolve<IESStockQuery>();

            _container.RegisterType<IGeoCitiesRepository, GeoCitiesRepository>();
            _geoCitiesRepo = container.Resolve<IGeoCitiesRepository>();
            _container.RegisterType<ICacheManager, CacheManager>();
            _container.RegisterType<IGeoCitiesCacheRepository, GeoCitiesCacheRepository>();
            _geoCitiesCacheRepository = container.Resolve<IGeoCitiesCacheRepository>();
        }

        /// <summary>
        /// Method for results and count of Used Car Search Page.
        /// Modified By : Sadhana Upadhyay on 10 Mar 2015
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public ResultsFiltersPagerDesktop GetElasticResults(ElasticClient client, FilterInputs filterInputs)
        {
            ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
            Tuple<List<StockBaseEntity>, int, int, int, int> onlyResults = searchQuery.GetSearchResults(client, elasticInputs);

            CountData onlyCounts = searchQuery.GetAggregationsCount(client, elasticInputs);

            PagerOutputEntity pager = new PagerOutputEntity();

            if (onlyCounts != null && onlyCounts.StockCount != null)
            {
                pager = searchQuery.GetPagerData(onlyCounts.StockCount.TotalStockCount, elasticInputs);
            }

            ResultsFiltersPagerDesktop usedJson = new ResultsFiltersPagerDesktop()
            {
                ResultsData = onlyResults.Item1,// search listings
                FiltersData = onlyCounts,
                PagerData = pager,
                LastNonFeaturedSlotRank = onlyResults.Item2 + filterInputs.lcr,
                LastDealerFeaturedSlotRank = onlyResults.Item3 + filterInputs.ldr,
                LastIndividualFeaturedSlotRank = onlyResults.Item4 + filterInputs.lir
            };

            return usedJson;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 5 Mar 2015
        /// Summary : To get stock result for android and mobile site
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public ResultsFiltersPagerAndroid GetElasticResultsAndroid(ElasticClient client, FilterInputs filterInputs)
        {
            ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
            Tuple<List<StockBaseEntity>, int, int, int, int> results = searchQuery.GetSearchResults(client, elasticInputs);
            List<StockResultsAndroidBase> onlyResults = Mapper.Map<List<StockBaseEntity>, List<StockResultsAndroidBase>>(results.Item1);

            FilterCountsAndroid onlyCounts = Mapper.Map<CountData, FilterCountsAndroid>(searchQuery.GetAggregationsCount(client, elasticInputs));

            PagerAndroidBase pager = Mapper.Map<PagerOutputEntity, PagerAndroidBase>(searchQuery.GetPagerData(onlyCounts.FiltersData.StockCount.TotalStockCount, elasticInputs));

            ResultsFiltersPagerAndroid usedJson = new ResultsFiltersPagerAndroid()
            {
                ResultsData = onlyResults,
                FiltersData = onlyCounts.FiltersData,
                PagerData = pager
            };

            for (int i = 0; i < usedJson.ResultsData.Count; i++)
            {
                usedJson.ResultsData[i].Price = Format.Numeric(usedJson.ResultsData[i].Price);
                usedJson.ResultsData[i].Km = Format.Numeric(usedJson.ResultsData[i].Km);
                usedJson.ResultsData[i].AbsureScore = GetAbsureRating(Convert.ToInt32(usedJson.ResultsData[i].AbsureScore));
                usedJson.ResultsData[i].MaskingNumber = String.Empty;
            }

            return usedJson;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : to get search result for desktop platform
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public ResultsFiltersPagerDesktop GetElasticResultsDesktop(ElasticClient client, FilterInputs filterInputs)
        {
            try
            {
                int rootId, cityId, price, versionSubSegmentId;
                ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
                Tuple<List<StockBaseEntity>, int, int, int, int> results = searchQuery.GetSearchResults(client, elasticInputs);
                CountData onlyCounts = searchQuery.GetAggregationsCount(client, elasticInputs);

                PagerOutputEntity pager = new PagerOutputEntity();

                if (onlyCounts != null && onlyCounts.StockCount != null)
                {
                    pager = searchQuery.GetPagerData(onlyCounts.StockCount.TotalStockCount, elasticInputs);
                }

                List<City> nearbyCities = searchQuery.GetNearbyCities(client, string.Empty, elasticInputs);

                ResultsFiltersPagerDesktop usedJson = new ResultsFiltersPagerDesktop()
                {
                    ResultsData = results.Item1,// stock results
                    FiltersData = onlyCounts,
                    PagerData = pager,
                    NearByCitiesWithCount = nearbyCities,
                    LastNonFeaturedSlotRank = results.Item2 + filterInputs.lcr,
                    LastDealerFeaturedSlotRank = results.Item3 + filterInputs.ldr,
                    LastIndividualFeaturedSlotRank = results.Item4 + filterInputs.lir,
                    ExcludeStocks = GetExcludedStocksFromResultListings(filterInputs, results.Item1),
                };

                for (int i = 0; i < usedJson.ResultsData.Count; i++)
                {
                    usedJson.ResultsData[i].PriceNumeric = usedJson.ResultsData[i].Price;
                    usedJson.ResultsData[i].KmNumeric = usedJson.ResultsData[i].Km;
                    usedJson.ResultsData[i].Price = Format.FormatFullPrice(usedJson.ResultsData[i].Price);
                    usedJson.ResultsData[i].AbsureScore = GetAbsureRating(Convert.ToInt32(usedJson.ResultsData[i].AbsureScore));
                    usedJson.ResultsData[i].Km = Format.Numeric(usedJson.ResultsData[i].Km);
                    usedJson.ResultsData[i].LastUpdatedOn = Format.GetDisplayTimeSpan(usedJson.ResultsData[i].LastUpdatedOn);
                    usedJson.ResultsData[i].Color = Format.UppercaseWords(usedJson.ResultsData[i].Color);
                    usedJson.ResultsData[i].MaskingNumber = String.Empty;
                    usedJson.ResultsData[i].MakeMonth = usedJson.ResultsData[i].MfgDate.Month;
                    if (usedJson.ResultsData[i].IsEligibleForFinance && usedJson.ResultsData[i].CwBasePackageId != Entity.Enum.CwBasePackageId.Franchise)
                    {
                        var stockFinanceData = StockFinanceBL.GetFinanceData(new FinanceUrlParameter
                        {
                            HostUrl = ConfigurationManager.AppSettings["CTFinanceDesktop"],
                            ProfileId = usedJson.ResultsData[i].ProfileId,
                            MakeId = Convert.ToInt32(usedJson.ResultsData[i].MakeId),
                            ModelId = Convert.ToInt32(usedJson.ResultsData[i].ModelId),
                            MakeYear = Convert.ToInt32(usedJson.ResultsData[i].MakeYear),
                            CityId = Convert.ToInt32(usedJson.ResultsData[i].CityId),
                            PriceNumeric = Convert.ToInt32(usedJson.ResultsData[i].PriceNumeric),
                            OwnerNumeric = Convert.ToInt16(usedJson.ResultsData[i].OwnerTypeId),
                            MakeMonth = usedJson.ResultsData[i].MakeMonth
                        });
                        usedJson.ResultsData[i].FinanceUrl = stockFinanceData.FinanceUrl;
                        usedJson.ResultsData[i].FinanceUrlText = stockFinanceData.FinanceUrlText;
                        usedJson.ResultsData[i].EmiFormatted = Format.GetValueInINR(usedJson.ResultsData[i].Emi);
                    }
                    usedJson.ResultsData[i].ValuationUrl = String.Format("/used/valuation/v1/report/?car={0}&year={1}&city={2}&askingPrice={3}&owner={4}&kms={5}", usedJson.ResultsData[i].VersionId, usedJson.ResultsData[i].MakeYear, usedJson.ResultsData[i].CityId, usedJson.ResultsData[i].PriceNumeric, usedJson.ResultsData[i].OwnerTypeId,usedJson.ResultsData[i].KmNumeric);
                    int.TryParse(usedJson.ResultsData[i].RootId, out rootId);
                    int.TryParse(usedJson.ResultsData[i].CityId, out cityId);
                    int.TryParse(usedJson.ResultsData[i].PriceNumeric, out price);
                    Int32.TryParse(usedJson.ResultsData[i].VersionSubSegmentID, out versionSubSegmentId);
                    usedJson.ResultsData[i].CertProgLogoUrl = StockBL.GetLogoUrlForStock(usedJson.ResultsData[i].CwBasePackageId, usedJson.ResultsData[i].CertProgLogoUrl);
                    usedJson.ResultsData[i].IsChatAvailable = false;
                }

                return usedJson;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        public ResultsRecommendation GetRecommendationResults(ElasticClient clientElastic, FilterInputs filterInputs)
        {
            ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
            List<StockBaseEntity> onlyResults = Mapper.Map<List<StockBaseEntity>, List<StockBaseEntity>>(searchQuery.GetRecommendationResults(clientElastic, elasticInputs));
            ResultsRecommendation usedJson = new ResultsRecommendation()
            {
                ResultsData = onlyResults
            };
            int count = usedJson.ResultsData.Count;
            for (int i = 0; i < count; i++)
            {
                usedJson.ResultsData[i].PriceNumeric = usedJson.ResultsData[i].Price;
                usedJson.ResultsData[i].Price = Format.FormatFullPrice(usedJson.ResultsData[i].Price);
                usedJson.ResultsData[i].MaskingNumber = String.Empty;
                usedJson.ResultsData[i].Url = $"{ usedJson.ResultsData[i].Url }?rk=r{ i + 1 }&isP=false";
                usedJson.ResultsData[i].Rank = $"r{i + 1}";
            }
            return usedJson;
        }

        public List<StockBaseEntity> GetRecommendationsForProfileId<TInput>(ElasticClient clientElastic, TInput tInput, int recommendationsCount)
        {
            List<StockBaseEntity> onlyResults = searchQuery.GetRecommendationsForProfileId(clientElastic, tInput, recommendationsCount);
            return onlyResults;
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

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : to get search result for mobile platform
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public StockResultsMobile GetElasticResultsMobile(ElasticClient client, FilterInputs filterInputs)
        {
            try
            {
                ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
                StockBaseMobile results = searchQuery.GetSearchResultsMobile(client, elasticInputs);
                var resultsData = new StockResultsMobile();
                resultsData.ResultData = results.ListStocks;
                resultsData.PrimaryCity = GetPrimaryCity(filterInputs.city);
                resultsData.IsAllStocksFetched = results.IsAllStocksFetched;
                resultsData.TotalStockCount = results.TotalStockCount;

                int lcr = results.LastNonFeaturedRank - filterInputs.lcr;
                int ldr = results.LastDealerFeaturedRank - filterInputs.ldr;
                int lir = results.LastIndividualFeaturedRank - filterInputs.lir;

                if (!results.IsAllStocksFetched)
                {
                    var excludeStocks = GetExcludedStocksFromResultListings(filterInputs, resultsData.ResultData);
                    resultsData.NextPageUrl = GetNextPageUrl(filterInputs, lcr, ldr, lir, excludeStocks, results: results);
                }

                if (resultsData.ResultData != null)
                {
                    for (int i = 0; i < resultsData.ResultData.Count; i++)
                    {
                        resultsData.ResultData[i].PriceNumeric = resultsData.ResultData[i].Price;
                        resultsData.ResultData[i].Price = Format.FormatFullPrice(resultsData.ResultData[i].Price);
                        resultsData.ResultData[i].KmNumeric = resultsData.ResultData[i].Km;
                        resultsData.ResultData[i].Km = Format.Numeric(resultsData.ResultData[i].Km);
                        resultsData.ResultData[i].MakeMonth = resultsData.ResultData[i].MfgDate.Month;
                        resultsData.ResultData[i].ValuationUrl = string.Format("/m/used/valuation/v1/report/?car={0}&year={1}&city={2}&askingPrice={3}&owner={4}{5}&kms={6}",
                                                                                resultsData.ResultData[i].VersionId, resultsData.ResultData[i].MakeYear,
                                                                                resultsData.ResultData[i].CityId, resultsData.ResultData[i].PriceNumeric,
                                                                                resultsData.ResultData[i].OwnerTypeId,
                                                                                string.IsNullOrEmpty(resultsData.ResultData[i].DealerRatingText) ? "" : string.Format("&ratingText={0}",
                                                                                                     HttpUtility.UrlEncode(resultsData.ResultData[i].DealerRatingText)),
                                                                                resultsData.ResultData[i].KmNumeric);
                        if (resultsData.ResultData[i].IsEligibleForFinance && resultsData.ResultData[i].CwBasePackageId != Entity.Enum.CwBasePackageId.Franchise)
                        {
                            var stockFinanceData = StockFinanceBL.GetFinanceData(new FinanceUrlParameter
                            {
                                HostUrl = ConfigurationManager.AppSettings["CTFinanceMsite"],
                                ProfileId = resultsData.ResultData[i].ProfileId,
                                MakeId = Convert.ToInt32(resultsData.ResultData[i].MakeId),
                                ModelId = Convert.ToInt32(resultsData.ResultData[i].ModelId),
                                MakeYear = Convert.ToInt32(resultsData.ResultData[i].MakeYear),
                                CityId = Convert.ToInt32(resultsData.ResultData[i].CityId),
                                PriceNumeric = Convert.ToInt32(resultsData.ResultData[i].PriceNumeric),
                                OwnerNumeric = Convert.ToInt16(resultsData.ResultData[i].OwnerTypeId),
                                MakeMonth = resultsData.ResultData[i].MakeMonth
                            });
                            resultsData.ResultData[i].FinanceUrl = stockFinanceData.FinanceUrl;
                            resultsData.ResultData[i].EmiFormatted = Format.GetValueInINR(resultsData.ResultData[i].Emi);
                        }
                        resultsData.ResultData[i].Url = $"{ resultsData.ResultData[i].Url }?rk={ i + 1 }&slot={ resultsData.ResultData[i].SlotId }";
                        if (!string.IsNullOrEmpty(resultsData.ResultData[i].DeliveryText))
                        {
                            resultsData.ResultData[i].Url = $"{ resultsData.ResultData[i].Url }&dc={ resultsData.ResultData[i].DeliveryCity }";
                        }
                    }
                }

                return resultsData;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : to get Search result for android platform api
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public StockResultsAndroid GetResultsDataAndroid(ElasticClient client, FilterInputs filterInputs)
        {
            ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
            Tuple<List<StockBaseEntity>, int, int, int, int> results = searchQuery.GetSearchResults(client, elasticInputs);
            UpdateInspectionText(results.Item1);
            var resultsData = new StockResultsAndroid()
            {
                StockResults = Mapper.Map<List<StockBaseEntity>, List<StockResultsAndroidBase>>(results.Item1),
                SortCriteria = GetSortCriteria(filterInputs)
            };


            if (resultsData.StockResults.Count >= GetPageSize(filterInputs))
            {
                if (!string.IsNullOrEmpty(filterInputs.pn) && Convert.ToInt32(filterInputs.pn) >= 10)
                    resultsData.NextPageUrl = "";
                else
                {
                    var excludeStocks = GetExcludedStocksFromResultListings(filterInputs, results.Item1);
                    resultsData.NextPageUrl = GetNextPageUrl(filterInputs, results.Item2, results.Item3, results.Item4, excludeStocks);
                }
            }
            else
                resultsData.NextPageUrl = "";

            for (int i = 0; i < resultsData.StockResults.Count; i++)
            {
                if (!string.IsNullOrEmpty(resultsData.StockResults[i].DeliveryText))
                    resultsData.StockResults[i].Url = "https://" + ConfigurationManager.AppSettings["HostUrl"].ToString() + "/api/UsedCarDetails/?car=" + resultsData.StockResults[i].ProfileId + "&dc=" + resultsData.StockResults[i].DeliveryCity;
                else
                    resultsData.StockResults[i].Url = "https://" + ConfigurationManager.AppSettings["HostUrl"].ToString() + "/api/UsedCarDetails/?car=" + resultsData.StockResults[i].ProfileId;
                resultsData.StockResults[i].FormattedPrice = Format.FormatFullPrice(resultsData.StockResults[i].Price);
                resultsData.StockResults[i].Price = Format.Numeric(resultsData.StockResults[i].Price);
                resultsData.StockResults[i].Km = Format.Numeric(resultsData.StockResults[i].Km);
                resultsData.StockResults[i].AbsureScore = GetAbsureRating(Convert.ToInt32(resultsData.StockResults[i].AbsureScore));
                resultsData.StockResults[i].MaskingNumber = String.Empty;
            }
            return resultsData;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 Apr 2015
        /// Summary : To get Next Page Url for ios and android app
        /// </summary>
        /// <param name="filterInputs"></param>
        /// <returns></returns>
        private string GetNextPageUrl(FilterInputs filterInputs, int lastNonFeaturedSlotRank, int lastDealerFeaturedSlotRank, int lastIndividualFeaturedSlotRank, string excludeStocks, string urlWithoutQs = "", StockBaseMobile results = null)
        {
            string nextPageUrl = string.Empty;
            if (string.IsNullOrEmpty(urlWithoutQs))
                nextPageUrl = string.Format("https://{0}/webapi/Classified/stock/?", ConfigurationManager.AppSettings["HostUrl"]);
            else
                nextPageUrl = urlWithoutQs;

            nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "pn=", string.IsNullOrEmpty(filterInputs.pn) ? 2 : Convert.ToInt32(filterInputs.pn) + 1);

            if (!string.IsNullOrEmpty(filterInputs.bodytype))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&bodytype=", filterInputs.bodytype.TrimEnd(' ').Replace(' ', '+'));

            if (!string.IsNullOrEmpty(filterInputs.budget))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&budget=", filterInputs.budget);

            if (!string.IsNullOrEmpty(filterInputs.car))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&car=", filterInputs.car.TrimEnd(' ').Replace(' ', '+'));

            if (!string.IsNullOrEmpty(filterInputs.city))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&city=", filterInputs.city);

            if (!string.IsNullOrEmpty(filterInputs.color))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&color=", filterInputs.color.TrimEnd(' ').Replace(' ', '+'));

            if (!string.IsNullOrEmpty(filterInputs.filterby))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&filterby=", filterInputs.filterby.TrimEnd(' ').Replace(' ', '+'));

            //Added By : Sadhana Upadhyay on 5 May 2015 to add filterbyadditional filter
            if (!string.IsNullOrEmpty(filterInputs.filterbyadditional))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&filterbyadditional=", filterInputs.filterbyadditional.TrimEnd(' ').Replace(' ', '+'));

            if (!string.IsNullOrEmpty(filterInputs.fuel))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&fuel=", filterInputs.fuel.TrimEnd(' ').Replace(' ', '+'));

            if (!string.IsNullOrEmpty(filterInputs.kms))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&kms=", filterInputs.kms);

            if (!string.IsNullOrEmpty(filterInputs.owners))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&owners=", filterInputs.owners.TrimEnd(' ').Replace(' ', '+'));

            if (!string.IsNullOrEmpty(filterInputs.ps))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&ps=", filterInputs.ps);

            if (!string.IsNullOrEmpty(filterInputs.sc))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&sc=", filterInputs.sc);

            if (!string.IsNullOrEmpty(filterInputs.so))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&so=", filterInputs.so);

            if (!string.IsNullOrEmpty(filterInputs.seller))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&seller=", filterInputs.seller.TrimEnd(' ').Replace(' ', '+'));

            if (!string.IsNullOrEmpty(filterInputs.trans))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&trans=", filterInputs.trans.TrimEnd(' ').Replace(' ', '+'));

            if (!string.IsNullOrEmpty(filterInputs.year))
                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&year=", filterInputs.year);

            nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&lcr=", (filterInputs.lcr + lastNonFeaturedSlotRank));

            nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&ldr=", (filterInputs.ldr + lastDealerFeaturedSlotRank));

            nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&lir=", (filterInputs.lir + lastIndividualFeaturedSlotRank));

            nextPageUrl = filterInputs.Latitude <= 0 ? nextPageUrl : $"{ nextPageUrl }&latitude={ filterInputs.Latitude }";
            nextPageUrl = filterInputs.Longitude <= 0 ? nextPageUrl : $"{ nextPageUrl }&longitude={ filterInputs.Longitude }";
            nextPageUrl = $"{nextPageUrl}&shouldfetchnearbycars={filterInputs.ShouldFetchNearbyCars}";
            NearbyCarsBucket bucket = results?.ListStocks?.LastOrDefault()?.NearbyCarsBucket == null ? NearbyCarsBucket.Default : results.ListStocks.LastOrDefault().NearbyCarsBucket;
            nextPageUrl = bucket == NearbyCarsBucket.Default ? nextPageUrl : $"{nextPageUrl}&lastnearbycarsbucket={results.ListStocks.LastOrDefault().NearbyCarsBucket}";
            if (filterInputs.Area > 0)
            {
                nextPageUrl = $"{nextPageUrl}&area={filterInputs.Area}";
            }
            
            if (results != null)
            {
                if (!string.IsNullOrEmpty(results.NearbyCityIds))
                    nextPageUrl = string.Format("{0}&nearbyCityId={1}&nearbyCityIds={2}&nearbyCityIdsStockCount={3}", nextPageUrl, results.NearbyCityId, results.NearbyCityIds, results.NearbyCityIdsStockCount);

                nextPageUrl = string.Format("{0}{1}{2}", nextPageUrl, "&stockfetched=", (filterInputs.stockFetched + (results.ListStocks != null ? results.ListStocks.Count : 0)));

            }
            nextPageUrl = !string.IsNullOrEmpty(excludeStocks) ? $"{nextPageUrl}&excludestocks={excludeStocks}" : nextPageUrl;
            return nextPageUrl;
        }

        private static string GetExcludedStocksFromResultListings(FilterInputs filterInputs, IEnumerable<StockBaseEntity> listings)
        {
            if(string.IsNullOrEmpty(filterInputs.pn) || filterInputs.pn == "1")
            {
                StringBuilder stocksToBeExcluded = new StringBuilder();
                foreach(var stock in listings)
                {
                    if(!stock.IsPremium)
                    {
                        break;
                    }
                    else
                    {
                        stocksToBeExcluded.AppendFormat("+{0}",stock.ProfileId);
                    }
                }
                return stocksToBeExcluded.Length > 0 ? stocksToBeExcluded.ToString(1, stocksToBeExcluded.Length - 1) : string.Empty;
            }
            return filterInputs.ExcludeStocks?.Replace(' ', '+');
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : to get filter result for android api
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public FilterCountsAndroid GetFilterResultAndroid(ElasticClient client, FilterInputs filterInputs)
        {
            ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
            Mapper.CreateMap<CountData, FiltersCountAndroidBase>();

            var aggCounts = searchQuery.GetAggregationsCount(client, elasticInputs);
            aggCounts.SellerTypeCount.Individual = 0;   //To disable individual seller filter
            var filtersData = new FilterCountsAndroid()
            {
                FiltersData = Mapper.Map<CountData, FiltersCountAndroidBase>(aggCounts)
            };

            return filtersData;
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : to get sort criteria for search page
        /// </summary>
        /// <returns></returns>
        private List<SortCriteriaAndroid> GetSortCriteria(FilterInputs filterInputs)
        {
            SortCriteriaAndroid sc = new SortCriteriaAndroid();
            sc.SortText = "Relevance";
            sc.SortFor = "Relevance";
            sc.SortOrder = "";

            string url = "https://" + ConfigurationManager.AppSettings["HostUrl"].ToString() + HttpContext.Current.Request.Url.AbsolutePath;
            sc.SortUrl = url + "?";

            if (!string.IsNullOrEmpty(filterInputs.city))
                sc.SortUrl = sc.SortUrl + "city=" + filterInputs.city + "&";

            if (!string.IsNullOrEmpty(filterInputs.car))
                sc.SortUrl = sc.SortUrl + "car=" + filterInputs.car.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.filterby))
                sc.SortUrl = sc.SortUrl + "filterby=" + filterInputs.filterby.TrimEnd(' ').Replace(' ', '+') + "&";

            //Added By : Sadhana Upadhyay on 5 May 2015 to add filterbyadditional filter
            if (!string.IsNullOrEmpty(filterInputs.filterbyadditional))
                sc.SortUrl = sc.SortUrl + "filterbyadditional=" + filterInputs.filterbyadditional.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.fuel))
                sc.SortUrl = sc.SortUrl + "fuel=" + filterInputs.fuel.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.bodytype))
                sc.SortUrl = sc.SortUrl + "bodytype=" + filterInputs.bodytype.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.seller))
                sc.SortUrl = sc.SortUrl + "seller=" + filterInputs.seller.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.owners))
                sc.SortUrl = sc.SortUrl + "owners=" + filterInputs.owners.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.trans))
                sc.SortUrl = sc.SortUrl + "trans=" + filterInputs.trans.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.color))
                sc.SortUrl = sc.SortUrl + "color=" + filterInputs.color.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.kms))
                sc.SortUrl = sc.SortUrl + "kms=" + filterInputs.kms + "&";

            if (!string.IsNullOrEmpty(filterInputs.year))
                sc.SortUrl = sc.SortUrl + "year=" + filterInputs.year + "&";

            if (!string.IsNullOrEmpty(filterInputs.budget))
                sc.SortUrl = sc.SortUrl + "budget=" + filterInputs.budget + "&";
            if (!string.IsNullOrEmpty(filterInputs.ps))
            {
                sc.SortUrl = sc.SortUrl + "pn=1&ps=" + filterInputs.ps;
            }
            else
            {
                sc.SortUrl = sc.SortUrl + "pn=1";
            }

            string sortUrl = sc.SortUrl;

            sc.SortUrl = sc.SortUrl + "&so=-1&sc=-1";
            sortCriterias.Add(sc);

            sc = new SortCriteriaAndroid();
            sc.SortText = "Price - Low To High";
            sc.SortFor = "Price";
            sc.SortOrder = "ASC";
            sc.SortUrl = sortUrl + "&so=0&sc=2";
            sortCriterias.Add(sc);

            sc = new SortCriteriaAndroid();
            sc.SortText = "Price - High To Low";
            sc.SortFor = "Price";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=2";
            sortCriterias.Add(sc);

            sc = new SortCriteriaAndroid();
            sc.SortText = "Year - Latest To Oldest";
            sc.SortFor = "Year";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=0";
            sortCriterias.Add(sc);

            sc = new SortCriteriaAndroid();
            sc.SortText = "Year - Oldest To Latest";
            sc.SortFor = "Year";
            sc.SortOrder = "ASC";
            sc.SortUrl = sortUrl + "&so=0&sc=0";
            sortCriterias.Add(sc);

            sc = new SortCriteriaAndroid();
            sc.SortText = "Kms - Low to High";
            sc.SortFor = "Kms";
            sc.SortOrder = "ASC";
            sc.SortUrl = sortUrl + "&so=0&sc=3";
            sortCriterias.Add(sc);

            sc = new SortCriteriaAndroid();
            sc.SortText = "Kms - High to Low";
            sc.SortFor = "Kms";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=3";
            sortCriterias.Add(sc);

            sc = new SortCriteriaAndroid();
            sc.SortText = "Last Updated - Latest To Oldest";
            sc.SortFor = "Last Updated";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=6";
            sortCriterias.Add(sc);

            sc = new SortCriteriaAndroid();
            sc.SortText = "Last Updated - Oldest To Latest";
            sc.SortFor = "Last Updated";
            sc.SortOrder = "ASC";
            sc.SortUrl = sortUrl + "&so=0&sc=6";
            sortCriterias.Add(sc);

            sc = new SortCriteriaAndroid();
            sc.SortText = "Certification Score - High to Low";
            sc.SortFor = "Certification Score";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=7";
            sortCriterias.Add(sc);

            return sortCriterias;

        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 10 Mar 2015
        /// Summary : to get sort criteria for search page for ios app
        /// </summary>
        /// <returns></returns>
        private List<SortCriteriaIos> GetSortCriteriaIOS(FilterInputs filterInputs)
        {

            SortCriteriaIos sc = new SortCriteriaIos();
            sc.SortText = "Relevance";
            sc.SortFor = "Relevance";
            sc.SortOrder = "";

            string url = "https://" + ConfigurationManager.AppSettings["HostUrl"].ToString() + HttpContext.Current.Request.Url.AbsolutePath;
            sc.SortUrl = url + "?";

            if (!string.IsNullOrEmpty(filterInputs.city))
                sc.SortUrl = sc.SortUrl + "city=" + filterInputs.city + "&";

            if (!string.IsNullOrEmpty(filterInputs.car))
                sc.SortUrl = sc.SortUrl + "car=" + filterInputs.car.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.filterby))
                sc.SortUrl = sc.SortUrl + "filterby=" + filterInputs.filterby.TrimEnd(' ').Replace(' ', '+') + "&";

            //Added By : Sadhana Upadhyay on 5 May 2015 to add filterbyadditional filter
            if (!string.IsNullOrEmpty(filterInputs.filterbyadditional))
                sc.SortUrl = sc.SortUrl + "filterbyadditional=" + filterInputs.filterbyadditional.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.fuel))
                sc.SortUrl = sc.SortUrl + "fuel=" + filterInputs.fuel.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.bodytype))
                sc.SortUrl = sc.SortUrl + "bodytype=" + filterInputs.bodytype.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.seller))
                sc.SortUrl = sc.SortUrl + "seller=" + filterInputs.seller.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.owners))
                sc.SortUrl = sc.SortUrl + "owners=" + filterInputs.owners.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.trans))
                sc.SortUrl = sc.SortUrl + "trans=" + filterInputs.trans.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.color))
                sc.SortUrl = sc.SortUrl + "color=" + filterInputs.color.TrimEnd(' ').Replace(' ', '+') + "&";

            if (!string.IsNullOrEmpty(filterInputs.kms))
                sc.SortUrl = sc.SortUrl + "kms=" + filterInputs.kms + "&";

            if (!string.IsNullOrEmpty(filterInputs.year))
                sc.SortUrl = sc.SortUrl + "year=" + filterInputs.year + "&";

            if (!string.IsNullOrEmpty(filterInputs.budget))
                sc.SortUrl = sc.SortUrl + "budget=" + filterInputs.budget + "&";
            if (!string.IsNullOrEmpty(filterInputs.ps))
            {
                sc.SortUrl = sc.SortUrl + "pn=1&ps=" + filterInputs.ps;
            }
            else
            {
                sc.SortUrl = sc.SortUrl + "pn=1";
            }

            string sortUrl = sc.SortUrl;

            sc.SortUrl = sc.SortUrl + "&so=-1&sc=-1";
            SortCriteria.Add(sc);

            sc = new SortCriteriaIos();
            sc.SortText = "Price - Low To High";
            sc.SortFor = "Price";
            sc.SortOrder = "ASC";
            sc.SortUrl = sortUrl + "&so=0&sc=2";
            SortCriteria.Add(sc);

            sc = new SortCriteriaIos();
            sc.SortText = "Price - High To Low";
            sc.SortFor = "Price";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=2";
            SortCriteria.Add(sc);

            sc = new SortCriteriaIos();
            sc.SortText = "Year - Latest To Oldest";
            sc.SortFor = "Year";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=0";
            SortCriteria.Add(sc);

            sc = new SortCriteriaIos();
            sc.SortText = "Year - Oldest To Latest";
            sc.SortFor = "Year";
            sc.SortOrder = "ASC";
            sc.SortUrl = sortUrl + "&so=0&sc=0";
            SortCriteria.Add(sc);

            sc = new SortCriteriaIos();
            sc.SortText = "Kms - Low to High";
            sc.SortFor = "Kms";
            sc.SortOrder = "ASC";
            sc.SortUrl = sortUrl + "&so=0&sc=3";
            SortCriteria.Add(sc);

            sc = new SortCriteriaIos();
            sc.SortText = "Kms - High to Low";
            sc.SortFor = "Kms";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=3";
            SortCriteria.Add(sc);

            sc = new SortCriteriaIos();
            sc.SortText = "Last Updated - Latest To Oldest";
            sc.SortFor = "Last Updated";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=6";
            SortCriteria.Add(sc);

            sc = new SortCriteriaIos();
            sc.SortText = "Last Updated - Oldest To Latest";
            sc.SortFor = "Last Updated";
            sc.SortOrder = "ASC";
            sc.SortUrl = sortUrl + "&so=0&sc=6";
            SortCriteria.Add(sc);

            sc = new SortCriteriaIos();
            sc.SortText = "Certification Score - High to Low";
            sc.SortFor = "Certification Score";
            sc.SortOrder = "DESC";
            sc.SortUrl = sortUrl + "&so=1&sc=7";
            SortCriteria.Add(sc);

            return SortCriteria;

        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 18 Mar 2015
        /// Summary : To convert absure score into absure rating
        /// </summary>
        /// <param name="absureScore"></param>
        /// <returns></returns>
        public static string GetAbsureRating(int absureScore)
        {
            string absureRating = string.Empty;
            if (absureScore >= 90)
            {
                absureRating = "5.0";
            }
            else if (absureScore >= 80)
            {
                absureRating = "4.5";
            }
            else if (absureScore >= 70)
            {
                absureRating = "4.0";
            }
            else if (absureScore >= 60)
            {
                absureRating = "3.5";
            }
            else if (absureScore < 60)
            {
                absureRating = "0";
            }
            return absureRating;
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 21 Apr 2015
        /// Summary : To get Stock result for ios app
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public StockResultIos GetResultsDataIos(ElasticClient client, FilterInputs filterInputs)
        {
            ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
            Tuple<List<StockBaseEntity>, int, int, int, int> results = searchQuery.GetSearchResults(client, elasticInputs);
            UpdateInspectionText(results.Item1);//Item1 = stockList
            var resultsData = new StockResultIos()
            {
                StockResults = Mapper.Map<List<StockBaseEntity>, List<StockResultsIosBase>>(results.Item1),
                SortCriteria = GetSortCriteriaIOS(filterInputs)
            };

            if (resultsData.StockResults.Count >= GetPageSize(filterInputs))
            {
                if (!string.IsNullOrEmpty(filterInputs.pn) && Convert.ToInt32(filterInputs.pn) >= 10)
                    resultsData.NextPageUrl = "";
                else
                {
                    var excludeStocks = GetExcludedStocksFromResultListings(filterInputs, results.Item1);
                    resultsData.NextPageUrl = GetNextPageUrl(filterInputs, results.Item2, results.Item3, results.Item4, excludeStocks);
                }
            }
            else
                resultsData.NextPageUrl = "";

            for (int i = 0; i < resultsData.StockResults.Count; i++)
            {
                if (!string.IsNullOrEmpty(resultsData.StockResults[i].DeliveryText))
                    resultsData.StockResults[i].Url = "https://" + ConfigurationManager.AppSettings["HostUrl"].ToString() + "/api/UsedCarDetails/?car=" + resultsData.StockResults[i].ProfileId + "&dc=" + resultsData.StockResults[i].DeliveryCity;
                else
                    resultsData.StockResults[i].Url = "https://" + ConfigurationManager.AppSettings["HostUrl"].ToString() + "/api/UsedCarDetails/?car=" + resultsData.StockResults[i].ProfileId;
                resultsData.StockResults[i].LargePicUrl = resultsData.StockResults[i].FrontImagePath.Replace("150x112", "640x428");
                resultsData.StockResults[i].FormattedPrice = Format.FormatFullPrice(resultsData.StockResults[i].Price);
                resultsData.StockResults[i].Price = Format.Numeric(resultsData.StockResults[i].Price);
                resultsData.StockResults[i].Km = Format.Numeric(resultsData.StockResults[i].Km);
                resultsData.StockResults[i].AbsureScore = GetAbsureRating(Convert.ToInt32(resultsData.StockResults[i].AbsureScore));
                resultsData.StockResults[i].MaskingNumber = String.Empty;
            }

            return resultsData;
        }

        private void UpdateInspectionText(List<StockBaseEntity> stockResultList)
        {
            foreach (var stockResult in stockResultList)
            {
                stockResult.InspectionText = String.Empty;
                stockResult.CertifiedLogoUrl = String.Empty;
                stockResult.HasWarranty = false;
            }
        }

        /// <summary>
        /// Created By : Sadhana Upadhyay on 28 May 2015
        /// Summary : filter count for IOS platform
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public FilterCountIos GetFilterResultIos(ElasticClient client, FilterInputs filterInputs)
        {
            ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
            Mapper.CreateMap<CountData, FilterCountIosBase>();

            var aggCounts = searchQuery.GetAggregationsCount(client, elasticInputs);
            aggCounts.SellerTypeCount.Individual = 0;   //To disable individual seller filter

            var filtersData = new FilterCountIos()
            {
                FiltersData = Mapper.Map<CountData, FilterCountIosBase>(aggCounts)
            };

            return filtersData;
        }

        /// <summary>
        /// Summary: call the Elastic query method for getting total stock count for all filters selected
        /// Author : Navead Kazi
        /// Date : 11/12/2015
        /// </summary>
        /// <param name="client"></param>
        /// <returns>total stock count</returns>
        public int GetTotalStockCount(ElasticClient client, FilterInputs filterInputs)
        {
            ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
            return searchQuery.GetTotalStockCount(client, elasticInputs);
        }

        public int GetStocksCountByField(ElasticClient client, FilterInputs filterInputs, string field, double fieldValue, bool greaterThanFieldValue)
        {
            ElasticOuptputs elasticInputs = processFilters.ProcessFilterParams(filterInputs);
            return searchQuery.GetStocksCountByField(client, elasticInputs, field, fieldValue, greaterThanFieldValue);
        }

            /// <summary>
            /// Summary: call the Elastic query method for getting total stock count for all filters selected
            /// Author : Navead Kazi
            /// Date : 11/12/2015
            /// </summary>
            /// <param name="client"></param>
            /// <returns>total stock count</returns>
            public List<CarMakeEntityBase> GetAllMakes(ElasticClient client)
        {
            return searchQuery.GetAllMakes(client);
        }

        public IEnumerable<StockBaseEntity> GetFrachiseCars(int size)
        {
            size = size > Constants.MaxLimitForFranchiseCars ? Constants.MaxLimitForFranchiseCars : size;
            string[] cities = ProcessCity(CustomerCookie.MasterCityId);
            return _esStockQueryRepository.GetFrachiseCars(cities, size);
        }

        private int GetPageSize(FilterInputs filters)
        {
            int pageSize = Int32.TryParse(filters.ps, out pageSize) ? pageSize : 20;
            return pageSize;
        }

        private static string[] ProcessCity(int city)
        {
            string[] cities;
            if (city == 1)
            {
                cities = ConfigurationManager.AppSettings["MumbaiAroundCityIds"].Split(',');
            }
            else if (city == 10)
            {
                cities = ConfigurationManager.AppSettings["DelhiNCRCityIds"].Split(',');
            }
            else if(city > 0)
            {
                cities = new string[] { city.ToString() };
            }
            else
            {
                cities = null;
            }
            return cities;
        }
    }   //End of class
}   //End of namespace
