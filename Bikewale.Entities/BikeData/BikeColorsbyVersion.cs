using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by: Aditi Srivastava on 17 Oct 2016
    /// Summary: To get colors by version id
    /// </summary>
    [Serializable, DataContract]
    public class BikeColorsbyVersion
    {
        [DataMember]
        public uint ColorId { get; set; }
        [DataMember]
        public string ColorName { get; set; }
        [DataMember]
        public IEnumerable<string> HexCode { get; set; }
    }
}
