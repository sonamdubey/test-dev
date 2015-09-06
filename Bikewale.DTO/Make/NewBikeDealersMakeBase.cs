using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Make
{
    /// <summary>
    /// Author  :   Sumit Kate
    /// Created :   04 Sept 2015
    /// NewBikeDealersMakeBase Entity
    /// </summary>
    public class NewBikeDealersMakeBase
    {
        /// <summary>
        /// Text
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
