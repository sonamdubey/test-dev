using System;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Crated By : Lucky Rathore on 12 Apr 2016
    /// Description : Area With its Lattitude and Longitude Entity.
    /// </summary>
    public class AreaLocationEntity
    {
        public UInt16 AreaId { get; set; }
        public GeoLocationEntity location { get; set; }
    }
}