using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.ServiceCenters
{
    /// <summary>
    /// Created By :  Sajal Gupta
    /// Created On  : 07 Nov 2016
    /// Description : Service center data on city listing page.
    /// </summary>

    [Serializable, DataContract]
    public class ServiceCenterDetails
    {
        [DataMember]
        public uint ServiceCenterId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Mobile { get; set; }
    }
}
