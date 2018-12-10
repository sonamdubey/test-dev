using Carwale.DAL.CoreDAL;
using Carwale.Entity.Classified;
using Carwale.Entity.Enum;
using Carwale.Interfaces.Dealers.Used;
using Carwale.Utility.Classified;
using Nest;
using System.Collections.Generic;

namespace Carwale.DAL.Dealers.Used
{
    public class UsedDealerStocksRepository : IUsedDealerStocksRepository
    {
        public ISearchResponse<StockBaseEntity> GetDealerFranchiseStocks(int dealerId, int from, int size)
        {
            ElasticClient elasticClient = ElasticClientInstance.GetInstance();
            var queryResponse = elasticClient.Search<StockBaseEntity>(s => s
                                .Index(Constants.ClassifiedElasticIndex)
                                .Type("stock")
                                .Size(size)
                                .From(from)
                                .Sort(sort => sort
                                        .Descending("sortScore")
                                     )
                                .Query(qcd => qcd
                                    .Bool(bqd => bqd
                                        .Filter(qcd2 => 
                                            {
                                                var queryContainer = qcd2.Term(new Field("dealerId"), dealerId);
                                                queryContainer &= qcd2.Term(new Field("cwBasePackageId"), CwBasePackageId.Franchise);
                                                return queryContainer;
                                            }
                                        )
                                    )
                                ));
            return queryResponse;
        }
    }
}
