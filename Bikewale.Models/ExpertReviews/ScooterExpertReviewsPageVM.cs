using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 17th Aug 2017
    /// Summary: VM for Expert Reviews for scooters page
    /// Modified by sajal Gupta on 23-08-2017
    /// Description : added PopularScooterMakesWidget
    /// </summary>
    public class ScooterExpertReviewsPageVM: ModelBase
    {
        public CMSContent Articles { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public PagerEntity PagerEntity { get; set; }
        public string PageH1 { get; set; }
        public MostPopularBikeWidgetVM MostPopularScooters { get; set; }       
        public IEnumerable<BikeMakeEntityBase> PopularScooterMakesWidget { get; set; }
        public string PopularScooterBrandsWidgetHeading { get; set; }
    }
}
