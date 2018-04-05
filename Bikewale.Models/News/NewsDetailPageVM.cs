using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Models.BikeModels;
using Bikewale.Models.Shared;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 29 Mar 2017
    /// Summary    : View Model for news details page
    /// Modified by : Rajan Chauhan on 26 Feb 2018
    /// Description : Added base class CMSArticleDetailPageVM
    /// </summary>
    public class NewsDetailPageVM : CMSArticleDetailPageVM
    {
        public ArticleDetails ArticleDetails { get; set; }
        public string BaseUrl { get; set; }

        public EditorialSeriesWidgetVM SeriesWidget { get; set; }
        public IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets { get; set; }
    }
}
