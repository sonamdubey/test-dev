using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Elastic
{
    public interface IProcessElasticJson
    {
		//Modified B y : Sadhana Upadhyay on 10 Mar 2015
        CountData ConvertAggregationsToJson(ISearchResponse<StockBaseEntity> filters, ElasticOuptputs filterInputs);
        Tuple<List<StockBaseEntity>, int, int, int, int> TakeTopResults(List<SearchResponse<StockBaseEntity>> onlyResults, ElasticOuptputs filterInputs, int slotCountFeatured, int PageSize, bool isRequestFromNearbyCity, int totalDiamondSlotCount);
    }
}
