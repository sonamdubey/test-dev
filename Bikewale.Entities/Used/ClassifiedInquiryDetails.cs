using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 27th August 2016
    /// Description : Bike ClassifiedInquiryDetails entity for used bikes
    /// </summary>
    [Serializable]
    public class ClassifiedInquiryDetails
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public BikeVersionEntityBase Version { get; set; }
        public StateEntityBase State { get; set; }
        public CityEntityBase City { get; set; }
        public BikeDetailsMin MinDetails { get; set; }
        public BikeDetails OtherDetails { get; set; }
    }
}
