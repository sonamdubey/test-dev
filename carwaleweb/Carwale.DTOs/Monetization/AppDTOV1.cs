using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Monetization
{
    public class AppDTOV1 : MonetizationDataPriorityDTO
    {
        [JsonProperty("sponsoredAds")]
        public AppDTO SponsoredAds { get; set; }
    }
}
