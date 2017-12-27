using Bikewale.Entities.BikeBooking;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.DealerLocator
{
    /// <summary>
    /// Created by  :   Sumit Kate on 27 Dec 2017
    /// Description :   Dealer Versions price list
    /// </summary>
    [System.Serializable, DataContract]
    public class DealerVersionPrices
    {
        [DataMember]
        public ICollection<PQ_Price> PriceList { get; set; }
        [DataMember]
        public uint OnRoadPrice { get; set; }
    }
}
