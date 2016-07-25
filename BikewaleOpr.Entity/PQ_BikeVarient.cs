using BikewaleOpr.Entities;
using System;
using System.Collections.Generic;

namespace BikewaleOpr.Entities
{
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
