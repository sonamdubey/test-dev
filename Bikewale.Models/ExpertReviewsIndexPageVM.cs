using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 21 Mar 2017
    /// Summary : Class have properties to render the expert reviews index page (View Model)
    /// </summary>
    public class ExpertReviewsIndexPageVM : ModelBase
    {
        // public IList<ArticleSummary> ArticlesList { get; set; }
        // public int TotalArticles { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        //public bool IsContentFound { get; set; }

        public CMSContent Articles { get; set; }
    }


}
