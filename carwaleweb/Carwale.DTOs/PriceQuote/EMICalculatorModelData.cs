using Carwale.DTOs.Campaigns;
using Carwale.Entity.Campaigns;
using Carwale.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    public class EmiCalculatorModelData
    {
        public int VersionId { get; set; }
        public int MakeId { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public int DownPaymentMinValue { get; set; }
        public long DownPaymentMaxValue { get; set; }
        public int DownPaymentDefaultValue { get; set; }
        public bool IsMetallic { get; set; }
        public string UniqueKey { get; set; }
        public DealerAdDTO DealerAd { get; set; }
        public CwPages Page { get; set; }

        //TODO: This is a temporary fix. It will be removed when DealerAd.PageProperties is used
        public LeadSourceDTO CtaDetails { get; set; }
        public ThirdPartyEmiDetailsDto ThirdPartyEmiDetails { get; set; }
        public bool IsEligibleForThirdPartyEmi { get; set; }
        public string PageName { get; set; }
        public string Platform { get; set; }
    }
}
