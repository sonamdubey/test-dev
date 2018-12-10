using Carwale.Entity.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Offers
{ 
    // added by ashish Verma
    public class OfferInputs :CustomerMinimal
    {
        public int VersionId { get; set; }
        public int DealerId { get; set; }
        public int CityId { get; set; }
        public string ZoneId { get; set; }
        public string InquirySourceId { get; set; }
        public string ModelName { get; set; }
        public ulong PQId { get; set; }
        public int OfferId { get; set; }
        public int LeadClickSource { get; set; }
        public int PlatformSourceId { get; set; }
        public ulong PQDealerAdLeadId { get; set; }
        public string CouopnCode { get; set; }
        public string Ltsrc { get; set; }
        public string UtmaCookie { get; set; }
        public string UtmzCookie { get; set; }

    }
}
