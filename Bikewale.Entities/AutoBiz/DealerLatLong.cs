using System;

namespace BikeWale.Entities.AutoBiz
{
    public class DealerLatLong
    {
        public uint DealerId { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public UInt16 ServingDistance { get; set; }
    }
}
