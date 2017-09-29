using Bikewale.Entities.Pager;
using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewsSearchVM
    {
        public BikeReviewsInfo ReviewsInfo { get; set; }
        public uint ModelId { get; set; }
        public string BikeName { get; set; }
        public SearchResult UserReviews { get; set; }
        public uint PageSize { get; set; }
        public PagerEntity Pager { get; set; }
        public FilterBy ActiveReviewCategory { get; set; }
        public string WriteReviewLink { get; set; }
        private bool _IsPagerNeeded = true;
        public bool IsPagerNeeded
        {
            get { return _IsPagerNeeded; }
            set { _IsPagerNeeded = value; }
        }
        public string WidgetHeading { get; set; }

        public QuestionsRatingValueByModel ObjQuestionValue { get; set; }  
        public uint ReviewsReadPerSession { get; set; }      
    }
}
