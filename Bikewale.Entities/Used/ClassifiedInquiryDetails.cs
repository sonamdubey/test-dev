using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 27th August 2016
    /// Description : Bike ClassifiedInquiryDetails entity for used bikes
    /// Modified By : Sushil Kumar on 17th August 2016
    /// Description : Added AdStatus and CustomerId for sold bikes scenario
    /// </summary>
    [Serializable]
    public class ClassifiedInquiryDetails //: BasicBikeEntityBase
    {
        public BikeVersionEntityBase Version { get; set; }
        public StateEntityBase State { get; set; }
        public CityEntityBase City { get; set; }
        public BikeDetailsMin MinDetails { get; set; }
        public BikeDetails OtherDetails { get; set; }
        public IList<BikePhoto> Photo { get; set; }
        public ushort PhotosCount { get { if (Photo != null) { return Convert.ToUInt16(Photo.Count); } return 0; } }
        public short AdStatus { get; set; }
        public uint CustomerId { get; set; }

        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsUpcoming { get; set; }
        public IList<SpecsItem> VersionMinSpecs { get; set; }
    }
}
