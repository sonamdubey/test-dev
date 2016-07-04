using Bikewale.DTO.AutoBiz;
using System;
using System.Collections.Generic;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Author  :   Sumit Kate
    /// Created :   08 Oct 2015
    /// Description :   Bike Version with On Road Price
    /// </summary>
    public class PQ_BikeVarientDTO
    {
        public MakeEntityBaseDTO objMake { get; set; }
        public ModelEntityBaseDTO objModel { get; set; }
        public VersionEntityBaseDTO objVersion { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public UInt32 OnRoadPrice { get; set; }
        public UInt32 BookingAmount { get; set; }
        public IList<PQ_PriceDTO> PriceList { get; set; }
    }
}