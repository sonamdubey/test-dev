using Carwale.DAL.CoreDAL;
using Carwale.Entity.Classified;
using Carwale.Entity.Classified.Enum;
using Carwale.Entity.Elastic;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Utility;
using Carwale.Utility.Classified;
using Microsoft.Practices.Unity;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.DAL.Classified.ElasticSearch
{
    public class ESStockQueryRepository : IESStockQueryRepository
    {
        private static readonly ElasticClient _client = ElasticClientInstance.GetInstance();
        private readonly IStockQueryProcessor _nonFeaturedStockQueryProcessor;
        private readonly IStockQueryProcessor _featuredStockQueryProcessor;
        private readonly IQueryContainerRepository<StockBaseEntity> _queryContainerDescriptor;
        private static readonly ElasticClient elasticClient = ElasticClientInstance.GetInstance();
        public ESStockQueryRepository([Dependency("nonFeatured")] IStockQueryProcessor nonFeaturedStockQueryProcessor
            , [Dependency("featured")] IStockQueryProcessor featuredStockQueryProcessor,
            IQueryContainerRepository<StockBaseEntity> queryContainerDescriptor)
        {
            _nonFeaturedStockQueryProcessor = nonFeaturedStockQueryProcessor;
            _featuredStockQueryProcessor = featuredStockQueryProcessor;
            _queryContainerDescriptor = queryContainerDescriptor;
        }
        public IEnumerable<StockBaseEntity> GetFrachiseCars(string[] cities, int size)
        {
            var searchResponse = _client.Search<StockBaseEntity>(s => s
                        .Type("stock")
                        .Index(Constants.ClassifiedElasticIndex)
                        .Size(size)
                        .Query(query => query
                                    .Bool(b => b
                                        .Must(m => m.Term("cwBasePackageId", Constants.FranchiseCarsPackageId)
                                                && m.Terms(term => term.Field("cityIds").Terms(cities))
                                             )
                                    )
                              )
                        .Sort(sort => sort
                                .Script(sc => sc
                                         .Type("number")
                                         .Descending()
                                         .Script(script => script
                                            .Inline("Math.random()")
                                            .Lang("painless")
                                         )
                                    )
                            )
                        );
            return searchResponse.Documents.ToList();
        }


        public StockForFilter GetStocksForSearchResults(ElasticOuptputs filterInputs)
        {
            var AreFeaturedStocksRequired = ShouldFetchFeaturedStocks(filterInputs.pn, filterInputs.so);
            var multiSearchResult = elasticClient.MultiSearch(msd =>
                {
                    var multiSearchQD = _nonFeaturedStockQueryProcessor.GetMultiSearchDescriptorForSearchPage(filterInputs, msd);
                    return AreFeaturedStocksRequired ? _featuredStockQueryProcessor.GetMultiSearchDescriptorForSearchPage(filterInputs, multiSearchQD)
                        : multiSearchQD;
                }
            );
            return ProcessSearchResponse(multiSearchResult, AreFeaturedStocksRequired, filterInputs);
        }

        public int GetTotalStockCount(ElasticOuptputs filterInputs)
        {
            ISearchResponse<StockBaseEntity> searchResponse;
            searchResponse = elasticClient.Search<StockBaseEntity>(ss => ss
            .Type("stock")
                .Index(Constants.ClassifiedElasticIndex)
                .Aggregations(agg => agg
                    .Filter("TotalStockCount", f => f
                        .Filter(fad => fad
                                .Bool(m => m
                                        .Must(qcd => _queryContainerDescriptor.GetCommonQueryContainerForSearchPage(filterInputs, filterInputs.carsWithPhotos, qcd)
                                            )
                                )
                            )
                            )
                          )

                );
            return Convert.ToInt32(searchResponse.Aggs.Filter("TotalStockCount").DocCount);

        }
        
        public string GetDetailsPageUrlByRegistrationNumber(string regNo)
        {
            var searchResponse = _client
                .Search<StockBaseEntity>(sd => sd
                    .Type("stock")
                    .Index(Constants.ClassifiedElasticIndex)
                    .Size(1)
                    .Source(sfd => sfd
                        .Includes(fd => fd
                            .Field(new Field("url"))
                        )
                    )
                    .Query(qcd => qcd
                        .Term(tqd => tqd
                            .Field(new Field("carRegNo")).Value(regNo)
                        )
                    )
                );
            return searchResponse.Documents.FirstOrDefault()?.Url;
        }

        private static bool ShouldFetchFeaturedStocks(int pageNumber, string sortOrder)
        {
            return pageNumber == 1 && string.IsNullOrWhiteSpace(sortOrder);
        }

        private static StockForFilter ProcessSearchResponse(IMultiSearchResponse multiSearchResponse
            , bool AreFeaturedStocksRequired
            , ElasticOuptputs filterInputs)
        {
            
            var dict = new Dictionary<CwBasePackageId, List<StockBaseEntity>>();
            SearchResponse<StockBaseEntity> nonFeaturedSearchResponse = multiSearchResponse.GetResponse<StockBaseEntity>(Constants.NonFeaturedStocksSearchBucket);
            var nonFeatureList = nonFeaturedSearchResponse?.Documents?.ToList();

            
            if (nonFeatureList != null && filterInputs.ShouldFetchNearbyCars)
            {
                for (int i = 0; i < nonFeatureList.Count; i++)
                {
                    nonFeatureList[i].NearbyCarsBucket = (NearbyCarsBucket)Convert.ToInt32(nonFeaturedSearchResponse.HitsMetaData.Hits.ElementAt(i).Sorts.FirstOrDefault());
                } 
            }
            dict.Add(CwBasePackageId.Default, nonFeatureList);
            
            if (nonFeatureList.IsNotNullOrEmpty() && AreFeaturedStocksRequired)
            {
                
                dict[CwBasePackageId.Franchise] = GetStockList(multiSearchResponse.GetResponse<StockBaseEntity>(Constants.FranchiseeStocksSearchBucket)
                    , filterInputs.ShouldFetchNearbyCars
                    , nonFeatureList.FirstOrDefault().NearbyCarsBucket);
                dict[CwBasePackageId.Diamond] = GetStockList(multiSearchResponse.GetResponse<StockBaseEntity>(Constants.DiamondStocksSearchBucket)
                    , filterInputs.ShouldFetchNearbyCars
                    , nonFeatureList.FirstOrDefault().NearbyCarsBucket);
                dict[CwBasePackageId.Platinum] = GetStockList(multiSearchResponse.GetResponse<StockBaseEntity>(Constants.PlatinumStocksSearchBucket)
                    , filterInputs.ShouldFetchNearbyCars
                    , nonFeatureList.FirstOrDefault().NearbyCarsBucket);
            }
            StockForFilter stockForFilter = new StockForFilter(dict);
            //Add Excluded Stock count for getting correct toatl stock count
            stockForFilter.Count = (nonFeaturedSearchResponse != null ? Convert.ToInt32(nonFeaturedSearchResponse?.Total): 0) 
                + (filterInputs.ExcludeStocks.IsNotNullOrEmpty() ? filterInputs.ExcludeStocks.Length : 0);
            return stockForFilter;
        }

        private static List<StockBaseEntity> GetStockList(ISearchResponse<StockBaseEntity> searchResponse, bool ShouldFetchNearbyCars, NearbyCarsBucket nonFeaturedNearbyCarsBucket)
        {
            if (searchResponse == null)
            {
                return null;
            }
            var result = new List<StockBaseEntity>();
            var dealerBuckets = searchResponse.Aggs.Terms("dealers");
            foreach (var bucket in dealerBuckets.Buckets)
            {
                var stocksList = bucket.TopHits("stocks").Documents<StockBaseEntity>();
                if (ShouldFetchNearbyCars)
                {
                    var carNearMeList = stocksList.Where(l =>
                                {
                                    NearbyCarsBucket nearbyBucket;
                                    Enum.TryParse(bucket.Key.Split('_')[0], out nearbyBucket);
                                    l.NearbyCarsBucket = nearbyBucket;
                                    return l.NearbyCarsBucket == nonFeaturedNearbyCarsBucket;
                                });
                    result.AddRange(carNearMeList);
                }
                else
                {
                    result.AddRange(bucket.TopHits("stocks").Documents<StockBaseEntity>());
                }

            }
            return result;
        }
    }
}
