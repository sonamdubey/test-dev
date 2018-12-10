using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.Elastic
{
    public interface IElasticSearchManager
    {
        T SearchIndexProfileRecommendation<T, TInput>(TInput tInput, int recommendationsCount);
        T SearchIndex<T>(string esIndex, FilterInputs filterInputs);
        int GetTotalStockCount(string esIndex, FilterInputs filterInputs);
        List<CarMakeEntityBase> GetAllMakes(string esIndex);
        int GetStocksCountByField(string esIndex, FilterInputs filterInputs, string field, double fieldValue, bool greaterThanFieldValue);
    }
}
