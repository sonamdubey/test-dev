using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Compare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    public class ComparisonMin : System.Web.UI.UserControl
    {
        protected Repeater rptCompareBike;

        public int TotalRecords { get; set; }
        public int FetchedRecordsCount { get; set; }
        public TopBikeCompareBase TopRecord { get; set; }
        public string Bike1ReviewLink { get; set; }
        public string Bike2ReviewLink { get; set; }
        public string Bike1ReviewText { get; set; }
        public string Bike2ReviewText { get; set; }
        public string TopCompareImage { get; set; }
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindControls();
        }

        private void BindControls()
        {
            BindBikeCompareControl.TotalRecords = this.TotalRecords;
            BindBikeCompareControl.FetchBikeCompares();
            BindBikeCompareControl.BindBikeCompare(rptCompareBike, 1);
            this.TopRecord = BindBikeCompareControl.FetchTopRecord();
            this.FetchedRecordsCount = BindBikeCompareControl.FetchedRecordCount;

            if (FetchedRecordsCount > 0)
            {
                this.TopCompareImage = Bikewale.Utility.Image.GetPathToShowImages(TopRecord.OriginalImagePath, TopRecord.HostURL, Bikewale.Utility.ImageSize._310x174);

                if (this.TopRecord.ReviewCount1 > 0)
                {
                    this.Bike1ReviewText = String.Format("{0} reviews", this.TopRecord.ReviewCount1);
                    this.Bike1ReviewLink = String.Format("/{0}-bikes/{1}/user-reviews/", this.TopRecord.MakeMaskingName1, this.TopRecord.ModelMaskingName1);
                }
                else
                {
                    this.Bike1ReviewText = "Write reviews";
                    this.Bike1ReviewLink = String.Format("/content/userreviews/writereviews.aspx?bikem={0}", this.TopRecord.ModelId1);
                }

                if (this.TopRecord.ReviewCount2 > 0)
                {
                    this.Bike2ReviewText = String.Format("{0} reviews", this.TopRecord.ReviewCount2);
                    this.Bike2ReviewLink = String.Format("/{0}-bikes/{1}/user-reviews/", this.TopRecord.MakeMaskingName2, this.TopRecord.ModelMaskingName2);
                }
                else
                {
                    this.Bike2ReviewText = "Write reviews";
                    this.Bike2ReviewLink = String.Format("/content/userreviews/writereviews.aspx?bikem={0}", this.TopRecord.ModelId2);
                }
            }
        }

        protected string FormatComparisonUrl(string make1MaskName, string model1MaskName, string make2MaskName, string model2MaskName)
        {
            string url = String.Empty;
            url = String.Format("/comparebikes/{0}-{1}-vs-{2}-{3}/", make1MaskName, model1MaskName, make2MaskName, model2MaskName);
            return url;
        }

        protected string FormatBikeCompareAnchorText(string bike1, string bike2)
        {
            string anchorText = String.Empty;
            anchorText = String.Format("{0} vs {1}", bike1, bike2);
            return anchorText;
        }
    }
}