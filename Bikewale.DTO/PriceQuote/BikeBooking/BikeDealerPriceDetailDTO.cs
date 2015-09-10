﻿using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.PriceQuote.BikeBooking
{
    /// <summary>
    /// Bike Details and Price Details
    /// Author  :   Sumit Kate
    /// Created :   10 Sept 2015
    /// </summary>
    public class BikeDealerPriceDetailDTO
    {
        /// <summary>
        /// Image Host URL
        /// </summary>
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        /// <summary>
        /// Image Path
        /// </summary>
        [JsonProperty("imagePath")]
        public string ImagePath { get; set; }
        /// <summary>
        /// On Road Price
        /// </summary>
        [JsonProperty("onRoadPrice")]
        public uint OnRoadPrice { get; set; }
        /// <summary>
        /// Booking Amount
        /// </summary>
        [JsonProperty("bookingAmount")]
        public uint BookingAmount { get; set; }
        /// <summary>
        /// Waiting Days
        /// </summary>
        [JsonProperty("noOfWaitingDays")]
        public uint NoOfWaitingDays { get; set; }

        /// <summary>
        /// Price breakup list
        /// </summary>
        [JsonProperty("priceList")]
        public IList<DealerVersionPriceItemDTO> PriceList { get; set; }

        /// <summary>
        /// Bike Version with min specifications
        /// </summary>
        [JsonProperty("minSpec")]
        public VersionMinSpecs MinSpec { get; set; }
        /// <summary>
        /// Make entity
        /// </summary>
        [JsonProperty("make")]
        public MakeBase Make { get; set; }
        /// <summary>
        /// Model Entity
        /// </summary>
        [JsonProperty("model")]
        public ModelBase Model { get; set; }
    }
}
