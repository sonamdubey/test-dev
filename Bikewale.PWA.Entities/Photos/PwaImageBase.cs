﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.PWA.Entities.Photos
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 12 Feb 2018.
    /// </summary>
    [Serializable, DataContract]
    public class PwaImageBase
    {
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string OriginalImgPath { get; set; }
    }
}
