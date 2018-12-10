using Carwale.DTOs.Geolocation;
using Carwale.DTOs.NewCars;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IPrices
    {
        ModelPriceDTO GetOnRoadPrice(int modelId, int cityId);
        int GetMinOnRoadPrice(IEnumerable<VersionPricesDTO> Versions);
        Carwale.DTOs.PriceQuote.Prices GetVersionPQ(int cityId, int versionId);
        PQ FilterPrices(int cityId, int versionId);
        EMIInformation GetEmiInformation(int price);
        void UpdateCache(List<string> keys);
        PriceOverview GetAveragePrice(VersionPrice versionPrice, int count, int priceStatus);
        PriceOverview GetExShowRoomPrice(VersionPrice versionPrice, int count, int priceStatus);
        PriceOverview GetOnRoadPrice(VersionPrice versionPrice, int count, int priceStatus);
        List<PQItem> MapFromVersionPriceQuoteDTO(IEnumerable<VersionPriceQuoteDTO> versionPricesList, int platformId);
        NearByCityDetailsDto GetNearbyCitiesDto(int versionId, int cityId, int count);
        List<CityDTO> GetNearbyCitieswithPrices(int versionId, int cityId, int count);
        CarPriceQuote GetModelPrices(int modelId, int cityId, bool isNew, bool isCachedData);
        CarPriceQuote GetModelCompulsoryPrices(int modelId, int cityId, bool isNew, bool isCachedData);
        List<VersionPriceQuote> GetVersionCompulsoryPrices(int versionId, int cityId, bool isCachedData);
        List<VersionPriceQuote> GetVersionCompulsoryPrices(int modelId, int versionId, int cityId, bool isCachedData);
        NearByCityDetails GetNearbyCities(int versionId, int cityId, int count);
        int GetVersionOnRoadPrice(int modelId, int versionId, int cityId);
    }
}
