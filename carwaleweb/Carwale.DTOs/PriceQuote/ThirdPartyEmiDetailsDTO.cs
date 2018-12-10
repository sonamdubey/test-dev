using Carwale.Entity.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    public class ThirdPartyEmiDetailsDto
    {
        [JsonProperty("loanAmount")]
        public string LoanAmount { get; set; }
        [JsonProperty("emi")]
        public string Emi { get; set; }
        [JsonProperty("lumpsumAmount")]
        public string LumpsumAmount { get; set; }
        [JsonProperty("tenure")]
        public string Tenure { get; set; }
        [JsonProperty("interestRate")]
        public float InterestRate { get; set; }
        [JsonProperty("emiType")]
        public EmiType EmiType { get; set; }
        [JsonProperty("emiTypeLabel")]
        public string EmiTypeLabel { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("emiTypeDescription")]
        public string EmiTypeDescription { get; set; }
        [JsonProperty("headerText")]
        public string HeaderText { get; set; }
        [JsonProperty("downPayment")]
        public string DownPayment { get; set; }
        [JsonProperty("websiteUrl")]
        public string WebsiteUrl { get; set; }
    }
}
