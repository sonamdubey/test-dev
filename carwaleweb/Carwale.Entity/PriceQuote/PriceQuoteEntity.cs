using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Carwale.Entity.PriceQuote
{
    public enum PQSource
    {
        CarWale = 1,
        CarWaleMobile = 43,
        CarWaleMobileApp = 74
    }

    [Serializable]
    public class PriceQuoteEntity : CustomersBasicInfo
    {
        /// <summary>
        /// Initializing the Property Names for PriceQuote Inputs
        /// </summary>       
        public int CarVersionId { get; set; }
        public ulong CustomerId { get; set; }
        public string BuyingPreferences { get; set; }
        public int BuyTimeDays { get; set; }
        public int PQPageId { get; set; }
        public bool InterestedInLoan { get; set; }

        public int PlatformSourceId { get; set; }
        public int LeadSourceCategoryId { get; set; }
        public int LeadSourceId { get; set; }
        public string LeadSourceName { get; set; }

        public ulong InquiryId { get; set; }
        public ulong QuoteId { get; set; }
        public DataTable OnRoadPrice { get; set; }
        public ulong CRMLeadId { get; set; }
        public int DealerId { get; set; }
        public bool IsAutoVerify { get; set; }
        public string CarName { get; set; }
        public ulong PQDealerAdLeadId { get; set; }
    }

    //****************************************************************************//
    // Property Names of Price Quote Outputs
    //***************************************************************************//
    public static class PQOutputsEntity
    {
        /// <summary>
        /// Initializing the Property Names for PriceQuote Outputs
        /// </summary>
        public static readonly string InquiryId = "InquiryId";
        public static readonly string QuoteId = "QuoteId";
        public static readonly string OnRoadPrice = "OnRoadPrice";
    }
}
