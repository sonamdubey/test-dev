using Bikewale.Entities.BikeBooking;
using BikeWale.Entities.AutoBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeWale.Entities.AutoBiz
{
    public class PQ_VersionPrice : PQ_Price
    {
        public UInt32 VersionId { get; set; }
    }
}