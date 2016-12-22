using Bikewale.Entities.Location;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 21 Jun 2016
    /// Description :   Popular City Dealers
    /// Modified by :  Subodh Jain on 21 Dec 2016
    /// Description :  Added  service center count
    /// </summary>
    [Serializable, DataContract]
    public class PopularCityDealerEntity : CityEntityBase
    {

        [DataMember]
        public uint DealerCount { get; set; }
        [DataMember]
        public uint ServiceCenterCount { get; set; }


    }
}
