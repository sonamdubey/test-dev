﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.AutoComplete
{    
    public class SuggestionList
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("payload")]
        public object Payload { get; set; }
    }
}
