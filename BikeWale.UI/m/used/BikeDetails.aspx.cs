using Bikewale.Mobile.Controls;
using System;

namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BikeDetails : System.Web.UI.Page
    {
        public SimilarUsedBikes ctrlSimilarUsedBikes;
        public OtherUsedBikeByCity ctrlOtherUsedBikes;

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            base.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUserControls();
            }
        }

        private void BindUserControls()
        {
            ctrlSimilarUsedBikes.InquiryId = 42512;
            ctrlSimilarUsedBikes.CityId = 1;
            ctrlSimilarUsedBikes.ModelId = 197;
            ctrlSimilarUsedBikes.TopCount = 5;
            ctrlSimilarUsedBikes.ModelName = "Duke 200";
            ctrlSimilarUsedBikes.ModelMaskingName = "duke200";
            ctrlSimilarUsedBikes.MakeName = "KTM";
            ctrlSimilarUsedBikes.MakeMaskingName = "ktm";
            ctrlSimilarUsedBikes.BikeName = string.Format("{0} {1}", ctrlSimilarUsedBikes.MakeName, ctrlSimilarUsedBikes.ModelName);



            ctrlOtherUsedBikes.InquiryId = 42512;
            ctrlOtherUsedBikes.CityId = 1;
            ctrlOtherUsedBikes.ModelId = 197;
            ctrlOtherUsedBikes.TopCount = 5;
        }

    }
}