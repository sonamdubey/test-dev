using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Carwale.Interfaces.Classified.ElasticSearch;
using Carwale.Utility.Classified;
using Nest;
using System;

namespace Carwale.DAL.Classified.ElasticSearch
{
    public class NonFeaturedStockQueryProcessor : IStockQueryProcessor
    {
        private readonly IQueryContainerRepository<StockBaseEntity> _queryContainerRepository;
        private readonly ISortDescriptorRepository _sortDescriptorRepository;
        public NonFeaturedStockQueryProcessor(IQueryContainerRepository<StockBaseEntity> queryContainerRepository, ISortDescriptorRepository sortDescriptorRepository)
        {
            _queryContainerRepository = queryContainerRepository;
            _sortDescriptorRepository = sortDescriptorRepository;
        }
        public MultiSearchDescriptor GetMultiSearchDescriptorForSearchPage(ElasticOuptputs filterInputs, MultiSearchDescriptor msd)
        {
            var stocksToFetch = !string.IsNullOrEmpty(filterInputs.ps) ? Convert.ToInt32(filterInputs.ps) : Constants.DefaultPageSize;
            return msd.Search<StockBaseEntity>(Constants.NonFeaturedStocksSearchBucket,sd => sd
                    .Type(Constants.ESClassifiedStockType)
                    .Index(Constants.ClassifiedElasticIndex)
                    .Query(qcd => _queryContainerRepository.GetCommonQueryContainerForSearchPage(filterInputs, filterInputs.carsWithPhotos, qcd)
                        && qcd.Bool(bqd => bqd.MustNot(qcd2 => qcd2.Terms(tqd => tqd.Field("profileId").Terms<string>(filterInputs.ExcludeStocks))))
                    )
                    .Sort(sod => _sortDescriptorRepository.GetSortDescriptorForNonFeaturedStocks(filterInputs, GetListingOrderByClause(filterInputs.sc), sod))
                    .From(filterInputs.lcr)
                    .Take(stocksToFetch)
                );
        }
        private static string GetListingOrderByClause(string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "0":
                    return "makeYear";
                case "2":
                    return "price";
                case "3":
                    return "kilometers";
                case "6":
                    return "lastUpdated";
                case "7":
                    return "certificationScore";
                case "8":
                    return "insertionDate";
                default:
                    return string.Empty;
            }
        }
    }
}
