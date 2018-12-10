using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    [Serializable]
    public class MaskingNumber
    {
        public string CampaignId { get; set; }
        public string ExpiryDate { get; set; }
        public string IsAssigned { get; set; }
        public string MappedNumbers { get; set; }
        public string Number { get; set; }
        public string ProductTypeId { get; set; }
        public string Provider { get; set; }
        public string SellerMobileMaskingId { get; set; }
        public string Tc_InquirySourceId { get; set; }
    } 
}
