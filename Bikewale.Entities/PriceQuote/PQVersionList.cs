using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote
{
    public class PQVersionList
    {
        public string BrakeType { get; set; }

        public bool AlloyWheels { get; set; }

        public bool ElectricStart { get; set; }

        public bool AntilockBrakingSystem { get; set; }

        public int VersionId { get; set; }

        public string VersionName { get; set; }

        public UInt64 Price { get; set; }

        public bool IsDealerPriceQuote { get; set; }

    }

    /// <summary>
    /// Created By : Sangram
    /// Created On : 15 Apr 2016 
    /// </summary>
    public class PQByCityAreaEntity
    {
        public bool IsCityExists { get; set; }
        public bool IsAreaExists { get; set; }
        public bool IsAreaSelected { get; set; }
        public bool IsExShowroomPrice { get; set; }
        public IEnumerable<BikeVersionMinSpecs> VersionList { get; set; }
    }
}
