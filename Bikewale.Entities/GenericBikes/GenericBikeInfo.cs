﻿using Bikewale.Entities.BikeData;
using System;

namespace Bikewale.Entities.GenericBikes
{
    /// <summary>
    /// Created by : Sushil Kuamr on 2nd Jan 2016
    /// DEscription : To store generic bike info related to model
    /// Modified By : Sushil Kumar on 5th Jan 2016
    /// Description : To get genericbike info with min specs
    /// Modified By : Aditi Srivastava on 23 Jan 2017
    /// Summary     : Added estimated min and max price and used, new and futuristic flags
    /// </summary>
    [Serializable]
    public class GenericBikeInfo
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImagePath { get; set; }
        public uint VideosCount { get; set; }
        public uint PhotosCount { get; set; }
        public uint NewsCount { get; set; }
        public uint ExpertReviewsCount { get; set; }
        public uint FeaturesCount { get; set; }
        public bool IsSpecsAvailable { get; set; }
        public MinSpecsEntity MinSpecs { get; set; }
        public uint BikePrice { get; set; }
        public uint EstimatedPriceMin { get; set; }
        public uint EstimatedPriceMax { get; set; }
        public bool IsUsed { get; set; }
        public bool IsNew { get; set; }
        public bool IsFuturistic { get; set; }
    }
}
