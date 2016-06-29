using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.DealerLocator
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 21 March 2016
    /// Description : for Dealer Detail.
    /// </summary>
    [Serializable, DataContract]
    public class DealerDetailEntity : NewBikeDealerBase
    {
        [DataMember]
        public AreaEntityBase Area { get; set; }
        [DataMember]
        public UInt16 DealerType { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string EMail { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string WorkingHours { get; set; }
    }
}
