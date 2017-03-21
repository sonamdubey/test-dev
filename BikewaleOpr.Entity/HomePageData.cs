using BikewaleOpr.Entity.BikeData;

namespace BikewaleOpr.Entity
{
    public class HomePageData
    {
        public bool IsSoldBikeDataUpatedShown { get; set; }
        public bool IsUsedBikeModelsAvailable { get; set; }
        public SoldUnitData SoldUnitsData { get; set; }
        public UsedBikeImagesByMakeNotificationData UsedModelsData { get; set; }
    }
}
