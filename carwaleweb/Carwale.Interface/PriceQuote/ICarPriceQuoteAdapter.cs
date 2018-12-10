using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface ICarPriceQuoteAdapter
    {
        CarPriceQuoteDTO GetModelPriceQuote(int modelId, int cityId, bool isNew, bool isSpecsMandatory, bool isCachedData);
        ResponseDTO InsertPriceQuote(CarPriceQuote pricesInput);
        ResponseDTO DeletePriceQuote(CarPriceQuote pricesInput);
        IEnumerable<VersionPrice> GetAllVersionPriceByModelCity(int modelId, int cityId, bool ORP = false, bool isNew = true);
        IDictionary<int, PriceOverview> GetVersionsPriceForSameModel(int modelId, List<int> versionList, int cityId, bool ORP = false);
        IDictionary<int, PriceOverview> GetVersionsPriceForDifferentModel(List<int> versionList, int cityId, bool ORP = false);
        IDictionary<int, PriceOverview> GetModelsCarPriceOverview(List<int> modelList, int cityId, bool ORP = false);
        PriceOverview GetAvailablePriceForModel(int modelId, int cityId, IEnumerable<VersionPrice> versionPriceList = null, bool ORP = false);
        PriceOverview GetAvailablePriceForVersion(int versionId, int cityId, bool ORP, bool isNew = true);
        int GetVersionExShowroomPrice(int modelId, int versionId, int cityId);
    }
}
