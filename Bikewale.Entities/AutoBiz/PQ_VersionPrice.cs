using Bikewale.Entities.BikeBooking;
using System;

namespace BikeWale.Entities.AutoBiz
{
    [Serializable]
    public class PQ_VersionPrice : PQ_Price
    {
        public UInt32 VersionId { get; set; }
    }
}