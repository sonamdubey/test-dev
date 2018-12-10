using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IPQCacheRepository
    {
        PQ GetPQ(int CityId, int VersionId);
        IEnumerable<ModelPrices> GetModelPrices(int modelId, int cityId);
        void StoreCarVersionDetails(int modelId, int cityId, List<CarVersionEntity> versionDetails);
        IDictionary<int, VersionAveragePrice> GetModelsVersionAveragePrices(int modelId, bool isNew);
        void StoreUserPQTakenModels(string cwcCookie, List<int> modelIds);
        IEnumerable<VersionPrice> GetAllVersionsPriceByModelCity(int modelId, int cityId);
        void StoreVersionPriceDetails(int versionId, int cityId, PriceOverview carPriceAvailability);
        void ReplaceVersionsPriceByModelCity(int modelId, int cityId);
        CarPriceQuote GetModelPricesCache(int modelId, int cityId, bool isNew);
        void StoreModelPrices(int modelId, int cityId, bool isNew, CarPriceQuote priceQuote);
    }
}
