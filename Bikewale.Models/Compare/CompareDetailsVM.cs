
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.PriceQuote;
using Bikewale.Models.Compare;
using System;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 23 May 2017
    /// Summary :- Compare Bike CompareDetailsVM
    /// Created By :Snehal Dange  on 25th Oct 2017
    /// Description : Added 
    /// Modified by sajal Gupta on 30-10-2017
    /// description : added ArticlesList
    /// Modified by sajal Gupta on 07-11-20177
    /// description : added SimilarBikes , SimilarBikesCompareWidgetText 
    /// </summary>
    public class CompareDetailsVM : ModelBase
    {
        public string KnowMoreLinkUrl { get; set; }
        public BikeCompareEntity Compare { get; set; }
        public bool isSponsoredBike { get; set; }
        public Int64 sponsoredVersionId { get; set; }
        public string comparisionText { get; set; }
        public bool isUsedBikePresent { get; set; }
        public string targetModels { get; set; }
        public string compareSummaryText { get; set; }
        public IEnumerable<SimilarCompareBikeEntity> topBikeCompares { get; set; }
        public string templateSummaryTitle { get; set; }
        public RecentExpertReviewsVM ArticlesList { get; set; }
        public PQSourceEnum PQSourceId { get; set; }
        public Bikewale.Comparison.Entities.SponsoredVersionEntityBase SponsoredBike { get; set; }
        public string KnowMoreLinkText { get; set; }

        public SimilarBikesComparisionVM SimilarBikeWidget { get; set; }
        public SimilarBikesWidgetVM SimilarBikes { get; set; }
        public string SimilarBikesCompareWidgetText { get; set; }
        public string DisclaimerText { get; set; }
    }
}
