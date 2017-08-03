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
    /// Description :   DTO for Version and Price
    /// </summary>
    public class VersionPriceDTO
    {
        [JsonProperty("itemCategoryId")]
        public uint ItemCategoryId { get; set; }
        [JsonProperty("itemName")]
        public string ItemName { get; set; }
        [JsonProperty("itemValue")]
        public uint ItemValue { get; set; }
    }
}
