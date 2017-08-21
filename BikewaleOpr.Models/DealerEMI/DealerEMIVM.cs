using BikewaleOpr.Entities;
using BikewaleOpr.Models.DealerPricing;

namespace BikewaleOpr.Models.DealerEMI
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 18 Aug 2017
    /// Description :   View model for dealer EMI page
    /// </summary>
    public class DealerEMIVM
    {
        public string PageTitle { get; set; }
        public DealerOperationPricingVM DealerOperationParams { get; set; }
        public EMI dealerEmiFormInfo { get; set; }
        public string DealerName { get; set; }
    }
}
