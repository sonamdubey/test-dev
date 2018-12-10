using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.Elastic;
using Nest;
using System;
using System.Collections.Generic;


namespace Carwale.Interfaces.Elastic
{
    //Modified By : Sadhana Upadhyay on 10 Mar 2015
    public interface IESStockQuery
    {
        Tuple<List<StockBaseEntity>, int, int, int, int> GetSearchResults(ElasticClient client, ElasticOuptputs filterInputs);
        StockBaseMobile GetSearchResultsMobile(ElasticClient client, ElasticOuptputs filterInputs);
        CountData GetAggregationsCount(ElasticClient client, ElasticOuptputs filterInputs);
        PagerOutputEntity GetPagerData(int totalCount, ElasticOuptputs filterInputs);
        int GetTotalStockCount(ElasticClient client, ElasticOuptputs filterInputs);
        List<City> GetNearbyCities(ElasticClient client, string cityId, ElasticOuptputs filterInputs);
        List<StockBaseEntity> GetRecommendationResults(ElasticClient client, ElasticOuptputs filterInputs);
        List<StockBaseEntity> GetRecommendationsForProfileId<TInput>(ElasticClient clientElastic, TInput tInput, int recommendationsCount);
        List<CarMakeEntityBase> GetAllMakes(ElasticClient clientElastic);
        int GetStocksCountByField(ElasticClient client, ElasticOuptputs filterInputs, string field, double fieldValue, bool greaterThanFieldValue);
        StockBaseEntity GetStockByProfileId(ElasticClient client, string profileId);
    }
}
