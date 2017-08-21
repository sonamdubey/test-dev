using BikewaleOpr.Entities;
using BikewaleOpr.Models.DealerPricing;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerFacility
{
    /// <summary>
    /// Written By : Snehal Dange on 7th August 2017
    /// Description : Model for dealer facility
    /// </summary>
    public class ManageDealerFacilityVM
    {
        public string PageTitle { get; set; }
        public IEnumerable<FacilityEntity> FacilityList { get; set; }
        public uint DealerId { get; set; }
        public DealerOperationPricingVM DealerOperationParams { get; set; }
    }
}
