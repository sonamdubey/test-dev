using Bikewale.BindViewModels.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.controls
{
    public class MMostPopularBikes : System.Web.UI.UserControl
    {
        public Repeater rptMostPopularBikes;

        public int? totalCount { get; set; }
        public int? makeId { get; set; }
        public int FetchedRecordsCount { get; set; }

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MostPopularBikes();
        }

        private void MostPopularBikes()
        {
            BindMostPopularBikesControl.totalCount = this.totalCount;
            BindMostPopularBikesControl.makeId = this.makeId;
            BindMostPopularBikesControl.BindMostPopularBikes(rptMostPopularBikes);
            this.FetchedRecordsCount = BindMostPopularBikesControl.FetchedRecordsCount;
        }
    }
}