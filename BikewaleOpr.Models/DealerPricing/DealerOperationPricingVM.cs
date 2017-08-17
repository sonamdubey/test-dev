using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.ContractCampaign;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   View model for dealer operation collapsable in dealer pricing management pricing sheet page
    /// </summary>
    public class DealerOperationPricingVM : DealerOperationVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public IEnumerable<DealerEntityBase> Dealers { get; set; }
        public string MakesString { get; set; }
        public string DealersString { get; set; }
    }
}
