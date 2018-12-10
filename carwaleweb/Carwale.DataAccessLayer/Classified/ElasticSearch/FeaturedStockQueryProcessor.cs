using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Utility.Classified;
using Nest;
using System;

namespace Carwale.DAL.Classified.ElasticSearch
{
    public class FeaturedStockQueryProcessor : IStockQueryProcessor
    {
        private readonly IQueryContainerRepository<StockBaseEntity> _queryContainerRepository;
        private readonly IAggregationQueryDescriptor _aggregationQueryDescriptor;
        private readonly Random _random = new Random();
        public FeaturedStockQueryProcessor(IQueryContainerRepository<StockBaseEntity> queryContainerRepository, IAggregationQueryDescriptor aggregationQueryDescriptor)
        {
            _queryContainerRepository = queryContainerRepository;
            _aggregationQueryDescriptor = aggregationQueryDescriptor;
        }

        public MultiSearchDescriptor GetMultiSearchDescriptorForSearchPage(ElasticOuptputs filterInputs, MultiSearchDescriptor msd)
        {
            int randomNumber = _random.Next(100000);
            return msd
                .Search<StockBaseEntity>(Constants.FranchiseeStocksSearchBucket ,sd =>
                    GetSearchDescriptorForFeaturedType(filterInputs, sd, CwBasePackageId.Franchise, randomNumber))
                .Search<StockBaseEntity>(Constants.DiamondStocksSearchBucket, sd =>
                    GetSearchDescriptorForFeaturedType(filterInputs, sd, CwBasePackageId.Diamond, randomNumber))
                .Search<StockBaseEntity>(Constants.PlatinumStocksSearchBucket,sd =>
                    GetSearchDescriptorForFeaturedType(filterInputs, sd, CwBasePackageId.Platinum, randomNumber));
        }

        private SearchDescriptor<StockBaseEntity> GetSearchDescriptorForFeaturedType(ElasticOuptputs filterInputs, SearchDescriptor<StockBaseEntity> searchDescriptor 
            , CwBasePackageId cwBasePackageId, int randomNumber)
        {
            return searchDescriptor
                .Type(Constants.ESClassifiedStockType)
                    .Index(Constants.ClassifiedElasticIndex)
                    .Query(qcd => _queryContainerRepository.GetCommonQueryContainerForSearchPage(filterInputs, Constants.C_getCarWithPhotos, qcd) &&
                            qcd.Term(new Field("cwBasePackageId"), cwBasePackageId))
                    .Aggregations(agcd => _aggregationQueryDescriptor.GetAggregationContainerForFeaturedStocks(randomNumber,filterInputs.FeaturedSlotsCount, agcd, filterInputs.Latitude
                        , filterInputs.Longitude, filterInputs.ShouldFetchNearbyCars))
                    .From(0)
                    .Take(0);
        }
    }
}
