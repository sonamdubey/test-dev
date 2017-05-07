﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using System;

namespace Bikewale.Entities.GenericBikes
{
    /// <summary>
    /// Created By : Sushil Kumar on 22nd DEc 2016
    /// Description : Entity for bikewale generic pages
    /// Modified by : Sajal Gupta on 02-01-2017
    /// Description : Added LastUpdatedModelSold
    /// Modified By  : SAJAL GUPTA on 05-01-2017
    /// Desc : Added NewsCount property
    /// Modified by : Sajal Gupta on 02-02-2017
    /// Description : Added UsedBikesCount, PriceInCity and city property
    /// </summary>
    [Serializable]
    public class BestBikeEntityBase : BasicBikeEntityBase
    {
        public string BikeName { get; set; }
        public uint TotalVersions { get; set; }
        public uint TotalModelColors { get; set; }
        public MinSpecsEntity MinSpecs { get; set; }
        public DateTime? LaunchDate { get; set; }
        public uint UnitsSold { get; set; }
        public uint PhotosCount { get; set; }
        public uint VideosCount { get; set; }
        public uint ExpertReviewsCount { get; set; }
        public uint Price { get; set; }
        public string FullModelDescription { get; set; }
        public string SmallModelDescription { get; set; }
        public string Description { get { return String.Concat(SmallModelDescription, FullModelDescription); } }
        public DateTime? LastUpdatedModelSold { get; set; }
        public uint NewsCount { get; set; }
        public uint UsedBikesCount { get; set; }
        public CityEntityBase UsedCity { get; set; }
        public string PriceInCity { get; set; }
    }
}
