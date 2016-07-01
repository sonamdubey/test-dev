using BikeWale.Entities.AutoBiz;
using System;
using System.Collections.Generic;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Author  :   Sumit Kate
    /// Created :   08 Oct 2015
    /// Description :   Bike Version with On Road Price
    /// </summary>
    public class PQ_BikeVarient
    {
        public MakeEntityBase objMake { get; set; }
        public ModelEntityBase objModel { get; set; }
        public VersionEntityBase objVersion { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public UInt32 OnRoadPrice { get; set; }
        public UInt32 BookingAmount { get; set; }
        public IList<PQ_Price> PriceList { get; set; }
    }
}