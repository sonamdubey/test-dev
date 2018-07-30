using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.UserReviews;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Author:   Sangram Nandkhile 
    /// Date Created: 23 May 2016
    /// Desc: User control for listing model reviews
    /// </summary>
    public class NewUserReviewsList : System.Web.UI.UserControl
    {
        protected Repeater rptUserReview;
        public int ModelId { get; set; }

        private int _reviewCount = 4;
        public int ReviewCount
        {
            get { return _reviewCount; }
            set { _reviewCount = value; }
        }
        protected ushort CssWidth = 6;
        public FilterBy Filter { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int VersionId { get; set; }

        public int FetchedRecordsCount { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string WidgetHeading { get; set; }
        public int ReviewId { get; set; }

        protected string linkTitle = string.Empty;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindUserReviews();
        }
        /// <summary>
        /// Modified By :- Subodh jain 17 Jan 2017
        /// Summary :- Added review id
        /// </summary>
        private void BindUserReviews()
        {
            BindUserReviewControl objUserReview = new BindUserReviewControl();
            objUserReview.ReviewId = ReviewId;
            objUserReview.ModelId = ModelId;
            objUserReview.PageNo = PageNo;
            objUserReview.PageSize = PageSize;
            objUserReview.VersionId = VersionId;
            objUserReview.Filter = Filter;
            objUserReview.RecordCount = ReviewCount;
            objUserReview.BindUserReview(rptUserReview);
            MakeMaskingName = objUserReview.MakeMaskingName;
            ModelMaskingName = objUserReview.ModelMaskingName;

            linkTitle = string.Format("{0} {1} User Reviews", MakeName, ModelName);

            this.FetchedRecordsCount = objUserReview.FetchedRecordsCount;
            if (this.FetchedRecordsCount == 1)
                CssWidth = 12;
        }

        public override void Dispose()
        {
            rptUserReview.DataSource = null;
            rptUserReview.Dispose();

            base.Dispose();
        }
    }
}