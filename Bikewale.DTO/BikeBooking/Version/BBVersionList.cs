using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeBooking.Version
{
    /// <summary>
    /// Bikebooking verison list
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBVersionList
    {
        [JsonProperty("versions")]
        public IEnumerable<BBVersionBase> Versions { get; set; }
    }
}
