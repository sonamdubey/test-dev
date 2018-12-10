using Carwale.DTOs.Deals;
using Carwale.Entity.Dealers;
using Carwale.DTOs.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CallSlugDTO
    {
        public string CarName { get; set; }
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public bool IsAdAvailable { get; set; }
        public string DealerMobile { get; set; }
        public string DealerName { get; set; }
        public DealsStockDTO AdvantageAdData { get; set; }
        public DealerAdDTO DealerAd { get; set; }
    }
}
