using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using System.Collections.Generic;

namespace Bikewale.Models.News
{
    /// <summary>
    /// Created by : Snehal Dange on 17th August ,2017
    /// Summary    : View model for scooters news listing page
    /// </summary>
    public class NewsScootersPageVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public CMSContent Articles { get; set; }
        public PagerEntity PagerEntity { get; set; }
        public string PageH1 { get; set; }
        public string PageH2 { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public IEnumerable<BikeMakeEntityBase> PopularScooterMakesWidget { get; set; }
        public string PopularScooterBrandsWidgetHeading { get; set; }

    }
}
