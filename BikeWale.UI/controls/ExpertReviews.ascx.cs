using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bikewale.BindViewModels.Controls;

namespace Bikewale.Controls
{
    public class ExpertReviews : System.Web.UI.UserControl
    {
        protected Repeater rptExpertReviews;
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string MoreExpertReviewUrl { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindExpertReviewsControl.TotalRecords = this.TotalRecords;
            BindExpertReviewsControl.MakeId = this.MakeId;
            BindExpertReviewsControl.ModelId = this.ModelId;
            BindExpertReviewsControl.BindExpertReviews(rptExpertReviews);
            this.FetchedRecordsCount = BindExpertReviewsControl.FetchedRecordsCount;

            if (String.IsNullOrEmpty(MakeMaskingName) && String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreExpertReviewUrl = "/road-tests/";
            }
            else if (String.IsNullOrEmpty(ModelMaskingName))
            {
                MoreExpertReviewUrl = String.Format("/{0}-bikes/road-tests/", MakeMaskingName);
            }
            else
            {
                MoreExpertReviewUrl = String.Format("/{0}-bikes/{1}/road-tests/", MakeMaskingName, ModelMaskingName);
            }
        }
    }
}