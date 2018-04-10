using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Modified By :Sangram Nandkhile on 29 Dec 2016
    /// Description : Added DisplayTextLarge, DisplayTextSmall
    /// </summary>
    [Serializable, DataContract]
    public class NewBikeDealers
    {
        [JsonProperty("dealerId"), DataMember]
        public UInt32 DealerId { get; set; }

        [JsonProperty("areaId"), DataMember]
        public UInt32 AreaId { get; set; }

        [JsonProperty("dealerName"), DataMember]
        public string Name { get; set; }

        [JsonProperty("emailId"), DataMember]
        public string EmailId { get; set; }

        [JsonProperty("mobileNo"), DataMember]
        public string MobileNo { get; set; }

        [JsonProperty("phoneNo"), DataMember]
        public string PhoneNo { get; set; }

        [JsonProperty("organization"), DataMember]
        public string Organization { get; set; }

        [JsonProperty("website"), DataMember]
        public string Website { get; set; }

        [JsonProperty("workingTime"), DataMember]
        public string WorkingTime { get; set; }

        [JsonProperty("address"), DataMember]
        public string Address { get; set; }

        [JsonProperty("distance"), DataMember]
        public string Distance { get; set; }

        [JsonProperty("maskingNumber"), DataMember]
        public string MaskingNumber { get; set; }

        [DataMember]
        public StateEntityBase objState { get; set; }
        [DataMember]
        public CityEntityBase objCity { get; set; }
        [DataMember]
        public AreaEntityBase objArea { get; set; }

        [JsonProperty("dealerPackageType"), DataMember]
        public DealerPackageTypes DealerPackageType { get; set; }

        [JsonProperty("displayTextLarge"), DataMember]
        public string DisplayTextLarge { get; set; }

        [JsonProperty("displayTextSmall"), DataMember]
        public string DisplayTextSmall { get; set; }

        [JsonProperty("isDSA"), DataMember]
        public bool IsDSA { get; set; }

    }   //End of Class
}   //End of namespace