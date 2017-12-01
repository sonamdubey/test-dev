﻿using System;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Newtonsoft.Json;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Modified By :Sangram Nandkhile on 29 Dec 2016
    /// Description : Added DisplayTextLarge, DisplayTextSmall
    /// Modified by :   Vivek Singh Tomar on 01st Nov 2017
    /// Description :   Added LeadSourceEnum to capture dealer lead source id
    /// </summary>
    public class NewBikeDealers
    {
        [JsonProperty("dealerId")]
        public UInt32 DealerId { get; set; }

        [JsonProperty("areaId")]
        public UInt32 AreaId { get; set; }

        [JsonProperty("dealerName")]
        public string Name { get; set; }

        [JsonProperty("emailId")]
        public string EmailId { get; set; }

        [JsonProperty("mobileNo")]
        public string MobileNo { get; set; }

        [JsonProperty("phoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty("organization")]
        public string Organization { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("workingTime")]
        public string WorkingTime { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("distance")]
        public string Distance { get; set; }

        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }

        public StateEntityBase objState { get; set; }

        public CityEntityBase objCity { get; set; }

        public AreaEntityBase objArea { get; set; }

        [JsonProperty("dealerPackageType")]
        public DealerPackageTypes DealerPackageType { get; set; }

        [JsonProperty("displayTextLarge")]
        public string DisplayTextLarge { get; set; }

        [JsonProperty("displayTextSmall")]
        public string DisplayTextSmall { get; set; }

        [JsonProperty("isDSA")]
        public bool IsDSA { get; set; }

        [JsonProperty("premiumDealerLeadSourceId")]
        public LeadSourceEnum PremiumDealerLeadSourceId { get; set; }

    }   //End of Class
}   //End of namespace