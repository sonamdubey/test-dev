using System;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Modified by :   Sumit Kate on 01 Aug 2016
    /// Description :   Added Area Id
    /// </summary>
    public class PQParameterEntity
    {
        public UInt32 VersionId { get; set; }
        public UInt32 CityId { get; set; }
        public UInt32 DealerId { get; set; }
        public uint AreaId { get; set; }
    }
}
