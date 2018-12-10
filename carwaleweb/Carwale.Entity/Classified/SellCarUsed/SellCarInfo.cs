using Carwale.Entity.Enum;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using System;

namespace Carwale.Entity.Classified.SellCarUsed
{
    [JsonObject, Serializable]
    public class SellCarInfo 
    {
        [JsonIgnore]
        public int TempInquiryId { get; set; }
        public int PinCode { get; set; }
        public int? AreaId { get; set; }
        public bool ShareToCT { get; set; }
        public int ManufactureYear { get; set; }
        public int ManufactureMonth { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public string AlternateFuel { get; set; }
        public string Color { get; set; }
        public int Owners { get; set; }
        public int KmsDriven { get; set; } 
        public int ExpectedPrice { get; set; }
        public int RecommendedPrice { get; set; }
        public InsuranceType? Insurance { get; set; }
        public int? InsuranceExpiryYear { get; set; }
        public int? InsuranceExpiryMonth { get; set; }
        public string RegistrationNumber { get; set; }
        public int SourceId { get; set; }
        public string Referrer { get; set; }
        public string IPAddress { get; set; }
        public int? Mileage { get; set; }
        public string Warranties { get; set; }
        public string Comments { get; set; }
        [JsonIgnore]
        public bool TakeLive { get; set; }
        [JsonIgnore]
        public int? StatusId { get; set; }
        [JsonIgnore]
        public bool? IsArchived { get; set; }
        [JsonIgnore]
        public string DeleteComments { get; set; }
        [JsonIgnore]
        public bool? IsPremium { get; set; }
        [JsonIgnore]
        public string MaskingNumber { get; set; }

        public CarRegistrationType RegType { get; set; }
        public bool? CustomerEditable { get; set; }

    }
}
