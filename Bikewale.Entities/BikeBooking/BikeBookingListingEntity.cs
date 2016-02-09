using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.BikeData;
namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Created by  :   Sumit Kate on 04 Feb 2016
    /// Description :   Bike Booking Listing Entity
    /// </summary>
    public class BikeBookingListingEntity
    {
        public BikeMakeEntityBase MakeEntity { get; set; }
        public BikeModelEntityBase ModelEntity { get; set; }
        public BikeVersionEntityBase VersionEntity { get; set; }
        
        public string BikeName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }        
        public bool HasABS { get; set; }
        public bool HasDisc { get; set; }
        public bool HasAlloyWheels { get; set; }
        public bool HasElectricStart { get; set; }
        public UInt16 Mileage { get; set; }
        public uint DealerId { get; set; }
        public uint ExShowroom { get; set; }
        public uint Discount { get; set; }
        public uint BookingAmount { get; set; }
        public uint PopularityIndex { get; set; }
        public int OnRoadPrice { get; set; }
        public float Displacement { get; set; }
        public UInt16 OfferCount { get { if (lstOffer != null) { return Convert.ToUInt16(lstOffer.Count); } return 0; } }
        public List<PQ_Price> PriceList { get; set; }
        public List<OfferEntity> lstOffer { get; set; }
    }
}
