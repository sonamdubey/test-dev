using Bikewale.Entities.BikeData;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By : Ashwini Todkar on 4 June 2014
    /// </summary>
    [Serializable, DataContract]
    public class NewBikeDealersMakeEntity : BikeMakeEntityBase
    {
        [DataMember]
        public int DealersCount { get; set; }
    }
}
