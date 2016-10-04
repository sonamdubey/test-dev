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
    /// Modeified By : Sajal Gupta on 26-09-2016
    /// Description : Added CityId, CampaignId, MakeName, MakeId, MakeMaskingName, CityMaskingName.
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
        [DataMember]
        public int CityId { get; set; }
        [DataMember]
        public uint CampaignId { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public int MakeId { get; set; }
        [DataMember]
        public string MakeMaskingName { get; set; }
        [DataMember]
        public string CityMaskingName { get; set; }
    }
}
