using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.manufacturecampaign;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Nov 2017
    /// Description :   EMI Calculator View Model for EMICalculator View
    /// </summary>
    public class EMICalculatorVM
    {
        public EMI EMI { get; set; }
        public uint BikePrice { get; set; }
        public string EMIJsonBase64 { get; set; }
        public bool IsMobile { get; set; }
        public ManufactureCampaignEMIEntity ESEMICampaign { get; set; }
        public string BikeName { get; set; }
        public uint PQId { get; set; }
        public bool IsPremiumDealer { get; set; }
        public NewBikeDealers DealerDetails { get; set; }
    }
}
