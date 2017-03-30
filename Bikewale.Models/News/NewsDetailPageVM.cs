using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 29 Mar 2017
    /// Summary    : View Model for news details page
    /// </summary>
    public class NewsDetailPageVM : ModelBase
    {
        public ArticleDetails ArticleDetails { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public BikeInfoVM BikeInfo { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string BaseUrl { get; set; }
    }
}
