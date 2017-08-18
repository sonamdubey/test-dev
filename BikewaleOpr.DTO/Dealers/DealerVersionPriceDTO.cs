using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 02 Aug 2017
    /// Description :   DTO for dealer version and belonging categories.
    /// </summary>
    public class DealerVersionPriceDTO
    {
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("numberOfDays")]
        public uint NumberOfDays { get; set; }
        [JsonProperty("bikeModelId")]
        public uint BikeModelId { get; set; }
        public IEnumerable<VersionPriceDTO> Categories { get; set; }
    }
}
