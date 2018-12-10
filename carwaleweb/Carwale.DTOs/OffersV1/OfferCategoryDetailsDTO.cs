using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.OffersV1
{
    public class OfferCategoryDetailsDto : CategoriesMasterDto
    {
        [JsonProperty("offerText")]
        public string OfferText { get; set; }
    }
}
