﻿
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Compare;
using Bikewale.Entities.PriceQuote;
using System;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 23 May 2017
    /// Summary :- Compare Bike CompareDetailsVM
    /// </summary>
    public class CompareDetailsVM : ModelBase
    {
        public string FeaturedBike { get; set; }
        public BikeCompareEntity Compare { get; set; }
        public bool isSponsoredBike { get; set; }
        public Int64 sponsoredVersionId { get; set; }
        public string comparisionText { get; set; }
        public bool isUsedBikePresent { get; set; }
        public string targetModels { get; set; }
        public string compareSummaryText { get; set; }
        public IEnumerable<SimilarCompareBikeEntity> topBikeCompares { get; set; }
        public string templateSummaryTitle { get; set; }
        public IEnumerable<ArticleSummary> ArticlesList { get; set; }
        public PQSourceEnum PQSourceId { get; set; }

    }
}
