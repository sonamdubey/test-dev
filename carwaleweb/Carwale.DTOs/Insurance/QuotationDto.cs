using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Insurance
{
    public class QuotationDto
    {
        public string Quotation { get; set; }
        public string ConfirmationStatus { get; set; }
        public long UniqueId { get; set; }
    }

    public class InsuranceResponse
    {
        [JsonProperty("breakdown")]
        public object Breakdown { get; set; }

        [JsonProperty("total")]
        public int? Total { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonProperty("useRedirect")]
        public bool UseRedirect { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("quoteId")]
        public string QuoteId { get; set; }
    }
}
