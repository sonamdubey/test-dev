using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.controls
{
    /// <summary>
    /// Created by Sangram Nandkhile
    /// On 18-Feb-2016
    /// </summary>
    public class SimilarVideos : System.Web.UI.UserControl
    {
        public Repeater rptSimilarVideos;
        public int TopCount { get; set; }
        public uint VideoBasicId { get; set; }
        public int FetchedRecordsCount { get; set; }
        public string sectionTitle { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindSimilarBikes();
        }

        private void BindSimilarBikes()
        {
            BindSimilarVideos objSimilar = new BindSimilarVideos();
            objSimilar.TotalRecords = 6;
            objSimilar.BindVideos(rptSimilarVideos, this.VideoBasicId);
            FetchedRecordsCount = objSimilar.FetchedRecordsCount;
        }

        public override void Dispose()
        {
            rptSimilarVideos.DataSource = null;
            rptSimilarVideos.Dispose();
            base.Dispose();
        }
    }
}