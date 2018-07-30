using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Used;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Controls
{
    /// <summary>
    /// Created by : Sushil Kumar on 10th Oct 2016
    /// Description : User control for binding similar used bikes desktop
    /// </summary>
    public class SimilarUsedBikes : System.Web.UI.UserControl
    {
        public IEnumerable<BikeDetailsMin> similarBikeList = null;
        public BindSimilarUsedBikes usedBikeViewModel;
        public ushort TopCount { get; set; }
        public int FetchedRecordsCount { get; set; }
        public bool ShowOtherBikes { get; set; }

        public string CityName { get; set; }
        public string CityMaskingName { get; set; }

        public string ModelName { get; set; }
        public string ModelMaskingName { get; set; }

        public string MakeName { get; set; }
        public string MakeMaskingName { get; set; }

        public string BikeName { get; set; }

        public uint InquiryId { get; set; }
        public uint CityId { get; set; }
        public uint ModelId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindOtherUsedBikes();
        }
        /// <summary>
        /// Created by : Sushil Kumar on 10th Oct 2016
        /// Description : User control for binding similar used bikes desktop
        /// </summary>
        private void BindOtherUsedBikes()
        {
            try
            {
                usedBikeViewModel = new BindSimilarUsedBikes();
                similarBikeList = usedBikeViewModel.BindUsedSimilarBikes(InquiryId, CityId, ModelId, TopCount);
                FetchedRecordsCount = usedBikeViewModel.FetchedRecordsCount;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, Request.ServerVariables["URL"] + " SimilarUsedBikes.BindOtherUsedBikes");
                
            }
        }
    }
}