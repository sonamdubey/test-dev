using Bikewale.BindViewModels.Controls;
using System;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Controls
{
    public class NewExpertReviewsWidget : System.Web.UI.UserControl
    {
        protected Repeater rptExpertReviews;

        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string MoreExpertReviewUrl { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        protected string linkTitle = string.Empty;
        private bool _showWidget = true;
        public bool ShowWidgetTitle { get { return _showWidget; } set { _showWidget = value; } }


        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindExpertReviewsControl ber = new BindExpertReviewsControl();
            ber.TotalRecords = this.TotalRecords;
            ber.MakeId = this.MakeId;
            ber.ModelId = this.ModelId;
            ber.BindExpertReviews(rptExpertReviews);
            this.FetchedRecordsCount = ber.FetchedRecordsCount;

            if (String.IsNullOrEmpty(MakeMaskingName) && String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreExpertReviewUrl = "/m/expert-reviews/";
                linkTitle = "Expert Reviews on Bikes";
            }
            else if (String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreExpertReviewUrl = String.Format("/m/{0}-bikes/expert-reviews/", MakeMaskingName);
                linkTitle = string.Format("{0} Expert Reviews", MakeName);
            }
            else
            {
                MoreExpertReviewUrl = String.Format("/m/{0}-bikes/{1}/expert-reviews/", MakeMaskingName, ModelMaskingName);
                linkTitle = string.Format("{0} {1} Expert Reviews", MakeName, ModelName);
            }
        }
    }
}