﻿using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Bike Details and Price Details
    /// Author  :   Sumit Kate
    /// Created :   10 Sept 2015
    /// </summary>
    public class BikeDealerPriceDetail
    {
        /// <summary>
        /// Image Host URL
        /// </summary>
        public string HostUrl { get; set; }
        /// <summary>
        /// Image Path
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// On Road Price
        /// </summary>
        public uint OnRoadPrice { get; set; }
        /// <summary>
        /// Booking Amount
        /// </summary>
        public uint BookingAmount { get; set; }
        /// <summary>
        /// Waiting Days
        /// </summary>
        public short NoOfWaitingDays { get; set; }
        /// <summary>
        /// Price breakup list
        /// </summary>
        public IList<DealerVersionPriceItemEntity> PriceList { get; set; }
        /// <summary>
        /// Bike Version with min specifications
        /// </summary>
        public BikeVersionMinSpecs MinSpec { get; set; }
        /// <summary>
        /// Make entity
        /// </summary>
        public BikeMakeEntityBase Make { get; set; }
        /// <summary>
        /// Model Entity
        /// </summary>
        public BikeModelEntityBase Model { get; set; }
        /// <summary>
        /// Bike Model Colors
        /// </summary>
        public IEnumerable<BikeVersionColorsWithAvailability> BikeModelColors { get; set; }
    }
}
