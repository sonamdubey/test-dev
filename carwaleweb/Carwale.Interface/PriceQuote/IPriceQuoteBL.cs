using Carwale.DTOs.PriceQuote;
using Carwale.Entity.Campaigns;
using Carwale.Entity.CarData;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.Interfaces.PriceQuote
{
    public interface IPriceQuoteBL
    {
        List<PuneThaneZones> GetSpecialPuneZones();
        List<PuneThaneZones> GetSpecialThaneZones();
        List<CarVersionEntity> GetCarVersionDetails(int modelId, int cityId);
        PQ GetPQDetails(string encryptedId);
        CarDataTrackingEntity GetBasicTrackingObject(PQInput pqInput);
        List<CarVersionEntity> GetVersions(int modelId, int cityId);
        List<Entity.Price.PriceQuote> GetVersionPrice(int modelId, int versionId, int cityId);
        bool FetchCrossSell(List<Entity.Price.PriceQuote> priceList, DealerAd dealerAd);
        long CalculateOnRoadPrice(List<PQItem> priceList);
        long CalculateOnRoadPrice(List<Entity.Price.PriceQuote> priceList);
    }
}
