using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Used;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by : Sushil Kumar on 10th Oct 2016
    /// Description : User control for binding other used bikes desktop
    /// </summary>
    public class OtherUsedBikeByCity : System.Web.UI.UserControl
    {
        public Repeater rptUsedBikes;
        public IEnumerable<OtherUsedBikeDetails> otherBikesinCity = null;
        public BindOtherUsedBikesForCity viewModel;
        public ushort TopCount { get; set; }
        public int FetchedRecordsCount { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public uint InquiryId { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindSimilarBikes();
        }

        /// <summary>
        /// Created by : Sushil Kumar on 10th Oct 2016
        /// Description : User control for binding other used bikes desktop
        /// </summary>
        private void BindSimilarBikes()
        {
            try
            {
                viewModel = new BindOtherUsedBikesForCity();

                otherBikesinCity = viewModel.GetOtherBikesByCityId(InquiryId, CityId, TopCount);
                FetchedRecordsCount = viewModel.FetchedRecordsCount;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("{0}_{1}_{2}_{3}_{4}", Request.ServerVariables["URL"], " OtherUsedBikeByCity.BindSimilarBikes", InquiryId, CityId, TopCount));
                
            }
        }
    }
}