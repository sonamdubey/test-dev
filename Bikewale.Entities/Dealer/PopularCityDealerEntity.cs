using Bikewale.Entities.Location;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created by  :   Sumit Kate on 21 Jun 2016
    /// Description :   Popular City Dealers
    /// </summary>
    [Serializable, DataContract]
    public class PopularCityDealerEntity
    {
        [DataMember]
        public CityEntityBase CityBase { get; set; }
        [DataMember]
        public uint NumOfDealers { get; set; }
    }
}
