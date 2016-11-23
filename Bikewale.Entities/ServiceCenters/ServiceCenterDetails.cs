using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.ServiceCenters
{
    /// <summary>
    /// Created By :  Sajal Gupta
    /// Created On  : 07 Nov 2016
    /// Description : Service center data on city listing page.
    /// Modified by Sajal Gupta on 16-11-2016 - added Lattitude, Longitude
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

        [DataMember]
        public string Lattitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }
    }
}
