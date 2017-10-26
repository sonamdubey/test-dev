﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [Serializable, DataContract]
    public class PwaBikeNews:PwaBikeCms
    {
        [DataMember]
        public List<PwaBikeDetails> BikesList { get; set; }
    }
}
