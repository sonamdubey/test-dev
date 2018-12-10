using System.Collections.Generic;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.Customers;
using Carwale.Entity.CarData;
using Carwale.Entity;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IPQRepository
    {        
        PQ GetPQ(int cityId, int versionId);
        List<ModelPrices> GetModelPrices(int modelId, int cityId);
        IDictionary<int, VersionAveragePrice> GetModelsVersionAveragePrices(int modelId, bool isNew, bool isMasterConnection);
        IEnumerable<VersionPrice> GetAllVersionsPriceByModelCity(int modelId, int cityId);
        List<VersionPrices> GetVersionsPriceList(int modelId, int cityId, bool isNew);
    }
}
