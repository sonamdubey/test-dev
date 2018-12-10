using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.OffersV1
{
    public class OfferDto
    {
        [JsonProperty("offerDetails")]
        public OfferDetailsDto OfferDetails { get; set; }
        [JsonProperty("categoryDetails")]
        public List<OfferCategoryDetailsDto> CategoryDetails { get; private set; }

        public OfferDto()
        {
            this.CategoryDetails = new List<OfferCategoryDetailsDto>();
        }
    }
}
