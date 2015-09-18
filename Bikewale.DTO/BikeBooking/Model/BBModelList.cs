using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.BikeBooking.Model
{
    /// <summary>
    /// Bikebooking model list
    /// Author  :   Sumit Kate
    /// Date    :   20 Aug 2015
    /// </summary>
    public class BBModelList
    {
        [JsonProperty("models")]
        public IEnumerable<BBModelBase> Models { get; set; }
    }
}
