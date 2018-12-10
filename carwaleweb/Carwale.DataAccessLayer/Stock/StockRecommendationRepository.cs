using Carwale.DAL.CoreDAL;
using Carwale.Entity.Classified;
using Carwale.Entity.Stock;
using Carwale.Interfaces.Stock;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Carwale.DAL.Stock
{
    public class StockRecommendationRepository : IStockRecommendationRepository
    {
        private static readonly string _liveListingName = ConfigurationManager.AppSettings["ElasticIndexName"];

        public IEnumerable<StockBaseEntity> GetRecommendations(StockRecoParamsData stockRecoParams)
        {

            ElasticClient client = ElasticClientInstance.GetInstance();
            var query = GetRecommendationQuery(stockRecoParams);
            var results = client.Search<StockBaseEntity>(query);
            return results.Documents;
        }

        private ISearchRequest GetRecommendationQuery(StockRecoParamsData stockRecoParams)
        {
            var recoFilters = GetRecoFilters(stockRecoParams.VersionSubSegmentId, stockRecoParams.RootId, stockRecoParams.CityId, stockRecoParams.MinPrice, stockRecoParams.MaxPrice, stockRecoParams.ProfileId);
            var recoSort = GetRecoSort(stockRecoParams.Price, stockRecoParams.RootId);

            return new SearchDescriptor<StockBaseEntity>()
                .Type("stock")
                .Index(_liveListingName)
                .Take(stockRecoParams.RecommendationsCount)
                .Query(q => q
                    .Bool(b1 => b1
                        .Filter(recoFilters)
                    )
                )
                .Sort(recoSort);
        }

        private Func<QueryContainerDescriptor<StockBaseEntity>, QueryContainer> GetRecoFilters(int versionSubSegmentId, int rootId, int cityId, int minPrice, int maxPrice, string profileId)
        {
            return qd =>
            {
                var qc = (qd.Term("versionSubSegmentID", versionSubSegmentId) | qd.Term("rootId", rootId))
                & qd.Term("cityId", cityId)
                & qd.Range(s => s.Field(field => field.PhotoCount).GreaterThan(0))
                & qd.Range(s => s.Field(field => field.Price).GreaterThanOrEquals(minPrice).LessThanOrEquals(maxPrice));

                if (profileId != null)
                {
                    qc = qc & !qd.Term("profileId", profileId.ToUpper());
                }

                return qc;
            };
        }

        private Func<SortDescriptor<StockBaseEntity>, IPromise<IList<ISort>>> GetRecoSort(int price, int rootId)
        {
            return sd => sd
               .Field(f => f.Field("sellerType"))
               .Script(sc => sc
                   .Type("number")
                   .Ascending()
                   .Script(script => script
                       .Inline("double diff = Math.abs(doc['price'].value - params.targetprice) / params.targetprice; int bucket = (int)(diff/0.25); return doc['rootId'].value == params.targetRootId ? bucket : bucket+0.5;")
                       .Lang("painless")
                       .Params(p => p
                           .Add("targetprice", price)
                           .Add("targetRootId", rootId)
                       )
                   )
               )
               .Field(f => f.Field(p => p.Price));
        }
    }

}