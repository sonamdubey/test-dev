using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Bike Color Entity
    /// </summary>
    [Serializable, DataContract]
    public class CompareBikeColor
    {
        [DataMember]
        public List<BikeColor> bikeColors { get; set; }
    }
}
