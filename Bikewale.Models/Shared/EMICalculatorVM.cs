using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.manufacturecampaign;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Nov 2017
    /// Description :   EMI Calculator View Model for EMICalculator View
    /// Modified by :   Vivek Singh Tomar on 01st Nov 2017
    /// Description :   Added LeadSourceEnum to capture dealer lead source id
    /// </summary>
    public class EMICalculatorVM
    {
        public EMI EMI { get; set; }
        public uint BikePrice { get; set; }
        public string EMIJsonBase64 { get; set; }
        public bool IsMobile { get; set; }
        public ManufactureCampaignEMIEntity ESEMICampaign { get; set; }
        public string BikeName { get; set; }
        public string PQId { get; set; }
        public bool IsPremiumDealer { get; set; }
        public NewBikeDealers DealerDetails { get; set; }
        public LeadSourceEnum PremiumDealerLeadSourceId { get; set; }
        public bool IsPrimaryDealer { get; set; }
        public bool IsManufacturerLeadAdShown { get; set; }
    }
}
