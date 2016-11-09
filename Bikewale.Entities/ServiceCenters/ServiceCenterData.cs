using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.ServiceCenters
{
    /// <summary>
    /// Created By :  Sajal Gupta
    /// Created On  : 07 Nov 2016
    /// Description : Service center data on city listing page.
    /// </summary>

    [Serializable, DataContract]
    public class ServiceCenterData
    {
        [DataMember]
        public uint Count { get; set; }

        [DataMember]
        public IEnumerable<ServiceCenterDetails> ServiceCenters { get; set; }
    }
}
