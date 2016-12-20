
using Bikewale.Entities.BikeData;
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By  : subodh jain on 19 Dec 2016
    /// Description : To hold dealer info by brand
    /// </summary>
    [Serializable, DataContract]

    public class DealerBrandEntity : BikeMakeEntityBase
    {
        [DataMember]
        public int DealerCount { get; set; }

    }
}
