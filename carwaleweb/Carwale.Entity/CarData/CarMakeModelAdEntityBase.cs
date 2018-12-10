﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable,JsonObject]
    public class CarMakeModelAdEntityBase : CarMakeModelEntityBase
    {
        [JsonProperty(PropertyName = "isSponsored")]
        public bool IsSponsored { get; set; }
    }
}
