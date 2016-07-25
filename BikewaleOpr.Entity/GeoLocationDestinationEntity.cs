using System;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Created By : Sumit Kate On 12 Apr 2016
    /// Description : Entity to handle response from google map API.
    /// </summary>
    public class GeoLocationDestinationEntity : GeoLocationEntity
    {
        public GeoLocationEntity Source { get; set; }
        public string StrDistance { get; set; }
        public double DistanceInKm { get { double d = 0; Double.TryParse(StrDistance, out d); return (d / 1000); } }
        public string AreaDistance { get { return String.Format("{0}:{1}", this.Id, this.DistanceInKm); } }
    }
}