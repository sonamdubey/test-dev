using Bikewale.DTO.BikeBooking.Make;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.DTO.BikeBooking.Version;
using Bikewale.DTO.Used.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Bikewale.DTO.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 14 Oct 2016
    /// Description :   Sell Bike Ad DTO
    /// </summary>
    public class SellBikeAdDTO
    {
        [JsonProperty("profileId")]
        public string ProfileId { get; set; }
        [JsonProperty("InquiryId")]
        public uint? InquiryId { get; set; }
        [JsonProperty("make")]
        public BBMakeBase Make { get; set; }
        [JsonProperty("model")]
        public BBModelBase Model { get; set; }
        [Required]
        [JsonProperty("version")]
        public BBVersionBase Version { get; set; }
        [Required]
        [JsonProperty("manufacturingYear")]
        public DateTime? ManufacturingYear { get; set; }
        [Required]
        [JsonProperty("kiloMeters")]
        public uint? KiloMeters { get; set; }
        [Required]
        [JsonProperty("cityId")]
        public uint? CityId { get; set; }
        [Required]
        [JsonProperty("expectedprice")]
        public UInt64? Expectedprice { get; set; }
        [Required]
        [JsonProperty("owner")]
        public ushort? Owner { get; set; }
        [Required]
        [JsonProperty("registrationPlace")]
        public string RegistrationPlace { get; set; }
        [Required]
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("colorId")]
        public uint? ColorId { get; set; }
        [Required]
        [JsonProperty("sourceId")]
        public ushort? SourceId { get; set; }
        [Required]
        [JsonProperty("pageUrl")]
        public string PageUrl { get; set; }
        [JsonProperty("status")]
        public SellAdStatus Status { get; set; }
        [JsonProperty("seller")]
        public SellerDTO Seller { get; set; }
        [JsonProperty("otherInfo")]
        public SellBikeAdOtherInformationDTO OtherInfo { get; set; }
        [JsonProperty("photoCount")]
        public ushort PhotoCount { get; set; }
        [JsonProperty("photos")]
        public IEnumerable<BikePhoto> Photos { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 14 Oct 2016
    /// Description :   Sell Bike Ad Other Information DTO
    /// </summary>
    public class SellBikeAdOtherInformationDTO
    {
        [JsonProperty("registrationNo")]
        public string RegistrationNo { get; set; }
        [JsonProperty("insuranceType")]
        public string InsuranceType { get; set; }
        [JsonProperty("adDescription")]
        public string AdDescription { get; set; }
        [JsonProperty("seller")]
        public SellerDTO Seller { get; set; }
    }

}
