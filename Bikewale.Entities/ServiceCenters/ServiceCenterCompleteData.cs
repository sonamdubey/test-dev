using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.ServiceCenters
{
    /// <summary>
    /// Created By :  Sajal Gupta
    /// Created On  : 08 Nov 2016
    /// Description : Service center complete data for city details page.
    /// </summary>

    [Serializable, DataContract]
    public class ServiceCenterCompleteData
    {
        [DataMember]
        public uint Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public uint CityId { get; set; }

        [DataMember]
        public string CityName { get; set; }

        [DataMember]
        public string CityMaskingName { get; set; }

        [DataMember]
        public uint StateId { get; set; }

        [DataMember]
        public uint AreaId { get; set; }

        [DataMember]
        public string Pincode { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Lattitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public uint MakeId { get; set; }

        [DataMember]
        public uint DealerId { get; set; }

        [DataMember]
        public uint IsActive { get; set; }
    }
}
