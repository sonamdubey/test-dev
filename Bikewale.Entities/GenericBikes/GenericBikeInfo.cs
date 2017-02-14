﻿using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;

namespace Bikewale.Entities.GenericBikes
{
    /// <summary>
    /// Created by : Sushil Kuamr on 2nd Jan 2016
    /// DEscription : To store generic bike info related to model
    /// Modified By : Sushil Kumar on 5th Jan 2016
    /// Description : To get genericbike info with min specs
    /// Modified By : Aditi Srivastava on 23 Jan 2017
    /// Summary     : Added estimated min and max price and used, new and futuristic flags
    /// Modified  By :- subodh Jain 10 Feb 2017
    /// Summary :- BikeInfo Slug details 
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
        public uint UserReview { get; set; }
        public bool IsSpecsAvailable { get; set; }
        public MinSpecsEntity MinSpecs { get; set; }
        public uint BikePrice { get; set; }
        public uint EstimatedPriceMin { get; set; }
        public uint EstimatedPriceMax { get; set; }
        public bool IsUsed { get; set; }
        public bool IsNew { get; set; }
        public bool IsFuturistic { get; set; }
        public UInt32 UsedBikeCount { get; set; }
        public uint UsedBikeMinPrice { get; set; }
        public uint DealersCount { get; set; }
        public ICollection<BikeInfoTab> Tabs { get; set; }
    }
    /// Created  By :- subodh Jain 10 Feb 2017
    /// Summary :- BikeInfo Slug details GetBikeInfo
    [Serializable]
    public class BikeInfoTab
    {
        public BikeInfoTabType Tab { get; set; }
        public string TabText { get; set; }
        public string URL { get; set; }
        public string IconText { get; set; }
        public string Title { get; set; }
        public uint Count { get; set; }
        public bool IsVisible { get; set; }
    }
    /// Created  By :- subodh Jain 10 Feb 2017
    /// Summary :- BikeInfo Slug details GetBikeInfo
    public enum BikeInfoTabType
    {
        ExpertReview = 1,
        Image = 2,
        Specs = 3,
        News = 4,
        Videos = 5,
        UserReview = 6,
        Dealers = 7

    }

}
