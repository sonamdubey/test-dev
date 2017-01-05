using Bikewale.Entities.BikeData;
using System;
namespace Bikewale.Entities.NewBikeSearch
{
    /// <summary>
    /// Modified by : Sajal Gupta on 02-01-2017
    /// Description : Added LastUpdatedModelSold
    /// Modified by : Sajal Gupta on 05-01-2017
    /// Deac : Added NewsCount
    /// /// </summary>
    [Serializable]
    public class SearchOutputEntityBase
    {
        public string BikeName { get; set; }
        public float Displacement { get; set; }
        public string FuelType { get; set; }
        public string Power { get; set; }
        public ushort FuelEfficiency { get; set; }
        public ushort KerbWeight { get; set; }
        public float MaximumTorque { get; set; }
        public string FinalPrice { get; set; }
        public string AvailableSpecs { get; set; }
        public BikeModelEntity BikeModel { get; set; }
        public string SmallDescription { get; set; }
        public string FullDescription { get; set; }
        public uint UnitsSold { get; set; }
        public DateTime LaunchedDate { get; set; }
        public uint PhotoCount { get; set; }
        public uint VideoCount { get; set; }
        public uint VersionCount { get; set; }
        public uint ColorCount { get; set; }
        public DateTime? LastUpdatedModelSold { get; set; }
        public uint NewsCount { get; set; }
    }
}
