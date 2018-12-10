using Carwale.Interfaces.CMS.UserReviews;
using Carwale.Service;
using Carwale.Utility;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    public class UserReviewDetail
    {
        /*
         Author:Rakesh Yadav
         Date Created: 18 july 2013
         Desc: define properties
         */
        private bool serverErrorOccured = false;
        [JsonIgnore]
        public bool ServerErrorOccured
        {
            get { return serverErrorOccured; }
            set { serverErrorOccured = value; }
        }

        // Added by Supriya on 10/6/2014
        private string shareUrl = "";
        [JsonProperty("shareUrl")]
        public string ShareUrl
        {
            get { return shareUrl; }
            set { shareUrl = value; }
        }

        [JsonProperty("tinyShareUrl")]
        public string TinyShareUrl { get; set; }

        private string makeName = "";
        [JsonProperty("makeName")]
        public string MakeName
        {
            get { return makeName; }
            set { makeName = value; }
        }

        private string modelId = "";
        [JsonProperty("modelId")]
        public string ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }

        private string modelName = "";
        [JsonProperty("modelName")]
        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        private string versionId = "";
        [JsonProperty("versionId")]
        public string VersionId
        {
            get { return versionId; }
            set { versionId = value; }
        }

        private string versionName = "";
        [JsonProperty("versionName")]
        public string VersionName
        {
            get { return versionName; }
            set { versionName = value; }
        }

        private string smallPicUrl = "";
        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl
        {
            get { return smallPicUrl; }
            set { smallPicUrl = value; }
        }

        private string largePicUrl = "";
        [JsonProperty("largePicUrl")]
        public string LargePicUrl
        {
            get { return largePicUrl; }
            set { largePicUrl = value; }
        }

        private string startPrice = "";
        [JsonProperty("startPrice")]
        public string StartPrice
        {
            get { return startPrice; }
            set { startPrice = value; }
        }

        private string reviewId = "";
        [JsonProperty("reviewId")]
        public string ReviewId
        {
            get { return reviewId; }
            set { reviewId = value; }
        }

        private string reviewCommentsUrl = "";
        [JsonProperty("reviewCommentsUrl")]
        public string ReviewCommentsUrl
        {
            get { return reviewCommentsUrl; }
            set { reviewCommentsUrl = value; }
        }

        [JsonProperty("commentCount")]
        public string CommentCount { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        public ReviewDetail reviewDetail = new ReviewDetail();
        /*
         Author:Rakesh Yadav
         Date Created: 18 july 2013
         Desc:Get detailed user review and start price 
         */
        public UserReviewDetail(string reviewId)
        {            
            IUserReviewsCache userReviewDetails = UnityBootstrapper.Resolve<IUserReviewsCache>();            
            var userReview=userReviewDetails.GetUserReviewDetailById(CustomParser.parseIntObject(reviewId), Carwale.Entity.CMS.CMSAppId.Carwale);
            ReviewId = reviewId;
            StartPrice = userReview.StartPrice;
            MakeName = userReview.Make;
            ModelName = userReview.Model;
            VersionId = userReview.VersionId;
            VersionName = userReview.Version;
            SmallPicUrl = ImageSizes.CreateImageUrl(userReview.HostUrl, ImageSizes._110X61, userReview.OriginalImgPath);
            LargePicUrl = ImageSizes.CreateImageUrl(userReview.HostUrl, ImageSizes._210X118, userReview.OriginalImgPath);
            OriginalImgPath = userReview.OriginalImgPath;
            HostUrl = userReview.HostUrl;
            ModelId = userReview.ModelId;
            ShareUrl = userReview.ShareUrl;
            reviewDetail.Title = userReview.Title;
            reviewDetail.ReviewDate = userReview.ReviewDate;
            reviewDetail.ReviewRate = userReview.ReviewRate;
            reviewDetail.Goods = userReview.Pros;
            reviewDetail.Bads = userReview.Cons;
            reviewDetail.Comment = userReview.Comments;
            reviewDetail.PurchasedAs = userReview.PurchasedAs;
            reviewDetail.FuelEconomy = userReview.FuelEconomy;
            reviewDetail.Familiarity = userReview.Familiarity;
            reviewDetail.Author =userReview.Author;      
            ReviewCommentsUrl = userReview.ReviewCommentsUrl;
            CommentCount = userReview.CommentsCount;
        }
    }
}