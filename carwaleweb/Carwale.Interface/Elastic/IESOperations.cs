using Carwale.DTOs.Classified;
using Carwale.DTOs.Classified.Stock;
using Carwale.DTOs.Classified.Stock.Ios;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Nest;
using System.Collections.Generic;

namespace Carwale.Interfaces.Elastic
{
    public interface IESOperations
    {
        ResultsFiltersPagerDesktop GetElasticResults(ElasticClient client, FilterInputs filterInputs);
        ResultsFiltersPagerAndroid GetElasticResultsAndroid(ElasticClient client, FilterInputs filterInputs);
        ResultsFiltersPagerDesktop GetElasticResultsDesktop(ElasticClient client, FilterInputs filterInputs);
        StockResultsMobile GetElasticResultsMobile(ElasticClient client, FilterInputs filterInputs);
        StockResultsAndroid GetResultsDataAndroid(ElasticClient client, FilterInputs filterInputs);
        StockResultIos GetResultsDataIos(ElasticClient client, FilterInputs filterInputs);
        FilterCountsAndroid GetFilterResultAndroid(ElasticClient client, FilterInputs filterInputs);
        FilterCountIos GetFilterResultIos(ElasticClient client, FilterInputs filterInputs);
        int GetTotalStockCount(ElasticClient client, FilterInputs filterInputs);
        ResultsRecommendation GetRecommendationResults(ElasticClient clientElastic, FilterInputs filterInputs);
        List<StockBaseEntity> GetRecommendationsForProfileId<TInput>(ElasticClient clientElastic, TInput tInput, int recommendationsCount);
        List<CarMakeEntityBase> GetAllMakes(ElasticClient client);
        int GetStocksCountByField(ElasticClient client, FilterInputs filterInputs, string field, double fieldValue, bool greaterThanFieldValue);
        IEnumerable<StockBaseEntity> GetFrachiseCars(int size);
    }
}
