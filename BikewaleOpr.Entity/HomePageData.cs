using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by : Sajal Gupta on 21-03-2017
    /// Descripton : Entity to hold home page data
    /// </summary>
    public class HomePageData
    {
        public bool IsSoldBikeDataUpatedShown { get; set; }
        public bool IsUsedBikeModelsAvailable { get; set; }
        public SoldUnitData SoldUnitsData { get; set; }
        public UsedBikeImagesByMakeNotificationData UsedModelsData { get; set; }
        public IEnumerable<BikeModelsByMake> BikeModelByMakeList { get; set; }
    }
}
