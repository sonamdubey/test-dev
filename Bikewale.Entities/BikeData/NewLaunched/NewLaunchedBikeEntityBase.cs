﻿
using Bikewale.Entities.Location;
using System;
namespace Bikewale.Entities.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Feb 2017
    /// Description :   NewLaunched Bike Entity Base
    /// Modified By:- subodh jain 09 march 2017
    /// summary:- added BodyStyleId
    /// </summary>
    [Serializable]
    public class NewLaunchedBikeEntityBase : BasicBikeEntityBase
    {
        public uint ReviewCount { get; set; }
        public double ReviewRate { get; set; }
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; }
        public MinSpecsEntity MinSpecs { get; set; }
        public DateTime LaunchedOn { get; set; }
        public uint Price { get; set; }
        public CityEntityBase City { get; set; }
        public uint BodyStyleId { get; set; }
    }
}
