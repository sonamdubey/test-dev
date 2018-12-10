using Carwale.DTOs.Monetization;
using Carwale.Entity.Geolocation;

namespace Carwale.Interfaces.Monetization
{
    public interface IMonetization
    {
        MonetizationModelDTO ModelAddUnits(int modelId, int cityId, string zoneId, int platform, string screenType);
        MonetizationModelDTOV1 ModelAddUnitsV1(int modelId, Location location, int platform, string screenType, int campaignId);
    }
}
