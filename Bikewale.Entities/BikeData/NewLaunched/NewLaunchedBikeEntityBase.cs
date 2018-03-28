
using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.BikeData.NewLaunched
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Feb 2017
    /// Description :   NewLaunched Bike Entity Base
    /// Modified By:- subodh jain 09 march 2017
    /// summary:- added BodyStyleId
    /// Modified by : Sanskar Gupta on 08 Feb 2018
    /// Descritpion : Added AvgPrice
    /// Modified by : Sanskar Gupta on 12 Feb 2018
    /// Description : Added ExshowroomPrice, VersionPrice, AvgPrice
    /// Modified by : Pratibha Verma on 28 Mar 2018
    /// Description : Added VersionId and SpecsItem
    /// </summary>
    [Serializable]
    public class NewLaunchedBikeEntityBase //: BasicBikeEntityBase
    {
        public uint ReviewCount { get; set; }
        public double ReviewRate { get; set; }
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; }
        public DateTime LaunchedOn { get; set; }
        public uint Price { get; set; }
        public CityEntityBase City { get; set; }
        public uint BodyStyleId { get; set; }

        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImagePath { get; set; }
        public string HostUrl { get; set; }
        public bool IsDiscontinued { get; set; }
        public bool IsUpcoming { get; set; }

        public uint VersionPrice { get; set; }

        public uint AvgPrice { get; set; }

        public uint ExshowroomPrice { get; set; }
        public int VersionId { get; set; }
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
    }
}
