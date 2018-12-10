using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.CarValuation
{
    public class ValuationResultsDTO
    {
        // Individual Values

        [JsonProperty("indValExcellent")]
        public uint IndividualValueExcellent { get; set; }


        [JsonProperty("indValGood")]
        public uint IndividualValueGood { get; set; }


        [JsonProperty("indValFair")]
        public uint IndividualValueFair { get; set; }

        //Dealer Properties

        [JsonProperty("dealValExcellent")]
        public uint DealerValueExcellent { get; set; }


        [JsonProperty("dealValGood")]
        public uint DealerValueGood { get; set; }


        [JsonProperty("dealValFair")]
        public uint DealerValueFair { get; set; }


        [JsonProperty("dealPrchsValFair")]
        public uint DealerPurchaseValueFair { get; set; }


        [JsonProperty("dealPrchsValGood")]
        public uint DealerPurchaseValueGood { get; set; }

        [JsonProperty("dealPrchsValExcellent")]
        public uint DealerPurchaseValueExcellent { get; set; }
    }
}
