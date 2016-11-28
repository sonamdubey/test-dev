using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.service
{
    [Serializable, DataContract]
    public class ServiceCenterLocatorList
    {
        [DataMember]
        public IEnumerable<ServiceCenterLocator> ServiceCenterDetailsList;
        [DataMember]
        public int ServiceCenterCount;
        [DataMember]
        public int CityCount;
    }
}
