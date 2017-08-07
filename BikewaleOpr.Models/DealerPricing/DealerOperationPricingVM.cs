using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.ContractCampaign;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    public class DealerOperationPricingVM : DealerOperationVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
        public IEnumerable<DealerEntityBase> Dealers { get; set; }
        public string MakesString { get; set; }
        public string DealersString { get; set; }
    }
}
