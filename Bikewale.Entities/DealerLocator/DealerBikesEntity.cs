using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 21 March 2016
    /// Description : Dealer Deatail Entity contail bike Models available at specific dealer and dealers deatail. 
    /// </summary>
    [Serializable, DataContract]
    public class DealerBikesEntity
    {
        [DataMember]
        public DealerDetailEntity DealerDetails { get; set; }
        [DataMember]
        public IEnumerable<MostPopularBikesBase> Models { get; set; }
    }
}
