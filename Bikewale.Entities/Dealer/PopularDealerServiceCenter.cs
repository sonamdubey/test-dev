using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created by  :   Subodh Jain on 21 dec 2016
    /// Description :   Popular City Service Center & dealer
    /// </summary>
    [Serializable, DataContract]
    public class PopularDealerServiceCenter
    {
        [DataMember]
        public ICollection<PopularCityDealerEntity> DealerDetails { get; set; }

        [DataMember]
        public uint TotalDealerCount { get; set; }
        [DataMember]
        public uint TotalServiceCenterCount { get; set; }
    }
}
