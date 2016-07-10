using System;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Crated By : Lucky Rathore on 12 Apr 2016
    /// Description : Dealer Location Entity.
    /// </summary>
    public class DealerLocationEntity
    {
        public UInt16 DealerId { get; set; }
        public GeoLocationEntity location { get; set; }
    }
}