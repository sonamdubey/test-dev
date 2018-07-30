using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bikewale.DTO.BikeBooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 04 Feb 2016
    /// Description :   Bike Booking Listing Entity
    /// </summary>
    public class BikeBookingListingDTO
    {
        [JsonProperty("makeEntity")]
        public Make.BBMakeBase MakeEntity { get; set; }
        [JsonProperty("modelEntity")]
        public Model.BBModelBase ModelEntity { get; set; }
        [JsonProperty("versionEntity")]
        public Version.BBVersionBase VersionEntity { get; set; }
        [JsonProperty("bikeName")]
        public string BikeName { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
        [JsonProperty("hasABS")]
        public bool HasABS { get; set; }
        [JsonProperty("hasDisc")]
        public bool HasDisc { get; set; }
        [JsonProperty("hasAlloyWheels")]
        public bool HasAlloyWheels { get; set; }
        [JsonProperty("hasElectricStart")]
        public bool HasElectricStart { get; set; }
        [JsonProperty("mileage")]
        public UInt16 Mileage { get; set; }
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("exShowroom")]
        public uint ExShowroom { get; set; }
        [JsonProperty("discount")]
        public uint Discount { get; set; }
        [JsonProperty("bookingAmount")]
        public uint BookingAmount { get; set; }
        [JsonProperty("popularityIndex")]
        public uint PopularityIndex { get; set; }
        [JsonProperty("onRoadPrice")]
        public uint OnRoadPrice { get; set; }
        [JsonProperty("displacement")]
        public float Displacement { get; set; }
        [JsonProperty("offerCount")]
        public uint OfferCount { get; set; }
        [JsonProperty("discountedPrice")]
        public uint DiscountedPrice { get { return OnRoadPrice - Discount;} }
        [JsonProperty("offers")]
        public List<BikeBookingOfferDTO> lstOffer { get; set; }
    }
}
