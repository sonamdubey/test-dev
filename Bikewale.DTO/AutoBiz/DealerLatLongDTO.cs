using System;

namespace BikeWale.DTO.AutoBiz
{
    public class DealerLatLongDTO
    {
        public uint DealerId { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public UInt16 ServingDistance { get; set; }
    }
}
