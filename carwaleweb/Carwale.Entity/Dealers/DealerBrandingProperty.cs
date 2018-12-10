using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    public class DealerBrandingProperty
    {
        public string DealerFirstName  { get; set; }
        public string DealerLastName { get; set; }
        public int DealerId { get; set; }
        public string DealerMaskingNumber { get; set; }
        public string DealerActualNumber { get; set; }
        public int AreaId { get; set; }
        public string Area { get; set; }
        public int CityId { get; set; }
        public int MakeId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int PackageId { get; set; }
        public Int64 ImpactCampaignId { get; set; }
        public string DealerName { get; set; }
        public string CityName { get; set; }
    }
}
