using Carwale.Entity.Classified.CarDetails;
using Carwale.DTOs.Classified.CarDetails;
using Carwale.Entity.Enum;

namespace Carwale.Interfaces.Classified.CarDetail
{
    public interface ICarDetail
    {
        CarDetailsEntity GetCompleteCarDetails(string ProfileId, Platform platformType);
        UsedCarDetails GetCompleteCarDetailsMobile(string profileId, string deliveryCity, string imeiCode, int usedCarNotificationId, int sourceId);
    }
}
