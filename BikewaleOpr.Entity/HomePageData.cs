using BikewaleOpr.Entity.BikeData;
using System.Collections.Generic;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by : Sajal Gupta on 21-03-2017
    /// Descripton : Entity to hold home page data
    /// Created by : Sanskar Gupta on 03 Jan 2018
    /// Description: List of all the New Launches Eligible for Promotion
    /// </summary>
    public class HomePageData
    {
        public bool IsSoldBikeDataUpatedShown { get; set; }
        public bool IsUsedBikeModelsAvailable { get; set; }
        public SoldUnitData SoldUnitsData { get; set; }
        public UsedBikeImagesByMakeNotificationData UsedModelsData { get; set; }
        public IEnumerable<BikeModelsByMake> BikeModelByMakeList { get; set; }
        public IEnumerable<BikeMakeModelData> PromotionBikes;
    }
}
