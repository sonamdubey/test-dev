using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.ServiceCenters
{
    /// <summary>
    /// Created By :  Sajal Gupta
    /// Created On  : 16 Nov 2016
    /// Description : Service center sms data.
    /// </summary>

    [Serializable, DataContract]
    public class ServiceCenterSMSData
    {
        [DataMember]
        public EnumServiceCenterSMSStatus SMSStatus { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public uint CityId { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string CityName { get; set; }
    }
}
