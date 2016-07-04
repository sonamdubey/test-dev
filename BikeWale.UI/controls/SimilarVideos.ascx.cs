using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Controls
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
        public ushort FetchedRecordsCount { get; set; }
        public string SectionTitle { get; set; }

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
        ///  Addition param for Similar Video controller
        /// </summary>
        private void BindSimilarBikes()
        {
            BindSimilarVideos objSimilar = new BindSimilarVideos();
            objSimilar.TotalRecords = 6;
            objSimilar.BindVideos(rptSimilarVideos, this.VideoBasicId);
            FetchedRecordsCount = objSimilar.FetchedRecordsCount;
        }

        /// <summary>
        /// Dispose the repeater datasource
        /// </summary>
        public override void Dispose()
        {
            rptSimilarVideos.DataSource = null;
            rptSimilarVideos.Dispose();
            base.Dispose();
        }
    }
}