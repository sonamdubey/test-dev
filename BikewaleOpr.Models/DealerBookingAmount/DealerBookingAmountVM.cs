using BikewaleOpr.Entity;
using BikewaleOpr.Models.DealerPricing;

namespace BikewaleOpr.Models.DealerBookingAmount
{
    public class DealerBookingAmountVM
    {
        public string PageTitle { get; set; }
        public ManageBookingAmountData DealerBookingAmountData { get; set; }
        public DealerOperationPricingVM DealerOperationParams { get; set; }
        public string DealerName { get; set; }
        public uint CityId { get; set; }
        public uint MakeId { get; set; }
        public uint DealerId { get; set; }
    }
}
