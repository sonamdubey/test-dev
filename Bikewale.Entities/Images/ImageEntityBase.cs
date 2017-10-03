using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Images
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 28-Sep-2017
    /// </summary>
    [Serializable, DataContract]
    public class ImageEntityBase
    {
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
    }
}