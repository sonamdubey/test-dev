using System;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Created By : Sumit Kate On 12 Apr 2016
    /// Description : Geo Location Entity.
    /// </summary>
    public class GeoLocationEntity
    {
        public UInt16 Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}