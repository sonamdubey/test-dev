using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Customers;

namespace Carwale.Entity.ThirdParty.Leads
{
    [Serializable]
    public class ThirdPartyInquiryDetails:CustomerMinimal
    {
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public int CityId { get; set; }
        public int PlatformSourceId { get; set; }
        public string PartnerSourceId { get; set; }
        public int DealerId { get; set; }
        public int StatusId { get; set; }
    }
}
