using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.CarDetails
{
    public class CarAdditionalInfo
    {
        [JsonProperty("isCarInWarranty")]
        public string IsCarInWarranty { get; set; }

        [JsonProperty("warrantyValidTill")]
        public string WarrantyValidTill { get; set; }

        [JsonProperty("warrantyProvidedBy")]
        public string WarrantyProvidedBy { get; set; }

        [JsonProperty("warrantyDetails")]
        public string WarrantyDetails { get; set; }

        [JsonProperty("hasExtendedWarranty")]
        public string HasExtendedWarranty { get; set; }

        [JsonProperty("extendedWarrantyValidFor")]
        public string ExtendedWarrantyValidFor { get; set; }

        [JsonProperty("extendedWarrantyProviderName")]
        public string ExtendedWarrantyProviderName { get; set; }

        [JsonProperty("extendedWarrantyDetails")]
        public string ExtendedWarrantyDetails { get; set; }

        [JsonProperty("hasAnyServiceRecords")]
        public string HasAnyServiceRecords { get; set; }

        [JsonProperty("serviceRecordsAvailableFor")]
        public string ServiceRecordsAvailableFor { get; set; }

        [JsonProperty("disclaimer")]
        public string Disclaimer { get; set; }

        [JsonProperty("hasRSAAvailable")]
        public string HasRSAAvailable { get; set; }

        [JsonProperty("rsaValidTill")]
        public string RSAValidTill { get; set; }

        [JsonProperty("rsaProviderName")]
        public string RSAProviderName { get; set; }

        [JsonProperty("rsaDetails")]
        public string RSADetails { get; set; }

        [JsonProperty("hasFreeRSA")]
        public string HasFreeRSA { get; set; }

        [JsonProperty("freeRSAValidFor")]
        public string FreeRSAValidFor { get; set; }

        [JsonProperty("freeRSAProvidedBy")]
        public string FreeRSAProvidedBy { get; set; }

        [JsonProperty("freeRSADetails")]
        public string FreeRSADetails { get; set; }

        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }

        [JsonProperty("modifications")]
        public string Modifications { get; set; }

        [JsonProperty("individualWarranty")]
        public string IndividualWarranty { get; set; }

        [JsonProperty("individualModifications")]
        public string IndividualModifications { get; set; }
    }
}
