using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Carwale.Entity.Classified.CarValuation
{
    public class CarValuationResults
    {
        [JsonProperty("id")]
        public int ValuationId { get; set; }

        // Individual Values        
        [JsonProperty("indValExcellent")]
        public uint IndividualValueExcellent { get; set; }
                
        [JsonProperty("indValGood")]
        public uint IndividualValueGood { get; set; }
                
        [JsonProperty("indValFair")]
        public uint IndividualValueFair { get; set; }

        [JsonProperty("indValPoor")]
        public uint IndividualValuePoor { get; set; }

        //Dealer Sale Properties        
        [JsonProperty("dealValExcellent")]
        public uint DealerValueExcellent { get; set; }
        
        [JsonProperty("dealValGood")]
        public uint DealerValueGood { get; set; }
        
        [JsonProperty("dealValFair")]
        public uint DealerValueFair { get; set; }

        //Dealer Purchase Properties
        [JsonProperty("dealPrchsValPoor")]
        public uint DealerPurchaseValuePoor { get; set; }

        [JsonProperty("dealPrchsValFair")]
        public uint DealerPurchaseValueFair { get; set; }

        [JsonProperty("dealPrchsValGood")]
        public uint DealerPurchaseValueGood { get; set; }

        [JsonProperty("dealPrchsValExcellent")]
        public uint DealerPurchaseValueExcellent { get; set; }
    }
}
