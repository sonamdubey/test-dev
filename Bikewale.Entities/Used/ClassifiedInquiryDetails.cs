using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.Used
{
    [Serializable]
    public class ClassifiedInquiryDetails
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public BikeVersionEntityBase Version { get; set; }
        public StateEntityBase State { get; set; }
        public CityEntityBase City { get; set; }
        public IList<BikePhoto> Photo { get; set; }
        public ushort PhotosCount { get { if (Photo != null) { return Convert.ToUInt16(Photo.Count); } return 0; } }
    }
}
