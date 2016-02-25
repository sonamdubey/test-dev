﻿using Newtonsoft.Json;
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
        [JsonProperty("makeName")]
        public string Text { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        [JsonProperty("makeId")]
        public string Value { get; set; }
    }
}
