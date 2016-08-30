using Bikewale.BindViewModels.Controls;
using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Bikewale.m.controls
{
    public class SimilarUsedBikes : System.Web.UI.UserControl
    {
        public Repeater rptUsedBikes;
        public IEnumerable<BikeDetailsMin> similarBikeList = null;
        public BindSimilarUsedBikes usedBikeViewModel;
        public int TopCount { get; set; }
        public ushort FetchedRecordsCount { get; set; }


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
        /// Bind similar bike widget
        /// </summary>
        private void BindSimilarBikes()
        {

            usedBikeViewModel = new BindSimilarUsedBikes();
            usedBikeViewModel.InquiryId = 42512;
            usedBikeViewModel.CityId = 1;
            usedBikeViewModel.ModelId = 197;
            usedBikeViewModel.TotalRecords = 6;
            similarBikeList = usedBikeViewModel.BindUsedSimilarBikes();
            //objSimilar.BindVideos(rptSimilarVideos, this.VideoBasicId);
            //FetchedRecordsCount = objSimilar.FetchedRecordsCount;
        }

        /// <summary>
        /// Dispose the repeater datasource
        /// </summary>
        public override void Dispose()
        {
            //rptSimilarVideos.DataSource = null;
            //rptSimilarVideos.Dispose();
            base.Dispose();
        }
    }
}