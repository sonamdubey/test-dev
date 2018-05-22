using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.MobileVerification
{
    /// <summary>
    /// Created By :  Sajal Gupta
    /// Created On  : 16 Nov 2016
    /// Description : Sms data.
    /// Modifier    : Kartik Rathod  on 30 apl 2018 added Pincode
    /// </summary>

    [Serializable, DataContract]
    public class SMSData
    {
        [DataMember]
        public EnumSMSStatus SMSStatus { get; set; }

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

        [DataMember]
        public string Area { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public string MakeName { get; set; }

        [DataMember]
        public string PinCode { get; set; }
    }
}
