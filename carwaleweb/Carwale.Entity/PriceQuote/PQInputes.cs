using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Customers;

namespace Carwale.Entity.PriceQuote
{
    public class PQInput : CustomerMinimal
    {
        public int CarVersionId { get; set; }
        public int CarModelId { get; set; }
        public int CityId { get; set; }
        public string BuyingPrefrences { get; set; }
        public bool InterestedInLoan { get; set; }
        public string ZoneId { get; set; }
        public int SourceId { get; set; }
        public string PageId { get; set; }
        public string BuyTimeDaysText { get; set; }
        public int BuyTimeDaysValue { get; set; }
        public bool IsMobileVerified { get; set; }
        public string ClientIp { get; set; }
        public bool IsSponsoredCarShowed { get; set; }
        public string Ltsrc { get; set; }
        public int CampaignId { get; set; }
        public string UtmaCookie { get; set; }
        public string UtmzCookie { get; set; }
        public int AreaId { get; set; }
    }
}
