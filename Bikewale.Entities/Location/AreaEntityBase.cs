using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Location
{
    /// <summary>
    /// Modified by :   Sumit Kate on 25 Jan 2016
    /// Description :   Added Serializable Attribute to the class
    /// </summary>
    [Serializable, DataContract]
    public class AreaEntityBase
    {
        [DataMember]
        public UInt32 AreaId { get; set; }
        [DataMember]
        public string AreaName { get; set; }
        [DataMember]
        public string PinCode { get; set; }
        [DataMember]
        public string AreaMaskingName { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public double Latitude { get; set; }
    }
}
