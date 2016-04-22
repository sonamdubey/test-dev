using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Entities.PriceQuote
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 18 Apr 2016
    /// Summary   : To hold entities which will be used for Model API v3 mapper
    /// </summary>
    public class PQByCityAreaEntity
    {
        public bool IsCityExists { get; set; }
        public bool IsAreaExists { get; set; }
        public bool IsAreaSelected { get; set; }
        public bool IsExShowroomPrice { get; set; }
        public IEnumerable<BikeVersionMinSpecs> VersionList { get; set; }
        public uint DealerId { get; set; }
        public ulong PqId { get; set; }
    }
}
