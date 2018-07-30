using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Models.Shared;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Modified by : Snehal Dange on 25th April 2018
    /// Description: Added PageWidgets
    /// </summary>
    public class DetailFeatureVM : ModelBase
    {
        public ArticlePageDetails objFeature { get; set; }
        public BikeMakeEntityBase objMake { get; set; }
        public BikeModelEntityBase objModel { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public EditCMSPhotoGalleryVM PhotoGallery { get; set; }
        public IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets { get; set; }
    }
}
