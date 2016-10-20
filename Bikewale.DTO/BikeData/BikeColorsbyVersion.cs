using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeData
{
    /// <summary>
    /// Created by: Aditi Srivastava on 17 Oct 2016
    /// Summary: To get colors by version id
    /// </summary>
   
    public class BikeColorsbyVersion
    {
        [JsonProperty("colorId")]
        public uint ColorId { get; set; }
        
        [JsonProperty("colorName")]
        public string ColorName { get; set; }

        [JsonProperty("hexCode")]
        public IEnumerable<string> HexCode { get; set; }
    }
}
