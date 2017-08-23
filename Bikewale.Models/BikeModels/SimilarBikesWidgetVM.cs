﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 24 Mar 2017
    /// Description :   SimilarBikesWidgetVM view model
    /// Created by: Vivek Singh Tomar on 23 Aug 2017
    /// Description: Added ExploreMoreLink to hold link to new landing page and Page to hold page id and page name
    /// </summary>
    public class SimilarBikesWidgetVM
    {
        public PQSourceEnum PQSourceId { get; set; }
        public bool ShowCheckOnRoadCTA { get; set; }
        public bool ShowPriceInCityCTA { get; set; }
        public IEnumerable<SimilarBikeEntity> Bikes { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public uint VersionId { get; set; }
        public string ExploreMoreLink { get; set; }
        public GAPages Page { get; set; }
    }
}
