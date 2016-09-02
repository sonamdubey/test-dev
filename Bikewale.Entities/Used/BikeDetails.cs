
using Bikewale.Entities.BikeData;
using System;
namespace Bikewale.Entities.Used
{
    /// <summary>
    /// Created By : Sushil Kumar on 27th August 2016
    /// Description : Bike Details entity for used bikes
    /// </summary>
    [Serializable]
    public class BikeDetails
    {
        public uint Id { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string Seller { get; set; }
        public VersionColor Color { get; set; }
        public string RegisteredAt { get; set; }
        public string Insurance { get; set; }
        public string Description { get; set; }
        public string RegistrationNo { get; set; }
    }
}
