using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;

namespace Bikewale.Models.ExpertReviews
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 17th Aug 2017
    /// Summary: VM for Expert Reviews for scooters page
    /// </summary>
    public class ExpertReviewsScootersPageVM
    {
        public CMSContent Articles { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public PagerEntity PagerEntity { get; set; }
        public string PageH1 { get; set; }
        public string PageH2 { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
    }
}
