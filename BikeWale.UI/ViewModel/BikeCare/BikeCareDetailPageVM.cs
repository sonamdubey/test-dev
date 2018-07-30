using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Models.Shared;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 31 Mar 2017
    /// Summary    : View model for Bike care details page
    /// </summary>
    public class BikeCareDetailPageVM : ModelBase
    {
        public ArticlePageDetails ArticleDetails { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public EditCMSPhotoGalleryVM PhotoGallery { get; set; }
        public IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets { get; set; }
    }
}
