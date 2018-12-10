using Carwale.BL.Dealers.Used;
using Carwale.BL.Stock;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation.LatLongURI;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Interfaces.Elastic;
using Carwale.Interfaces.Stock;
using Carwale.Notifications;
using Carwale.Utility;
using Carwale.Utility.Classified;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace Carwale.BL.Elastic.Used
{
    public class ESStockQuery : IESStockQuery
    {
        /// <summary>
        /// Class includes all ElasticSearch Query for Searching Results in Used Car Search Page
        /// Written By : Jugal on 03 Dec 2014
        /// </summary>
        private readonly IPager _pager;
        private readonly Random random;
        private readonly IProcessElasticJson elasticJson;
        private readonly IAggregationQueryDescriptor _aggregationQueryDescriptor;
        private readonly ISortDescriptorRepository _sortDescriptorRepository;
        private readonly IStockManager _stockManager;
        private const string deliveryText = "Delivery available in ";
        private static readonly double priceUpperLimitPercentage, priceLowerLimitPercentage, kmUpperLimit;
        private static readonly string[] _firstFeaturedSlotCities;
        private static readonly string delhiNCRCityIds = string.Empty, mumbaiAroundCityIds = string.Empty, carTradeCertificationId = string.Empty;
        private const int _size = 1000;
        private const int _defaultPageSize = 24;
        private const int _defaultPageNo = 1;
        private int _featuredSlotCount = 4;
        private const int _firstFeaturedSlotCount = 1;
        private const int _carsNearMeBucketRange = 8;
        static ESStockQuery()
        {
            double.TryParse(ConfigurationManager.AppSettings["PriceLowerLimitPercent"], out priceLowerLimitPercentage);
            double.TryParse(ConfigurationManager.AppSettings["PriceUpperLimitPercent"], out priceUpperLimitPercentage);
            double.TryParse(ConfigurationManager.AppSettings["KmUpperLimit"], out kmUpperLimit);
            delhiNCRCityIds = ConfigurationManager.AppSettings["DelhiNCRCityIds"];
            mumbaiAroundCityIds = ConfigurationManager.AppSettings["MumbaiAroundCityIds"];
            carTradeCertificationId = ConfigurationManager.AppSettings["CartradeCertificationId"];
            _firstFeaturedSlotCities = ConfigurationManager.AppSettings["UsedDiamondDealerPriorityCities"].Split(',');
        }

        public ESStockQuery(FilterInputs _elasticInputs, IProcessElasticJson _elasticJson, IPager pager
            , IAggregationQueryDescriptor aggregationQueryDescriptor
            , IStockManager stockManager
            , ISortDescriptorRepository sortDescriptorRepository)
        {
            elasticJson = _elasticJson;
            _pager = pager;
            random = new Random();
            _aggregationQueryDescriptor = aggregationQueryDescriptor;
            _stockManager = stockManager;
            _sortDescriptorRepository = sortDescriptorRepository;
        }

        /// <summary>
        /// Get Results Based on the Filters || Added By Jugal On 05 Dec 2014
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public Tuple<List<StockBaseEntity>, int, int, int, int> GetSearchResults(ElasticClient client, ElasticOuptputs filterInputs)
        {
            Tuple<List<StockBaseEntity>, int, int, int, int> listingData = null;
            try
            {
                string luxuryCarDealerId = ConfigurationManager.AppSettings["LuxuryCarDealerIds"];

                int ps = !String.IsNullOrEmpty(filterInputs.ps) ? Convert.ToInt32(filterInputs.ps) : 24;

                if (HttpContext.Current.Request.Cookies.AllKeys.Contains("_cwv") && !String.IsNullOrEmpty(HttpContext.Current.Request.Cookies["_cwv"].Value))
                {
                    HttpCookie sessionCookie = HttpContext.Current.Request.Cookies["_cwv"];
                    string[] sessionIdValue = sessionCookie.Value.Split('.');
                    if (sessionIdValue.Length > 1)
                        filterInputs.sessionId = sessionIdValue[2];
                }
                if (string.IsNullOrEmpty(filterInputs.sessionId))
                    filterInputs.sessionId = DateTime.Now.ToString("yyyyddMMhh");

                IMultiSearchResponse results = null;
                if (filterInputs.multiCityName == null && filterInputs.cities != null && filterInputs.cities.Length > 1)
                {
                    int noOfNBCities = filterInputs.cities.Length;
                    string[] nbCities = filterInputs.cities;
                    for (int i = 0; i < noOfNBCities; i++)
                    {
                        if (nbCities != null)
                            filterInputs.cities = new String[] { nbCities[i] };
                        results = GetSearchPageResults(client, luxuryCarDealerId, filterInputs);
                        List<SearchResponse<StockBaseEntity>> onlyResults = new List<SearchResponse<StockBaseEntity>>(results.GetResponses<StockBaseEntity>());
                        listingData = elasticJson.TakeTopResults(onlyResults, filterInputs, _featuredSlotCount, ps, false, _firstFeaturedSlotCount);
                        if (listingData.Item1.Count > 0 && nbCities != null)
                            listingData.Item1[listingData.Item1.Count - 1].NBCityStripId = nbCities[i];

                    }
                }
                else
                {
                    results = GetSearchPageResults(client, luxuryCarDealerId, filterInputs);
                    List<SearchResponse<StockBaseEntity>> onlyResults = new List<SearchResponse<StockBaseEntity>>(results.GetResponses<StockBaseEntity>());
                    listingData = elasticJson.TakeTopResults(onlyResults, filterInputs, _featuredSlotCount, ps,false, _firstFeaturedSlotCount);
                    if (listingData.Item1.Count > 0 && filterInputs.cities != null)
                        listingData.Item1[listingData.Item1.Count - 1].NBCityStripId = filterInputs.cities[0];

                }
                SetRecommendationAndDealerCarsUrl(listingData?.Item1);
            }
            catch (SqlException ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetSearchResults()");
                objErr.LogException();
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetSearchResults()");
                objErr.LogException();
            }

            return listingData;
        }

        /// <summary>
        /// Returns the stocks from current and next nearby cities(in order of stock count)
        /// Returns last rank of non-premium, dealer premium, individual premium cars.
        /// Returns flag to indicate whether more cars are available or not.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [Obsolete("Not used Anymore")]
        public StockBaseMobile GetSearchResultsMobile(ElasticClient client, ElasticOuptputs filterInputs)
        {
            Tuple<List<StockBaseEntity>, int, int, int, int> listingData = null;
            StockBaseMobile stockBaseMobile = new StockBaseMobile();
            List<StockBaseEntity> finalResult = new List<StockBaseEntity>(); //To add result listing from main city+ from nearby cities

            try
            {
                string luxuryCarDealerId = ConfigurationManager.AppSettings["LuxuryCarDealerIds"];

                int pageSize = !String.IsNullOrEmpty(filterInputs.ps) ? Convert.ToInt32(filterInputs.ps) : 24;

                if (HttpContext.Current.Request.Cookies.AllKeys.Contains("_cwv") && !String.IsNullOrEmpty(HttpContext.Current.Request.Cookies["_cwv"].Value))
                {
                    HttpCookie sessionCookie = HttpContext.Current.Request.Cookies["_cwv"];
                    string[] sessionIdValue = sessionCookie.Value.Split('.');
                    if (sessionIdValue.Length > 1)
                        filterInputs.sessionId = sessionIdValue[2];
                }
                if (string.IsNullOrEmpty(filterInputs.sessionId))
                    filterInputs.sessionId = DateTime.Now.ToString("yyyyddMMhh");

                IMultiSearchResponse results = null;
                string[] citiesOriginal = filterInputs.cities;

                bool isRequestFromNearbyCity = false;
                //If client already has nearbyCityId, fetch results from that city.
                if (filterInputs.nearbyCityId != 0)
                {
                    stockBaseMobile.TotalStockCount = GetTotalStockCount(client, filterInputs);
                    filterInputs.cities = new[] { filterInputs.nearbyCityId.ToString() };
                    isRequestFromNearbyCity = true;
                }

                int stockCountFetched = 0;
                bool isNearbyCityMessageRequired = false;
                int loopCounter = 0;
                int currentNearbyCityStockCount = 0;
                stockBaseMobile.NearbyCityId = filterInputs.nearbyCityId;
                stockBaseMobile.NearbyCityIds = filterInputs.nearbyCityIds;
                stockBaseMobile.NearbyCityIdsStockCount = filterInputs.nearbyCityIdsStockCount;

                while (stockCountFetched < pageSize)
                {
                    if (loopCounter > 24)//[loopCounter > 24 (Max ps)] check to avoid any unexpected infinite while loop case. so that loop terminates after executing max. 24 times. 
                    {
                        break;
                    }
                    results = GetSearchPageResults(client, luxuryCarDealerId, filterInputs);
                    List<SearchResponse<StockBaseEntity>> onlyResults = new List<SearchResponse<StockBaseEntity>>(results.GetResponses<StockBaseEntity>());
                    listingData = elasticJson.TakeTopResults(onlyResults, filterInputs, _featuredSlotCount, pageSize, isRequestFromNearbyCity, _firstFeaturedSlotCount);
                    var currentCityListings = listingData.Item1;
                    if (!isRequestFromNearbyCity)
                    {
                        stockBaseMobile.TotalStockCount = listingData.Item5;
                    }
                    SetNearbyCityText(isNearbyCityMessageRequired, currentCityListings, currentNearbyCityStockCount, isRequestFromNearbyCity);
                    finalResult.AddRange(currentCityListings);
                    stockCountFetched += listingData.Item1.Count;

                    if (stockCountFetched < pageSize) //If with current city query, we were not able to fetch listings equal to page size, then try fetching from nearby cities.
                    {
                        Tuple<int, int> nearbyCityIdAndCount = new Tuple<int, int>(0, 0);
                        //Get nearby city id or next city id to fetch nearby city listings (NOT for mumbai-all, delhi ncr, all india case)
                        if (filterInputs.cities != null && filterInputs.cities.Length > 0 && filterInputs.cities[0] != Constants.AllIndiaCityId && filterInputs.multiCityId == 0)
                        {
                            //returns city id of first nearby city OR next nearby cities. nextCityId to be used to fetch further listings.
                            nearbyCityIdAndCount = GetNextCityIdAndStockCount(client, filterInputs, filterInputs.nearbyCityId, stockBaseMobile);
                        }
                        filterInputs.nearbyCityId = nearbyCityIdAndCount.Item1; //set the city in filterInputs so that elastic query will return result for this city in next loop request.
                        stockBaseMobile.NearbyCityId = nearbyCityIdAndCount.Item1; //set nearbyCityId to be sent to client so that server knows last stock cityId                      

                        if (nearbyCityIdAndCount.Item1 == 0) //if no nextCityId, break.
                        {
                            stockBaseMobile.IsAllStocksFetched = true;
                            break;
                        }
                        else
                        {
                            //If we have nextCityId, reset the filterInputs parameters.
                            ResetStockParameters(ref pageSize, ref stockCountFetched, ref isNearbyCityMessageRequired, ref currentNearbyCityStockCount, nearbyCityIdAndCount, nearbyCityIdAndCount.Item1, filterInputs);
                            isRequestFromNearbyCity = true;
                        }
                    }
                    loopCounter++;
                }

                filterInputs.cities = citiesOriginal;

                stockBaseMobile.ListStocks = finalResult;
                stockBaseMobile.LastNonFeaturedRank = filterInputs.lcr + listingData.Item2; // rank of last non premium car
                stockBaseMobile.LastDealerFeaturedRank = filterInputs.ldr + listingData.Item3;// rank of last dealer premium car
                stockBaseMobile.LastIndividualFeaturedRank = filterInputs.lir + listingData.Item4;// rank of last individual premium car


                if (stockBaseMobile.ListStocks.Count < Convert.ToInt32(filterInputs.ps)) //If we are unable to fetch listings = page size, then we don't have more listings.
                {
                    stockBaseMobile.IsAllStocksFetched = true; //This would be used by client to not send further requests as listings have ended.
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetSearchResultsMobile()");
                objErr.LogException();
            }
            return stockBaseMobile;
        }

        private void SetRecommendationAndDealerCarsUrl(ICollection<StockBaseEntity> stockList)
        {
            if (stockList != null)
            {
                foreach (var item in stockList)
                {
                    if (item.CwBasePackageId == CwBasePackageId.Franchise)
                    {
                        item.DealerCarsUrl = UsedDealerStocksBL.GetDealerOtherCarsUrl(item.SellerName,item.DealerId);
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


        //Moved to StockSearchLogic
        /// <summary>
        /// This will reset the stock rank params i.e. lcr-next non premium car to be fetched, ldr-for dealer, lir- for individual
        /// page size: ps would be reduced based on number of listings already fetched. To mantain consistency across pages i.e. each page should have number of listings = ps
        /// Since page size has been reduced, resetting the totalStockCountFetched. 
        /// 
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="stockCountFetched"></param>
        /// <param name="isNearbyCityMessageRequired"></param>
        /// <param name="nextCityId"></param>
        private void ResetStockParameters(ref int ps, ref int stockCountFetched, ref bool isNearbyCityMessageRequired, ref int currentNearbyCityStockCount, Tuple<int, int> nearbyCityIdAndCount, int nextCityId, ElasticOuptputs filterInputs)
        {
            filterInputs.lcr = 0;
            filterInputs.ldr = 0;
            ps = ((string.IsNullOrEmpty(filterInputs.ps)) ? _defaultPageSize : Convert.ToInt32(filterInputs.ps)) - stockCountFetched;
            stockCountFetched = 0;
            filterInputs.cities = new[] { nextCityId.ToString() };
            isNearbyCityMessageRequired = true;
            currentNearbyCityStockCount = nearbyCityIdAndCount.Item2;
            if (ps < _featuredSlotCount)
            {
                _featuredSlotCount = ps;
            }
        }

        //Moved to StockSearchLogic
        private static void SetNearbyCityText(bool isNearbyCityMessageRequired, List<StockBaseEntity> currentCityListings, int currentNearbyCityStockCount, bool isRequestFromNearbyCity)
        {
            string nearbyCityName = string.Empty;
            if (isNearbyCityMessageRequired && currentCityListings != null && currentCityListings.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(currentCityListings[0].DeliveryText))
                {
                    nearbyCityName = currentCityListings[0].DeliveryText.Substring(deliveryText.Length);
                }
                else
                {
                    nearbyCityName = currentCityListings[0].CityName;
                }

                string nearbyCityDisplayText = $"More Cars from { nearbyCityName } ({ currentNearbyCityStockCount }";
                currentCityListings[0].NearbyCityText = currentNearbyCityStockCount == 1 ? $"{ nearbyCityDisplayText } Car)" : $"{ nearbyCityDisplayText } Cars)";
                currentCityListings.Select(currentCityListing => { currentCityListing.IsNearbyCityListing = true; return currentCityListing; }).ToList();
            }

            if (isRequestFromNearbyCity && currentCityListings != null && currentCityListings.Count > 0)
            {
                currentCityListings.Select(currentCityListing => { currentCityListing.IsNearbyCityListing = true; return currentCityListing; }).ToList();
            }
        }

        //Moved to stock manager
        /// <summary>
        /// Returns first nearby city if only primary city cars have loaded OR next nearby city from nearby city list.
        /// Returns zero if no nearby city or all nearby cities have been returned already.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cities"></param>
        /// <param name="nearbyCity"></param>
        /// <param name="nearbyCities"></param>
        /// <param name="stockBaseMobile"></param>
        /// <returns></returns>
        private Tuple<int, int> GetNextCityIdAndStockCount(ElasticClient client, ElasticOuptputs filterInputs, int nearbyCity, StockBaseMobile stockBaseMobile)
        {
            List<string> lstNearbyCityIds;
            List<string> lstNearbyCityCounts;
            if (string.IsNullOrWhiteSpace(filterInputs.nearbyCityIds)) //How to differentiate b/w length 0 and nearby city not yet fetched
            {
                List<City> lstNearbyCities = GetNearbyCities(client, string.Empty, filterInputs);
                lstNearbyCityIds = lstNearbyCities.Select(nc => nc.CityId.ToString()).ToList();
                lstNearbyCityCounts = lstNearbyCities.Select(nc => nc.CityCount.ToString()).ToList();
                filterInputs.nearbyCityIds = string.Join(",", lstNearbyCityIds);
                filterInputs.nearbyCityIdsStockCount = string.Join(",", lstNearbyCityCounts);
                stockBaseMobile.NearbyCityIds = filterInputs.nearbyCityIds;
                stockBaseMobile.NearbyCityIdsStockCount = filterInputs.nearbyCityIdsStockCount;
                if (lstNearbyCities == null || lstNearbyCities.Count == 0)
                {
                    filterInputs.nearbyCityIds = string.Empty;
                    return new Tuple<int, int>(0, 0);
                }
                else
                {
                    return new Tuple<int, int>(CustomParser.parseIntObject(lstNearbyCityIds[0].Trim()), CustomParser.parseIntObject(lstNearbyCityCounts[0].Trim()));
                }
            }
            else
            {
                lstNearbyCityIds = filterInputs.nearbyCityIds.Trim().Split(',').ToList();
                lstNearbyCityCounts = filterInputs.nearbyCityIdsStockCount.Trim().Split(',').ToList();
                for (int i = 0; i < lstNearbyCityIds.Count - 1; i++)
                {
                    if (lstNearbyCityIds[i] == nearbyCity.ToString().Trim())
                    {
                        return new Tuple<int, int>(CustomParser.parseIntObject(lstNearbyCityIds[i + 1].Trim()), CustomParser.parseIntObject(lstNearbyCityCounts[i + 1].Trim()));
                    }
                }
            }
            return new Tuple<int, int>(0, 0);
        }
        private IMultiSearchResponse GetSearchPageResults(ElasticClient client, string luxuryCarDealerId, ElasticOuptputs filterInputs)
        {
            int randNum = random.Next(100000);
            string sortField = GetListingOrderByClause(filterInputs.sc);
            int ps = !string.IsNullOrEmpty(filterInputs.ps) ? Convert.ToInt32(filterInputs.ps) : _defaultPageSize;
            var results = GetSearchPageQueryResults(client, sortField, ps, filterInputs, randNum);
            return results;
        }

        private IMultiSearchResponse GetSearchPageQueryResults(ElasticClient client, string sortField, int ps, ElasticOuptputs filterInputs, int randNum)
        {
            var results = client.MultiSearch(a => a
               //Take 20 results sorted by SortScore
               .Search<StockBaseEntity>(ss =>
               {
                  return ss
                       .Type("stock")
                       .Index(Constants.ClassifiedElasticIndex)
                       .Query(qq => GetQueryContainerForSearchPage(filterInputs, filterInputs.carsWithPhotos, qq))
                       .Sort(so => _sortDescriptorRepository.GetSortDescriptorForNonFeaturedStocks(filterInputs, sortField, so))
                       .From(filterInputs.lcr).Take(ps);
               })
               //query to get featured dealer listing
               .Search<StockBaseEntity>(s =>
               {
                   return IsFeaturedSlotAvailable(filterInputs.pn) ? s.Type("stock")
                                                                       .Index(Constants.ClassifiedElasticIndex)
                                                                       .Query(query => CommonQueryForSearchPage(filterInputs, query) &&
                                                                           query.Term("sellerType", "1") && query.Term("isPremium", "1")
                                                                       )
                                                                       .Aggregations(agg => _aggregationQueryDescriptor.GetAggregationContainerForFeaturedStocks(randNum, _featuredSlotCount, agg, filterInputs.Latitude, filterInputs.Longitude, filterInputs.ShouldFetchNearbyCars))
                                                                       .From(0)
                                                                       .Take(0)
                                                                    : null;
               })
               //query to get diamond dealer listing
               .Search<StockBaseEntity>(s =>
               {
                   return IsFranchiseOrDiamondSlotAvailable(filterInputs) ? s.Type("stock")
                                                                   .Index(Constants.ClassifiedElasticIndex)
                                                                  .Query(query => CommonQueryForSearchPage(filterInputs, query) &&
                                                                      (query.Term("packageType", Constants.DiamondDealerPackageType) || 
                                                                      query.Terms(tqd => tqd.Field("cwBasePackageId").Terms(CwBasePackageId.Franchise, CwBasePackageId.Diamond)))
                                                                  )
                                                                  .Aggregations(agg => _aggregationQueryDescriptor.GetAggregationContainerDescriptorForFirstSlot(randNum, _firstFeaturedSlotCount, agg))
                                                                  .From(0)
                                                                  .Take(0)
                                                              : null;
               }));
            return results;
        }

        public StockBaseEntity GetStockByProfileId(ElasticClient client, string profileId)
        {
            StockBaseEntity stock = null;

            try
            {
                var r = client.Search<StockBaseEntity>(s => s
                    .Index(Constants.ClassifiedElasticIndex)
                    .Type("stock")
                    .Size(1)
                    .Query(q => q
                        .Match(m => m.Field("profileId").Query(profileId))
                    )
                );

                if (r.HitsMetaData.Hits.Count > 0)
                {
                    stock = r.Documents.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetStockByProfileId()");
                objErr.LogException();
            }
            return stock;
        }

        public int GetStocksCountByField(ElasticClient client, ElasticOuptputs filterInputs, string field, double fieldValue, bool greaterThanFieldValue)
        {

            var results = client
                .Search<StockBaseEntity>(ss =>
                {
                    ss = ss.Type("stock");
                    ss = ss.Index(Constants.ClassifiedElasticIndex)
                        .Query(qq => GetQueryContainerForSearchPage(filterInputs, filterInputs.carsWithPhotos, qq))
                        .Aggregations(a => a
                            .Filter("sort_score_filter_aggs", f => f
                                .Filter(fd => fd
                                    .Range(r =>
                                    {
                                        return greaterThanFieldValue
                                                ? r.GreaterThan(fieldValue)
                                                    .Field(field)
                                                : r.LessThan(fieldValue)
                                                    .Field(field);
                                    })
                                )
                                .Aggregations(agg => agg
                                    .ValueCount("rank_count", count => count
                                        .Field(field)
                                    )
                                )
                            )
                        )
                        .From(filterInputs.lcr)
                        .Take(0);
                    return ss;
                });

            return Convert.ToInt32(results.Aggs.Filter("sort_score_filter_aggs").ValueCount("rank_count").Value);
        }

        private static bool IsFeaturedSlotAvailable(int pageNum)
        {
            return pageNum == _defaultPageNo;
        }

        private bool IsFranchiseOrDiamondSlotAvailable(ElasticOuptputs filterInputs)
        {
            return filterInputs.pn == _defaultPageNo && filterInputs.cities != null 
                && (_firstFeaturedSlotCities.Any(str => str == filterInputs.cities[0]) || random.Next(10) < 7); 
        }

        private QueryContainer CommonQueryForSearchPage(ElasticOuptputs filterInputs, QueryContainerDescriptor<StockBaseEntity> query)
        {
            return query
                .FunctionScore(fun => fun
                   .Functions(functions => functions
                      .ScriptScore(ss => ss
                         .Script(scr => scr
                            .Inline("if (doc['sortScore'].value > 0.3) {5 + Math.random()} else {Math.random()}")
                            .Lang("painless")
                         )
                      )
                   )
                   .Query(qq => GetQueryContainerForSearchPage(filterInputs, Constants.C_getCarWithPhotos, qq))
                );
        }

        private QueryContainer GetQueryContainerForSearchPage(ElasticOuptputs filterInputs, string carsWithPhotos, QueryContainerDescriptor<StockBaseEntity> queryContainerDescriptor)
        {
            double photoCount;
            double.TryParse(carsWithPhotos, out photoCount);
            return queryContainerDescriptor.Bool(b => b
               .Filter(ff => ff
                  .Bool(bb => bb
                     .Must(mm =>
                       {
                           QueryContainer bf =
                               mm.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                           if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                               bf |= mm.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                           else
                               bf &= mm.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                           if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                               bf &= !mm.Term("packageType", Constants.DiamondDealerPackageType); // diamond dealer packages are excluded for all india
                           else
                               bf &= mm.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                           QueryContainer sellerQc = mm.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                           if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                           {
                               sellerQc &= !mm.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                               sellerQc |= mm.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                           }


                           bf &= sellerQc && mm.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                               mm.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                               mm.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                               mm.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                               mm.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                               mm.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                   .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                               mm.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                   .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                               mm.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                   .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));

                           if (filterInputs.IsCarTradeCertifiedCars)
                           {
                               bf &= mm.Term("certificationId", carTradeCertificationId);
                           }
                            
                            if(filterInputs.IsFranchiseCars)
                            {
                                bf &= mm.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                            }

                           bf &= mm.Range(y => y.Field("photoCount").GreaterThanOrEquals(photoCount));
                           return bf;
                       })
                  )
               ));
        }

        public List<StockBaseEntity> GetRecommendationResults(ElasticClient client, ElasticOuptputs filterInputs)
        {
            List<StockBaseEntity> lstRecommendations = new List<StockBaseEntity>();
            List<StockBaseEntity> lstRecommendationsAll = new List<StockBaseEntity>();

            try
            {
                filterInputs.kmMin = "0";

                double budgetMin, budgetMax, kmMax;
                if (double.TryParse(filterInputs.budgetMin, out budgetMin) &&
                    double.TryParse(filterInputs.budgetMax, out budgetMax) &&
                    double.TryParse(filterInputs.kmMax, out kmMax))
                {
                    double priceKmRangePercent = 0; //To increase the price/km limit to fetch recommended results till no recommendation is found or checked max 5 times.
                    do
                    {
                        long budgetMinRounded = Convert.ToInt64(budgetMin * (priceLowerLimitPercentage - priceKmRangePercent));
                        long budgetMaxRounded = Convert.ToInt64(budgetMax * (priceUpperLimitPercentage + priceKmRangePercent));
                        long kmMxRounded = Convert.ToInt64(kmMax + (kmUpperLimit + (priceKmRangePercent * 200000)));

                        filterInputs.budgetMin = budgetMinRounded.ToString();
                        filterInputs.budgetMax = budgetMaxRounded.ToString();
                        filterInputs.kmMax = kmMxRounded.ToString();

                        var results = client
                            .Search<StockBaseEntity>(s => s
                            .Type("stock")
                           .Index(Constants.ClassifiedElasticIndex)
                            .Take(1000)

                            .Query(q => q
                                .Bool(bl => bl
                                   .Filter(f => f
                                      .Bool(bb => bb
                                 .Must(ms =>
                                   {
                                       QueryContainer filterContainer = null;
                                       filterContainer &=
                                        ms.Term("versionSubSegmentID", filterInputs.subSegmentID) &&
                                       ms.Terms(terms => terms.Field(field => field.CityId).Terms<string>(filterInputs.cities)) &&
                                         ms.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                         !ms.Term("profileId", filterInputs.profileId) &&
                                          ms.Range(y => y.Field(new Field("photoCount")).GreaterThanOrEquals(Constants.C_getCarWithPhotos == "" ? 0 : Convert.ToDouble(Constants.C_getCarWithPhotos))) &&
                                         ms.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                           .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                         ms.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                           .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));

                                       if (!String.IsNullOrEmpty(filterInputs.profileId))
                                       {
                                           filterContainer &=
                                                  (!ms.Term("profileId", filterInputs.profileId.ToLower()) &&
                                                   !ms.Term("profileId", filterInputs.profileId.ToUpper()));
                                       }
                                       return filterContainer;
                                   })))))

                            .Sort(st => st
                                             .Script(sc => sc
                                             .Type("number")
                                             .Descending()
                                             .Script(script => script
                                                 .Inline("signum(doc['svScore'].value)").Lang("groovy")
                                                   )))
                            .Sort(ss => ss.Descending(p => p.SortScore))


                        );

                        lstRecommendationsAll.AddRange(results.Documents);
                        priceKmRangePercent = priceKmRangePercent + 0.1;
                    } while (lstRecommendationsAll.Count == 0 && priceKmRangePercent <= 0.5);

                    int count = AddSameRootRecommendations(lstRecommendations, lstRecommendationsAll, filterInputs);

                    count = AddSameSubSegmentRecommendations(lstRecommendations, lstRecommendationsAll, count);
                }
            }
            catch (SqlException ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetRecommendationResults()");
                objErr.LogException();
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetRecommendationResults()");
                objErr.LogException();
            }
            return lstRecommendations;
        }

        public List<StockBaseEntity> GetRecommendationsForProfileId<TInput>(ElasticClient client, TInput tInput, int recommendationsCount)
        {
            List<StockBaseEntity> lstRecommendations = null;
            List<StockBaseEntity> lstRecommendationsAll = null;
            double priceKmRangePercent = 0;

            try
            {
                var r = client.Search<StockRecommendationsProfileId>(s => s
                    //TODO
                    .Index(Constants.ClassifiedElasticIndex)
                    .Type("stock")
                    .Take(1)
                    .Source(sr => sr
                    .Includes(fi => fi
                        .Field(f => f.Kilometers)
                        .Field(f => f.Price)
                        .Field(f => f.CityIds)
                        .Field(f => f.BodyStyleId)
                        .Field(f => f.ProfileId)
                        .Field(f => f.VersionSubSegmentID)
                        .Field(f => f.RootId)
                        .Field(f => f.MakeId)
                    ))
                    .Query(q => q
                    .Term(p => p.ProfileId, tInput)));


                double budgetMin = 0, budgetMax = 0, kmMax = 0;

                if (r.HitsMetaData.Hits.Count > 0)
                {
                    lstRecommendations = new List<StockBaseEntity>();
                    lstRecommendationsAll = new List<StockBaseEntity>();
                    StockRecommendationsProfileId p = new StockRecommendationsProfileId();

                    foreach (var x in r.Hits)
                    {
                        p.VersionSubSegmentID = x.Source.VersionSubSegmentID;
                        p.CityIds = x.Source.CityIds;
                        p.BodyStyleId = x.Source.BodyStyleId;
                        p.ProfileId = x.Source.ProfileId;
                        p.KmMin = 0;
                        p.RootId = x.Source.RootId;
                        if (double.TryParse(x.Source.Price, out budgetMin)) { p.BudgetMin = budgetMin; }
                        if (double.TryParse(x.Source.Price, out budgetMax)) { p.BudgetMax = budgetMax; }
                        if (double.TryParse(x.Source.Kilometers, out kmMax)) { p.KmMax = kmMax; }

                    }
                    do
                    {
                        foreach (var l in r.Hits)
                        {
                            p.BudgetMin = budgetMin * (priceLowerLimitPercentage - priceKmRangePercent);

                            p.BudgetMax = budgetMax * (priceUpperLimitPercentage + priceKmRangePercent);
                            p.KmMax = kmMax + kmUpperLimit + (priceKmRangePercent * 200000);
                        }


                        var finalResult = client
                             .Search<StockBaseEntity>(s => s
                             .Type("stock")
                             .Index(Constants.ClassifiedElasticIndex)
                             .Take(1000)
                             .Query(q => q
                                 .Bool(b => b
                             .Filter(f => f
                             .Bool(bb => bb
                             .Must(ms =>
                             {
                                 QueryContainer queryContainer = null;
                                 queryContainer &=
                                    ms.Term("versionSubSegmentID", p.VersionSubSegmentID) &&
                                    ms.Terms(terms => terms.Field(field => field.CityId).Terms<string>(p.CityIds)) &&
                                    ms.Terms(terms => terms.Field("bodyStyleId").Terms<string>(new string[] { p.BodyStyleId })) &&
                                 !ms.Term("profileId", p.ProfileId) &&
                                    ms.Range(y => y.Field(new Field("photoCount")).GreaterThanOrEquals(Constants.C_getCarWithPhotos == "" ? 0 : Convert.ToDouble(Constants.C_getCarWithPhotos))) &&
                                    ms.Range(ra => ra.Field("price").GreaterThanOrEquals(p.BudgetMin).LessThanOrEquals(p.BudgetMax)) &&
                                    ms.Range(k => k.Field("kilometers").GreaterThanOrEquals(p.KmMin).LessThanOrEquals(p.KmMax));

                                 if (!String.IsNullOrEmpty(p.ProfileId))
                                 {
                                     queryContainer &=
                                            (!ms.Term("profileId", p.ProfileId.ToLower()) &&
                                             !ms.Term("profileId", p.ProfileId.ToUpper()));
                                 }
                                 return queryContainer;
                             })))))

                                .Sort(st => st
                                         .Script(sc => sc
                                         .Type("number")
                                         .Descending()
                                         .Script(script => script
                                             .Inline("signum(doc['svScore'].value)").Lang("groovy")
                                               )))
                                .Sort(ss => ss.Descending(pp => pp.SortScore))

                                );

                        lstRecommendationsAll.AddRange(finalResult.Documents);
                        priceKmRangePercent = priceKmRangePercent + 0.1;
                    } while (lstRecommendationsAll.Count == 0 && priceKmRangePercent <= 0.5);

                    int count = AddSameRootRecommendationsForProfile(lstRecommendations, lstRecommendationsAll, p, recommendationsCount);

                    count = AddSameSubSegmentRecommendations(lstRecommendations, lstRecommendationsAll, count);
                }
            }
            catch (SqlException ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetRecommendationsForProfileId()");
                objErr.LogException();
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetRecommendationsForProfileId()");
                objErr.LogException();
            }
            return lstRecommendations;
        }
        /// <summary>
        /// Add the same sub segment recommendation if 6 not same root found. Added in sort score order(highest first)
        /// </summary>
        /// <param name="lstRecommendations"></param>
        /// <param name="lstRecommendationsAll"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static int AddSameSubSegmentRecommendations(List<StockBaseEntity> lstRecommendations, List<StockBaseEntity> lstRecommendationsAll, int count)
        {
            for (int i = 0; i < lstRecommendationsAll.Count && count > 0; i++)
            {
                lstRecommendations.Add(lstRecommendationsAll[i]);
                count--;
            }
            return count;
        }

        /// <summary>
        /// Add recommendations of same root as of car. Ordered by sort score.
        /// Removing elements to eliminate duplicates when we will pick same segment cars in the next step.
        /// </summary>
        /// <param name="lstRecommendations"></param>
        /// <param name="lstRecommendationsAll"></param>
        /// <returns></returns>
        private int AddSameRootRecommendations(List<StockBaseEntity> lstRecommendations, List<StockBaseEntity> lstRecommendationsAll, ElasticOuptputs filterInputs)
        {
            int count = 25;

            for (int i = 0; i < lstRecommendationsAll.Count && count > 0; i++)
            {
                if (filterInputs.NewRoots != null && lstRecommendationsAll[i].RootId == filterInputs.NewRoots[0])
                {
                    lstRecommendations.Add(lstRecommendationsAll[i]);
                    lstRecommendationsAll.RemoveAt(i);
                    count--;
                    i--;
                }
            }

            return count;
        }

        /// <summary>
        /// Add recommendations of same root as of car. Ordered by sort score.
        /// Removing elements to eliminate duplicates when we will pick same segment cars in the next step.
        /// </summary>
        /// <param name="lstRecommendations"></param>
        /// <param name="lstRecommendationsAll"></param>
        /// <param name="RecommendationsForProfileId"></param>
        /// <returns></returns>
        public int AddSameRootRecommendationsForProfile(List<StockBaseEntity> lstRecommendations, List<StockBaseEntity> lstRecommendationsAll, StockRecommendationsProfileId p, int count)
        {
            for (int i = 0; i < lstRecommendationsAll.Count && count > 0; i++)
            {
                if (p.RootId != null && lstRecommendationsAll[i].RootId == p.RootId)
                {
                    lstRecommendations.Add(lstRecommendationsAll[i]);
                    lstRecommendationsAll.RemoveAt(i);
                    count--;
                    i--;
                }
            }
            return count;
        }

        /// <summary>
        /// Get Total stock count of all filters applied from elatic search
        /// Author: Navead Kazi
        /// Date: 11/12/2015
        /// </summary>
        /// <param name="client">Elastic Client</param>
        /// <returns>Total count</returns>
        public int GetTotalStockCount(ElasticClient client, ElasticOuptputs filterInputs)
        {
            ISearchResponse<StockBaseEntity> searchResponse;
            searchResponse = client.Search<StockBaseEntity>(ss => ss
            .Type("stock")
                .Index(Constants.ClassifiedElasticIndex)
                .Aggregations(aa => aa
                              .Filter("TotalStockCount", e => e
                                  .Filter(f => f
                                      .Bool(m => m
                                          .Must(n =>
                                          {
                                              QueryContainer objFc = n.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));
                                              if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                  objFc |= n.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                              else
                                                  objFc &= n.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                              if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                  objFc &= !n.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                              else
                                                  objFc &= n.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                              QueryContainer sellerQc = n.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                              if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                              {
                                                  sellerQc &= !n.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                  sellerQc |= n.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                              }

                                              objFc &= sellerQc && n.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                      n.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                      n.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                      n.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                      n.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                       n.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                n.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                n.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));



                                              if (filterInputs.IsCarTradeCertifiedCars)
                                              {
                                                  objFc &= n.Term("certificationId", carTradeCertificationId);
                                              }

                                              if (filterInputs.IsFranchiseCars)
                                              {
                                                  objFc &= n.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                              }

                                              objFc &= n.Range(y => y.Field(new Field("photoCount")).GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));
                                              return objFc;
                                          })
                                           )
                                           )
                                   )
                                   )

                );
            return Convert.ToInt32(searchResponse.Aggs.Filter("TotalStockCount").DocCount);

        }

        /// <summary>
        /// Get Aggregations Count based on input query || Added By Jugal on 05 Dec 2014
        /// </summary>
        /// <param name="ElasticClient"></param>
        /// <returns></returns>
        public CountData GetAggregationsCount(ElasticClient client, ElasticOuptputs filterInputs)
        {
            ISearchResponse<StockBaseEntity> searchResponse;
            int fromSize = filterInputs.lcr;

            searchResponse = client.Search<StockBaseEntity>(ss => ss
                                  //.Index("livelisting")
                                  .Type("stock")
                                  .Index(Constants.ClassifiedElasticIndex)
                            .From(fromSize)
                            .Take(0)

                           //Aggregations for All available filters
                           .Aggregations(aa => aa
                              .Filter("TotalStockCount", e => e
                                  .Filter(f => f
                                      .Bool(m => m
                                          .Must(n =>
                                          {
                                              QueryContainer objFc = n.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));
                                              if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                  objFc |= n.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                              else
                                                  objFc &= n.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                              if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                  objFc &= !n.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                              else
                                                  objFc &= n.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                              QueryContainer sellerQc = n.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                              if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                              {
                                                  sellerQc &= !n.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                  sellerQc |= n.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                              }

                                              objFc &= sellerQc && n.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                      n.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                      n.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                      n.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                      n.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                       n.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                n.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                n.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));



                                              if (filterInputs.IsCarTradeCertifiedCars)
                                              {
                                                  objFc &= n.Term("certificationId", carTradeCertificationId);
                                              }

                                              if (filterInputs.IsFranchiseCars)
                                              {
                                                  objFc &= n.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                              }

                                              objFc &= n.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));
                                              return objFc;
                                          })
                                           )
                                           )
                                   )

                              .Filter("CityCount", e => e
                                  .Filter(f => f
                                      .Bool(m => m
                                          .Must(n =>
                                          {
                                              QueryContainer objFc =
                                              n.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                              if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                  objFc |= n.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                              else
                                                  objFc &= n.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));

                                              QueryContainer sellerQc = n.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                              if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                              {
                                                  sellerQc &= !n.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                  sellerQc |= n.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                              }

                                              objFc &= sellerQc && n.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                              n.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                              n.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                              n.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                              n.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                               n.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                n.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                n.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));



                                              if (filterInputs.IsCarTradeCertifiedCars)
                                              {
                                                  objFc &= n.Term("certificationId", carTradeCertificationId);
                                              }

                                              if (filterInputs.IsFranchiseCars)
                                              {
                                                  objFc &= n.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                              }

                                              objFc &= n.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                              return objFc;
                                          })
                                           )
                                           )

                                          .Aggregations(g => g
                                           .Terms("CityIdCount", h => h.Size(_size).Field("citiesMapping.keyword"))
                                           )
                                   )

                                .Filter("FuelTypeCount", ff => ff
                                  .Filter(c => c
                                      .Bool(bb => bb
                                          .Must(q =>
                                          {
                                              QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                              if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                  objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                              else
                                                  objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                              if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                  objFc &= !q.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                              else
                                                  objFc &= q.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                              QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                              if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                              {
                                                  sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                  sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                              }

                                              objFc &= sellerQc && q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                      q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                      q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                      q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                       q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));




                                              if (filterInputs.IsCarTradeCertifiedCars)
                                              {
                                                  objFc &= q.Term("certificationId", carTradeCertificationId);
                                              }

                                              if (filterInputs.IsFranchiseCars)
                                              {
                                                  objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                              }

                                              objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                              return objFc;
                                          })
                                           )
                                  )
                                     .Aggregations(d => d
                                         .Terms("FuelIdCount", t => t.Size(_size).Field("fuelType"))
                                          )
                                  )

                                  .Filter("MakeCount", i => i
                                      .Filter(j => j
                                          .Bool(bb => bb
                                              //dont use Must because we need Count of all Make
                                              .Should(q =>
                                              {
                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  QueryContainer objFc = sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                      q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                      q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                      q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                      q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                       q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));

                                                  if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                      objFc &= !q.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                  return objFc;

                                              })
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                      .Aggregations(k => k
                                      .Terms("MakeIdCount", l => l.Size(_size).Field("makeMapping.keyword")
                                         .Aggregations(kk => kk
                                             .Filter("Root_Count", f => f
                                                 .Filter(ff => ff
                                                     .Bool(bb => bb
                                                         .Should(q => q
                                                         .Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.cars))
                                                             )
                                                             .Must(mm => mm.MatchAll())
                                                         )
                                                     )
                                                 .Aggregations(k2 => k2
                                                     .Terms("RootIdCount", f2 => f2.Size(_size).Field("rootMapping.keyword"))
                                                     )
                                                 )
                                                 )
                                         )
                                     )
                                  )

                                   //Added By : Sadhana Upadhyay on 24 March 2015 : To get Count of all Make Count with root Count
                                   .Filter("AllMakeCount", i => i
                                       .Filter(j => j
                                           )
                                      .Aggregations(k => k
                                      .Terms("AllMakeIdCount", l => l.Size(_size).Field("makeMapping.keyword")
                                         .Aggregations(kk => kk
                                             .Filter("AllRoot_Count", f => f
                                                 .Filter(ff => ff
                                                     .Bool(bb => bb
                                                         .Should(q => q
                                                         .Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.cars))
                                                             )
                                                             .Must(mm => mm.MatchAll())
                                                         )
                                                     )
                                                 .Aggregations(k2 => k2
                                                     .Terms("AllRootIdCount", f2 => f2.Size(_size).Field("rootMapping.keyword"))
                                                     )
                                                 )
                                                 )
                                         )
                                     )
                                  )

                                  //Modified By : Sadhana Upadhyay on 24 March 2015 Removed Condition for Bodytype to get count of all bodyType
                                  .Filter("BodyTypeCount", i => i
                                      .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {
                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                      objFc &= !q.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                          q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                          q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                          q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                            q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));

                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                  return objFc;
                                              })
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                      .Aggregations(k => k
                                     .Terms("BodyIdCount", l => l.Size(_size).Field("bodyStyle"))
                                      )
                                  )

                                   .Filter("ColorCount", i => i
                                      .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {
                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                      objFc &= !q.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                          q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                          q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                          q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                            q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));



                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                  return objFc;
                                              })
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                      .Aggregations(k => k
                                     .Terms("ColorIdCount", l => l.Size(_size).Field("usedCarMasterColorsId"))
                                      )
                                  )

                                   .Filter("TransmissionCount", i => i
                                      .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {
                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));


                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                      objFc &= !q.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                  q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                  q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                  q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                    q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));



                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                  return objFc;
                                              })
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                      .Aggregations(k => k
                                     .Terms("TransmissionIdCount", l => l.Size(_size).Field("transmission"))
                                      )
                                  )

                                  .Filter("SellerCount", i => 
                                          _aggregationQueryDescriptor.GetSellerTypeCountDescriptor(filterInputs))

                                  .Filter("OwnersCount", i => i
                                      .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {
                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                      objFc &= !q.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                          q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                          q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                          q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                            q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));



                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                  return objFc;
                                              })
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                      .Aggregations(k => k
                                     .Terms("OwnerTypeCount", l => l.Size(_size).Field("owners"))
                                      )
                                  )

                                  .Filter("AreaCount", i => i
                                      .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {
                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                      q.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities)) &&
                                                  q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                  q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                  q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                  q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                    q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));



                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                  return objFc;
                                              })
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                      .Aggregations(k => k
                                      .Terms("AreaIdCount", l => l.Size(_size).Field("areaId"))
                                          )
                                          )

                                        .Filter("FilterBy2", i => i
                                       .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {

                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                      objFc &= !q.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Term("carWithPhoto", "1") &&
                                                  q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                  q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                  q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                  q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                  q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                    q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));


                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  return objFc;
                                              })
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                         .Aggregations(k => k
                                      .Terms("CarsWithPhotosCount", l => l.Size(_size).Field("carWithPhoto"))
                                          )
                                       )

                                       //CarTrade Certified Cars Count Start
                                       .Filter("FilterBy1", i => i
                                       .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {
                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  if (filterInputs.cities == null || filterInputs.cities[0] == Constants.AllIndiaCityId)
                                                      objFc &= !q.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("cityIds").Terms<string>(filterInputs.cities));

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                  q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                  q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                  q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                  q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                    q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));



                                                  objFc &= q.Term("certificationId", carTradeCertificationId);

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  if (filterInputs.carsWithPhotos == "1")
                                                      objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                  return objFc;
                                              })
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                         .Aggregations(k => k
                                      .Terms("CarTradeCertifiedCarsCount", l => l.Size(_size).Field("certificationId"))
                                          )
                                       )
                                       //CarTrade Certified Cars Count Ends

                                       //Franchies Cars Count Starts
                                       .Filter("FilterBy3", i =>
                                          _aggregationQueryDescriptor.GetFranchiseCarsCountDescriptor(filterInputs))
                                       //Franchies Cars Count Ends

                                       //For MumbaiAround And Delhi Arount || Separate Aggregation
                                       .Filter("MumbaiAroundCities", i => i
                                           .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {
                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                  q.Terms(terms => terms.Field("cityIds").Terms<string>(mumbaiAroundCityIds.Split(new char[] { ',' }))) &&
                                                  q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                  q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                  q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                  q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                    q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));


                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                  return objFc;
                                              }
                                            )
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                  )

                                  .Filter("DelhiAroundCities", i => i
                                           .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {

                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                  q.Terms(terms => terms.Field("cityIds").Terms<string>(delhiNCRCityIds.Split(new char[] { ',' }))) &&
                                                  q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                  q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                  q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                  q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                    q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));


                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                  return objFc;
                                              }
                                            )
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                  )

                                  .Filter("AllIndia", i => i
                                           .Filter(j => j
                                          .Bool(bb => bb
                                              .Should(q =>
                                              {

                                                  QueryContainer objFc = q.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                  if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                      objFc |= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  else
                                                      objFc &= q.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                  objFc &= !q.Term("packageType", Constants.DiamondDealerPackageType);// diamond dealer packages are excluded for all india

                                                  QueryContainer sellerQc = q.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                  if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                  {
                                                      sellerQc &= !q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                      sellerQc |= q.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                  }

                                                  objFc &= sellerQc && q.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                  q.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                  q.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                  q.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                  q.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                    q.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                q.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                q.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));


                                                  if (filterInputs.IsCarTradeCertifiedCars)
                                                  {
                                                      objFc &= q.Term("certificationId", carTradeCertificationId);
                                                  }

                                                  if (filterInputs.IsFranchiseCars)
                                                  {
                                                      objFc &= q.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                  }

                                                  objFc &= q.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));
                                                  return objFc;
                                              }
                                            )
                                            .Must(mq => mq.MatchAll())
                                           )
                                      )
                                  )

                                      )
                 );

            var filtersCount = elasticJson.ConvertAggregationsToJson(searchResponse, filterInputs);
            
            return filtersCount;
        }

        public PagerOutputEntity GetPagerData(int totalCount, ElasticOuptputs filterInputs)
        {
            var pagerInputs = new PagerEntity()
            {
                pageNo = filterInputs.pn,
                pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["UsedListPageSize"].ToString()),
                pagerSlotSize = Convert.ToInt32(ConfigurationManager.AppSettings["UsedListPageSlotSize"].ToString()),
                totalResults = totalCount
            };

            var pagerData = new PagerOutputEntity();
            pagerData = _pager.GetPager<PagerOutputEntity>(pagerInputs);


            return pagerData;
        }

        private string GetListingOrderByClause(string sortCriteria)
        {
            string retVal = string.Empty;
            try
            {
                switch (sortCriteria)
                {
                    case "0":
                        retVal = "makeYear";
                        break;

                    case "2":
                        retVal = "price";
                        break;

                    case "3":
                        retVal = "kilometers";
                        break;

                    case "6":
                        retVal = "lastUpdated";
                        break;

                    case "7":
                        retVal = "certificationScore";
                        break;

                    case "8":
                        retVal = "insertionDate";
                        break;

                    default:
                        retVal = String.Empty;//Priority, SellerType DESC, LastUpdated DESC
                        break;
                }
            }
            catch (SqlException ex)
            {
                var objErr = new ExceptionHandler(ex, "QueryFormatter.GetListingOrderByClause()");
                objErr.LogException();
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "QueryFormatter.GetListingOrderByClause()");
                objErr.LogException();
            }
            return retVal;
        }

        /// <summary>
        /// Added By : Sadhana Upadhyay on 24 March 2015
        /// Summary : to get nearby cities 
        /// Modified By : Sadhana Upadhyay on 13 Apr 2015
        /// Summary : To get Nearby cities for mumbai and arround and Delhi NCR
        /// </summary>
        /// <returns></returns>
        public List<City> GetNearbyCities(ElasticClient clientElastic, string cityId, ElasticOuptputs filterInputs)
        {
            List<City> citiesList = new List<City>();

            double LatSecPerKm = 32.57940665, LongSecPerKm = 34.63696611;
            double MaxLat = 0.0, MinLat = 0.0, MaxLong = 0.0, MinLong = 0.0;
            int Distance = 50;

            cityId = !string.IsNullOrEmpty(cityId) ? cityId : (filterInputs.cities != null ? filterInputs.cities[0] : string.Empty);

            try
            {
                if (!string.IsNullOrEmpty(cityId))
                {
                    if (cityId == "3000")
                        cityId = "1";
                    else if (cityId == "3001")
                        cityId = "10";

                    //To get Lat, long of city
                    var result = clientElastic.Search<LatLongURI>(qq => qq
                    .Index(Constants.ClassifiedElasticIndex)
                                       .Type("stock")
                                       .Take(1)
                                       .Query(q => q
                                           .Bool(b => b
                                            .Filter(f => f
                                                .Bool(bb => bb
                                                    .Must(mm => mm
                                                        .Term("cityId", cityId)
                                                    )
                                                )
                                            )
                                           )
                                       )
                                   );
                    var objLatLong = new LatLongURI();
                    foreach (var i in result.Documents)
                    {
                        objLatLong.Latitude = i.Latitude;
                        objLatLong.Longitude = i.Longitude;
                    }

                    MaxLat = Convert.ToDouble(objLatLong.Latitude) + Distance * LatSecPerKm;
                    MinLat = Convert.ToDouble(objLatLong.Latitude) - Distance * LatSecPerKm;
                    MaxLong = Convert.ToDouble(objLatLong.Longitude) + Distance * LongSecPerKm;
                    MinLong = Convert.ToDouble(objLatLong.Longitude) - Distance * LongSecPerKm;

                    var cities = clientElastic.Search<City>(qq => qq
                    .Index(Constants.ClassifiedElasticIndex)
                                .Type("stock")
                                    .Query(q => q
                                        .Range(y => y.Field("lattitude").GreaterThanOrEquals(MinLat).LessThanOrEquals(MaxLat)) &&
                                        q.Range(y => y.Field("longitude").GreaterThanOrEquals(MinLong).LessThanOrEquals(MaxLong)) &&
                                        q.Bool(b => b
                                            .Filter(ff => ff
                                                .Bool(bb => bb
                                                    .Must(n =>
                                                    {
                                                        QueryContainer objFc =
                                                        n.Terms(terms => terms.Field("makeId").Terms<string>(filterInputs.NewMakes));

                                                        if (filterInputs.NewMakes != null && filterInputs.NewMakes.Length > 0)
                                                            objFc |= n.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));
                                                        else
                                                            objFc &= n.Terms(terms => terms.Field("rootId").Terms<string>(filterInputs.NewRoots));

                                                        QueryContainer sellerQc = n.Terms(terms => terms.Field("sellerType").Terms<string>(filterInputs.sellers));
                                                        if (filterInputs.sellers != null && filterInputs.sellers.Contains("1"))
                                                        {
                                                            sellerQc &= !n.Terms(terms => terms.Field("packageType").Terms<string>(Constants.DealerPackageExcluded));
                                                            sellerQc |= n.Terms(terms => terms.Field("packageType").Terms<string>(Constants.PaidIndPackageTypes));
                                                        }

                                                        objFc &= sellerQc && n.Terms(terms => terms.Field("fuelTypeId").Terms<string>(filterInputs.fuels)) &&
                                                  n.Terms(terms => terms.Field("transmissionId").Terms<string>(filterInputs.transmissions)) &&
                                                  n.Terms(terms => terms.Field("usedCarMasterColorsId").Terms<string>(filterInputs.colors)) &&
                                                  n.Terms(terms => terms.Field("bodyStyleId").Terms<string>(filterInputs.bodytypes)) &&
                                                  n.Terms(terms => terms.Field("ownerTypeId").Terms<string>(filterInputs.owners)) &&
                                                          n.Range(y => y.Field(new Field("makeYear")).GreaterThanOrEquals(filterInputs.yearMin == "" ? 0 : Convert.ToDouble(filterInputs.yearMin))
                                                                    .LessThanOrEquals(filterInputs.yearMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.yearMax))) &&
                                                                n.Range(r => r.Field(new Field("price")).GreaterThanOrEquals(filterInputs.budgetMin == "" ? 0 : Convert.ToDouble(filterInputs.budgetMin))
                                                                    .LessThanOrEquals(filterInputs.budgetMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.budgetMax))) &&
                                                                n.Range(k => k.Field(new Field("kilometers")).GreaterThanOrEquals(filterInputs.kmMin == "" ? 0 : Convert.ToDouble(filterInputs.kmMin))
                                                                    .LessThanOrEquals(filterInputs.kmMax == "" ? int.MaxValue : Convert.ToDouble(filterInputs.kmMax)));


                                                        if (filterInputs.IsCarTradeCertifiedCars)
                                                        {
                                                            objFc &= n.Term("certificationId", carTradeCertificationId);
                                                        }

                                                        if (filterInputs.IsFranchiseCars)
                                                        {
                                                            objFc &= n.Term("cwBasePackageId", Constants.FranchiseCarsPackageId);
                                                        }

                                                        objFc &= n.Range(y => y.Field("photoCount").GreaterThanOrEquals(filterInputs.carsWithPhotos == "" ? 0 : Convert.ToDouble(filterInputs.carsWithPhotos)));

                                                        return objFc;
                                                    })
                                                    .MustNot(mm => mm.Term("cityId", cityId))
                                                )
                                            )
                                        )
                                    )
                                    .Aggregations(aa => aa
                                    .Terms("CityIdCount", h => h.Size(_size).Field("cityId"))
                                    .Terms("CityNameCount", h => h.Size(_size).Field("cityName"))
                                )
                            );

                    BucketAggregate cityCount = (Nest.BucketAggregate)cities.Aggregations["CityIdCount"];
                    BucketAggregate cityName = (Nest.BucketAggregate)cities.Aggregations["CityNameCount"];
                    var cityList = cityCount.Items;
                    var cityNameList = cityName.Items;

                    using (var cityIdListEnumerator = cityList.GetEnumerator())
                    using (var cityNameListEnumerator = cityNameList.GetEnumerator())
                    {
                        while (cityIdListEnumerator.MoveNext() && cityNameListEnumerator.MoveNext())
                        {
                            var currCityIdObject = cityIdListEnumerator.Current;
                            var currCityNameObject = cityNameListEnumerator.Current;

                            City city = new City()
                            {
                                CityId = (int)((long)((Nest.KeyedBucket<object>)(currCityIdObject)).Key),
                                CityName = (string)(((Nest.KeyedBucket<object>)(currCityNameObject)).Key),
                                CityCount = (int)((long)(((Nest.KeyedBucket<object>)(currCityIdObject)).DocCount))
                            };
                            citiesList.Add(city);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ESStockQuery.GetNearbyCities()");
                objErr.LogException();
            }
            return citiesList;
        }   //End of GetNearbyCities

        public List<CarMakeEntityBase> GetAllMakes(ElasticClient clientElastic)
        {
            List<CarMakeEntityBase> lstCarMakes = new List<CarMakeEntityBase>();
            try
            {
                var result = clientElastic.Search<StockBaseEntity>(qq => qq
                .Type("stock")
                    .Index(Constants.ClassifiedElasticIndex)
                        .Size(0)
                        .Aggregations(a => a
                            .Terms("makes", te => te
                                .Size(_size)
                                .Field("makeMapping.keyword")
                            )

                        )
                    );

                BucketAggregate makeBucket = (Nest.BucketAggregate)result.Aggregations["makes"];
                var makeList = makeBucket.Items;

                foreach (KeyedBucket<object> i in makeList)
                {
                    string[] makeMap = ((string)i.Key).Split('~');

                    CarMakeEntityBase carMakeBase = new CarMakeEntityBase()
                    {
                        MakeId = Convert.ToInt32(makeMap[0]),
                        MakeName = makeMap[1].ToString()
                    };
                    lstCarMakes.Add(carMakeBase);
                }

                lstCarMakes = lstCarMakes.OrderBy(o => o.MakeName).ToList();

            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ESStockQuery.GetAllMakes()");
                objErr.LogException();
            }
            return lstCarMakes;
        }

    }   //End of class
}   //End of namespace