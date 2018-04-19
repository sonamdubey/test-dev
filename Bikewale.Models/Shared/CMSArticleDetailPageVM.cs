using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Models.BikeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Bikewale.Models.Shared
{
    /// <summary>
    /// Created By  : Rajan Chauhan on 26 Feb 2018
    /// Summary     : Base class for article detail pages
    /// </summary>
    public class CMSArticleDetailPageVM : ModelBase
    {
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public MostPopularBikeWidgetVM MostPopularBikes { get; set; }
        public MostPopularBikeWidgetVM SeriesBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public BikeInfoVM BikeInfo { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public MostPopularBikeWidgetVM MostPopularMakeBikes { get; set; }
        public EditorialSeriesWidgetVM SeriesWidget { get; set; }
        public SimilarBikesWidgetVM SimilarBikes { get; set; }
        public IEnumerable<BikeMakeEntityBase> PopularScooterMakesWidget { get; set; }

        public MultiTabsWidgetVM PopularBikesAndPopularScootersWidget { get; set; }
        public MultiTabsWidgetVM UpcomingBikesAndUpcomingScootersWidget { get; set; }
        public MultiTabsWidgetVM PopularBikesAndUpcomingBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeBikesAndBodyStyleBikesWidget { get; set; }
        public MultiTabsWidgetVM PopularMakeScootersAndOtherBrandsWidget { get; set; }
        public MultiTabsWidgetVM PopularScootersAndUpcomingScootersWidget { get; set; }
        public MultiTabsWidgetVM SeriesBikesAndOtherBrands { get; set; }
        public MultiTabsWidgetVM SeriesBikesAndModelBodyStyleBikes { get; set; }

        public PwaReduxStore ReduxStore { get; set; }
        public IHtmlString ServerRouterWrapper { get; set; }
        public string WindowState { get; set; }

        public EnumBikeBodyStyles BodyStyle { get; set; }
        public bool IsSeriesAvailable { get; set; }
        public bool IsScooter { get; set; }
        public IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets { get; set; }

    }
}
