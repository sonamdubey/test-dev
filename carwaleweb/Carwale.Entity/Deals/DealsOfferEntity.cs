using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Deals
{
   [Serializable]
   public class DealsOfferEntity
    {
       [JsonProperty("categoryId")]
       public int CategoryId { get; set; }
       [JsonProperty("offerWorth")]
       public int OfferWorth { get; set; }
       [JsonProperty("additionalComments")]
       public string AdditionalComments { get; set; }
       [JsonProperty("description")]
       public string Description { get; set; }
       [JsonProperty("stockId")]
       public int StockId { get; set; }
    }
}
