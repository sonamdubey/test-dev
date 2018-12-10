using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.Entity.Dealers;
using Carwale.Entity.Geolocation;
using System.Collections.Generic;

namespace Carwale.DTOs.LeadForm
{
    public class LeadFormModelData
    {
        public List<DealerAdDTO> DealerAd { get; set; }
        public Location CustLocation { get; set; }
    }
}
