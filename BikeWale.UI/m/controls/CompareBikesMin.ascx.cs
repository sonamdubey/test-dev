using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Compare;
using System;

namespace Bikewale.Mobile.Controls
{
    /// <summary>
    /// Modified By : Sushil Kumar on 27th Oct 2016
    /// Description : Removed unused methods for reviews and topcompare image
    /// </summary>
    public class CompareBikesMin : System.Web.UI.UserControl
    {
        public uint TotalRecords { get; set; }
        public int FetchedRecordsCount { get; set; }
        public TopBikeCompareBase TopRecord { get; set; }
        public string Bike1ReviewLink { get; set; }
        public string Bike2ReviewLink { get; set; }
        public string Bike1ReviewText { get; set; }
        public string Bike2ReviewText { get; set; }
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
            BindBikeCompareControl objComp = new BindBikeCompareControl();
            objComp.TotalRecords = this.TotalRecords;
            objComp.FetchBikeCompares();
            this.TopRecord = objComp.FetchTopRecord();
            this.FetchedRecordsCount = objComp.FetchedRecordCount;

        }

        protected string FormatComparisonUrl(string make1MaskName, string model1MaskName, string make2MaskName, string model2MaskName, uint versionId1, uint versionId2)
        {
            string url = String.Empty;
            url = String.Format("/m/comparebikes/{0}-{1}-vs-{2}-{3}/?bike1={4}&bike2={5}", make1MaskName, model1MaskName, make2MaskName, model2MaskName, versionId1, versionId2);
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