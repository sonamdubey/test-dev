
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By  : subodh jain on 19 Dec 2016
    /// Description : To hold dealer info by brand
    /// </summary>
    [Serializable, DataContract]

    public class DealerBrandEntity
    {

        [DataMember]
        public int MakeId { get; set; }

        [DataMember]
        public string MakeName { get; set; }

        [DataMember]
        public string MakeMaskingName { get; set; }

        [DataMember]
        public int DealerCount { get; set; }

        [DataMember]
        public string LogoURL { get; set; }

        [DataMember]
        public string HostURL { get; set; }

    }
}
