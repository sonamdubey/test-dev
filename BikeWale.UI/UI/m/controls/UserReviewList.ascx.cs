using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.UserReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Author:Rakesh Yadav 
    /// Date Created: 12 Sep 2015
    /// Desc: User control for listing model reviews
    /// </summary>
    public class UserReviewList : System.Web.UI.UserControl
    {
        protected Repeater rptUserReview;

        public int ModelId { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }

        private int _reviewCount = 4;
        public int ReviewCount
        {
            get { return _reviewCount; }
            set { _reviewCount = value; }
        }

        public FilterBy Filter { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int VersionId { get; set; }


        public int FetchedRecordsCount { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindUserReviews();
        }

        private void BindUserReviews()
        {
            BindUserReviewControl objUserReview = new BindUserReviewControl();
            objUserReview.ModelId = ModelId;
            objUserReview.PageNo = PageNo;
            objUserReview.PageSize = PageSize;
            objUserReview.VersionId = VersionId;
            objUserReview.Filter = Filter;
            objUserReview.RecordCount = ReviewCount;
            objUserReview.BindUserReview(rptUserReview);
            FetchedRecordsCount = objUserReview.FetchedRecordsCount;
            MakeMaskingName = objUserReview.MakeMaskingName;            
            ModelMaskingName = objUserReview.ModelMaskingName;
        }
    }
}