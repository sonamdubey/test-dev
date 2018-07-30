using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// Created By: Aditi Srivastava on 17 Oct 2016
    /// Description: list of all colors by version
    /// </summary>
    /// BikeColorsbyVersionsDTO

    public class BikeColorsbyVersionDTO
    {
        [JsonProperty("colors")]
        public IEnumerable<BikeColorsbyVersionsDTO> VersionColors { get; set; }

    }
}
