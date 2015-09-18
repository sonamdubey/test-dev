using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeBooking.Make
{
    /// <summary>
    /// Bikebooking Make base
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBMakeBase
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("maskingName")]
        public string MaskingName { get; set; }
    }
}
