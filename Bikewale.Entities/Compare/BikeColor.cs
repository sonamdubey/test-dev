using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Bike Color
    /// Author  :   Sumit Kate
    /// Date    :   25 Aug 2015
    /// </summary>
    [Serializable, DataContract]
    public class BikeColor
    {
        [DataMember]
        public int ColorId { get; set; }
        [DataMember]
        public uint VersionId { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public List<string> HexCodes { get; set; }
    }
    [Serializable, DataContract]
    public class BikeModelColor
    {
        [DataMember]
        public int ModelColorId { get; set; }
        [DataMember]
        public string HexCode { get; set; }
    }

}
