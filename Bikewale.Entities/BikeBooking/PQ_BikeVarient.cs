using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Author  :   Sumit Kate
    /// Created :   08 Oct 2015
    /// Description :   Bike Version with On Road Price
    /// </summary>
    public class PQ_BikeVarient
    {
        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public BikeVersionEntityBase objVersion { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public UInt32 OnRoadPrice { get; set; }
        public UInt32 BookingAmount { get; set; }
        public IList<PQ_Price> PriceList { get; set; }
    }
}
