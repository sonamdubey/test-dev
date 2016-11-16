using Newtonsoft.Json;
using System;
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
        [Required]
        [JsonProperty("versionId")]
        public ushort? VersionId { get; set; }
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
        [JsonProperty("seller")]
        public SellBikeAdSellerDTO Seller { get; set; }
        [JsonProperty("otherInfo")]
        public SellBikeAdOtherInformationDTO OtherInfo { get; set; }
        [JsonProperty("photoCount")]
        public ushort PhotoCount { get; set; }
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
    }

}
