using Newtonsoft.Json;
using System;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Modified by :   Sumit Kate on 14 Mar 2016
    /// Description :   Added new property. MaskingNumber, DealerPackageType
    /// </summary>
    public class NewBikeDealersDTO
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

        public StateEntityBaseDTO objState { get; set; }

        public CityEntityBaseDTO objCity { get; set; }

        public AreaEntityBaseDTO objArea { get; set; }
        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }

        [JsonProperty("dealerPackageType")]
        public DealerPackageTypes DealerPackageType { get; set; }
    }   //End of Class
}   //End of namespace
