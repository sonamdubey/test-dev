
using Bikewale.Entities.Location;
using System;
namespace Bikewale.Entities.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Feb 2017
    /// Description :   NewLaunched Bike Entity Base
    /// </summary>
    [Serializable]
    public class NewLaunchedBikeEntityBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public uint ReviewCount { get; set; }
        public double ReviewRate { get; set; }
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; }
        public MinSpecsEntity MinSpecs { get; set; }
        public DateTime LaunchedOn { get; set; }
        public uint Price { get; set; }
        public CityEntityBase City { get; set; }
    }
}
